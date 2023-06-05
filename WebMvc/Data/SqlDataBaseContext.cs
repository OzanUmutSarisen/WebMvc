using Microsoft.EntityFrameworkCore;
using WebMvc.Data.Mapping;
using WebMvc.Models.Domain;

namespace WebMvc.Data
{
    public class SqlDataBaseContext : DbContext
    {
        public SqlDataBaseContext(DbContextOptions option) : base(option)
        {

        }
        public virtual DbSet<Faculties> Faculties { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Teachers> Teachers { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Videos> Videos { get; set; }
        public virtual DbSet<Questions> Questions { get; set; }
        public virtual DbSet<Answers> Answers { get; set; }
        public virtual DbSet<Accesses> Accesses { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<CourseStudent> CoursesStudents { get; set; }
        //public virtual DbSet<AccessesTeachers> AccessesTeachers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CourseStudentMapping());
        }
    }
}
