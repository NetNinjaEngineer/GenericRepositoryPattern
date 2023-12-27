using ApplyingGenericRepositoryPattern.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplyingGenericRepositoryPattern.Data.Config;

public class CoursesWithPreRequestsModelConfiguration :
    IEntityTypeConfiguration<CoursesWithPreRequestsModel>
{
    public void Configure(EntityTypeBuilder<CoursesWithPreRequestsModel> builder)
    {
        builder.HasNoKey();
        builder.ToView("CoursesWithPreRequests");
    }
}

