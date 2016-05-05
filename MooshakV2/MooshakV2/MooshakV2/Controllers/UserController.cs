using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MooshakV2.Services;
using System.Web.Mvc;

namespace MooshakV2.Controllers
{
    public class UserController: Controllers
    {
        private UserService service;

        public UserController()
        {
            service = new UserService();
        }

        //Further about the user
        public ActionResult User(int userId)
        {
            return View();
        }

        //Create User
        public ActionResult Create()
        {
            return View();
        }

        //Edit User
        public ActionResult Edit()
        {
            return View();
        }

        //Remove User
        public ActionResult Remove()
        {
            return View();
        }

        //Get all users
        public ActionResult AllUsers(int id)
        {
            return View();
        }

        //Get all students
        public ActionResult AllStudents()
        {
            return View();
        }

        //Get all teachers
        public ActionResult AllTeachers()
        {
            return View();
        }
     
        //Show settings
        public ActionResult Settings(int userId)
        {
            return View();
        }
 
    }
}