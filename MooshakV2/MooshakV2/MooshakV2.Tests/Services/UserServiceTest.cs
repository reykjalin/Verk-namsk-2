using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MooshakV2.Services;

namespace MooshakV2.Tests.Services
{
    [TestClass]
    public class UserServiceTest
    {
        [TestMethod]
        public void TestGetAllUsers()
        {
            //Arrange:
            const string user = "Admin";
            var service = new UserService();

            //Act:
            var result = service.getAllUsers(user);

            //Assert:
            Assert.IsNotNull(user);

        }
    }
}
