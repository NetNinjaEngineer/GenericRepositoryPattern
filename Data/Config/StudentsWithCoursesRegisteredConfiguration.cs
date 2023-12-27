using ApplyingGenericRepositoryPattern.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplyingGenericRepositoryPattern.Data.Config;

public class StudentsWithCoursesRegisteredConfiguration :
    IEntityTypeConfiguration<ShowStudentsWithCoursesRegisteredModel>
{
    public void Configure(EntityTypeBuilder<ShowStudentsWithCoursesRegisteredModel> builder)
    {
        builder.HasNoKey();
        builder.ToView("ShowStudentsWithCoursesRegistered");
    }
}

