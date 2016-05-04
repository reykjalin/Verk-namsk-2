namespace Mooshak2_FirstTest_KR.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DatabaseDataContext : DbContext
    {
        public DatabaseDataContext()
            : base("name=DatabaseDataContext")
        {
        }

        public virtual DbSet<Assignment> assignments { get; set; }
        public virtual DbSet<AssignmentPart> assignmentParts { get; set; }
        public virtual DbSet<Course> courses { get; set; }
        public virtual DbSet<CourseStudent> courseStudents { get; set; }
        public virtual DbSet<CourseTA> courseTAs { get; set; }
        public virtual DbSet<CourseTeacher> courseTeachers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<Assignment>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Assignment>()
                .HasMany(e => e.AssignmentParts)
                .WithRequired(e => e.Assignment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AssignmentPart>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Assignments)
                .WithRequired(e => e.Course)
                .WillCascadeOnDelete(false);
        }
    }
}
