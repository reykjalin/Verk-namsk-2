﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MooshakV2.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public interface IAppDataContext
    {
        IDbSet<FriendConnection> FriendConnections { get; set;  }
        /// <summary>
        /// svona liek meirar heeer
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IAppDataContext
    {
        public IDbSet<FriendConnection> FriendConnections { get; set; }
        /// <summary>
        /// asdasd meira
        /// </summary>
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}