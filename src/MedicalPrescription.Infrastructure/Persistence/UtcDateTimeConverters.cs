using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MedicalPrescription.Infrastructure.Persistence;

internal sealed class UtcDateTimeConverter() : ValueConverter<DateTime, DateTime>(
    value => value.ToUniversalTime(),
    value => DateTime.SpecifyKind(value, DateTimeKind.Utc));

internal sealed class NullableUtcDateTimeConverter() : ValueConverter<DateTime?, DateTime?>(
    value => value.HasValue ? value.Value.ToUniversalTime() : null,
    value => value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : null);
