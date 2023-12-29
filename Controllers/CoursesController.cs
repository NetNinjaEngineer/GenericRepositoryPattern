using ApplyingGenericRepositoryPattern.Dtos;
using ApplyingGenericRepositoryPattern.Entities;
using ApplyingGenericRepositoryPattern.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApplyingGenericRepositoryPattern.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CoursesController(IGenericRepository<Course> courseRepository,
    IGenericRepository<Department> departmentRepository, IMapper mapper) : ControllerBase
{
    private readonly IGenericRepository<Course> _courseRepository = courseRepository;
    private readonly IGenericRepository<Department> _departmentRepository = departmentRepository;
    private readonly IMapper _mapper = mapper;

    [HttpPost("Post")]
    public async Task<IActionResult> PostAsync([FromForm] CourseRequestModel model)
    {
        var departments = await _departmentRepository.GetAllAsync();
        if (!departments.Any(d => d.DepartmentId == model.DepartmentId))
            return BadRequest("Invalid Department Id");

        var course = new Course
        {
            CourseName = model.CourseName,
            CreditHours = model.CreditHours,
            CourseMark = model.CourseMark,
            CourseCode = model.CourseCode,
            DepartmentId = model.DepartmentId,
            PreRequest = model.PreRequest
        };

        await _courseRepository.CreateAsync(course);
        await _courseRepository.SaveChangesAsync();

        return Ok(model);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id, c => c.Department);
        if (course == null)
            return BadRequest($"There is no course with id: {id}");
        return Ok(new CourseDto()
        {
            CourseId = id,
            CourseCode = course.CourseCode,
            CourseName = course.CourseName,
            CourseMark = course.CourseMark,
            CreditHours = course.CreditHours,
            PreRequest = course.PreRequest,
            Department = course.Department.DepartmentName
        });

    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllCoursesAsync()
    {
        var courses = await _courseRepository.GetAllAsync(c => c.Department);
        if (!courses.Any())
            return BadRequest("There is no courses yet.");

        var result = courses.Select(c => new CourseDto()
        {
            CourseCode = c.CourseCode,
            CourseId = c.CourseId,
            CourseName = c.CourseName,
            CourseMark = c.CourseMark,
            CreditHours = c.CreditHours,
            PreRequest = c.PreRequest,
            Department = c.Department.DepartmentName
        });

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourseAsync([FromBody] CourseRequestModel model, int id)
    {
        var validDepartment = _departmentRepository.GetAllAsync().Result
            .Any(d => d.DepartmentId == model.DepartmentId);

        if (!validDepartment)
            return BadRequest("Not Valid Department Id, Try Again !!!");

        var existCourse = await _courseRepository.GetByIdAsync(id);
        if (existCourse == null)
            return BadRequest($"No course founded with id: {id}");

        existCourse.CourseId = id;
        existCourse.PreRequest = model.PreRequest;
        existCourse.CourseMark = model.CourseMark;
        existCourse.CourseCode = model.CourseCode;
        existCourse.CreditHours = model.CreditHours;
        existCourse.DepartmentId = model.DepartmentId;

        var updatedCourse = await _courseRepository.UpdateAsync(existCourse);
        await _courseRepository.SaveChangesAsync();
        return Ok(updatedCourse);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourseAsync(int id)
    {
        var existCourse = await _courseRepository.GetByIdAsync(id);
        if (existCourse is not null)
        {
            var deletedCourse = await _courseRepository.DeleteAsync(existCourse);
            var mappedCourse = _mapper.Map<MappingCourse>(deletedCourse);
            return Ok(mappedCourse);
        }

        return BadRequest($"There is no course with id '{id}' to delete !!!");
    }

}
