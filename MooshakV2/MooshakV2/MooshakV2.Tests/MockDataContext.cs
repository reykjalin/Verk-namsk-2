using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using system.Data.Entity;
using MooshakV2.Models;
using MooshakV2.Models.entities;

namespace MooshakV2.Tests
{
    class MockDataContext : DatabaseDataContext
    {
        /// <summary>
        /// Sets up the fake database.
        /// </summary>
        public MockDataContext()
        {
            // We're setting our DbSets to be InMemoryDbSets rather than using SQL Server.
            Connections = new InMemoryDbSet<Connection>();
        }

        public IDbSet<Connection> FriendConnections { get; set; }
        // TODO: bætið við fleiri færslum hér
        // eftir því sem þeim fjölgar í AppDataContext klasanum ykkar!

        public int SaveChanges()
        {
            // Pretend that each entity gets a database id when we hit save.
            int changes = 0;

            return changes;
        }
        public void Dispose()
        {
            // Do nothing!
        }
    }
}
