using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mooshak2_FirstTest_KR.Models;
using Mooshak2_FirstTest_KR.Models.ViewModels;

namespace Mooshak2_FirstTest_KR.Services
{
    public class CourseService
    {
        private ApplicationDbContext database;

        public CourseService() { database = new ApplicationDbContext(); }

        public List<CourseViewModel> getAllCourses() { return new List<CourseViewModel>(); }

        public List<CourseViewModel> getCourseById(int courseId) { return new List<CourseViewModel>(); }
    }
}