using MedicalPrescription.Domain.Medications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalPrescription.Infrastructure.Persistence.Configurations;

internal sealed class MedicationConfiguration : IEntityTypeConfiguration<Medication>
{
    public void Configure(EntityTypeBuilder<Medication> builder)
    {
        builder.ToTable("medications");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id).HasColumnName("id");
        builder.Property(m => m.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
        builder.Property(m => m.ActiveIngredient).HasColumnName("active_ingredient").HasMaxLength(150).IsRequired();
        builder.Property(m => m.PharmaceuticalForm).HasColumnName("pharmaceutical_form").HasMaxLength(50).IsRequired();
        builder.Property(m => m.Concentration).HasColumnName("concentration").HasMaxLength(50);

        builder.Ignore(m => m.DisplayName);
    }
}
