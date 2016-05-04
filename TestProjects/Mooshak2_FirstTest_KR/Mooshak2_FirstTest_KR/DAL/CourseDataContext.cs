namespace Mooshak2_FirstTest_KR.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CourseDataContext : DbContext
    {
        public CourseDataContext()
            : base("name=CourseDataContext")
        {
        }

        public virtual DbSet<Course> courses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.description)
                .IsUnicode(false);
        }
    }
}
