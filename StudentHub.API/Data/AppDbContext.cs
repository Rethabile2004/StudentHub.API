using Microsoft.EntityFrameworkCore;
using StudentHub.API.Models;

namespace StudentHub.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<LecturerCourse> LecturerCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //  DEPARTMENT CONFIGURATION 
            modelBuilder.Entity<Department>(entity =>
            {
                // Primary key
                entity.HasKey(d => d.Id);

                // Column constraints
                entity.Property(d => d.Name).IsRequired().HasMaxLength(200);
                entity.Property(d => d.Code).IsRequired().HasMaxLength(50);

                // Unique index on Code so no two departments have the same code
                entity.HasIndex(d => d.Code).IsUnique();
            });

            //  LECTURER CONFIGURATION 
            modelBuilder.Entity<Lecturer>(entity =>
            {
                // Primary key
                entity.HasKey(l => l.Id);

                // Column constraints
                entity.Property(l => l.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(l => l.LastName).IsRequired().HasMaxLength(100);
                entity.Property(l => l.Email).IsRequired().HasMaxLength(256);

                // Unique email across all lecturers
                entity.HasIndex(l => l.Email).IsUnique();

                // Relationship: One Department has many Lecturers
                // A lecturer must belong to exactly one department
                // If department is deleted, lecturers stay (Restrict prevents orphaning)
                entity.HasOne(l => l.Department)
                    .WithMany(d => d.Lecturers)
                    .HasForeignKey(l => l.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //  COURSE CONFIGURATION 
            modelBuilder.Entity<Course>(entity =>
            {
                // Primary key
                entity.HasKey(c => c.Id);

                // Column constraints
                entity.Property(c => c.Code).IsRequired().HasMaxLength(50);
                entity.Property(c => c.Title).IsRequired().HasMaxLength(200);
                entity.Property(c => c.Credits).IsRequired();

                // Unique course code per institution
                entity.HasIndex(c => c.Code).IsUnique();

                // Relationship: One Department offers many Courses
                // A course must belong to exactly one department
                // If department is deleted, courses stay (Restrict prevents orphaning)
                entity.HasOne(c => c.Department)
                    .WithMany(d => d.Courses)
                    .HasForeignKey(c => c.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //  STUDENT CONFIGURATION 
            modelBuilder.Entity<Student>(entity =>
            {
                // Primary key
                entity.HasKey(s => s.Id);

                // Column constraints
                entity.Property(s => s.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(s => s.LastName).IsRequired().HasMaxLength(100);
                entity.Property(s => s.StudentNumber).IsRequired().HasMaxLength(20);
                entity.Property(s => s.Email).IsRequired().HasMaxLength(256);

                // Unique student number and email
                entity.HasIndex(s => s.StudentNumber).IsUnique();
                entity.HasIndex(s => s.Email).IsUnique();

                // No foreign key on Student - independent entity
            });

            //  ENROLLMENT CONFIGURATION 
            modelBuilder.Entity<Enrollment>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.Id);

                // Column constraints
                entity.Property(e => e.EnrollmentDate).IsRequired();

                // Relationship: One Student has many Enrollments
                // If student is deleted, their enrollments are deleted too (Cascade)
                entity.HasOne(e => e.Student)
                    .WithMany(s => s.Enrollments)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relationship: One Course has many Enrollments
                // If course is deleted, all enrollments for that course are deleted (Cascade)
                entity.HasOne(e => e.Course)
                    .WithMany(c => c.Enrollments)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Unique constraint: A student can enroll in a course only once
                entity.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique();
            });

            //  LECTURER COURSE CONFIGURATION (Many-to-Many) 
            modelBuilder.Entity<LecturerCourse>(entity =>
            {
                // Primary key
                entity.HasKey(lc => lc.Id);

                // Relationship: One Lecturer teaches many Courses
                // If lecturer is deleted, their course assignments are deleted (Cascade)
                entity.HasOne(lc => lc.Lecturer)
                    .WithMany(l => l.LecturerCourses)
                    .HasForeignKey(lc => lc.LecturerId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relationship: One Course has many Lecturers
                // If course is deleted, all lecturer assignments are deleted (Cascade)
                entity.HasOne(lc => lc.Course)
                    .WithMany(c => c.LecturerCourses)
                    .HasForeignKey(lc => lc.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Unique constraint: A lecturer teaches a course only once
                entity.HasIndex(lc => new { lc.LecturerId, lc.CourseId }).IsUnique();
            });
        }
    }
}