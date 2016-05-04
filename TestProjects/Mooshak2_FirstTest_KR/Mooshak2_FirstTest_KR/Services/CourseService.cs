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
        private CourseDataContext contextDb = new CourseDataContext();

        public CourseService() { database = new ApplicationDbContext(); }

        public List<CourseViewModel> getAllCourses() { return new List<CourseViewModel>(); }

        public CourseViewModel getCourseById(int? courseId)
        {
            if(courseId == null)
            {
                return null;
            }
            var course = (from c in contextDb.courses
                         where c.courseId == courseId
                         select c).FirstOrDefault();

            if(course != null)
            {
                var model = new CourseViewModel();
                model.title = course.title;
                model.description = course.description;
                return model;
            }
            return null;
        }
    }
}