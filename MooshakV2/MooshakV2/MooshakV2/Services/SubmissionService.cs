﻿
using MooshakV2.DAL;
using MooshakV2.Models;
using MooshakV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakV2.Services
{
    public class SubmissionService
    {
        private DatabaseDataContext contextDb;

        public SubmissionService()
        {
            contextDb = new DatabaseDataContext();

        }

        public List<SubmissionViewModel> getAllSubmissions()
        {
            var submissionList = (from alist in contextDb.submissions
                                  select alist).ToList();
            var submissionModelList = new List<SubmissionViewModel>();
            foreach (var submission in submissionList)
            {
                var viewModel = new SubmissionViewModel();
                viewModel.success = submission.success;
                viewModel.date = submission.date;
                viewModel.count = submission.count;
                viewModel.id = submission.Id;
                viewModel.assignmentId = submission.assignmentId;
                viewModel.partId = submission.partId;
                viewModel.userId = submission.userId;
                viewModel.filename = submission.filename;
                viewModel.mime = submission.mime;
                submissionModelList.Add(viewModel);
            }
            return submissionModelList;
        }

        public List<HistoryViewModel> getAllHistoryViewModels()
        {
            var submissionList = (from alist in contextDb.submissions
                                  select alist).ToList();
            var historyModelList = new List<HistoryViewModel>();

            foreach (var submission in submissionList)
            {
                var viewModel = new HistoryViewModel();
                viewModel.id = submission.Id;
                viewModel.userName = (from users in contextDb.aspNetUsers
                                      where submission.userId == users.Id
                                      select users.UserName).FirstOrDefault();

                var courseId = (from ass in contextDb.assignments
                                where submission.assignmentId == ass.id
                                select ass.courseId).FirstOrDefault();

                viewModel.course = (from courses in contextDb.courses
                                    where courseId == courses.id
                                    select courses.title).FirstOrDefault();

                viewModel.assignment = (from ass in contextDb.assignments
                                        where submission.assignmentId == ass.id
                                        select ass.title).FirstOrDefault();

                viewModel.assignmentPart = (from assPart in contextDb.assignmentParts
                                            where submission.partId == assPart.id
                                            select assPart.title).FirstOrDefault();

                viewModel.success = submission.success;
                viewModel.count = submission.count;
                viewModel.filename = submission.filename;
                viewModel.date = submission.date;

                viewModel.assignmentId = submission.assignmentId;
               
                historyModelList.Add(viewModel);
            }


            return historyModelList;
        }

        public List<HistoryViewModel> getAllHistoryViewModelsByID(string userId)
        {
            var submissionList = (from alist in contextDb.submissions
                                  where alist.userId == userId
                                  select alist).ToList();

            var historyModelList = new List<HistoryViewModel>();

            foreach (var item in submissionList)
            {
                var history = new HistoryViewModel();

                history.id = item.Id;
                history.userName = (from users in contextDb.aspNetUsers
                                      where item.userId == users.Id
                                      select users.UserName).FirstOrDefault();

                var courseId = (from ass in contextDb.assignments
                                where item.assignmentId == ass.id
                                select ass.courseId).FirstOrDefault();

                history.course = (from courses in contextDb.courses
                                    where courseId == courses.id
                                    select courses.title).FirstOrDefault();

                history.assignment = (from ass in contextDb.assignments
                                        where item.assignmentId == ass.id
                                        select ass.title).FirstOrDefault();

                history.assignmentPart = (from assPart in contextDb.assignmentParts
                                            where item.partId == assPart.id
                                            select assPart.title).FirstOrDefault();

                history.success = item.success;
                history.count = item.count;
                history.filename = item.filename;
                history.date = item.date;

                history.assignmentId = item.assignmentId;

                historyModelList.Add(history);
            }

            return historyModelList;
        }


        public List<SubmissionViewModel> getAllSubmissionsFromAssignment(int assignmentId)
        {
            var submissionEntities = (from subs in contextDb.submissions
                                      where assignmentId == subs.assignmentId
                                      select subs).ToList();

            var submissionList = new List<SubmissionViewModel>();
            
            foreach (var item in submissionEntities)
            {
                var submission = new SubmissionViewModel();

                submission.assignmentId = item.assignmentId;
                submission.count = item.count;
                submission.date = item.date;
                submission.filename = item.filename;
                submission.id = item.Id;
                submission.mime = item.mime;
                submission.partId = item.partId;
                submission.success = item.success;
                submission.userId = item.userId;

                submissionList.Add(submission);

            }

            return submissionList;
        }


        public bool addSubmission(SubmissionViewModel newSubmissionModel)
        {
            Submission newSubmission = new Submission();
            newSubmission.success = newSubmissionModel.success;
            newSubmission.date = newSubmissionModel.date;
            newSubmission.count = newSubmissionModel.count;
            newSubmission.Id = newSubmissionModel.id;
            newSubmission.assignmentId = newSubmissionModel.assignmentId;
            newSubmission.partId = newSubmissionModel.partId;
            newSubmission.userId = newSubmissionModel.userId;
            newSubmission.filename = newSubmissionModel.filename;
            newSubmission.mime = newSubmissionModel.mime;

            contextDb.submissions.Add(newSubmission);
            contextDb.SaveChanges();

            return true;
        }
    }
}