using ApplyingGenericRepositoryPattern.DTO;
using ApplyingGenericRepositoryPattern.Entities;
using ApplyingGenericRepositoryPattern.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApplyingGenericRepositoryPattern.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly IGenericRepository<Department> _departmentRepository;
    private readonly IMapper _mapper;

    public DepartmentsController(IGenericRepository<Department> departmentRepository, IMapper mapper)
    {
        _departmentRepository = departmentRepository;
        _mapper = mapper;
    }

    [HttpGet("GetAllDepartments")]
    public async Task<IActionResult> GetAllDepartmentsAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        if (departments == null)
            return NotFound("There is no departments yet.");
        IEnumerable<DepartmentDto> mappedDepartments = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
        return Ok(mappedDepartments);
    }

    [HttpGet("GetDepartment")]
    public async Task<IActionResult> GetDepartmentAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department is null)
            return BadRequest($"No department founded with id '{id}'");
        DepartmentDto mappedDepartment = _mapper.Map<DepartmentDto>(department);
        return Ok(mappedDepartment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDeparmentAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department is null)
            return BadRequest($"No department founded with id '{id}'");

        var deletedDepartment = await _departmentRepository.DeleteAsync(department);
        DepartmentDto mappedDepartment = _mapper.Map<DepartmentDto>(deletedDepartment);
        return Ok(mappedDepartment);
    }

    [HttpPost("Post")]
    public async Task<IActionResult> PostDepartmentAsync([FromBody] DepartmentRequestModel request)
    {
        var mappedDepartment = _mapper.Map<Department>(request);
        await _departmentRepository.CreateAsync(mappedDepartment);
        await _departmentRepository.SaveChangesAsync();
        return Ok(mappedDepartment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartmentAsync(int id, DepartmentRequestModel model)
    {
        var existDepartment = await _departmentRepository.GetByIdAsync(id);
        if (existDepartment == null)
            return BadRequest($"No departnent founded with id: {id}");

        existDepartment.DepartmentId = id;
        existDepartment.DepartmentName = model.DepartmentName;
        existDepartment.DepartmentAbbreviation = model.DepartmentAbbreviation;

        var updatedDepartment = await _departmentRepository.UpdateAsync(existDepartment);

        var mappedDepartment = _mapper.Map<DepartmentDto>(updatedDepartment);

        return Ok(mappedDepartment);
    }

}
