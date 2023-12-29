using ApplyingGenericRepositoryPattern.Entities;
using ApplyingGenericRepositoryPattern.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ApplyingGenericRepositoryPattern.Data;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public ApplicationDbContext() { }

    public DbSet<Department> Departments { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CoursesWithPreRequestsModel> CoursesWithPreRequests { get; set; }
    public DbSet<ShowStudentsWithCoursesRegisteredModel> StudentsWithCoursesRegistered { get; set; }
    public DbSet<CoursesWithDepartmentsAndPreRequestsModel> CoursesWithDepartmentsAndPreRequests { get; set; }
    public DbSet<CoursesWithDepartmentsModel> CoursesWithDepartments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

    }

}
