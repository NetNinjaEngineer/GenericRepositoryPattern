using ApplyingGenericRepositoryPattern.Dtos;
using ApplyingGenericRepositoryPattern.Entities;
using AutoMapper;

namespace ApplyingGenericRepositoryPattern.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EnrollementDTO, Enrollment>();
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.Department,
                options => options.MapFrom(src => src.Department!.DepartmentName
        ));


        CreateMap<CourseRequestModel, Course>();

    }
}
