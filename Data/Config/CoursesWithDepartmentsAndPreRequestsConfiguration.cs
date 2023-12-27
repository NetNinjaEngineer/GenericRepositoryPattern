using ApplyingGenericRepositoryPattern.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplyingGenericRepositoryPattern.Data.Config;

public class CoursesWithDepartmentsAndPreRequestsConfiguration :
    IEntityTypeConfiguration<CoursesWithDepartmentsAndPreRequestsModel>
{
    public void Configure(EntityTypeBuilder<CoursesWithDepartmentsAndPreRequestsModel> builder)
    {
        builder.HasNoKey();
        builder.ToView("ShowCoursesWithDepartmentsAndPreRequests");
    }
}
