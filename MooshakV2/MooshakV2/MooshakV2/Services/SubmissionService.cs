
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