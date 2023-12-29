using ApplyingGenericRepositoryPattern.Dtos;
using ApplyingGenericRepositoryPattern.Entities;
using ApplyingGenericRepositoryPattern.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApplyingGenericRepositoryPattern.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StudentsController(IGenericRepository<Student> studentRepository, IMapper mapper) : ControllerBase
{
    private readonly IGenericRepository<Student> _studentRepository = studentRepository;
    private readonly IMapper _mapper = mapper;

    [HttpPost("Post")]
    public async Task<IActionResult> PostAsync([FromForm] StudentRequestModel model)
    {
        var student = _mapper.Map<Student>(model);

        await _studentRepository.CreateAsync(student);
        await _studentRepository.SaveChangesAsync();

        return Ok(model);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentAsync(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null)
            return BadRequest($"There is no student with id: {id}");
        return Ok(_mapper.Map<StudentDto>(student));

    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllStudentsAsync()
    {
        var students = await _studentRepository.GetAllAsync();
        if (!students.Any())
            return BadRequest("There is no students yet.");

        IEnumerable<StudentDto> result = _mapper.Map<IEnumerable<StudentDto>>(students);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudentAsync([FromBody] StudentRequestModel model, int id)
    {
        var existStudent = await _studentRepository.GetByIdAsync(id);
        if (existStudent == null)
            return BadRequest($"No student founded with id: {id}");

        existStudent.StudentId = id;
        existStudent.FirstName = model.FirstName;
        existStudent.LastName = model.LastName;
        existStudent.Email = model.Email;
        existStudent.Phone = model.Phone;

        var updatedStudent = await _studentRepository.UpdateAsync(existStudent);

        var mappedStudent = _mapper.Map<StudentDto>(updatedStudent);

        return Ok(mappedStudent);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudentAsync(int id)
    {
        var existStudent = await _studentRepository.GetByIdAsync(id);
        if (existStudent is not null)
        {
            var deletedStudent = await _studentRepository.DeleteAsync(existStudent);
            var mappedStudent = _mapper.Map<StudentDto>(deletedStudent);
            return Ok(mappedStudent);
        }

        return BadRequest($"There is no student with id '{id}' to delete !!!");
    }

}
