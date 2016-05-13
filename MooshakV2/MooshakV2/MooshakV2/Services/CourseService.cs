using MooshakV2.DAL;
using MooshakV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakV2.Services
{
    /// <summary>
    /// CourseService sér um alla þunga lógík fyrir CourseController.
    /// </summary>
    public class CourseService
    {
        private DatabaseDataContext contextDb = new DatabaseDataContext();

        /// <summary>
        /// Smiður. Upphafsstillir contextDb klasabreytuna.
        /// </summary>
        public CourseService() { contextDb = new DatabaseDataContext(); }

        /// <summary>
        /// Returns a list of all courses in database
        /// </summary>
        /// <returns>
        /// List
        /// </returns>
        public List<CourseViewModel> getAllCourses()
        {
            // Sæki öll course úr DB
            var courseList = (from clist in contextDb.courses
                              select clist).ToList();

            // Fylli í List sem geymir öll CourseViewModel
            var courseModelList = new List<CourseViewModel>();
            foreach (var course in courseList)
            {
                // Fylli í nýtt CourseViewModel
                var viewModel = new CourseViewModel();
                viewModel.title = course.title;
                viewModel.description = course.description;
                viewModel.id = course.id;
                // Bæti nýja ViewModeli við listann
                courseModelList.Add(viewModel);
            }
            return courseModelList;
        }

        /// <summary>
        /// Sækir Course í gagnagrunn sem hefur ID 'id'
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseViewModel getCourseById(int? id)
        {
            // 'id' er ekki null, það er athugað í controller
            var course = (from c in contextDb.courses
                          where c.id == id
                          select c).FirstOrDefault();

            // Ef 'course' er null, þá er course ekki til í DB
            if (course == null)
                return null;

            // Fylli inn í nýtt ViewModel
            var model = new CourseViewModel();
            model.title = course.title;
            model.description = course.description;
            model.id = course.id;

            return model;
        }

        /// <summary>
        /// Uppfærir Course með sama ID og 'newData' með gögnum úr 'newData'
        /// </summary>
        /// <param name="newData"></param>
        /// <returns>
        /// bool
        /// </returns>
        public bool updateCourse(CourseViewModel newData)
        {
            // Passa að það séu gögn í ViewModelinu sem er sent í fallið
            if (newData != null)
            {
                var oldCourse = (from clist in contextDb.courses
                                 where clist.id == newData.id
                                 select clist).SingleOrDefault();

                // Ef oldCourse er null, þá er course-ið (af einhverjum ástæðum) ekki til
                if (oldCourse != null)
                {
                    // Breyti upplýsingum í oldCourse
                    oldCourse.description = newData.description;
                    oldCourse.title = newData.title;

                    // Vista breytingar í DB
                    contextDb.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Býr til nýtt Course og setur í gagnagrunn með upplýsingum úr
        /// 'newCourseModel'
        /// </summary>
        /// <param name="newCourseModel"></param>
        /// <returns></returns>
        public bool addCourse(CourseViewModel newCourseModel)
        {
            // newCourse er athugað í controller, veit því að það er valid
            Course newCourse = new Course();
            newCourse.title = newCourseModel.title;
            newCourse.description = newCourseModel.description;

            contextDb.courses.Add(newCourse);
            contextDb.SaveChanges();
            return true;
        }

        /// <summary>
        /// Eyðir Course með ID 'id' úr gagnagrunni.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool removeCourse(int id)
        {
            // toRemove eytt úr DB, veit að id er valid út af GET fyrirspurn í controller
            contextDb.courses.Remove((from course in contextDb.courses
                                      where course.id == id
                                      select course).SingleOrDefault());
            contextDb.SaveChanges();
            return true;
        }

        public bool addUserToCourse(int courseId, UserViewModel user, ApplicationUserManager userManager)
        {
            var userEntity = (from u in contextDb.aspNetUsers
                              where u.UserName == user.userName
                              select u).SingleOrDefault();
            var course = (from c in contextDb.courses
                          where c.id == courseId
                          select c).SingleOrDefault();
            if(userEntity != null && course != null)
            {
                CourseStudent student = new CourseStudent();
                student.courseId = courseId;
                student.userId = userEntity.Id;
                contextDb.courseStudents.Add(student);
                contextDb.SaveChanges();
                return true;
            }
            return false;
        }
    }
}