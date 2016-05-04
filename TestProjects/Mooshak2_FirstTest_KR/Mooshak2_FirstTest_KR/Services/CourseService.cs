using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mooshak2_FirstTest_KR.DAL;
using Mooshak2_FirstTest_KR.Models;
using Mooshak2_FirstTest_KR.Models.ViewModels;

namespace Mooshak2_FirstTest_KR.Services
{
    public class CourseService
    {
        private ApplicationDbContext database = new ApplicationDbContext();
        private DatabaseDataContext contextDb = new DatabaseDataContext();

        public CourseService() { database = new ApplicationDbContext(); }

        public List<CourseViewModel> getAllCourses()
        {
            var courseList = (from clist in contextDb.courses
                             select clist).ToList();
            var courseModelList = new List<CourseViewModel>();
            foreach(var course in courseList)
            {
                var viewModel = new CourseViewModel();
                viewModel.title = course.title;
                viewModel.description = course.description;
                viewModel.id = course.id;

                courseModelList.Add(viewModel);
            }
            return courseModelList;
        }

        public CourseViewModel getCourseById(int? courseId)
        {
            if(courseId != null)
            {
                var course = (from c in contextDb.courses
                              where c.id == courseId
                              select c).FirstOrDefault();

                if(course != null)
                {
                    var model = new CourseViewModel();
                    model.title = course.title;
                    model.description = course.description;
                    model.id = course.id;

                    return model;
                }
            }
            return null;
        }
    }
}