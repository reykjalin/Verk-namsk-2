namespace MooshakV2.DAL
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

        public virtual DbSet<C__MigrationHistory> c__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> aspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> aspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> aspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> aspNetUsers { get; set; }
        public virtual DbSet<Assignment> assignments { get; set; }
        public virtual DbSet<AssignmentPart> assignmentParts { get; set; }
        public virtual DbSet<Course> courses { get; set; }
        public virtual DbSet<Submission> submissions { get; set; }
        public virtual DbSet<Table> tables { get; set; }
        public virtual DbSet<UserDetail> userDetails { get; set; }
        public virtual DbSet<CourseStudent> courseStudents { get; set; }
        public virtual DbSet<CourseTA> courseTAs { get; set; }
        public virtual DbSet<CourseTeacher> courseTeachers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Submissions)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.userId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasOptional(e => e.UserDetail)
                .WithRequired(e => e.AspNetUser);

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

            modelBuilder.Entity<Assignment>()
                .HasMany(e => e.Submissions)
                .WithRequired(e => e.Assignment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AssignmentPart>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<AssignmentPart>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<AssignmentPart>()
                .HasMany(e => e.Submissions)
                .WithRequired(e => e.AssignmentPart)
                .HasForeignKey(e => e.partId)
                .WillCascadeOnDelete(false);

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

            modelBuilder.Entity<UserDetail>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<UserDetail>()
                .Property(e => e.ssn)
                .IsUnicode(false);
        }
    }
}
