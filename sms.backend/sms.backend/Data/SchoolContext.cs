using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sms.backend.Models;

namespace sms.backend.Data
{
    public class SchoolContext : IdentityDbContext<ApplicationUser>
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Staff?> Staff { get; set; }
        public DbSet<Class?> Classes { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<TeacherEnrollment> TeacherEnrollments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure the base method is called

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.UserId, r.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            modelBuilder.Entity<Enrollment>()
                .HasKey(e => new { e.StudentId, e.ClassId });
            modelBuilder.Entity<TeacherEnrollment>()
                .HasKey(e => new { e.StaffId, e.ClassId });
            modelBuilder.Entity<Mark>()
                .Property(m => m.Value)
                .HasColumnType("decimal(18,2)");
            // Define other relationships and keys as needed
        }
    }
}