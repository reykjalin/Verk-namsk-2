﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MooshakV2.DAL;
using MooshakV2.ViewModels;
using System.Web.Security;
using MooshakV2.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using MooshakV2.Controllers;

namespace MooshakV2.Services
{
    public class UserService
    {
        private DatabaseDataContext contextDb;

        public UserService() { contextDb = new DatabaseDataContext(); }

        public List<UserViewModel> getAllUsers(ApplicationUserManager userManager)
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

                try
                {
                    var role = userManager.GetRoles(user.Id).FirstOrDefault();
                    userModel.roleId = Convert.ToInt32((from roles in contextDb.aspNetRoles
                                                        where roles.Name == role
                                                        select roles.Id).FirstOrDefault());
                }
                    catch (Exception e)
                {
                    userModel.roleId = 4;
                }

            userListModel.Add(userModel);
            }
            return userListModel;
        }

        public UserViewModel getUserByUserName(string userName, ApplicationUserManager userManager)
        {
            // 'userName' athugað í controller, veit að það er valid
            var userEntity = (from user in contextDb.aspNetUsers
                             where user.UserName == userName
                             select user).SingleOrDefault();
            if(userEntity != null)
            {
                UserViewModel user = new UserViewModel();
                user.userName = userEntity.UserName;
                user.email = userEntity.Email;
                // Get role ID, default value is 4 (Student) if role isn't found.
                try
                {
                    var role = userManager.GetRoles(userEntity.Id).FirstOrDefault();
                    user.roleId = Convert.ToInt32((from roles in contextDb.aspNetRoles
                                                   where roles.Name == role
                                                   select roles.Id).FirstOrDefault());
                }
                catch(Exception e)
                {
                    user.roleId = 4;
                }
                return user;
            }
            return null;
        }

        public bool changeUser(UserViewModel newUserInfo, ApplicationUserManager userManager)
        {
            // TODO: Change to use userId. Add userId to UserViewModel
            // Get usera information from userName
            var user = (from users in contextDb.aspNetUsers
                        where users.UserName == newUserInfo.userName
                        select users).SingleOrDefault();
            if(user != null)
            {
                user.UserName = newUserInfo.userName;
                user.Email = newUserInfo.email;

                // Get current user role
                var role = userManager.GetRoles(user.Id).FirstOrDefault();
                // If user didn't have a role, don't remove the user from his roles
                if(role != null)
                    userManager.RemoveFromRoles(user.Id, role);
                // Get name of new role from DB
                var roleName = (from roles in contextDb.aspNetRoles
                                where roles.Id == newUserInfo.roleId.ToString()
                                select roles.Name).FirstOrDefault();
                // Add new role to user
                userManager.AddToRole(user.Id, roleName);
                // Update DB
                contextDb.SaveChanges();
                return true;
            }
            return false;
        }

        public List<AspNetRole> getRoles()
        {
            return (from roles in contextDb.aspNetRoles
                    select roles).ToList();
        }
    }
}