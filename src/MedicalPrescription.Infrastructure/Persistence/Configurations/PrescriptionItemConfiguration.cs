using MedicalPrescription.Domain.Medications;
using MedicalPrescription.Domain.Prescriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalPrescription.Infrastructure.Persistence.Configurations;

internal sealed class PrescriptionItemConfiguration : IEntityTypeConfiguration<PrescriptionItem>
{
    public void Configure(EntityTypeBuilder<PrescriptionItem> builder)
    {
        builder.ToTable("prescription_items");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id).HasColumnName("id");
        builder.Property(i => i.PrescriptionId).HasColumnName("prescription_id").IsRequired();
        builder.Property(i => i.MedicationId).HasColumnName("medication_id");
        builder.Property(i => i.MedicationName).HasColumnName("medication_name").HasMaxLength(200).IsRequired();
        builder.Property(i => i.PharmaceuticalForm).HasColumnName("pharmaceutical_form").HasMaxLength(50).IsRequired();
        builder.Property(i => i.Dose).HasColumnName("dose").HasPrecision(10, 3).IsRequired();
        builder.Property(i => i.DoseUnit).HasColumnName("dose_unit").HasMaxLength(20).IsRequired();
        builder.Property(i => i.Frequency).HasColumnName("frequency").HasMaxLength(60).IsRequired();
        builder.Property(i => i.StartDate).HasColumnName("start_date").IsRequired();
        builder.Property(i => i.DurationDays).HasColumnName("duration_days");
        builder.Property(i => i.ContinuousUse).HasColumnName("continuous_use").IsRequired();
        builder.Property(i => i.AdministrationRoute).HasColumnName("administration_route").HasMaxLength(40).IsRequired();
        builder.Property(i => i.UsageDeadline).HasColumnName("usage_deadline");
        builder.Property(i => i.Notes).HasColumnName("notes").HasMaxLength(1000);

        builder.Ignore(i => i.IsCatalogued);

        builder.HasOne<Medication>()
            .WithMany()
            .HasForeignKey(i => i.MedicationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
