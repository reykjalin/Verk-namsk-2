using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MooshakV2.DAL;
using MooshakV2.ViewModels;
using System.Web.Security;
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;
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

        public List<UserDetailViewModel> getAllUsers(ApplicationUserManager userManager)
        {
            // Get all users
            var userList = (from user in contextDb.aspNetUsers
                            join u in contextDb.userDetails on user.UserName equals u.userName
                            orderby u.name
                            select user).ToList();

            // Move from entities to view models
            List<UserDetailViewModel> userListModel = new List<UserDetailViewModel>();
            foreach (var user in userList)
            {
                UserDetailViewModel userModel = new UserDetailViewModel();
                userModel.userModel = new UserViewModel();
                userModel.userModel.userName = user.UserName;
                userModel.userModel.email = user.Email;
                // Get user details
                var userDetail = (from detail in contextDb.userDetails
                                  where detail.userName == userModel.userModel.userName
                                  select detail).SingleOrDefault();
                if(userDetail != null)
                {
                    userModel.name = userDetail.name;
                    userModel.ssn = userDetail.ssn;
                }

                // Add user role to model
                try
                {
                    var role = userManager.GetRoles(user.Id).FirstOrDefault();
                    userModel.userModel.roleName = (from roles in contextDb.aspNetRoles
                                                    where roles.Name == role
                                                    select roles.Name).FirstOrDefault();
                }
                catch (Exception e)
                {
                    userModel.userModel.roleName = "Student";
                }
                // Add to user list
                userListModel.Add(userModel);
            }
            return userListModel;
        }

        public UserDetailViewModel getUserByUserName(string userName, ApplicationUserManager userManager)
        {
            // 'userName' athugað í controller, veit að það er valid
            var userEntity = (from user in contextDb.aspNetUsers
                             where user.UserName == userName
                             select user).SingleOrDefault();
            if(userEntity != null)
            {
                UserDetailViewModel user = new UserDetailViewModel();
                user.userModel = new UserViewModel();
                user.userModel.userName = userEntity.UserName;
                user.userModel.email = userEntity.Email;
                // Get role ID, default value is 4 (Student) if role isn't found.
                try
                {
                    var role = userManager.GetRoles(userEntity.Id).FirstOrDefault();
                    user.userModel.roleName = (from roles in contextDb.aspNetRoles
                                     where roles.Name == role
                                     select roles.Name).FirstOrDefault();
                }
                catch(Exception e)
                {
                    user.userModel.roleName = "Student";
                }
                // Get user details
                var userDetail = (from details in contextDb.userDetails
                                  where details.userName == user.userModel.userName
                                  select details).SingleOrDefault();
                if(userDetail != null)
                {
                    user.name = userDetail.name;
                    user.ssn = userDetail.ssn;
                }

                return user;
            }
            return null;
        }

        public List<UserDetailViewModel> getUsersInCourse(int courseId, ApplicationUserManager userManager)
        {
            var userEntityList = (from u in contextDb.aspNetUsers
                                  join s in contextDb.courseStudents on u.UserName equals s.userId
                                  where s.courseId == courseId 
                                  select u).ToList();
            var userDetialEntityList = (from u in contextDb.userDetails
                                        join s in contextDb.courseStudents on u.userId equals s.userId
                                        where s.courseId == courseId
                                        select u).ToList();
            // Breyta entity listum í einn UserDetailViewModel lista
            var userList = new List<UserDetailViewModel>();
            foreach(var user in userEntityList)
            {
                // Breyta í viewmodel og bæta við lista
                var newUser = new UserDetailViewModel();
                // ...
                // ...
                userList.Add(newUser);
            }

            return userList;
        }

        public bool changeUser(UserDetailViewModel newUserInfo, ApplicationUserManager userManager)
        {
            // TODO: Maybe change to use userId. Add userId to UserViewModel
            // Get usera information from userName
            var user = (from users in contextDb.aspNetUsers
                        where users.UserName == newUserInfo.userModel.userName
                        select users).SingleOrDefault();
            var userDetail = (from details in contextDb.userDetails
                              where details.userName == newUserInfo.userModel.userName
                              select details).SingleOrDefault();
            if(user != null)
            {
                user.UserName = newUserInfo.userModel.userName;
                user.Email = newUserInfo.userModel.email;

                // Get current user role
                var role = userManager.GetRoles(user.Id).ToArray();
                // Remove user from roles
                userManager.RemoveFromRoles(user.Id, role);
                // Get name of new role from DB
                var roleName = (from roles in contextDb.aspNetRoles
                                where roles.Name == newUserInfo.userModel.roleName
                                select roles.Name).FirstOrDefault();
                // Add new role to user
                userManager.AddToRole(user.Id, roleName);

                if(userDetail != null)
                {
                    userDetail.userName = newUserInfo.userModel.userName;
                    userDetail.name = newUserInfo.name;
                    userDetail.ssn = newUserInfo.ssn;

                    // Update DB
                    contextDb.SaveChanges();
                    return true;
                }
                else
                    return addUserDetailsToDb(newUserInfo);
            }
            return false;
        }

        public List<AspNetRole> getRoles()
        {
            return (from roles in contextDb.aspNetRoles
                    select roles).ToList();
        }

        public bool deleteUser(UserDetailViewModel toRemoveModel, ApplicationUserManager userManager)
        {
            // Get user from DB
            var user = (from users in contextDb.aspNetUsers
                        where users.UserName == toRemoveModel.userModel.userName
                        select users).SingleOrDefault();
            var userDetail = (from details in contextDb.userDetails
                              where details.userName == toRemoveModel.userModel.userName
                              select details).SingleOrDefault();
            if (user != null)
            {
                // Remove user from all roles
                if (!userManager.GetRoles(user.Id).FirstOrDefault().IsEmpty())
                    userManager.RemoveFromRoles(user.Id, userManager.GetRoles(user.Id).ToArray());
                // Remove user from DB
                contextDb.aspNetUsers.Remove(user);
                if (userDetail != null)
                {
                    contextDb.userDetails.Remove(userDetail);
                }
                contextDb.SaveChanges();
                return true;
            }
            return false;
        }

        public List<UserDetailViewModel> getAllStudents(ApplicationUserManager userManager)
        {
            return getUsersByRole(userManager, "Student");
        }

        public List<UserDetailViewModel> getAllTAs(ApplicationUserManager userManager)
        {
            return getUsersByRole(userManager, "TA");
        }

        public List<UserDetailViewModel> getAllTeachers(ApplicationUserManager userManager)
        {
            return getUsersByRole(userManager, "Teacher");
        }

        public List<UserDetailViewModel> getAllAdmins(ApplicationUserManager userManager)
        {
            return getUsersByRole(userManager, "Admin");
        }

        private List<UserDetailViewModel> getUsersByRole(ApplicationUserManager userManager, string role)
        {
            var users = (from u in contextDb.aspNetUsers
                         select u).ToList();

            var userList = new List<UserDetailViewModel>();
            foreach (var user in users)
            {
                if(userManager.GetRoles(user.Id).Contains(role))
                {
                    var model = new UserDetailViewModel();
                    model.userModel = new UserViewModel();
                    model.userModel.userName = user.UserName;
                    model.userModel.email = user.Email;
                    model.userModel.roleName = userManager.GetRoles(user.Id).FirstOrDefault();

                    var userDetail = (from details in contextDb.userDetails
                                      where details.userName == user.UserName
                                      select details).SingleOrDefault();
                    if(userDetail != null)
                    {
                        model.name = userDetail.name;
                        model.ssn = userDetail.ssn;
                    }

                    userList.Add(model);
                }
            }
            return userList;
        }

        public bool addUserDetailsToDb(UserDetailViewModel userInfo)
        {
            var userEntity = (from u in contextDb.aspNetUsers
                              where u.UserName == userInfo.userModel.userName
                              select u).FirstOrDefault();
            if(userEntity != null)
            {
                UserDetail newUser = new UserDetail();
                newUser.userName = userEntity.UserName;
                newUser.userId = userEntity.Id;
                newUser.name = userInfo.name;
                newUser.ssn = userInfo.ssn;

                contextDb.userDetails.Add(newUser);
                contextDb.SaveChanges();
                return true;
            }
            return false;
        }

        public List<UserDetailViewModel> searchForUser(string searchQuery, ApplicationUserManager userManager)
        {
            var userList = (from users in contextDb.aspNetUsers
                            join u in contextDb.userDetails on users.UserName equals u.userName
                            where users.UserName.Contains(searchQuery) ||
                                  users.Email.Contains(searchQuery) ||
                                  u.name.Contains(searchQuery) ||
                                  u.ssn.Contains(searchQuery)
                            select users).ToList();

            var searchResult = new List<UserDetailViewModel>();
            foreach(var user in userList)
            {
                var uDetail = (from details in contextDb.userDetails
                               where details.userName == user.UserName
                               select details).FirstOrDefault();
                var userModel = new UserDetailViewModel();
                userModel.userModel = new UserViewModel();
                userModel.name = uDetail.name;
                userModel.ssn = uDetail.ssn;
                userModel.userModel.userName = user.UserName;
                userModel.userModel.email = user.Email;
                userModel.userModel.roleName = userManager.GetRoles(user.Id).FirstOrDefault();

                searchResult.Add(userModel);
            }
            return searchResult;
        }
    }
}