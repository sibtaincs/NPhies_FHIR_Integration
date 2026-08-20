using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPhies_FHIR_Integration.Domain.Entities;

namespace NPhies_FHIR_Integration.Infrastructure.Data.Configurations;

public class ClaimAccidentConfiguration : IEntityTypeConfiguration<ClaimAccident>
{
    public void Configure(EntityTypeBuilder<ClaimAccident> entity)
    {
    entity.HasKey(c => c.Id);
        entity.Property(c => c.ClaimId).IsRequired().HasMaxLength(100);
   entity.Property(c => c.AccidentDate).IsRequired();
     entity.Property(c => c.AccidentType).IsRequired().HasMaxLength(50);
    entity.Property(c => c.AccidentTypeSystem).HasMaxLength(200);
    entity.Property(c => c.AccidentLocation).HasMaxLength(500);
 entity.Property(c => c.AccidentLocationCity).HasMaxLength(100);
        entity.Property(c => c.AccidentLocationState).HasMaxLength(100);
        entity.Property(c => c.AccidentLocationCountry).HasMaxLength(2);

    entity.HasIndex(c => c.ClaimId);
        entity.HasIndex(c => c.AccidentDate);
     entity.HasIndex(c => c.AccidentType);

   entity.HasOne(c => c.Claim).WithMany().HasForeignKey(c => c.ClaimId).OnDelete(DeleteBehavior.Cascade);
  }
}

public class ClaimItemModifierConfiguration : IEntityTypeConfiguration<ClaimItemModifier>
{
    public void Configure(EntityTypeBuilder<ClaimItemModifier> entity)
    {
     entity.HasKey(c => c.Id);
 entity.Property(c => c.ClaimItemId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.ModifierCode).IsRequired().HasMaxLength(50);
entity.Property(c => c.ModifierSystem).HasMaxLength(200);
      entity.Property(c => c.ModifierDisplay).HasMaxLength(255);

   entity.HasIndex(c => c.ClaimItemId);
     entity.HasIndex(c => c.ModifierCode);

        entity.HasOne(c => c.ClaimItem).WithMany().HasForeignKey(c => c.ClaimItemId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class ClaimProcedureConfiguration : IEntityTypeConfiguration<ClaimProcedure>
{
  public void Configure(EntityTypeBuilder<ClaimProcedure> entity)
    {
        entity.HasKey(c => c.Id);
        entity.Property(c => c.ClaimId).IsRequired().HasMaxLength(100);
 entity.Property(c => c.ProcedureCode).IsRequired().HasMaxLength(100);
  entity.Property(c => c.ProcedureSystem).HasMaxLength(500);
        entity.Property(c => c.ProcedureDisplay).HasMaxLength(255);
        entity.Property(c => c.ProcedureDate);
        entity.Property(c => c.ProcedureType).HasMaxLength(50);
        entity.Property(c => c.ProcedureTypeSystem).HasMaxLength(500);

        entity.HasIndex(c => c.ClaimId);
   entity.HasIndex(c => c.ProcedureCode);
   entity.HasIndex(c => c.ProcedureDate);

     entity.HasOne(c => c.Claim).WithMany().HasForeignKey(c => c.ClaimId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class PaymentReconciliationConfiguration : IEntityTypeConfiguration<PaymentReconciliation>
{
public void Configure(EntityTypeBuilder<PaymentReconciliation> entity)
    {
      entity.HasKey(p => p.Id);
   entity.Property(p => p.PaymentReconciliationId).IsRequired().HasMaxLength(100);
        entity.Property(p => p.PaymentAmount).IsRequired().HasPrecision(18, 2);
     entity.Property(p => p.FhirPaymentReconciliationJson).HasColumnType("ntext");

entity.HasIndex(p => p.PaymentReconciliationId).IsUnique();

      entity.HasOne(p => p.PaymentIssuer).WithMany().HasForeignKey("PaymentIssuerId").OnDelete(DeleteBehavior.Restrict);
        entity.HasMany<PaymentReconciliationDetail>().WithOne(d => d.PaymentReconciliation).HasForeignKey(d => d.PaymentReconciliationId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class PaymentReconciliationDetailConfiguration : IEntityTypeConfiguration<PaymentReconciliationDetail>
{
    public void Configure(EntityTypeBuilder<PaymentReconciliationDetail> entity)
 {
        entity.HasKey(p => p.Id);
 entity.Property(p => p.PaymentReconciliationId).IsRequired().HasMaxLength(100);
     entity.Property(p => p.ComponentPayment).HasPrecision(18, 2);

     entity.HasIndex(p => p.PaymentReconciliationId);

        entity.HasOne(p => p.PaymentReconciliation).WithMany().HasForeignKey(p => p.PaymentReconciliationId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class VisionPrescriptionConfiguration : IEntityTypeConfiguration<VisionPrescription>
{
    public void Configure(EntityTypeBuilder<VisionPrescription> entity)
    {
        entity.HasKey(v => v.Id);
        entity.Property(v => v.ClaimItemId).IsRequired().HasMaxLength(100);

        entity.HasIndex(v => v.ClaimItemId);

     entity.HasOne(v => v.ClaimItem).WithMany().HasForeignKey(v => v.ClaimItemId).OnDelete(DeleteBehavior.Cascade);
  }
}

public class OralDetailConfiguration : IEntityTypeConfiguration<OralDetail>
{
    public void Configure(EntityTypeBuilder<OralDetail> entity)
    {
entity.HasKey(o => o.Id);
        entity.Property(o => o.ClaimItemId).IsRequired().HasMaxLength(100);

        entity.HasIndex(o => o.ClaimItemId);

        entity.HasOne(o => o.ClaimItem).WithMany().HasForeignKey(o => o.ClaimItemId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class ClaimErrorConfiguration : IEntityTypeConfiguration<ClaimError>
{
    public void Configure(EntityTypeBuilder<ClaimError> entity)
    {
        entity.HasKey(c => c.Id);
      entity.Property(c => c.ClaimId).HasMaxLength(100);
entity.Property(c => c.ClaimResponseId).HasMaxLength(100);
   entity.Property(c => c.ErrorCode).IsRequired().HasMaxLength(100);
   entity.Property(c => c.ErrorCodeSystem).HasMaxLength(500);
        entity.Property(c => c.ErrorSeverity).IsRequired().HasMaxLength(50);
        entity.Property(c => c.ErrorDescription).HasMaxLength(2000);
        entity.Property(c => c.ErrorDetails).HasMaxLength(2000);
        entity.Property(c => c.ErrorPath).HasMaxLength(500);
   entity.Property(c => c.ErrorExpression).HasMaxLength(500);

        entity.HasIndex(c => c.ClaimId);
    entity.HasIndex(c => c.ClaimResponseId);
   entity.HasIndex(c => c.ErrorCode);
        entity.HasIndex(c => c.ErrorSeverity);

entity.HasOne(c => c.Claim).WithMany().HasForeignKey(c => c.ClaimId).OnDelete(DeleteBehavior.Cascade);
entity.HasOne(c => c.ClaimResponse).WithMany().HasForeignKey(c => c.ClaimResponseId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class PaymentNoticeConfiguration : IEntityTypeConfiguration<PaymentNotice>
{
    public void Configure(EntityTypeBuilder<PaymentNotice> entity)
    {
        entity.HasKey(p => p.Id);
  entity.Property(p => p.Id).HasMaxLength(100);
        entity.Property(p => p.PaymentNoticeId).IsRequired().HasMaxLength(100);
     entity.Property(p => p.IdentifierSystem).HasMaxLength(500);
        entity.Property(p => p.IdentifierValue).HasMaxLength(100);
   entity.Property(p => p.Status).IsRequired().HasMaxLength(50);
   entity.Property(p => p.CreatedDate).IsRequired();
entity.Property(p => p.PaymentDate);
        entity.Property(p => p.PaymentIdentifierSystem).HasMaxLength(500);
entity.Property(p => p.PaymentIdentifierValue).HasMaxLength(100);
   entity.Property(p => p.Amount).IsRequired().HasPrecision(18, 2);
 entity.Property(p => p.Currency).IsRequired().HasMaxLength(3);
  entity.Property(p => p.PaymentStatus).HasMaxLength(50);
        entity.Property(p => p.PaymentStatusSystem).HasMaxLength(500);
  entity.Property(p => p.ProviderId).HasMaxLength(100);
      entity.Property(p => p.PayeeId).HasMaxLength(100);
        entity.Property(p => p.RecipientSystem).HasMaxLength(500);
  entity.Property(p => p.RecipientValue).HasMaxLength(100);
        entity.Property(p => p.FhirPaymentNoticeJson).HasColumnType("ntext");

        entity.HasIndex(p => p.PaymentNoticeId).IsUnique();
   entity.HasIndex(p => p.IdentifierValue);
        entity.HasIndex(p => p.Status);
        entity.HasIndex(p => p.PaymentStatus);
  entity.HasIndex(p => p.PaymentDate);
      entity.HasIndex(p => p.CreatedDate);
entity.HasIndex(p => p.PaymentIdentifierValue);
        entity.HasIndex(p => p.ProviderId);
      entity.HasIndex(p => p.PayeeId);
        entity.HasIndex(p => new { p.Status, p.PaymentStatus });
entity.HasIndex(p => new { p.ProviderId, p.CreatedDate });

        entity.HasOne(p => p.Provider).WithMany().HasForeignKey(p => p.ProviderId).OnDelete(DeleteBehavior.Restrict);
     entity.HasOne(p => p.Payee).WithMany().HasForeignKey(p => p.PayeeId).OnDelete(DeleteBehavior.Restrict);
    }
}
