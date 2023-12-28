using ApplyingGenericRepositoryPattern.Dtos;
using ApplyingGenericRepositoryPattern.Entities;
using ApplyingGenericRepositoryPattern.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApplyingGenericRepositoryPattern.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CoursesController(IGenericRepository<Course> courseRepository,
    IMapper mapper, IGenericRepository<Department> departmentRepository) : ControllerBase
{
    private readonly IGenericRepository<Course> _courseRepository = courseRepository;
    private readonly IGenericRepository<Department> _departmentRepository = departmentRepository;
    private readonly IMapper _mapper = mapper;

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetCoursesAsync()
    {
        var courses = await _courseRepository.GetAllAsync(x => x.Department!);
        if (!courses.Any())
            return NotFound("There is no courses yet.");
        IEnumerable<CourseDto> result = _mapper.Map<IEnumerable<CourseDto>>(courses);

        return Ok(result);

    }

    [HttpPost("Post")]
    public async Task<IActionResult> PostAsync([FromBody] CourseRequestModel model)
    {
        var departments = await _departmentRepository.GetAllAsync();
        if (!departments.Any(x => x.DepartmentId == model.DepartmentId))
            return BadRequest("Invalid Department Id");

        var course = _mapper.Map<Course>(model);

        var createdCourse = await _courseRepository.Create(course);

        return Ok(createdCourse);

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseByIdAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id, x => x.Department!);

        if (course is null)
            return NotFound("There is no course with id: {id}");

        var courseDto = _mapper.Map<CourseDto>(course);

        return Ok(courseDto);
    }

}
