/*using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MooshakV2.Services;
using MooshakV2.Tests;
using MooshakV2.Models.entities;

namespace MooshakV2.Tests.Services
{
    [TestClass]
    public class UserServiceTest
    {
        private UserService _service;
        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext
            _service = new UserService();

        }
        [TestMethod]
        public void TestGetAllUsers()
        {
            //Arrange:
            const string user = "Admin";
            var service = new UserService();

            //Act:
            var result = _service.getAllUsers(user);

            //Assert:
            Assert.IsNotNull(user);

        }
    }
}*/
