using ApplyingGenericRepositoryPattern.Entities;
using ApplyingGenericRepositoryPattern.Repository;

namespace ApplyingGenericRepositoryPattern.DAL;

public interface IUnitOfWork
{
    IGenericRepository<Student> StudentRepository { get; }
    IGenericRepository<Course> CourseRepository { get; }
    IGenericRepository<Enrollment> EnrollmentRepository { get; }
    IGenericRepository<Department> DepartmentRepository { get; }
    Task CreateTransaction();
    Task CommitTransaction();
    Task RollbackTransaction();
    Task Save();
}
