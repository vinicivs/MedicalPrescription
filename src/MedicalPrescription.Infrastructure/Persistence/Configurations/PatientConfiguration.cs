using MedicalPrescription.Domain.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalPrescription.Infrastructure.Persistence.Configurations;

internal sealed class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("patients");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("id");
        builder.Property(p => p.FullName).HasColumnName("full_name").HasMaxLength(150).IsRequired();
        builder.Property(p => p.Cpf).HasColumnName("cpf").HasMaxLength(11).IsFixedLength().IsRequired();
        builder.Property(p => p.BirthDate).HasColumnName("birth_date").IsRequired();
        builder.Property(p => p.Sex).HasColumnName("sex").HasConversion<string>().HasMaxLength(10).IsRequired();

        builder.HasIndex(p => p.Cpf).IsUnique();
    }
}
