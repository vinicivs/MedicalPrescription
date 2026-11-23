using MedicalPrescription.Domain.Prescriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalPrescription.Infrastructure.Persistence.Configurations;

internal sealed class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> builder)
    {
        builder.ToTable("prescriptions");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("id");
        builder.Property(p => p.PatientId).HasColumnName("patient_id").IsRequired();
        builder.Property(p => p.Type).HasColumnName("type").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(p => p.FreeText).HasColumnName("free_text").HasColumnType("text");
        builder.Property(p => p.IssuedAt).HasColumnName("issued_at").IsRequired();
        builder.Property(p => p.UpdatedAt).HasColumnName("updated_at");

        builder.OwnsOne(p => p.Prescriber, prescriber =>
        {
            prescriber.Property(x => x.Name).HasColumnName("prescriber_name").HasMaxLength(150).IsRequired();
            prescriber.Property(x => x.Council).HasColumnName("prescriber_council").HasMaxLength(10).IsRequired();
            prescriber.Property(x => x.CouncilNumber).HasColumnName("prescriber_council_number").HasMaxLength(20).IsRequired();
            prescriber.Property(x => x.CouncilState).HasColumnName("prescriber_council_state").HasMaxLength(2).IsFixedLength().IsRequired();
        });
        builder.Navigation(p => p.Prescriber).IsRequired();

        builder.HasOne(p => p.Patient)
            .WithMany()
            .HasForeignKey(p => p.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Items)
            .WithOne()
            .HasForeignKey(i => i.PrescriptionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(p => p.Items).HasField("_items").UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(p => p.PatientId);
        builder.HasIndex(p => p.IssuedAt);
    }
}
