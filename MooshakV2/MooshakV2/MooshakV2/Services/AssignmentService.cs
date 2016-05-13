using MooshakV2.DAL;
using MooshakV2.Models;
using MooshakV2.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MooshakV2.Services
{
    public class AssignmentService
    {
        private DatabaseDataContext contextDb;
        public AssignmentService()
        {
            contextDb = new DatabaseDataContext();
        }

        public List<AssignmentViewModel> getAllAssignments()
        {
            var assignmentList = (from alist in contextDb.assignments
                                  select alist).ToList();

            var assignmentModelList = new List<AssignmentViewModel>();
            foreach (var assignment in assignmentList)
            {
                var viewModel = new AssignmentViewModel();
                viewModel.title = assignment.title;
                viewModel.description = assignment.description;
                viewModel.weight = assignment.weight;
                viewModel.id = assignment.id;
                viewModel.courseId = assignment.courseId;
                assignmentModelList.Add(viewModel);
            }
            return assignmentModelList;
        }

        public List<AssignmentViewModel> getAllAssignmentsInCourse(int courseId)
        {
            var assignmentEntities = (from assignments in contextDb.assignments
                                      where courseId == assignments.courseId
                                      select assignments).ToList();

            var assignmentList = new List<AssignmentViewModel>();

            foreach (var item in assignmentEntities)
            {
                var assignment = new AssignmentViewModel();

                assignment.title = item.title;
                assignment.description = item.description;
                assignment.id = item.id;
                assignment.weight = item.weight;
                assignment.courseId = item.courseId;

                assignmentList.Add(assignment);
            }

            return assignmentList;
        }

        public bool addAssignment(AssignmentViewModel newAssignmentModel)
        {
            Assignment newAssignment = new Assignment();
            newAssignment.title = newAssignmentModel.title;
            newAssignment.description = newAssignmentModel.description;
            newAssignment.weight = newAssignmentModel.weight;
            newAssignment.courseId = newAssignmentModel.courseId;
            newAssignment.input = newAssignmentModel.input;
            newAssignment.output = newAssignmentModel.output;
            newAssignment.handInDate = newAssignmentModel.date;

            contextDb.assignments.Add(newAssignment);
            contextDb.SaveChanges();
            /*
             * Get assignment id, MASSIVE ASSUMPTION: no assignments have the same 
             * title/descr/weight/courseId, otherwise this will fail
             */
            var assId = (from a in contextDb.assignments
                         where a.title == newAssignment.title &&
                               a.description == newAssignment.description &&
                               a.weight == newAssignment.weight &&
                               a.courseId == newAssignment.courseId
                         select a.id).Single();
            foreach(var part in newAssignmentModel.assignmentParts)
                addPart(part, assId);

            return true;
        }

        public bool removePart(AssignmentPartViewModel toDelModel)
        {
            // TODO: implement
            var toDel = (from p in contextDb.assignmentParts
                         where p.id == toDelModel.id
                         select p).SingleOrDefault();
            if(toDel != null)
            {
                contextDb.assignmentParts.Remove(toDel);
                contextDb.SaveChanges();
                return true;
            }
            return false;
        }

        public AssignmentViewModel getAssignmentById(int? id)
        {
            var assignment = (from a in contextDb.assignments
                              where a.id == id
                              select a).FirstOrDefault();
            var parts = (from ap in contextDb.assignmentParts
                         where ap.assignmentId == id
                         select ap).ToList();

            if (assignment == null)
                return null;

            var model = new AssignmentViewModel();
            model.title = assignment.title;
            model.description = assignment.description;
            model.weight = assignment.weight;
            model.id = assignment.id;
            model.courseId = assignment.courseId;
            model.input = assignment.input;
            model.output = assignment.output;
            model.date = assignment.handInDate;

            // Convert parts (List<AssignmentPart>) to list of partmodels (List<AssignmentPartViewModel>)
            var partList = (from p in parts
                            select partToPartViewModel(p)).ToList();
            model.assignmentParts = partList;

            return model;
        }

        private AssignmentPartViewModel partToPartViewModel(AssignmentPart part)
        {
            var partModel = new AssignmentPartViewModel();
            partModel.id = part.id;
            partModel.title = part.title;
            partModel.description = part.description;
            partModel.weight = part.weight;
            return partModel;
        }

        public bool updateAssignment(AssignmentViewModel newData)
        {
            if (newData != null)
            {
                var oldAssignment = (from a in contextDb.assignments
                                     where a.id == newData.id
                                     select a).SingleOrDefault();

                if (oldAssignment != null)
                {
                    oldAssignment.title = newData.title;
                    oldAssignment.description = newData.description;
                    oldAssignment.weight = newData.weight;
                    oldAssignment.id = newData.id;
                    oldAssignment.courseId = newData.courseId;
                    oldAssignment.input = newData.input;
                    oldAssignment.output = newData.output;
                    oldAssignment.handInDate = newData.date;

                    contextDb.SaveChanges();
                    foreach(var part in newData.assignmentParts)
                    {
                        if(!addPart(part, newData.id))
                            return false;
                    }
                    return true;
                }
            }
            return false;
        }

        public bool addPart(AssignmentPartViewModel part, int assId)
        {
            // Check for other parts under same assignment
            var otherParts = (from oparts in contextDb.assignmentParts
                              where oparts.assignmentId == assId
                              select oparts).ToList();
            // Check for an old version of this part
            var old = (from o in contextDb.assignmentParts
                       where o.id == part.id
                       select o).SingleOrDefault();
            // Default part number is 1
            int partNr = 1;

            if (otherParts.Count > 0)
            { 
                // update old if found
                if (old != null)
                {
                    old.title = part.title;
                    old.description = part.description;
                    old.weight = part.weight;
                    contextDb.SaveChanges();
                    return true;
                }
                // If old not found, that means there are other parts under this assignment => update partNr
                partNr = (from o in contextDb.assignmentParts
                          where o.assignmentId == assId
                          select o.partNr).ToList().Max() + 1;
            }

            // create new part
            var newPart = new AssignmentPart();
            newPart.title = part.title;
            newPart.description = part.description;
            newPart.weight = part.weight;
            newPart.assignmentId = assId;
            newPart.partNr = partNr;

            newPart.id = (from p in contextDb.assignmentParts
                          select p.id).ToList().Max() + 1;

            // Add part to DB and update
            contextDb.assignmentParts.Add(newPart);
            contextDb.SaveChanges();
            return true;
        }

        public bool removeAssignment(int id)
        {
            // Find assignment to delete
            var toDel = (from a in contextDb.assignments
                         where a.id == id
                         select a).SingleOrDefault();
            // Check for null
            if(toDel != null)
            {
                // Get assignment parts
                var parts = (from p in contextDb.assignmentParts
                             where p.assignmentId == toDel.id
                             select p).ToList();
                // Delete all parts
                foreach(var part in parts)
                {
                    // Return false if removing part fails
                    if(!removePart(partToPartViewModel(part)))
                        return false;
                }
                // Remove assignment
                contextDb.assignments.Remove(toDel);
                contextDb.SaveChanges();
                return true;
            }
            return false;
        }

        public void submitFile(FileUploadViewModel aFile, string userId)
        {
            if (aFile != null)
            {
                var fileExtension = Path.GetExtension(aFile.file.FileName);
                var fileContentType = aFile.file.ContentType;

                Submission newSubmit = new Submission();
                newSubmit.assignmentId = aFile.assignmentId.Value;
                newSubmit.mime = fileContentType;
                newSubmit.filename = aFile.file.FileName;
                newSubmit.date = DateTime.Now;
                newSubmit.userId = userId;
                newSubmit.partId = 6;
                newSubmit.success = aFile.success;
                newSubmit.count = aFile.count;

                var id = (from i in contextDb.submissions
                                select i.Id).ToList();
                if(id.Count > 0)
                {
                    newSubmit.Id = id.Max() + 1;
                }
                else
                {
                    newSubmit.Id = 1;
                }


                newSubmit = contextDb.submissions.Add(newSubmit);
                contextDb.SaveChanges();

                contextDb.Entry(newSubmit).State = System.Data.Entity.EntityState.Modified;
                contextDb.SaveChanges();
            }
        }
    }
}