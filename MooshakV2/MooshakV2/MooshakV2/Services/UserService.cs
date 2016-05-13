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

        /// <summary>
        /// Constructor. Initializes the database connection via contextDb <DatabaseDataContext>
        /// </summary>
        public UserService() { contextDb = new DatabaseDataContext(); }

        /// <summary>
        /// Returns all users in database.
        /// </summary>
        /// <param name="userManager"></param>
        /// <returns>All users in database</returns>
        public List<UserDetailViewModel> getAllUsers(ApplicationUserManager userManager)
        {
            // Get all users from DB
            var userList = (from user in contextDb.aspNetUsers
                            join u in contextDb.userDetails on user.UserName equals u.userName
                            orderby u.name
                            select user).ToList();

            // Move from entities to view models
            List<UserDetailViewModel> userListModel = new List<UserDetailViewModel>();
            foreach (var user in userList)
            {
                UserDetailViewModel userModel = new UserDetailViewModel();
                userModel.userModel = userEntityToModel(user, userManager);

                // Get detailed user information
                var userDetail = (from detail in contextDb.userDetails
                                  where detail.userName == userModel.userModel.userName
                                  select detail).SingleOrDefault();
                // Make sure details exist before moving information
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
                    // If user has no role, set role as Student
                    userModel.userModel.roleName = "Student";
                    userManager.AddToRole(user.Id, "Student");
                }
                // Add model to user list
                userListModel.Add(userModel);
            }
            return userListModel;
        }

        /// <summary>
        /// Find user in database using a user name
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userManager"></param>
        /// <returns>UserDetialViewModel containing user information if user exists, otherwise returns null</returns>
        public UserDetailViewModel getUserByUserName(string userName, ApplicationUserManager userManager)
        {
            // 'userName' checked in controller so we know it's valid
            var userEntity = (from user in contextDb.aspNetUsers
                             where user.UserName == userName
                             select user).SingleOrDefault();
            // Make sure a user is found
            if(userEntity != null)
            {
                // Create model for user entity
                UserDetailViewModel user = new UserDetailViewModel();
                user.userModel = userEntityToModel(userEntity, userManager);
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
                    // If user has no role, set it to "Student"
                    user.userModel.roleName = "Student";
                    userManager.AddToRole(userEntity.Id, "Student");
                }

                // Get user details and add to model if the details exist
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

        /// <summary>
        /// Get all users in course with ID = 'courseId'
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userManager"></param>
        /// <returns>A list of users in course with ID = 'courseId', null if course doesn't exist</returns>
        public List<UserDetailViewModel> getUsersInCourse(int courseId, ApplicationUserManager userManager)
        {
            // Get user information and details
            var userEntityList = (from u in contextDb.aspNetUsers
                                  join s in contextDb.courseStudents on u.Id equals s.userId
                                  where s.courseId == courseId 
                                  select u).ToList();
            var userDetailEntityList = (from u in contextDb.userDetails
                                        join s in contextDb.courseStudents on u.userId equals s.userId
                                        where s.courseId == courseId
                                        select u).ToList();
            // Something went wrong if lists aren't the same size, the lists should be of 1:1 ratio
            if(userEntityList.Count != userDetailEntityList.Count)
                return null;
            
            // Change from user entity lists to a list containing UserDetialViewModels
            var userList = new List<UserDetailViewModel>();
            for(int i = 0; i < userEntityList.Count; i++)
            {
                // Change to view model and add to list
                var newUser = new UserDetailViewModel();
                newUser.name = userDetailEntityList[i].name;
                newUser.ssn = userDetailEntityList[i].ssn;
                newUser.userModel = userEntityToModel(userEntityList[i], userManager);
                userList.Add(newUser);
            }

            return userList;
        }

        /// <summary>
        /// Converts a user entity to a user view model, AspNetUser --> UserViewModel
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userManager"></param>
        /// <returns>Model representation of user 'user'</returns>
        private UserViewModel userEntityToModel(AspNetUser user, ApplicationUserManager userManager)
        {
            var model = new UserViewModel();
            model.userName = user.UserName;
            model.email = user.Email;
            model.roleName = userManager.GetRoles(user.Id).SingleOrDefault();
            return model;
        }

        /// <summary>
        /// Change user information in database using information from 'newUserInfo'
        /// </summary>
        /// <param name="newUserInfo"></param>
        /// <param name="userManager"></param>
        /// <returns>true if update succeded, false otherwise</returns>
        public bool changeUser(UserDetailViewModel newUserInfo, ApplicationUserManager userManager)
        {
            // Get usera information from userName
            var user = (from users in contextDb.aspNetUsers
                        where users.UserName == newUserInfo.userModel.userName
                        select users).SingleOrDefault();
            var userDetail = (from details in contextDb.userDetails
                              where details.userName == newUserInfo.userModel.userName
                              select details).SingleOrDefault();
            if(user != null)
            {
                // Add basic info from model
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

                // If user doesn't exist in UserDetail table add user details to database
                if(userDetail != null)
                {
                    // Add basic info
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

        /// <summary>
        /// Get all roles currently stored in the database
        /// </summary>
        /// <returns>List of available roles</returns>
        public List<AspNetRole> getRoles()
        {
            return (from roles in contextDb.aspNetRoles
                    select roles).ToList();
        }

        /// <summary>
        /// Deletes user with information corresponding to information in 'toRemoveModel'
        /// </summary>
        /// <param name="toRemoveModel"></param>
        /// <param name="userManager"></param>
        /// <returns>true if succeded, false otherwise</returns>
        public bool deleteUser(UserDetailViewModel toRemoveModel, ApplicationUserManager userManager)
        {
            // Get user info from DB
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

        /// <summary>
        /// Get a list of all students.
        /// </summary>
        /// <param name="userManager"></param>
        /// <returns>List of all students in database</returns>
        public List<UserDetailViewModel> getAllStudents(ApplicationUserManager userManager)
        {
            return getUsersByRole(userManager, "Student");
        }

        /// <summary>
        /// Get all teaching assistants.
        /// </summary>
        /// <param name="userManager"></param>
        /// <returns>List of all teaching assistants in database</returns>
        public List<UserDetailViewModel> getAllTAs(ApplicationUserManager userManager)
        {
            return getUsersByRole(userManager, "TA");
        }

        /// <summary>
        /// Get all teachers.
        /// </summary>
        /// <param name="userManager"></param>
        /// <returns>List of all teachers in database</returns>
        public List<UserDetailViewModel> getAllTeachers(ApplicationUserManager userManager)
        {
            return getUsersByRole(userManager, "Teacher");
        }

        /// <summary>
        /// Get all administrators.
        /// </summary>
        /// <param name="userManager"></param>
        /// <returns>List of all administrators in database</returns>
        public List<UserDetailViewModel> getAllAdmins(ApplicationUserManager userManager)
        {
            return getUsersByRole(userManager, "Admin");
        }

        /// <summary>
        /// Get all users in a certain role specified by 'role'. Used by functions that get users by a certain role,
        /// e.g. getAllStudents()
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="role"></param>
        /// <returns>List of all users in a certain role</returns>
        private List<UserDetailViewModel> getUsersByRole(ApplicationUserManager userManager, string role)
        {
            // Get all users
            var users = (from u in contextDb.aspNetUsers
                         select u).ToList();
            // Convert from entities to view models
            var userList = new List<UserDetailViewModel>();
            foreach (var user in users)
            {
                // Only add users in role 'role' to the list
                if(userManager.GetRoles(user.Id).Contains(role))
                {
                    // Create the model
                    var model = new UserDetailViewModel();
                    model.userModel = userEntityToModel(user, userManager);

                    // Get user details and add to user info if those details exist
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

        /// <summary>
        /// Add user details to database using information from 'userInfo'
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns>true if update succeeds, false otherwise</returns>
        public bool addUserDetailsToDb(UserDetailViewModel userInfo)
        {
            // Make sure user exsists in the database, if not don't add details to DB
            var userEntity = (from u in contextDb.aspNetUsers
                              where u.UserName == userInfo.userModel.userName
                              select u).FirstOrDefault();
            if(userEntity != null)
            {
                // Create entity for DB
                UserDetail newUser = new UserDetail();
                newUser.userName = userEntity.UserName;
                newUser.userId = userEntity.Id;
                newUser.name = userInfo.name;
                newUser.ssn = userInfo.ssn;

                // Update DB
                contextDb.userDetails.Add(newUser);
                contextDb.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Search for user using pattern found in 'searchQuery'
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="userManager"></param>
        /// <returns>List of users that have information that fits 'searchQuery'</returns>
        public List<UserDetailViewModel> searchForUser(string searchQuery, ApplicationUserManager userManager)
        {
            // Get user entities that contain information found in 'searchQuery'
            var userList = (from users in contextDb.aspNetUsers
                            join u in contextDb.userDetails on users.UserName equals u.userName
                            where users.UserName.Contains(searchQuery) ||
                                  users.Email.Contains(searchQuery) ||
                                  u.name.Contains(searchQuery) ||
                                  u.ssn.Contains(searchQuery)
                            select users).ToList();

            // Move entities to list
            var searchResult = new List<UserDetailViewModel>();
            foreach(var user in userList)
            {
                // Get user details
                var uDetail = (from details in contextDb.userDetails
                               where details.userName == user.UserName
                               select details).FirstOrDefault();
                // Create model
                var userModel = new UserDetailViewModel();
                userModel.userModel = userEntityToModel(user, userManager);
                userModel.name = uDetail.name;
                userModel.ssn = uDetail.ssn;
                
                // Add model to list
                searchResult.Add(userModel);
            }
            return searchResult;
        }
    }
}