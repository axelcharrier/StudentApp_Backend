namespace StudentApp.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentApp.Domain.Entities;

/// <summary>
/// Provides configuration for the Student entity type
/// </summary>
/// <remarks>This class is used to configure entity properties and constraints for the Student entity </remarks>
internal sealed class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    /// <summary>
    /// Configures the entity type for the Student model with property constraints.
    /// </summary>
    /// <remarks>Sets the maximum length to 100 characters </remarks>
    /// <param name="builder">The builder used to configure the Student entity type.</param>
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.Property(r => r.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(r => r.LastName).HasMaxLength(100).IsRequired();
    }
}
