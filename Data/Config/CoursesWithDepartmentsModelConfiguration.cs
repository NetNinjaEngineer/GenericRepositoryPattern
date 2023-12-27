using ApplyingGenericRepositoryPattern.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplyingGenericRepositoryPattern.Data.Config;

public class CoursesWithDepartmentsModelConfiguration :
    IEntityTypeConfiguration<CoursesWithDepartmentsModel>
{
    public void Configure(EntityTypeBuilder<CoursesWithDepartmentsModel> builder)
    {
        builder.HasNoKey();
        builder.ToView("ShowCoursesWithDepartments");
    }
}
