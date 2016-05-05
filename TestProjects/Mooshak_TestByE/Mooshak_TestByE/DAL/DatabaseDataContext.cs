namespace Mooshak_TestByE.DAL
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

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Assignment> Assignments { get; set; }
        public virtual DbSet<AssignmentPart> AssignmentParts { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseStudent> CourseStudents { get; set; }
        public virtual DbSet<CourseTA> CourseTAs { get; set; }
        public virtual DbSet<CourseTeacher> CourseTeachers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

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
