using Mooshak_TestByE.Models;
using Mooshak_TestByE.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mooshak_TestByE.Services
{
    public class CourseService
    {
        private ApplicationDbContext database;
        public CourseService()
        {
            database = new ApplicationDbContext();
        }

        public List<CourseViewModel> getAllCourses()
        {
            return new List<CourseViewModel>();
        }

        public CourseViewModel getCourseById(int courseId)
        {
            return new CourseViewModel();
        }
    }
}