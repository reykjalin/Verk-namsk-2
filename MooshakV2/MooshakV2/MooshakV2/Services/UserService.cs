using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MooshakV2.DAL;
using MooshakV2.ViewModels;
using System.Web.Security;
using MooshakV2.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace MooshakV2.Services
{
    public class UserService
    {
        private DatabaseDataContext contextDb;
        private RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

        public UserService() { contextDb = new DatabaseDataContext(); }

        public List<UserViewModel> getAllUsers()
        {
            // Get all users
            var userList = (from user in contextDb.aspNetUsers
                            select user).ToList();

            // Move from entities to view models
            List<UserViewModel> userListModel = new List<UserViewModel>();
            foreach (var user in userList)
            {
                UserViewModel userModel = new UserViewModel();
                userModel.userName = user.UserName;
                userModel.email = user.Email;
                var role = roleManager.FindByName(user.UserName);

                // Get role ID, default value is 4 (Student) if role isn't found.
                try
                {
                    userModel.roleId = Convert.ToInt32(role.Id);
                }
                catch(Exception e)
                {
                    userModel.roleId = 4;
                }

                userListModel.Add(userModel);
            }
            return userListModel;
        }
    }
}