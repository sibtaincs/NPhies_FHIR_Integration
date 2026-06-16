using Microsoft.EntityFrameworkCore;
using NPhies_FHIR_Integration.Domain.Entities;

namespace NPhies_FHIR_Integration.Infrastructure.Data;

/// <summary>
/// Application DbContext for NPhies FHIR Integration
/// Manages all domain entities and their relationships
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Constructor with DbContextOptions
    /// </summary>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSets for all domain entities

    /// <summary>
    /// Patient (Member) entities
    /// </summary>
    public DbSet<Patient> Patients { get; set; } = null!;

    /// <summary>
    /// Coverage (Insurance Policy) entities
    /// </summary>
    public DbSet<Coverage> Coverages { get; set; } = null!;

    /// <summary>
    /// Organization (Provider/Insurer) entities
    /// </summary>
    public DbSet<Organization> Organizations { get; set; } = null!;

    /// <summary>
    /// Location (Service Facility) entities
    /// </summary>
    public DbSet<Location> Locations { get; set; } = null!;

    /// <summary>
    /// Practitioner (Healthcare Provider) entities
    /// </summary>
    public DbSet<Practitioner> Practitioners { get; set; } = null!;

    /// <summary>
    /// MessageHeader (FHIR Message Envelope) entities
    /// </summary>
    public DbSet<MessageHeader> MessageHeaders { get; set; } = null!;

    /// <summary>
    /// CoverageEligibilityRequest entities
    /// </summary>
    public DbSet<CoverageEligibilityRequest> CoverageEligibilityRequests { get; set; } = null!;

    /// <summary>
    /// EligibilityItem entities
    /// </summary>
    public DbSet<EligibilityItem> EligibilityItems { get; set; } = null!;

    /// <summary>
    /// EligibilityItemModifier entities
    /// </summary>
    public DbSet<EligibilityItemModifier> EligibilityItemModifiers { get; set; } = null!;

    /// <summary>
    /// CoverageEligibilityResponse entities
    /// </summary>
    public DbSet<CoverageEligibilityResponse> CoverageEligibilityResponses { get; set; } = null!;

    /// <summary>
    /// BenefitBalance entities
    /// </summary>
    public DbSet<BenefitBalance> BenefitBalances { get; set; } = null!;

    /// <summary>
    /// Benefit entities
    /// </summary>
    public DbSet<Benefit> Benefits { get; set; } = null!;

    /// <summary>
    /// EligibilityError entities
    /// </summary>
    public DbSet<EligibilityError> EligibilityErrors { get; set; } = null!;

    /// <summary>
    /// Encounter entities
    /// </summary>
    public DbSet<Encounter> Encounters { get; set; } = null!;

    /// <summary>
    /// Claim entities
    /// </summary>
    public DbSet<Claim> Claims { get; set; } = null!;

    /// <summary>
    /// ClaimItem entities
    /// </summary>
    public DbSet<ClaimItem> ClaimItems { get; set; } = null!;

    /// <summary>
    /// ClaimItemDetail entities
    /// </summary>
    public DbSet<ClaimItemDetail> ClaimItemDetails { get; set; } = null!;

    /// <summary>
    /// ClaimDiagnosis entities
    /// </summary>
    public DbSet<ClaimDiagnosis> ClaimDiagnoses { get; set; } = null!;

    /// <summary>
    /// ClaimCareTeam entities
    /// </summary>
    public DbSet<ClaimCareTeam> ClaimCareTeams { get; set; } = null!;

    /// <summary>
    /// ClaimSupportingInfo entities
    /// </summary>
    public DbSet<ClaimSupportingInfo> ClaimSupportingInfos { get; set; } = null!;

    /// <summary>
    /// ClaimRelated entities
    /// </summary>
    public DbSet<ClaimRelated> ClaimRelatedClaims { get; set; } = null!;

    /// <summary>
    /// ClaimResponse entities
    /// </summary>
    public DbSet<ClaimResponse> ClaimResponses { get; set; } = null!;

    /// <summary>
    /// ClaimResponseInsurance entities
    /// </summary>
    public DbSet<ClaimResponseInsurance> ClaimResponseInsurances { get; set; } = null!;

    /// <summary>
    /// ClaimResponseAddItem entities
    /// </summary>
    public DbSet<ClaimResponseAddItem> ClaimResponseAddItems { get; set; } = null!;

    /// <summary>
    /// ClaimResponseAdjudication entities
    /// </summary>
    public DbSet<ClaimResponseAdjudication> ClaimResponseAdjudications { get; set; } = null!;

    /// <summary>
    /// ClaimResponseTotal entities
    /// </summary>
    public DbSet<ClaimResponseTotal> ClaimResponseTotals { get; set; } = null!;

    /// <summary>
    /// ClaimResponseDiagnosisExt entities
    /// </summary>
    public DbSet<ClaimResponseDiagnosisExt> ClaimResponseDiagnosesExt { get; set; } = null!;

    /// <summary>
    /// ClaimResponseSupportingInfoExt entities
    /// </summary>
    public DbSet<ClaimResponseSupportingInfoExt> ClaimResponseSupportingInfosExt { get; set; } = null!;

    /// <summary>
    /// Task entities (for polling and work items)
    public DbSet<PollTask> Tasks { get; set; } = null!;

    /// <summary>
    /// PaymentReconciliation entities (for payment tracking)
    /// </summary>
    public DbSet<PaymentReconciliation> PaymentReconciliations { get; set; } = null!;

    /// <summary>
    /// PaymentReconciliationDetail entities (payment detail line items)
    /// </summary>
    public DbSet<PaymentReconciliationDetail> PaymentReconciliationDetails { get; set; } = null!;

    /// <summary>
    /// PaymentNotice entities (payment acknowledgments)
    /// </summary>
    public DbSet<PaymentNotice> PaymentNotices { get; set; } = null!;

    /// <summary>
    /// CommunicationRequest entities (insurer-provider communications)
    /// </summary>
    public DbSet<CommunicationRequest> CommunicationRequests { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure all entity relationships and constraints
        ConfigurePatientEntity(modelBuilder);
        ConfigureCoverageEntity(modelBuilder);
        ConfigureOrganizationEntity(modelBuilder);
        ConfigureLocationEntity(modelBuilder);
        ConfigurePractitionerEntity(modelBuilder);
        ConfigureMessageHeaderEntity(modelBuilder);
        ConfigureCoverageEligibilityRequestEntity(modelBuilder);
        ConfigureEligibilityItemEntity(modelBuilder);
        ConfigureEligibilityItemModifierEntity(modelBuilder);
        ConfigureCoverageEligibilityResponseEntity(modelBuilder);
        ConfigureBenefitBalanceEntity(modelBuilder);
        ConfigureBenefitEntity(modelBuilder);
        ConfigureEligibilityErrorEntity(modelBuilder);
        ConfigureEncounterEntity(modelBuilder);
        ConfigureClaimEntity(modelBuilder);
        ConfigureClaimItemEntity(modelBuilder);
        ConfigureClaimItemDetailEntity(modelBuilder);
        ConfigureClaimDiagnosisEntity(modelBuilder);
        ConfigureClaimCareTeamEntity(modelBuilder);
        ConfigureClaimSupportingInfoEntity(modelBuilder);
        ConfigureClaimRelatedEntity(modelBuilder);
        ConfigureClaimResponseEntity(modelBuilder);
        ConfigureClaimResponseInsuranceEntity(modelBuilder);
        ConfigureClaimResponseAddItemEntity(modelBuilder);
        ConfigureClaimResponseAdjudicationEntity(modelBuilder);
        ConfigureClaimResponseTotalEntity(modelBuilder);
        ConfigureClaimResponseDiagnosisExtEntity(modelBuilder);
        ConfigureClaimResponseSupportingInfoExtEntity(modelBuilder);
        ConfigureTaskEntity(modelBuilder);
        ConfigurePaymentReconciliationEntity(modelBuilder);
        ConfigurePaymentReconciliationDetailEntity(modelBuilder);
        ConfigurePaymentNoticeEntity(modelBuilder);
        ConfigureCommunicationRequestEntity(modelBuilder);
    }

    /// <summary>
    /// Configure Patient entity
    /// </summary>
    private void ConfigurePatientEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Patient>();

        // Primary Key
        entity.HasKey(p => p.Id);

        // Properties
        entity.Property(p => p.MRN).IsRequired().HasMaxLength(50);
        entity.Property(p => p.IdentifierSystem).HasMaxLength(500);
        entity.Property(p => p.NationalId).HasMaxLength(50);
        entity.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
        entity.Property(p => p.LastName).IsRequired().HasMaxLength(100);
        entity.Property(p => p.Gender).HasMaxLength(1);
        entity.Property(p => p.Email).HasMaxLength(255);
        entity.Property(p => p.Phone).HasMaxLength(20);
        entity.Property(p => p.AddressLine1).HasMaxLength(255);
        entity.Property(p => p.AddressLine2).HasMaxLength(255);
        entity.Property(p => p.City).HasMaxLength(100);
        entity.Property(p => p.State).HasMaxLength(100);
        entity.Property(p => p.PostalCode).HasMaxLength(20);
        entity.Property(p => p.Country).HasMaxLength(2);
        entity.Property(p => p.Status).IsRequired().HasMaxLength(50);
        entity.Property(p => p.CreatedAt).IsRequired();
        entity.Property(p => p.UpdatedAt);
        entity.Property(p => p.IsActive).IsRequired();

        // Indexes
        entity.HasIndex(p => p.MRN).IsUnique();
        entity.HasIndex(p => p.Status);
        entity.HasIndex(p => p.IsActive);

        // Relationships
        entity.HasMany(p => p.Coverages)
            .WithOne(c => c.Patient)
        .HasForeignKey(c => c.PatientId)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(p => p.EligibilityRequests)
      .WithOne(e => e.Patient)
  .HasForeignKey(e => e.PatientId)
        .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(p => p.Claims)
    .WithOne(c => c.Patient)
 .HasForeignKey(c => c.PatientId)
    .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure Coverage entity
    /// </summary>
    private void ConfigureCoverageEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Coverage>();

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.PolicyNumber).IsRequired().HasMaxLength(100);
        entity.Property(c => c.MemberID).IsRequired().HasMaxLength(50);
        entity.Property(c => c.CoverageType).IsRequired().HasMaxLength(50);
        entity.Property(c => c.Status).IsRequired().HasMaxLength(50);
        entity.Property(c => c.SubscriberMRN).HasMaxLength(50);
        entity.Property(c => c.RelationToSubscriber).HasMaxLength(50);
        entity.Property(c => c.AnnualDeductible).HasPrecision(18, 2);
        entity.Property(c => c.DeductibleMet).HasPrecision(18, 2);
        entity.Property(c => c.Copay).HasPrecision(18, 2);
        entity.Property(c => c.CoinsurancePercent).HasPrecision(5, 2);
        entity.Property(c => c.OutOfPocketMax).HasPrecision(18, 2);

        // Indexes
        entity.HasIndex(c => c.PolicyNumber).IsUnique();
        entity.HasIndex(c => c.MemberID);
        entity.HasIndex(c => c.Status);
        entity.HasIndex(c => c.PatientId);

        // Relationships
        entity.HasOne(c => c.Patient)
    .WithMany(p => p.Coverages)
      .HasForeignKey(c => c.PatientId)
 .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Insurer)
     .WithMany()
         .HasForeignKey(c => c.InsurerId)
        .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(c => c.EligibilityRequests)
            .WithOne(e => e.Coverage)
   .HasForeignKey(e => e.CoverageId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(c => c.Claims)
       .WithOne(c => c.Coverage)
          .HasForeignKey(c => c.CoverageId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure Organization entity
    /// </summary>
    private void ConfigureOrganizationEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Organization>();

        // Primary Key
        entity.HasKey(o => o.Id);

        // Properties
        entity.Property(o => o.OrganizationName).IsRequired().HasMaxLength(255);
        entity.Property(o => o.LicenseNumber).IsRequired().HasMaxLength(100);
        entity.Property(o => o.LicenseSystem).HasMaxLength(500);
        entity.Property(o => o.OrganizationType).IsRequired().HasMaxLength(50);
        entity.Property(o => o.SpecializationType).HasMaxLength(100);
        entity.Property(o => o.Website).HasMaxLength(500);
        entity.Property(o => o.Email).HasMaxLength(255);
        entity.Property(o => o.PhoneNumber).HasMaxLength(20);
        entity.Property(o => o.AddressLine1).HasMaxLength(255);
        entity.Property(o => o.AddressLine2).HasMaxLength(255);
        entity.Property(o => o.City).HasMaxLength(100);
        entity.Property(o => o.State).HasMaxLength(100);
        entity.Property(o => o.PostalCode).HasMaxLength(20);
        entity.Property(o => o.Status).IsRequired().HasMaxLength(50);

        // Indexes
        entity.HasIndex(o => o.LicenseNumber).IsUnique();
        entity.HasIndex(o => o.OrganizationType);
        entity.HasIndex(o => o.Status);

        // Relationships
        entity.HasMany(o => o.Locations)
     .WithOne(l => l.Organization)
.HasForeignKey(l => l.OrganizationId)
   .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(o => o.Practitioners)
                  .WithOne(p => p.Organization)
          .HasForeignKey(p => p.OrganizationId)
           .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(o => o.SubmittedClaims)
     .WithOne(c => c.Provider)
     .HasForeignKey(c => c.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(o => o.ProcessedClaims)
                 .WithOne(c => c.Insurer)
              .HasForeignKey(c => c.InsurerId)
                 .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(o => o.EligibilityRequests)
 .WithOne(e => e.Provider)
            .HasForeignKey(e => e.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(o => o.EligibilityResponses)
            .WithOne(e => e.Insurer)
            .HasForeignKey(e => e.InsurerId)
  .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure Location entity
    /// </summary>
    private void ConfigureLocationEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Location>();

        // Primary Key
        entity.HasKey(l => l.Id);

        // Properties
        entity.Property(l => l.LocationName).IsRequired().HasMaxLength(255);
        entity.Property(l => l.LocationLicense).IsRequired().HasMaxLength(100);
        entity.Property(l => l.LicenseSystem).HasMaxLength(500);
        entity.Property(l => l.OrganizationId).IsRequired();
        entity.Property(l => l.FacilityType).HasMaxLength(50);
        entity.Property(l => l.FacilityTypeDescription).HasMaxLength(255);
        entity.Property(l => l.AddressLine1).HasMaxLength(255);
        entity.Property(l => l.AddressLine2).HasMaxLength(255);
        entity.Property(l => l.City).HasMaxLength(100);
        entity.Property(l => l.State).HasMaxLength(100);
        entity.Property(l => l.PostalCode).HasMaxLength(20);
        entity.Property(l => l.Country).HasMaxLength(2);
        entity.Property(l => l.Phone).HasMaxLength(20);
        entity.Property(l => l.Email).HasMaxLength(255);
        entity.Property(l => l.Status).IsRequired().HasMaxLength(50);

        // Indexes
        entity.HasIndex(l => l.LocationLicense).IsUnique();
        entity.HasIndex(l => l.OrganizationId);
        entity.HasIndex(l => l.Status);

        // Relationships
        entity.HasOne(l => l.Organization)
                    .WithMany(o => o.Locations)
                    .HasForeignKey(l => l.OrganizationId)
                    .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(l => l.Claims)
            .WithOne(c => c.ServiceLocation)
      .HasForeignKey(c => c.LocationId)
     .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure Practitioner entity
    /// </summary>
    private void ConfigurePractitionerEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Practitioner>();

        // Primary Key
        entity.HasKey(p => p.Id);

        // Properties
        entity.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
        entity.Property(p => p.LastName).IsRequired().HasMaxLength(100);
        entity.Property(p => p.LicenseNumber).IsRequired().HasMaxLength(100);
        entity.Property(p => p.LicenseSystem).HasMaxLength(500);
        entity.Property(p => p.Specialization).HasMaxLength(100);
        entity.Property(p => p.Qualification).HasMaxLength(255);
        entity.Property(p => p.Title).HasMaxLength(50);
        entity.Property(p => p.Email).HasMaxLength(255);
        entity.Property(p => p.Phone).HasMaxLength(20);
        entity.Property(p => p.Status).IsRequired().HasMaxLength(50);

        // Indexes
        entity.HasIndex(p => p.LicenseNumber).IsUnique();
        entity.HasIndex(p => p.Status);

        // Relationships
        entity.HasOne(p => p.Organization)
      .WithMany(o => o.Practitioners)
  .HasForeignKey(p => p.OrganizationId)
     .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(p => p.Claims)
            .WithOne(c => c.Practitioner)
.HasForeignKey(c => c.PractitionerId)
   .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure MessageHeader entity
    /// </summary>
    private void ConfigureMessageHeaderEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<MessageHeader>();

        // Primary Key
        entity.HasKey(m => m.Id);

        // Properties
        entity.Property(m => m.MessageUUID).IsRequired().HasMaxLength(50);
        entity.Property(m => m.CorrelationId).HasMaxLength(50);
        entity.Property(m => m.EventCode).IsRequired().HasMaxLength(100);
        entity.Property(m => m.EventSystem).HasMaxLength(500);
        entity.Property(m => m.DestinationName).HasMaxLength(255);
        entity.Property(m => m.DestinationEndpoint).HasMaxLength(500);
        entity.Property(m => m.FocusResourceType).HasMaxLength(100);
        entity.Property(m => m.FocusResourceId).HasMaxLength(100);
        entity.Property(m => m.SourceName).HasMaxLength(255);
        entity.Property(m => m.SourceEndpoint).HasMaxLength(500);
        entity.Property(m => m.Status).IsRequired().HasMaxLength(50);
        entity.Property(m => m.ResponseStatus).HasMaxLength(50);
        entity.Property(m => m.ErrorCode).HasMaxLength(100);
        entity.Property(m => m.ErrorMessage).HasMaxLength(1000);
        entity.Property(m => m.BundleContent).HasColumnType("ntext");
        entity.Property(m => m.ResponseBundleContent).HasColumnType("ntext");

        // Indexes
        entity.HasIndex(m => m.MessageUUID).IsUnique();
        entity.HasIndex(m => m.Status);
        entity.HasIndex(m => m.EventCode);

        // Relationships
        entity.HasMany(m => m.EligibilityRequests)
       .WithOne(e => e.MessageHeader)
    .HasForeignKey(e => e.MessageHeaderId)
   .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(m => m.EligibilityResponses)
         .WithOne(e => e.MessageHeader)
      .HasForeignKey(e => e.MessageHeaderId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(m => m.Claims)
            .WithOne(c => c.MessageHeader)
            .HasForeignKey(c => c.MessageHeaderId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure CoverageEligibilityRequest entity
    /// </summary>
    private void ConfigureCoverageEligibilityRequestEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<CoverageEligibilityRequest>();

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.MessageUUID).IsRequired().HasMaxLength(50);
        entity.Property(c => c.RequestId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.MessageHeaderId).HasMaxLength(50);
        entity.Property(c => c.RequestType).IsRequired().HasMaxLength(50);
        entity.Property(c => c.PurposeJson).HasColumnType("nvarchar(max)");
        entity.Property(c => c.Status).IsRequired().HasMaxLength(50);
        entity.Property(c => c.Priority).HasMaxLength(50);
        entity.Property(c => c.ServiceType).HasMaxLength(100);
        entity.Property(c => c.EligibilityStatus).HasMaxLength(50);
        entity.Property(c => c.MessageStatus).HasMaxLength(50);
        entity.Property(c => c.FhirRequestBundle).HasColumnType("ntext");
        entity.Property(c => c.FhirResponseBundle).HasColumnType("ntext");

        // Indexes
        entity.HasIndex(c => c.RequestId).IsUnique();
        entity.HasIndex(c => c.Status);
        entity.HasIndex(c => c.PatientId);
        entity.HasIndex(c => c.CoverageId);

        // Relationships
        entity.HasOne(c => c.MessageHeader)
                 .WithMany(m => m.EligibilityRequests)
           .HasForeignKey(c => c.MessageHeaderId)
                 .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Patient)
         .WithMany(p => p.EligibilityRequests)
            .HasForeignKey(c => c.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Coverage)
       .WithMany(c => c.EligibilityRequests)
  .HasForeignKey(c => c.CoverageId)
        .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Provider)
              .WithMany(o => o.EligibilityRequests)
                  .HasForeignKey(c => c.ProviderId)
          .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Insurer)
       .WithMany()
  .HasForeignKey(c => c.InsurerId)
    .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Enterer)
        .WithMany()
.HasForeignKey(c => c.EntererPractitionerId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(c => c.Items)
            .WithOne(i => i.EligibilityRequest)
     .HasForeignKey(i => i.EligibilityRequestId)
  .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(c => c.Response)
               .WithOne(r => r.EligibilityRequest)
         .HasForeignKey<CoverageEligibilityResponse>(r => r.EligibilityRequestId)
        .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure EligibilityItem entity
    /// </summary>
    private void ConfigureEligibilityItemEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<EligibilityItem>();

        // Primary Key
        entity.HasKey(e => e.Id);

        // Properties
        entity.Property(e => e.EligibilityRequestId).IsRequired();
        entity.Property(e => e.SequenceNumber);
        entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
        entity.Property(e => e.CategorySystem).HasMaxLength(500);
        entity.Property(e => e.CategoryDescription).HasMaxLength(255);
        entity.Property(e => e.ProductOrServiceCode).HasMaxLength(100);
        entity.Property(e => e.ProductOrServiceSystem).HasMaxLength(500);
        entity.Property(e => e.ProductOrServiceDescription).HasMaxLength(255);
        entity.Property(e => e.DiagnosisCodes).HasColumnType("nvarchar(max)");
        entity.Property(e => e.Notes).HasMaxLength(1000);

        // Indexes
        entity.HasIndex(e => e.EligibilityRequestId);
        entity.HasIndex(e => e.Category);

        // Relationships
        entity.HasOne(e => e.EligibilityRequest)
  .WithMany(r => r.Items)
     .HasForeignKey(e => e.EligibilityRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(e => e.Modifiers)
       .WithOne(m => m.EligibilityItem)
   .HasForeignKey(m => m.EligibilityItemId)
    .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure EligibilityItemModifier entity
    /// </summary>
    private void ConfigureEligibilityItemModifierEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<EligibilityItemModifier>();

        // Primary Key
        entity.HasKey(m => m.Id);

        // Properties
        entity.Property(m => m.EligibilityItemId).IsRequired();
        entity.Property(m => m.ModifierCode).IsRequired().HasMaxLength(100);
        entity.Property(m => m.ModifierSystem).HasMaxLength(500);
        entity.Property(m => m.ModifierDescription).HasMaxLength(255);

        // Relationships
        entity.HasOne(m => m.EligibilityItem)
   .WithMany(e => e.Modifiers)
            .HasForeignKey(m => m.EligibilityItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure CoverageEligibilityResponse entity
    /// </summary>
    private void ConfigureCoverageEligibilityResponseEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<CoverageEligibilityResponse>();

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.ResponseUUID).IsRequired().HasMaxLength(50);
        entity.Property(c => c.RequestId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.EligibilityRequestId).IsRequired();
        entity.Property(c => c.MessageHeaderId).HasMaxLength(50);
        entity.Property(c => c.Status).IsRequired().HasMaxLength(50);
        entity.Property(c => c.Outcome).IsRequired().HasMaxLength(50);
        entity.Property(c => c.ProcessingStatus).HasMaxLength(255);
        entity.Property(c => c.EligibilityStatus).HasMaxLength(50);
        entity.Property(c => c.InsurerId).HasMaxLength(100);
        entity.Property(c => c.PatientId).HasMaxLength(100);
        entity.Property(c => c.CoverageId).HasMaxLength(100);
        entity.Property(c => c.NetworkStatus).HasMaxLength(50);
        entity.Property(c => c.NetworkName).HasMaxLength(255);
        entity.Property(c => c.CoveredServicesJson).HasColumnType("nvarchar(max)");
        entity.Property(c => c.ExcludedServicesJson).HasColumnType("nvarchar(max)");
        entity.Property(c => c.LimitationsJson).HasColumnType("nvarchar(max)");
        entity.Property(c => c.ExplanationOfBenefits).HasMaxLength(1000);
        entity.Property(c => c.FhirResponseContent).HasColumnType("ntext");

        // Indexes
        entity.HasIndex(c => c.ResponseUUID).IsUnique();
        entity.HasIndex(c => c.RequestId);
        entity.HasIndex(c => c.Outcome);
        entity.HasIndex(c => c.EligibilityRequestId).IsUnique();

        // Relationships
        entity.HasOne(c => c.EligibilityRequest)
        .WithOne(r => r.Response)
 .HasForeignKey<CoverageEligibilityResponse>(c => c.EligibilityRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.MessageHeader)
             .WithMany(m => m.EligibilityResponses)
          .HasForeignKey(c => c.MessageHeaderId)
                 .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Insurer)
     .WithMany(o => o.EligibilityResponses)
            .HasForeignKey(c => c.InsurerId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Patient)
      .WithMany()
      .HasForeignKey(c => c.PatientId)
 .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Coverage)
               .WithMany()
     .HasForeignKey(c => c.CoverageId)
    .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(c => c.BenefitBalances)
          .WithOne(b => b.EligibilityResponse)
                .HasForeignKey(b => b.EligibilityResponseId)
                .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(c => c.Errors)
            .WithOne(e => e.EligibilityResponse)
         .HasForeignKey(e => e.EligibilityResponseId)
       .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure BenefitBalance entity
    /// </summary>
    private void ConfigureBenefitBalanceEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<BenefitBalance>();

        // Primary Key
        entity.HasKey(b => b.Id);

        // Properties
        entity.Property(b => b.EligibilityResponseId).IsRequired();
        entity.Property(b => b.SequenceNumber);
        entity.Property(b => b.Category).IsRequired().HasMaxLength(50);
        entity.Property(b => b.CategorySystem).HasMaxLength(500);
        entity.Property(b => b.CategoryDescription).HasMaxLength(255);

        // Indexes
        entity.HasIndex(b => b.EligibilityResponseId);
        entity.HasIndex(b => b.Category);

        // Relationships
        entity.HasOne(b => b.EligibilityResponse)
     .WithMany(r => r.BenefitBalances)
            .HasForeignKey(b => b.EligibilityResponseId)
    .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(b => b.Benefits)
     .WithOne(b => b.BenefitBalance)
        .HasForeignKey(b => b.BenefitBalanceId)
   .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure Benefit entity
    /// </summary>
    private void ConfigureBenefitEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Benefit>();

        // Primary Key
        entity.HasKey(b => b.Id);

        // Properties
        entity.Property(b => b.BenefitBalanceId).IsRequired();
        entity.Property(b => b.SequenceNumber);
        entity.Property(b => b.BenefitType).IsRequired().HasMaxLength(50);
        entity.Property(b => b.BenefitTypeSystem).HasMaxLength(500);
        entity.Property(b => b.BenefitTypeDescription).HasMaxLength(255);
        entity.Property(b => b.AllowedAmount).HasPrecision(18, 2);
        entity.Property(b => b.AllowedCurrency).HasMaxLength(3);
        entity.Property(b => b.AllowedUnit).HasMaxLength(50);
        entity.Property(b => b.UsedAmount).HasPrecision(18, 2);
        entity.Property(b => b.PercentageAmount).HasPrecision(5, 2);
        entity.Property(b => b.Description).HasMaxLength(1000);

        // Indexes
        entity.HasIndex(b => b.BenefitBalanceId);
        entity.HasIndex(b => b.BenefitType);

        // Relationships
        entity.HasOne(b => b.BenefitBalance)
            .WithMany(b => b.Benefits)
   .HasForeignKey(b => b.BenefitBalanceId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure EligibilityError entity
    /// </summary>
    private void ConfigureEligibilityErrorEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<EligibilityError>();

        // Primary Key
        entity.HasKey(e => e.Id);

        // Properties
        entity.Property(e => e.EligibilityRequestId).HasMaxLength(100);
        entity.Property(e => e.EligibilityResponseId).HasMaxLength(100);
        entity.Property(e => e.ErrorCode).IsRequired().HasMaxLength(100);
        entity.Property(e => e.ErrorCodeSystem).HasMaxLength(500);
        entity.Property(e => e.ErrorMessage).IsRequired().HasMaxLength(1000);
        entity.Property(e => e.ErrorDetails).HasMaxLength(2000);
        entity.Property(e => e.Severity).IsRequired().HasMaxLength(50);
        entity.Property(e => e.ErrorLocation).HasMaxLength(255);
        entity.Property(e => e.ErrorField).HasMaxLength(255);
        entity.Property(e => e.AdditionalContext).HasMaxLength(1000);

        // Indexes
        entity.HasIndex(e => e.Severity);
        entity.HasIndex(e => e.ErrorCode);

        // Relationships
        entity.HasOne(e => e.EligibilityRequest)
         .WithMany()
            .HasForeignKey(e => e.EligibilityRequestId)
        .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.EligibilityResponse)
 .WithMany(r => r.Errors)
          .HasForeignKey(e => e.EligibilityResponseId)
  .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure Encounter entity
    /// </summary>
    private void ConfigureEncounterEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Encounter>();

        // Primary Key
        entity.HasKey(e => e.Id);

        // Properties
        entity.Property(e => e.EncounterId).IsRequired().HasMaxLength(100);
        entity.Property(e => e.IdentifierSystem).HasMaxLength(500);
        entity.Property(e => e.IdentifierValue).HasMaxLength(100);
        entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
        entity.Property(e => e.Class).IsRequired().HasMaxLength(50);
        entity.Property(e => e.ServiceType).HasMaxLength(100);
        entity.Property(e => e.ServiceTypeSystem).HasMaxLength(500);
        entity.Property(e => e.AdmitSource).HasMaxLength(50);
        entity.Property(e => e.AdmitSourceSystem).HasMaxLength(500);
        entity.Property(e => e.ServiceEventType).HasMaxLength(100);
        entity.Property(e => e.IntendedLengthOfStay).HasMaxLength(100);
        entity.Property(e => e.FhirEncounterJson).HasColumnType("ntext");

        // Indexes
        entity.HasIndex(e => e.EncounterId).IsUnique();
        entity.HasIndex(e => e.Status);
        entity.HasIndex(e => e.Class);
        entity.HasIndex(e => e.PatientId);

        // Relationships
        entity.HasOne(e => e.Patient)
     .WithMany()
                 .HasForeignKey(e => e.PatientId)
          .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(e => e.ServiceProvider)
    .WithMany()
      .HasForeignKey(e => e.ServiceProviderId)
   .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure Claim entity
    /// </summary>
    private void ConfigureClaimEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Claim>();

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.ClaimNumber).IsRequired().HasMaxLength(100);
        entity.Property(c => c.ClaimIdentifierSystem).HasMaxLength(500);
        entity.Property(c => c.ClaimIdentifierValue).HasMaxLength(100);
        entity.Property(c => c.Status).IsRequired().HasMaxLength(50);
        entity.Property(c => c.ClaimType).IsRequired().HasMaxLength(50);
        entity.Property(c => c.ClaimTypeSystem).HasMaxLength(500);
        entity.Property(c => c.ClaimSubType).HasMaxLength(50);
        entity.Property(c => c.Use).IsRequired().HasMaxLength(50);
        entity.Property(c => c.Priority).HasMaxLength(50);
        entity.Property(c => c.PrioritySystem).HasMaxLength(500);
        entity.Property(c => c.PayeeType).HasMaxLength(50);
        entity.Property(c => c.PayeeTypeSystem).HasMaxLength(500);
        entity.Property(c => c.Total).HasPrecision(18, 2);
        entity.Property(c => c.TotalCurrency).HasMaxLength(3);
        entity.Property(c => c.FhirClaimBundle).HasColumnType("ntext");

        // NEW: Episode and Offline Fields
        entity.Property(c => c.EpisodeIdentifierSystem).HasMaxLength(500);
        entity.Property(c => c.EpisodeIdentifierValue).HasMaxLength(100);
        entity.Property(c => c.EligibilityOfflineReference).HasMaxLength(100);
        entity.Property(c => c.AuthorizationOfflineDate);

        // Indexes
        entity.HasIndex(c => c.ClaimNumber).IsUnique();
        entity.HasIndex(c => c.Status);
        entity.HasIndex(c => c.Use);
        entity.HasIndex(c => c.PatientId);

        // NEW: Indexes for episode and offline fields
        entity.HasIndex(c => c.EpisodeIdentifierValue);
        entity.HasIndex(c => c.EligibilityOfflineReference);
    }

    /// <summary>
    /// Configure ClaimItem entity
    /// </summary>
    private void ConfigureClaimItemEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimItem>();

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.ClaimId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.Sequence).IsRequired();
        entity.Property(c => c.ProductOrServiceCode).IsRequired().HasMaxLength(100);
        entity.Property(c => c.ProductOrServiceSystem).HasMaxLength(500);
        entity.Property(c => c.AltProductOrServiceCode).HasMaxLength(100);
        entity.Property(c => c.AltProductOrServiceSystem).HasMaxLength(500);
        entity.Property(c => c.Quantity).HasPrecision(18, 2);
        entity.Property(c => c.UnitPrice).HasPrecision(18, 2);
        entity.Property(c => c.Net).HasPrecision(18, 2);
        entity.Property(c => c.Notes).HasMaxLength(1000);

        // NEW: Patient Invoice Fields
        entity.Property(c => c.PatientInvoiceSystem).HasMaxLength(500);
        entity.Property(c => c.PatientInvoiceValue).HasMaxLength(100);

        // Indexes
        entity.HasIndex(c => c.ClaimId);
        entity.HasIndex(c => c.Sequence);

        // NEW: Index for patient invoice
        entity.HasIndex(c => c.PatientInvoiceValue);
    }

    /// <summary>
    /// Configure ClaimItemDetail entity
    /// </summary>
    private void ConfigureClaimItemDetailEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimItemDetail>();

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.ClaimItemId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.Sequence).IsRequired();
        entity.Property(c => c.ProductOrServiceCode).IsRequired().HasMaxLength(100);
        entity.Property(c => c.ProductOrServiceSystem).HasMaxLength(500);
        entity.Property(c => c.AltProductOrServiceCode).HasMaxLength(100);
        entity.Property(c => c.AltProductOrServiceSystem).HasMaxLength(500);
        entity.Property(c => c.Quantity).HasPrecision(18, 2);
        entity.Property(c => c.UnitPrice).HasPrecision(18, 2);
        entity.Property(c => c.Net).HasPrecision(18, 2);
        entity.Property(c => c.Notes).HasMaxLength(1000);

        // Indexes
        entity.HasIndex(c => c.ClaimItemId);
        entity.HasIndex(c => c.Sequence);

        // Relationships
        entity.HasOne(c => c.ClaimItem)
          .WithMany(c => c.Details)
              .HasForeignKey(c => c.ClaimItemId)
        .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ClaimDiagnosis entity
    /// </summary>
    private void ConfigureClaimDiagnosisEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimDiagnosis>();

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.ClaimId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.Sequence).IsRequired();
        entity.Property(c => c.DiagnosisCode).IsRequired().HasMaxLength(100);
        entity.Property(c => c.DiagnosisSystem).HasMaxLength(500);
        entity.Property(c => c.DiagnosisType).HasMaxLength(50);
        entity.Property(c => c.OnAdmissionCode).HasMaxLength(10);
        entity.Property(c => c.Notes).HasMaxLength(1000);

        // Indexes
        entity.HasIndex(c => c.ClaimId);
        entity.HasIndex(c => c.DiagnosisCode);

        // NEW: Index for on-admission
        entity.HasIndex(c => c.OnAdmissionCode);
    }

    /// <summary>
    /// Configure ClaimCareTeam entity
    /// </summary>
    private void ConfigureClaimCareTeamEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimCareTeam>();

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.ClaimId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.Sequence).IsRequired();
        entity.Property(c => c.PractitionerId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.Role).HasMaxLength(50);
        entity.Property(c => c.RoleSystem).HasMaxLength(500);
        entity.Property(c => c.Qualification).HasMaxLength(100);
        entity.Property(c => c.QualificationSystem).HasMaxLength(500);
        entity.Property(c => c.Notes).HasMaxLength(1000);

        // Indexes
        entity.HasIndex(c => c.ClaimId);
        entity.HasIndex(c => c.PractitionerId);

        // Relationships
        entity.HasOne(c => c.Claim)
            .WithMany(c => c.CareTeam)
   .HasForeignKey(c => c.ClaimId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(c => c.Practitioner)
    .WithMany()
       .HasForeignKey(c => c.PractitionerId)
   .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure ClaimSupportingInfo entity
    /// </summary>
    private void ConfigureClaimSupportingInfoEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimSupportingInfo>();

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.ClaimId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.Sequence).IsRequired();
        entity.Property(c => c.Category).IsRequired().HasMaxLength(100);
        entity.Property(c => c.CategorySystem).HasMaxLength(500);
        entity.Property(c => c.CodeValue).HasMaxLength(100);
        entity.Property(c => c.StringValue).HasMaxLength(4000);
        entity.Property(c => c.QuantityValue).HasPrecision(18, 2);
        entity.Property(c => c.QuantityUnit).HasMaxLength(50);
        entity.Property(c => c.Notes).HasMaxLength(1000);

        // Indexes
        entity.HasIndex(c => c.ClaimId);
        entity.HasIndex(c => c.Category);

        // Relationships
        entity.HasOne(c => c.Claim)
          .WithMany(c => c.SupportingInfo)
            .HasForeignKey(c => c.ClaimId)
        .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ClaimRelated entity
    /// </summary>
    private void ConfigureClaimRelatedEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimRelated>();

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.ClaimId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.RelatedClaimIdentifierSystem).HasMaxLength(500);
        entity.Property(c => c.RelatedClaimIdentifierValue).HasMaxLength(100);
        entity.Property(c => c.Relationship).HasMaxLength(50);
        entity.Property(c => c.RelationshipSystem).HasMaxLength(500);
        entity.Property(c => c.Notes).HasMaxLength(1000);

        // Indexes
        entity.HasIndex(c => c.ClaimId);

        // Relationships
        entity.HasOne(c => c.Claim)
   .WithMany(c => c.RelatedClaims)
              .HasForeignKey(c => c.ClaimId)
   .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ClaimResponse entity
    /// </summary>
    private void ConfigureClaimResponseEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimResponse>();

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.ClaimId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.ResponseIdentifierSystem).HasMaxLength(500);
        entity.Property(c => c.ResponseIdentifierValue).HasMaxLength(100);
        entity.Property(c => c.ClaimResponseStatus).HasMaxLength(50);
        entity.Property(c => c.ClaimType).HasMaxLength(50);
        entity.Property(c => c.ClaimTypeSystem).HasMaxLength(500);
        entity.Property(c => c.ClaimSubType).HasMaxLength(50);
        entity.Property(c => c.Use).HasMaxLength(50);
        entity.Property(c => c.RequestIdentifierSystem).HasMaxLength(500);
        entity.Property(c => c.RequestIdentifierValue).HasMaxLength(100);
        entity.Property(c => c.PreAuthRef).HasMaxLength(100);
        entity.Property(c => c.FhirClaimResponseJson).HasColumnType("ntext");

        // Indexes
        entity.HasIndex(c => c.ClaimId).IsUnique();
        entity.HasIndex(c => c.PreAuthRef);

        // Relationships
        entity.HasOne(c => c.Claim)
            .WithOne()
  .HasForeignKey<ClaimResponse>(c => c.ClaimId)
       .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Patient)
     .WithMany()
        .HasForeignKey(c => c.PatientId)
        .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Insurer)
 .WithMany()
       .HasForeignKey(c => c.InsurerId)
         .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Requestor)
      .WithMany()
       .HasForeignKey(c => c.RequestorId)
         .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.ServiceProvider)
  .WithMany()
      .HasForeignKey(c => c.ServiceProviderId)
        .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(c => c.Insurance)
             .WithOne(i => i.ClaimResponse)
        .HasForeignKey(i => i.ClaimResponseId)
        .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(c => c.AddItems)
      .WithOne(a => a.ClaimResponse)
              .HasForeignKey(a => a.ClaimResponseId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(c => c.Totals)
        .WithOne(t => t.ClaimResponse)
   .HasForeignKey(t => t.ClaimResponseId)
         .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(c => c.DiagnosesExt)
        .WithOne(d => d.ClaimResponse)
              .HasForeignKey(d => d.ClaimResponseId)
    .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(c => c.SupportingInfoExt)
         .WithOne(s => s.ClaimResponse)
       .HasForeignKey(s => s.ClaimResponseId)
     .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ClaimResponseInsurance entity
    /// </summary>
    private void ConfigureClaimResponseInsuranceEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimResponseInsurance>();

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.ClaimResponseId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.Sequence).IsRequired();
        entity.Property(c => c.CoverageId).HasMaxLength(100);

        // NEW: Pre-Auth References
        entity.Property(c => c.PreAuthReferences).HasMaxLength(500);

        // Indexes
        entity.HasIndex(c => c.ClaimResponseId);
        entity.HasIndex(c => c.Sequence);

        // Relationships
        entity.HasOne(c => c.ClaimResponse)
            .WithMany(c => c.Insurance)
  .HasForeignKey(c => c.ClaimResponseId)
 .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(c => c.Coverage)
        .WithMany()
               .HasForeignKey(c => c.CoverageId)
                .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure ClaimResponseAddItem entity
    /// </summary>
    private void ConfigureClaimResponseAddItemEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimResponseAddItem>();

        entity.HasKey(c => c.Id);
        entity.Property(c => c.ClaimResponseId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.Sequence).IsRequired();
        entity.Property(c => c.ProductOrServiceCode).HasMaxLength(100);
        entity.Property(c => c.ProductOrServiceSystem).HasMaxLength(500);
        entity.Property(c => c.BenefitAmount).HasPrecision(18, 2);
        entity.Property(c => c.SubmittedAmount).HasPrecision(18, 2);
        entity.Property(c => c.Notes).HasMaxLength(1000);

        entity.HasIndex(c => c.ClaimResponseId);

        entity.HasOne(c => c.ClaimResponse)
           .WithMany(cr => cr.AddItems)
                    .HasForeignKey(c => c.ClaimResponseId)
           .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(c => c.Adjudications)
     .WithOne(a => a.ClaimResponseAddItem)
   .HasForeignKey(a => a.ClaimResponseAddItemId)
          .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ClaimResponseAdjudication entity
    /// </summary>
    private void ConfigureClaimResponseAdjudicationEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimResponseAdjudication>();

        entity.HasKey(c => c.Id);
        entity.Property(c => c.ClaimResponseAddItemId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.AdjudicationCategory).IsRequired().HasMaxLength(50);
        entity.Property(c => c.AdjudicationSystem).HasMaxLength(500);
        entity.Property(c => c.Amount).HasPrecision(18, 2);
        entity.Property(c => c.Notes).HasMaxLength(1000);

        entity.HasIndex(c => c.ClaimResponseAddItemId);

        entity.HasOne(c => c.ClaimResponseAddItem)
   .WithMany(a => a.Adjudications)
            .HasForeignKey(c => c.ClaimResponseAddItemId)
     .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ClaimResponseTotal entity
    /// </summary>
    private void ConfigureClaimResponseTotalEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimResponseTotal>();

        entity.HasKey(c => c.Id);
        entity.Property(c => c.ClaimResponseId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.Category).IsRequired().HasMaxLength(50);
        entity.Property(c => c.CategorySystem).HasMaxLength(500);
        entity.Property(c => c.Amount).IsRequired().HasPrecision(18, 2);
        entity.Property(c => c.Currency).HasMaxLength(3);
        entity.Property(c => c.Notes).HasMaxLength(1000);

        entity.HasIndex(c => c.ClaimResponseId);

        entity.HasOne(c => c.ClaimResponse)
             .WithMany(cr => cr.Totals)
                   .HasForeignKey(c => c.ClaimResponseId)
        .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ClaimResponseDiagnosisExt entity
    /// </summary>
    private void ConfigureClaimResponseDiagnosisExtEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimResponseDiagnosisExt>();

        entity.HasKey(c => c.Id);
        entity.Property(c => c.ClaimResponseId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.Sequence).IsRequired();
        entity.Property(c => c.DiagnosisCode).IsRequired().HasMaxLength(100);
        entity.Property(c => c.DiagnosisSystem).HasMaxLength(500);
        entity.Property(c => c.DiagnosisType).HasMaxLength(50);
        entity.Property(c => c.Notes).HasMaxLength(1000);

        entity.HasIndex(c => c.ClaimResponseId);

        entity.HasOne(c => c.ClaimResponse)
            .WithMany(cr => cr.DiagnosesExt)
        .HasForeignKey(c => c.ClaimResponseId)
    .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ClaimResponseSupportingInfoExt entity
    /// </summary>
    private void ConfigureClaimResponseSupportingInfoExtEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimResponseSupportingInfoExt>();

        entity.HasKey(c => c.Id);
        entity.Property(c => c.ClaimResponseId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.Sequence).IsRequired();
        entity.Property(c => c.Category).IsRequired().HasMaxLength(100);
        entity.Property(c => c.CategorySystem).HasMaxLength(500);
        entity.Property(c => c.CodeValue).HasMaxLength(100);
        entity.Property(c => c.StringValue).HasMaxLength(4000);
        entity.Property(c => c.QuantityValue).HasPrecision(18, 2);
        entity.Property(c => c.QuantityUnit).HasMaxLength(50);
        entity.Property(c => c.Notes).HasMaxLength(1000);

        entity.HasIndex(c => c.ClaimResponseId);

        entity.HasOne(c => c.ClaimResponse)
                  .WithMany(cr => cr.SupportingInfoExt)
      .HasForeignKey(c => c.ClaimResponseId)
         .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure Task entity (for polling and work items)
    /// </summary>
    private void ConfigureTaskEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PollTask>();

        // Primary Key
        entity.HasKey(t => t.Id);

        // Properties
        entity.Property(t => t.TaskId).IsRequired().HasMaxLength(100);
        entity.Property(t => t.TaskIdentifierSystem).HasMaxLength(500);
        entity.Property(t => t.TaskIdentifierValue).HasMaxLength(100);
        entity.Property(t => t.Status).IsRequired().HasMaxLength(50);
        entity.Property(t => t.Intent).IsRequired().HasMaxLength(50);
        entity.Property(t => t.Priority).IsRequired().HasMaxLength(50);
        entity.Property(t => t.Code).IsRequired().HasMaxLength(100);
        entity.Property(t => t.CodeSystem).HasMaxLength(500);
        entity.Property(t => t.CodeDisplay).HasMaxLength(255);
        entity.Property(t => t.AuthoredOn).IsRequired();
        entity.Property(t => t.LastModified).IsRequired();
        entity.Property(t => t.RequesterId).IsRequired().HasMaxLength(100);
        entity.Property(t => t.OwnerId).IsRequired().HasMaxLength(100);
        entity.Property(t => t.PollInputType).HasMaxLength(100);
        entity.Property(t => t.PollInputValue).HasMaxLength(100);

        // NEW CANCELLATION/FOCUS FIELDS
        entity.Property(t => t.FocusResourceType).HasMaxLength(100);
        entity.Property(t => t.FocusIdentifierSystem).HasMaxLength(500);
        entity.Property(t => t.FocusIdentifierValue).HasMaxLength(100);
        entity.Property(t => t.ReasonCode).HasMaxLength(50);
        entity.Property(t => t.ReasonCodeSystem).HasMaxLength(500);

        // OUTPUT/RESPONSE FIELDS
        entity.Property(t => t.OutputType).HasMaxLength(100);
        entity.Property(t => t.OutputTypeSystem).HasMaxLength(500);
        entity.Property(t => t.OutputBundleId).HasMaxLength(100);
        entity.Property(t => t.OutputBundleReference).HasMaxLength(500);
        entity.Property(t => t.ResponseCode).HasMaxLength(50);
        entity.Property(t => t.ResponseIdentifier).HasMaxLength(100);
        entity.Property(t => t.MetaTag).HasMaxLength(100);

        entity.Property(t => t.Notes).HasMaxLength(1000);
        entity.Property(t => t.MessageHeaderId).HasMaxLength(100);
        entity.Property(t => t.FhirTaskJson).HasColumnType("ntext");

        // Indexes
        entity.HasIndex(t => t.TaskId).IsUnique();
        entity.HasIndex(t => t.Status);
        entity.HasIndex(t => t.Code);
        entity.HasIndex(t => t.RequesterId);
        entity.HasIndex(t => t.OwnerId);
        entity.HasIndex(t => t.Priority);

        // NEW INDEXES FOR OUTPUT/RESPONSE
        entity.HasIndex(t => t.OutputBundleId);
        entity.HasIndex(t => t.ResponseCode);
        entity.HasIndex(t => t.MetaTag);

        // NEW INDEXES FOR CANCELLATION
        entity.HasIndex(t => t.FocusIdentifierValue);
        entity.HasIndex(t => t.ReasonCode);
    }

    /// <summary>
    /// Override SaveChanges to set update timestamps
    /// </summary>
    public override int SaveChanges()
    {
        SetUpdateTimestamps();
        return base.SaveChanges();
    }

    /// <summary>
    /// Override SaveChangesAsync to set update timestamps
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetUpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Configure PaymentReconciliation entity
    /// </summary>
    private void ConfigurePaymentReconciliationEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PaymentReconciliation>();

        // Primary Key
        entity.HasKey(p => p.Id);

        // Properties
        entity.Property(p => p.PaymentReconciliationId).IsRequired().HasMaxLength(100);
        entity.Property(p => p.IdentifierSystem).HasMaxLength(500);
        entity.Property(p => p.IdentifierValue).HasMaxLength(100);
        entity.Property(p => p.Status).IsRequired().HasMaxLength(50);
        entity.Property(p => p.Outcome).HasMaxLength(50);
        entity.Property(p => p.Disposition).HasMaxLength(1000);
        entity.Property(p => p.PeriodStart).IsRequired();
        entity.Property(p => p.PeriodEnd).IsRequired();
        entity.Property(p => p.CreatedDate).IsRequired();
        entity.Property(p => p.PaymentDate);
        entity.Property(p => p.PaymentAmount).IsRequired().HasPrecision(18, 2);
        entity.Property(p => p.PaymentCurrency).HasMaxLength(3);
        entity.Property(p => p.PaymentMethodType).HasMaxLength(50);
        entity.Property(p => p.PaymentMethodSystem).HasMaxLength(500);
        entity.Property(p => p.PaymentIdentifierSystem).HasMaxLength(500);
        entity.Property(p => p.PaymentIdentifierValue).HasMaxLength(100);
        entity.Property(p => p.PaymentIssuerId).IsRequired().HasMaxLength(100);
        entity.Property(p => p.RequestorId).IsRequired().HasMaxLength(100);
        entity.Property(p => p.FhirPaymentReconciliationJson).HasColumnType("ntext");

        // Indexes
        entity.HasIndex(p => p.PaymentReconciliationId).IsUnique();
        entity.HasIndex(p => p.Status);
        entity.HasIndex(p => p.Outcome);
        entity.HasIndex(p => p.PaymentDate);
        entity.HasIndex(p => p.PaymentIssuerId);
        entity.HasIndex(p => p.RequestorId);
        entity.HasIndex(p => p.PeriodStart);
        entity.HasIndex(p => p.PeriodEnd);

        // Relationships
        entity.HasOne(p => p.PaymentIssuer)
       .WithMany()
        .HasForeignKey(p => p.PaymentIssuerId)
       .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(p => p.Requestor)
     .WithMany()
      .HasForeignKey(p => p.RequestorId)
         .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(p => p.Details)
               .WithOne(d => d.PaymentReconciliation)
               .HasForeignKey(d => d.PaymentReconciliationId)
               .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure PaymentReconciliationDetail entity
    /// </summary>
    private void ConfigurePaymentReconciliationDetailEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PaymentReconciliationDetail>();

        // Primary Key
        entity.HasKey(d => d.Id);

        // Properties
        entity.Property(d => d.PaymentReconciliationId).IsRequired().HasMaxLength(100);
        entity.Property(d => d.DetailType).IsRequired().HasMaxLength(50);
        entity.Property(d => d.RequestIdentifierSystem).HasMaxLength(500);
        entity.Property(d => d.RequestIdentifierValue).HasMaxLength(100);
        entity.Property(d => d.RequestReference).HasMaxLength(500);
        entity.Property(d => d.ResponseIdentifierSystem).HasMaxLength(500);
        entity.Property(d => d.ResponseIdentifierValue).HasMaxLength(100);
        entity.Property(d => d.ResponseReference).HasMaxLength(500);
        entity.Property(d => d.DetailDate);
        entity.Property(d => d.Amount).IsRequired().HasPrecision(18, 2);
        entity.Property(d => d.AmountCurrency).HasMaxLength(3);
        entity.Property(d => d.SubmitterId).HasMaxLength(100);
        entity.Property(d => d.PayeeId).HasMaxLength(100);
        entity.Property(d => d.ComponentPayment).HasPrecision(18, 2);
        entity.Property(d => d.EarlyFee).HasPrecision(18, 2);
        entity.Property(d => d.NphiesFee).HasPrecision(18, 2);
        entity.Property(d => d.Notes).HasMaxLength(1000);

        // Indexes
        entity.HasIndex(d => d.PaymentReconciliationId);
        entity.HasIndex(d => d.DetailType);
        entity.HasIndex(d => d.DetailDate);
        entity.HasIndex(d => d.SubmitterId);
        entity.HasIndex(d => d.PayeeId);
        entity.HasIndex(d => d.Amount);

        // Relationships
        entity.HasOne(d => d.PaymentReconciliation)
          .WithMany(p => p.Details)
            .HasForeignKey(d => d.PaymentReconciliationId)
        .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(d => d.Submitter)
         .WithMany()
              .HasForeignKey(d => d.SubmitterId)
       .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(d => d.Payee)
        .WithMany()
         .HasForeignKey(d => d.PayeeId)
               .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Set UpdatedAt timestamp for modified entities
    /// </summary>
    private void SetUpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
   .Where(e => e.Entity is BaseEntity && e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            ((BaseEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Configure PaymentNotice entity
    /// </summary>
    private void ConfigurePaymentNoticeEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PaymentNotice>();

        // Primary Key
        entity.HasKey(p => p.Id);

        // Properties
        entity.Property(p => p.PaymentNoticeId).IsRequired().HasMaxLength(100);
        entity.Property(p => p.IdentifierSystem).HasMaxLength(500);
        entity.Property(p => p.IdentifierValue).HasMaxLength(100);
        entity.Property(p => p.Status).IsRequired().HasMaxLength(50);
        entity.Property(p => p.CreatedDate).IsRequired();
        entity.Property(p => p.PaymentDate);
        entity.Property(p => p.PaymentIdentifierSystem).HasMaxLength(500);
        entity.Property(p => p.PaymentIdentifierValue).HasMaxLength(100);
        entity.Property(p => p.Amount).IsRequired().HasPrecision(18, 2);
        entity.Property(p => p.Currency).HasMaxLength(3);
        entity.Property(p => p.PaymentStatus).HasMaxLength(50);
        entity.Property(p => p.PaymentStatusSystem).HasMaxLength(500);
        entity.Property(p => p.ProviderId).HasMaxLength(100);
        entity.Property(p => p.PayeeId).HasMaxLength(100);
        entity.Property(p => p.RecipientSystem).HasMaxLength(500);
        entity.Property(p => p.RecipientValue).HasMaxLength(100);
        entity.Property(p => p.FhirPaymentNoticeJson).HasColumnType("ntext");

        // Indexes
        entity.HasIndex(p => p.PaymentNoticeId).IsUnique();
        entity.HasIndex(p => p.Status);
        entity.HasIndex(p => p.PaymentStatus);
        entity.HasIndex(p => p.PaymentDate);
        entity.HasIndex(p => p.ProviderId);
        entity.HasIndex(p => p.PayeeId);

        // Relationships
        entity.HasOne(p => p.Provider)
   .WithMany()
            .HasForeignKey(p => p.ProviderId)
 .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(p => p.Payee)
 .WithMany()
      .HasForeignKey(p => p.PayeeId)
    .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure CommunicationRequest entity
    /// </summary>
    private void ConfigureCommunicationRequestEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<CommunicationRequest>();

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.CommunicationRequestId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.IdentifierSystem).HasMaxLength(500);
        entity.Property(c => c.IdentifierValue).HasMaxLength(100);
        entity.Property(c => c.Status).IsRequired().HasMaxLength(50);
        entity.Property(c => c.Category).HasMaxLength(50);
        entity.Property(c => c.CategorySystem).HasMaxLength(500);
        entity.Property(c => c.Priority).HasMaxLength(50);
        entity.Property(c => c.SubjectPatientId).HasMaxLength(100);
        entity.Property(c => c.AboutResourceType).HasMaxLength(100);
        entity.Property(c => c.AboutIdentifierSystem).HasMaxLength(500);
        entity.Property(c => c.AboutIdentifierValue).HasMaxLength(100);
        entity.Property(c => c.PayloadContent).HasColumnType("nvarchar(2000)");
        entity.Property(c => c.RecipientId).HasMaxLength(100);
        entity.Property(c => c.SenderId).HasMaxLength(100);
        entity.Property(c => c.FhirCommunicationRequestJson).HasColumnType("ntext");

        // Indexes
        entity.HasIndex(c => c.CommunicationRequestId).IsUnique();
        entity.HasIndex(c => c.Status);
        entity.HasIndex(c => c.Category);
        entity.HasIndex(c => c.Priority);
        entity.HasIndex(c => c.SubjectPatientId);
        entity.HasIndex(c => c.RecipientId);
        entity.HasIndex(c => c.SenderId);
        entity.HasIndex(c => c.AboutIdentifierValue);

        // Relationships
        entity.HasOne(c => c.SubjectPatient)
       .WithMany()
         .HasForeignKey(c => c.SubjectPatientId)
          .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Recipient)
                 .WithMany()
          .HasForeignKey(c => c.RecipientId)
       .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Sender)
       .WithMany()
           .HasForeignKey(c => c.SenderId)
       .OnDelete(DeleteBehavior.Restrict);
    }
}
