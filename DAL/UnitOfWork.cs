using ApplyingGenericRepositoryPattern.Data;
using ApplyingGenericRepositoryPattern.Entities;
using ApplyingGenericRepositoryPattern.Repository;
using ApplyingGenericRepositoryPattern.Repository.Implementation;

namespace ApplyingGenericRepositoryPattern.DAL;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;
    private IGenericRepository<Student>? _studentRepository;
    private IGenericRepository<Course>? _courseRepository;
    private IGenericRepository<Department>? _departmentRepository;
    private IGenericRepository<Enrollment>? _enrollmentRepository;
    private bool _transactionStarted = false;

    public IGenericRepository<Student> StudentRepository => _studentRepository
        ??= new GenericRepository<Student>(_context);

    public IGenericRepository<Course> CourseRepository => _courseRepository
        ??= new GenericRepository<Course>(_context);

    public IGenericRepository<Enrollment> EnrollmentRepository => _enrollmentRepository
        ??= new GenericRepository<Enrollment>(_context);

    public IGenericRepository<Department> DepartmentRepository => _departmentRepository
        ??= new GenericRepository<Department>(_context);

    public async Task CommitTransaction()
    {
        if (_transactionStarted)
        {
            await _context.Database.CommitTransactionAsync();
            _transactionStarted = false;
        }
    }

    public async Task CreateTransaction()
    {
        await _context.Database.BeginTransactionAsync();
        _transactionStarted = true;
    }

    public async Task RollbackTransaction()
    {
        if (_transactionStarted)
        {
            await _context.Database.RollbackTransactionAsync();
            _transactionStarted = false;
        }

    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}
