using MooshakV2.DAL;
using MooshakV2.Models;
using MooshakV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
				viewModel.date = assignment.handInDate;
                viewModel.id = assignment.id;
                viewModel.input = assignment.input;
                viewModel.output = assignment.output;
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
                assignment.date = item.handInDate;
                assignment.input = item.input;
                assignment.output = item.output;

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

        public bool removeSubmission(SubmissionViewModel toDelModel)
        {
            // TODO: implement
            var toDel = (from s in contextDb.submissions
                         where s.Id == toDelModel.id
                         select s).SingleOrDefault();
            if (toDel != null)
            {
                contextDb.submissions.Remove(toDel);
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

        private SubmissionViewModel partToSubmissionViewModel(Submission sub)
        {
            var subModel = new SubmissionViewModel();
            subModel.success = sub.success;
            subModel.date = sub.date;
            subModel.count = sub.count;
            subModel.id = sub.Id;
            subModel.assignmentId = sub.assignmentId;
            subModel.partId = sub.partId;
            subModel.userId = sub.userId;
            subModel.filename = sub.filename;
            subModel.mime = sub.mime;
            return subModel;
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

                // Get submissions
                var submissions = (from s in contextDb.submissions
                                   where s.assignmentId == toDel.id
                                   select s).ToList();
                // Delete all submissions
                foreach (var sub in submissions)
                {

                    //Return false if removing submission fails
                    if (!removeSubmission(partToSubmissionViewModel(sub)))
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
                newSubmit.partId = 1;
                newSubmit.success = 0;
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

        public string getInput(FileUploadViewModel upload)
        {
            var input = (from i in contextDb.assignments
                         where i.id == upload.assignmentId
                         select i.input).SingleOrDefault();

            return input;

        }

        public string getOutput(FileUploadViewModel upload)
        {
            var expectedOutput = (from i in contextDb.assignments
                                  where i.id == upload.assignmentId
                                  select i.output).SingleOrDefault();

            return expectedOutput;
        }

        //Did not manage to get the compiler to work properly, we first tried to implement it in AssignmentService.cs and then after same failed tries we
        //tried to implement it in AssignmentController.cs but we were to short on time.
        //This is the soon to be Compiler.
        /*public int checkSuccess(FileUploadViewModel upload)
        {

            var path = Path.GetDirectoryName("~/AllFiles/");
            var workingFolder = path + "\\Temp\\";
            var fileName = upload.file.FileName;
            var noExtension = Path.GetFileNameWithoutExtension(fileName);
            var exeFilePath = workingFolder + noExtension + ".exe";

            var compilerFolder = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\VC\\bin\\";

            Process compiler = new Process();
            compiler.StartInfo.FileName = "cmd.exe";
            compiler.StartInfo.WorkingDirectory = workingFolder;
            compiler.StartInfo.RedirectStandardInput = true;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.StartInfo.UseShellExecute = false;

            compiler.Start();
            compiler.StandardInput.WriteLine("\"" + compilerFolder + "vcvars32.bat" + "\"");
            compiler.StandardInput.WriteLine("cl.exe /nologo /EHsc " + fileName);
            compiler.StandardInput.WriteLine("exit");
            string output = compiler.StandardOutput.ReadToEnd();
            compiler.WaitForExit();
            compiler.Close();

            var input = (from i in contextDb.assignments
                         where i.id == upload.assignmentId
                         select i.input).SingleOrDefault();

            var expectedOutput = (from i in contextDb.assignments
                                  where i.id == upload.assignmentId
                                  select i.output).SingleOrDefault();
            

            if (System.IO.File.Exists(exeFilePath))
            {
                var processInfoExe = new ProcessStartInfo(exeFilePath, "");
                processInfoExe.UseShellExecute = false;
                processInfoExe.RedirectStandardOutput = true;
                processInfoExe.RedirectStandardError = true;
                processInfoExe.CreateNoWindow = true;
                using (var processExe = new Process())
                {
                    processExe.StartInfo = processInfoExe;
                    processExe.Start();
                    processExe.StandardInput.WriteLine(input);
                    processExe.StandardInput.Close();

                    // We then read the output of the program:
                    var lines = new List<string>();
                    while (!processExe.StandardOutput.EndOfStream)
                    {
                        lines.Add(processExe.StandardOutput.ReadLine());
                    }

                    StringBuilder builder = new StringBuilder();
                    foreach (string line in lines) // Loop through all strings
                    {
                        builder.Append(line); // Append string to StringBuilder
                    }
                    string result = builder.ToString(); // Get string from StringBuilder

                    if (result == expectedOutput)
                    {
                        return 1;
                    }

                }
            }

            return 0;
        }*/
    }
}