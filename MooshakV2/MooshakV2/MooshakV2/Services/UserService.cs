using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MooshakV2.DAL;
using MooshakV2.ViewModels;

namespace MooshakV2.Services
{
    public class UserService
    {
        private DatabaseDataContext contextDb;

        public UserService() { contextDb = new DatabaseDataContext(); }

        public List<UserViewModel> getAllUsers()
        {
            var userList = (from user in contextDb.aspNetUsers
                            select user).ToList();

            List<UserViewModel> userListModel = new List<UserViewModel>();
            foreach (var user in userList)
            {
                UserViewModel userModel = new UserViewModel();
                userModel.userName = user.UserName;
                userModel.email = user.Email;
                userListModel.Add(userModel);
            }
            return userListModel;
        }
    }
}