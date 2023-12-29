using ApplyingGenericRepositoryPattern.Dtos;
using ApplyingGenericRepositoryPattern.Entities;
using AutoMapper;

namespace ApplyingGenericRepositoryPattern.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Student, StudentDto>();
        CreateMap<Course, CourseDto>();
        CreateMap<Course, MappingCourse>();
        CreateMap<Department, DepartmentDto>();
        CreateMap<DepartmentRequestModel, Department>();
    }
}
