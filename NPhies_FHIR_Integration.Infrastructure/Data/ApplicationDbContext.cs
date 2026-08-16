using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NPhies_FHIR_Integration.Domain.Entities;
using NPhies_FHIR_Integration.Domain.CodeableConcept.Models;

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        // Suppress pending model changes warning - Phase 3 services are application-layer only
        optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
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
    /// CancellationRequest entities (for claim cancellation requests)
    /// </summary>
    public DbSet<CancellationRequest> CancellationRequests { get; set; } = null!;

    /// <summary>
    /// CancellationResponse entities (for claim cancellation responses)
    /// </summary>
    public DbSet<CancellationResponse> CancellationResponses { get; set; } = null!;

    /// <summary>
    /// Communication entities (for insurer-provider communications)
    /// </summary>
    public DbSet<Communication> Communications { get; set; } = null!;

    /// <summary>
    /// CommunicationRequest entities (insurer-provider communication requests)
    /// </summary>
    public DbSet<CommunicationRequest> CommunicationRequests { get; set; } = null!;

    /// <summary>
    /// PollingRecord entities (polling request/response history and audit trail)
    /// </summary>
    public DbSet<PollingRecord> PollingRecords { get; set; } = null!;

    /// <summary>
    /// Task entities (for polling and work items)
    /// </summary>
    // public DbSet<PollTask> Tasks { get; set; } = null!;

    // MASTER DATA TABLES

    /// <summary>
    /// ServiceCodeMaster - Medical services/procedures with NPHIES mappings
    /// </summary>
    public DbSet<ServiceCodeMaster> ServiceCodeMasters { get; set; } = null!;

    /// <summary>
    /// MedicationCodeMaster - Medications with NPHIES mappings
    /// </summary>
    public DbSet<MedicationCodeMaster> MedicationCodeMasters { get; set; } = null!;

    /// <summary>
    /// MedicalDeviceCodeMaster - Medical devices with NPHIES mappings
    /// </summary>
    public DbSet<MedicalDeviceCodeMaster> MedicalDeviceCodeMasters { get; set; } = null!;

    /// <summary>
    /// DiagnosisCodeMaster - ICD diagnosis codes with NPHIES mappings
    /// </summary>
    public DbSet<DiagnosisCodeMaster> DiagnosisCodeMasters { get; set; } = null!;

    /// <summary>
    /// ModifierCodeMaster - Procedure modifiers
    /// </summary>
    public DbSet<ModifierCodeMaster> ModifierCodeMasters { get; set; } = null!;

    /// <summary>
    /// BenefitCodeMaster - Benefit category codes
    /// </summary>
    public DbSet<BenefitCodeMaster> BenefitCodeMasters { get; set; } = null!;

    /// <summary>
    /// PayerMaster - Insurance companies/payers
    /// </summary>
    public DbSet<PayerMaster> PayerMasters { get; set; } = null!;

    /// <summary>
    /// PayerPolicyMaster - Insurance policies
    /// </summary>
    public DbSet<PayerPolicyMaster> PayerPolicyMasters { get; set; } = null!;

    /// <summary>
    /// PolicyBenefitCoverage - Benefits covered under each policy
    /// </summary>
    public DbSet<PolicyBenefitCoverage> PolicyBenefitCoverages { get; set; } = null!;

    /// <summary>
    /// ClaimSubmissionRules - Validation rules for claims
    /// </summary>
    public DbSet<ClaimSubmissionRules> ClaimSubmissionRules { get; set; } = null!;

    /// <summary>
    /// NphiesCodeMapping - Centralized code mappings
    /// </summary>
    public DbSet<NphiesCodeMapping> NphiesCodeMappings { get; set; } = null!;

    /// <summary>
    /// ClinicMaster - Extended clinic information
    /// </summary>
    public DbSet<ClinicMaster> ClinicMasters { get; set; } = null!;

    /// <summary>
    /// DoctorMaster - Extended doctor information
    /// </summary>
    public DbSet<DoctorMaster> DoctorMasters { get; set; } = null!;

    /// <summary>
    /// DoctorQualification - Doctor qualifications
    /// </summary>
    public DbSet<DoctorQualification> DoctorQualifications { get; set; } = null!;

    /// <summary>
    /// ErrorCodeMaster - NPHIES error codes (1,682 codes)
    /// </summary>
    public DbSet<ErrorCodeMaster> ErrorCodeMasters { get; set; } = null!;

    /// <summary>
    /// CodeSystem - Terminology source metadata (CodeSystems like HL7, NPHIES, WHO)
    /// </summary>
    public DbSet<CodeSystemEntity> CodeSystems { get; set; } = null!;

    /// <summary>
    /// Concept - Individual codes within each CodeSystem (~50,000+ codes)
    /// </summary>
    public DbSet<ConceptEntity> Concepts { get; set; } = null!;


    /// <summary>
    /// ValueSet - Logical groups of codes for specific use cases
    /// </summary>
    public DbSet<ValueSetEntity> ValueSets { get; set; } = null!;

    /// <summary>
    /// ValueSetCodeSystemMap - Links ValueSets to CodeSystems (M:N relationship)
    /// </summary>
    public DbSet<ValueSetCodeSystemMapEntity> ValueSetCodeSystemMaps { get; set; } = null!;

    /// <summary>
    /// ProfileElement - FHIR path bindings to ValueSets
    /// </summary>
    public DbSet<ProfileElementEntity> ProfileElements { get; set; } = null!;

    /// <summary>
    /// ConceptCodeFilter - Specific code restrictions for ValueSets
    /// </summary>
    public DbSet<ConceptCodeFilterEntity> ConceptCodeFilters { get; set; } = null!;

    /// <summary>
    /// ValidationRule - NPHIES-specific validation constraints (1,682+ rules)
    /// </summary>
    public DbSet<ValidationRuleEntity> ValidationRules { get; set; } = null!;

    /// <summary>
    /// NphiesMessageType - Message type definitions (eligibility, claim, etc.)
    /// </summary>
    public DbSet<NphiesMessageTypeEntity> NphiesMessageTypes { get; set; } = null!;

    /// <summary>
    /// NphiesMessageRequiredElement - Required fields per message type
    /// </summary>
    public DbSet<NphiesMessageRequiredElementEntity> NphiesMessageRequiredElements { get; set; } = null!;

    /// <summary>
    /// AppealRequest entities
    /// </summary>
    // TEMPORARILY REMOVED - Will be added in separate migration to avoid FK cycle issues
    // public DbSet<AppealRequest> AppealRequests { get; set; } = null!;

    /// <summary>
    /// User entities
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// Refresh token entities
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    /// <summary>
    /// Login attempt tracking
    /// </summary>
    public DbSet<LoginAttempt> LoginAttempts { get; set; } = null!;

    /// <summary>
    /// Audit logs
    /// </summary>
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    /// <summary>
    /// API rate limit logs
    /// </summary>
    public DbSet<ApiRateLimitLog> RateLimitLogs { get; set; } = null!;

    // PRE-AUTHORIZATION ENTITIES

    /// <summary>
    /// Pre-Authorization Requests
    /// </summary>
    public DbSet<PreAuthorizationRequest> PreAuthorizationRequests { get; set; } = null!;

    /// <summary>
    /// Pre-Authorization Items
    /// </summary>
    public DbSet<PreAuthorizationItem> PreAuthorizationItems { get; set; } = null!;

    /// <summary>
    /// Pre-Authorization Diagnoses
    /// </summary>
    public DbSet<PreAuthorizationDiagnosis> PreAuthorizationDiagnoses { get; set; } = null!;

    /// <summary>
    /// Pre-Authorization Supporting Info
    /// </summary>
    public DbSet<PreAuthorizationSupportingInfo> SupportingInfos { get; set; } = null!;

    /// <summary>
    /// Pre-Authorization Responses
    /// </summary>
    public DbSet<PreAuthorizationResponse> PreAuthorizationResponses { get; set; } = null!;

    /// <summary>
    /// Pre-Authorization Response Items
    /// </summary>
    public DbSet<PreAuthorizationResponseItem> PreAuthorizationResponseItems { get; set; } = null!;

    /// <summary>
    /// Pre-Authorization Response Errors
    /// </summary>
    public DbSet<PreAuthorizationResponseError> PreAuthorizationResponseErrors { get; set; } = null!;

    // CLAIM EXTENSION ENTITIES

    /// <summary>
    /// Claim Accidents
    /// </summary>
    public DbSet<ClaimAccident> ClaimAccidents { get; set; } = null!;

    /// <summary>
    /// Claim Item Modifiers
    /// </summary>
    public DbSet<ClaimItemModifier> ClaimItemModifiers { get; set; } = null!;

    /// <summary>
    /// Claim Procedures
    /// </summary>
    public DbSet<ClaimProcedure> ClaimProcedures { get; set; } = null!;

    /// <summary>
    /// Payment Reconciliations
    /// </summary>
    public DbSet<PaymentReconciliation> PaymentReconciliations { get; set; } = null!;

    /// <summary>
    /// Payment Reconciliation Details
    /// </summary>
    public DbSet<PaymentReconciliationDetail> PaymentReconciliationDetails { get; set; } = null!;

    /// <summary>
    /// Vision Prescriptions
    /// </summary>
    public DbSet<VisionPrescription> VisionPrescriptions { get; set; } = null!;

    /// <summary>
    /// Oral/Dental Details
    /// </summary>
    public DbSet<OralDetail> OralDetails { get; set; } = null!;

    /// <summary>
    /// Claim Errors
    /// </summary>
    public DbSet<ClaimError> ClaimErrors { get; set; } = null!;

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

        // Claim-related entity configurations
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

        // Task Management Configuration (Cancellation)
        ConfigureCancellationRequestEntity(modelBuilder);
        ConfigureCancellationResponseEntity(modelBuilder);

        // Communication Configuration
        ConfigureCommunicationEntity(modelBuilder);
        ConfigureCommunicationRequestEntity(modelBuilder);

        // Polling Configuration
        ConfigurePollingRecordEntity(modelBuilder);

        // PRE-AUTHORIZATION CONFIGURATION
        // NOTE: PreAuthorization entities are not yet implemented in Domain
        // Uncomment when PreAuthorization entities are added
         ConfigurePreAuthorizationRequestEntity(modelBuilder);
        ConfigurePreAuthorizationItemEntity(modelBuilder);
        ConfigurePreAuthorizationDiagnosisEntity(modelBuilder);
        ConfigurePreAuthorizationSupportingInfoEntity(modelBuilder);
        ConfigurePreAuthorizationResponseEntity(modelBuilder);
        ConfigurePreAuthorizationResponseItemEntity(modelBuilder);
        ConfigurePreAuthorizationResponseErrorEntity(modelBuilder);

        // CLAIM EXTENSION CONFIGURATION
        ConfigureClaimAccidentEntity(modelBuilder);
        ConfigureClaimItemModifierEntity(modelBuilder);
        ConfigureClaimProcedureEntity(modelBuilder);
        ConfigurePaymentReconciliationEntity(modelBuilder);
        ConfigurePaymentReconciliationDetailEntity(modelBuilder);
        ConfigureVisionPrescriptionEntity(modelBuilder);
        ConfigureOralDetailEntity(modelBuilder);
        ConfigureClaimErrorEntity(modelBuilder);

        // MASTER DATA CONFIGURATION
        ConfigureServiceCodeMasterEntity(modelBuilder);
        ConfigureMedicationCodeMasterEntity(modelBuilder);
        ConfigureMedicalDeviceCodeMasterEntity(modelBuilder);
        ConfigureDiagnosisCodeMasterEntity(modelBuilder);
        ConfigureModifierCodeMasterEntity(modelBuilder);
        ConfigureBenefitCodeMasterEntity(modelBuilder);
        ConfigurePayerMasterEntity(modelBuilder);
        ConfigurePayerPolicyMasterEntity(modelBuilder);
        ConfigurePolicyBenefitCoverageEntity(modelBuilder);
        ConfigureClaimSubmissionRulesEntity(modelBuilder);
        ConfigureNphiesCodeMappingEntity(modelBuilder);
        ConfigureClinicMasterEntity(modelBuilder);
        ConfigureDoctorMasterEntity(modelBuilder);
        ConfigureDoctorQualificationEntity(modelBuilder);
        ConfigureErrorCodeMasterEntity(modelBuilder);

        // ========== CODEABLE CONCEPT CONFIGURATION ==========
        ConfigureCodeSystemEntity(modelBuilder);
        ConfigureConceptEntity(modelBuilder);
        ConfigureValueSetEntity(modelBuilder);
        ConfigureValueSetCodeSystemMapEntity(modelBuilder);
        ConfigureProfileElementEntity(modelBuilder);
        ConfigureConceptCodeFilterEntity(modelBuilder);
        ConfigureValidationRuleEntity(modelBuilder);
        ConfigureNphiesMessageTypeEntity(modelBuilder);
        ConfigureNphiesMessageRequiredElementEntity(modelBuilder);

        // ========== USER AUTHENTICATION CONFIGURATION ==========
        ConfigureUserEntity(modelBuilder);
        ConfigureRefreshTokenEntity(modelBuilder);
        ConfigureLoginAttemptEntity(modelBuilder);
        ConfigureAuditLogEntity(modelBuilder);
        ConfigureApiRateLimitLogEntity(modelBuilder);

        // APPEAL CONFIGURATION - TEMPORARILY REMOVED to avoid FK cycles
        ConfigureAppealRequestEntity(modelBuilder);
        ConfigureAppealStatusHistoryEntity(modelBuilder);
        ConfigureAppealDocumentEntity(modelBuilder);

        // GLOBAL: Set all Id columns (not yet explicitly configured) to HasMaxLength(100)
        // This fixes FK column length mismatches systematically
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var idProperty = entity.FindProperty("Id");
            if (idProperty?.GetMaxLength() == null && idProperty?.ClrType == typeof(string))
            {
                idProperty.SetMaxLength(100);
            }
        }

        // TODO: Re-enable these entities after fixing schema design and ID column lengths
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
        entity.Property(o => o.Id).HasMaxLength(100); // Explicit length for all FK references

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

        // NEW: Saudi Arabia Specific Fields
        entity.Property(o => o.MOHLicenseNumber).HasMaxLength(50);
        entity.Property(o => o.CHINumber).HasMaxLength(50);
        entity.Property(o => o.NphiesOrganizationId).HasMaxLength(100);
        entity.Property(o => o.NphiesProviderId).HasMaxLength(100);
        entity.Property(o => o.NphiesPayerId).HasMaxLength(100);
        entity.Property(o => o.TaxRegistrationNumber).HasMaxLength(50);

        // Indexes
        entity.HasIndex(o => o.LicenseNumber).IsUnique();
        entity.HasIndex(o => o.OrganizationType);
        entity.HasIndex(o => o.Status);

        // NEW: Indexes for Saudi Arabia fields
        entity.HasIndex(o => o.MOHLicenseNumber).IsUnique().HasFilter("[MOHLicenseNumber] IS NOT NULL");
        entity.HasIndex(o => o.CHINumber).IsUnique().HasFilter("[CHINumber] IS NOT NULL");
        entity.HasIndex(o => o.NphiesOrganizationId).HasFilter("[NphiesOrganizationId] IS NOT NULL");
        entity.HasIndex(o => o.NphiesProviderId).HasFilter("[NphiesProviderId] IS NOT NULL");
        entity.HasIndex(o => o.NphiesPayerId).HasFilter("[NphiesPayerId] IS NOT NULL");
        entity.HasIndex(o => o.TaxRegistrationNumber).HasFilter("[TaxRegistrationNumber] IS NOT NULL");

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

        // NEW: Saudi Arabia Specific Fields
        entity.Property(p => p.PractitionerLicenseNumber).HasMaxLength(50);
        entity.Property(p => p.LicenseIssuingAuthority).HasMaxLength(100);
        entity.Property(p => p.LicenseExpiryDate);
        entity.Property(p => p.NationalIdentificationNumber).HasMaxLength(20);
        entity.Property(p => p.PractitionerRole).HasMaxLength(100);
        entity.Property(p => p.PractitionerRoleSystem).HasMaxLength(200);

        // Indexes
        entity.HasIndex(p => p.LicenseNumber).IsUnique();
        entity.HasIndex(p => p.Status);

        // NEW: Indexes for Saudi Arabia fields
        entity.HasIndex(p => p.PractitionerLicenseNumber).IsUnique().HasFilter("[PractitionerLicenseNumber] IS NOT NULL");
        entity.HasIndex(p => p.NationalIdentificationNumber).IsUnique().HasFilter("[NationalIdentificationNumber] IS NOT NULL");
        entity.HasIndex(p => p.LicenseExpiryDate).HasFilter("[LicenseExpiryDate] IS NOT NULL");
        entity.HasIndex(p => p.PractitionerRole).HasFilter("[PractitionerRole] IS NOT NULL");

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

        // Properties - Explicitly set Id to match FK column lengths
        entity.Property(m => m.Id).HasMaxLength(50); // Match the MessageHeaderId length in other entities

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

        // NEW: Accident Information (inline)
        entity.Property(c => c.AccidentDate);
        entity.Property(c => c.AccidentType).HasMaxLength(50);
        entity.Property(c => c.AccidentTypeSystem).HasMaxLength(200);

        // NEW: Funds Reserve
        entity.Property(c => c.FundsReserveCode).HasMaxLength(50);
        entity.Property(c => c.FundsReserveSystem).HasMaxLength(200);

        // NEW: Referral and Prescription References
        entity.Property(c => c.ReferralIdentifier).HasMaxLength(100);
        entity.Property(c => c.PrescriptionIdentifier).HasMaxLength(100);
        entity.Property(c => c.OriginalPrescriptionIdentifier).HasMaxLength(100);
        entity.Property(c => c.PreAuthorizationRef).HasMaxLength(100);

        // NEW: Billable Period
        entity.Property(c => c.BillablePeriodStart);
        entity.Property(c => c.BillablePeriodEnd);

        // Indexes
        entity.HasIndex(c => c.ClaimNumber).IsUnique();
        entity.HasIndex(c => c.Status);
        entity.HasIndex(c => c.Use);
        entity.HasIndex(c => c.PatientId);

        // NEW: Indexes for episode and offline fields
        entity.HasIndex(c => c.EpisodeIdentifierValue);
        entity.HasIndex(c => c.EligibilityOfflineReference);

        // NEW: Indexes for referral/prescription/pre-auth
        entity.HasIndex(c => c.PreAuthorizationRef);
        entity.HasIndex(c => c.ReferralIdentifier);
        entity.HasIndex(c => c.PrescriptionIdentifier);

        // Relationships
        entity.HasOne(c => c.Patient)
            .WithMany(p => p.Claims)
 .HasForeignKey(c => c.PatientId)
        .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Coverage)
       .WithMany(c => c.Claims)
       .HasForeignKey(c => c.CoverageId)
           .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Provider)
        .WithMany(o => o.SubmittedClaims)
            .HasForeignKey(c => c.ProviderId)
      .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Insurer)
           .WithMany(o => o.ProcessedClaims)
      .HasForeignKey(c => c.InsurerId)
         .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Practitioner)
        .WithMany(p => p.Claims)
 .HasForeignKey(c => c.PractitionerId)
     .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.ServiceLocation)
    .WithMany(l => l.Claims)
       .HasForeignKey(c => c.LocationId)
     .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.MessageHeader)
   .WithMany(m => m.Claims)
      .HasForeignKey(c => c.MessageHeaderId)
         .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(c => c.Items)
        .WithOne()
            .HasForeignKey(i => i.ClaimId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(c => c.Diagnoses)
      .WithOne()
            .HasForeignKey(d => d.ClaimId)
     .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(c => c.CareTeam)
            .WithOne(ct => ct.Claim)
    .HasForeignKey(ct => ct.ClaimId)
   .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(c => c.SupportingInfo)
      .WithOne(si => si.Claim)
  .HasForeignKey(si => si.ClaimId)
      .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(c => c.RelatedClaims)
   .WithOne(rc => rc.Claim)
     .HasForeignKey(rc => rc.ClaimId)
         .OnDelete(DeleteBehavior.Cascade);
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

        // NEW: Body Site and Sub-Site
        entity.Property(c => c.BodySiteCode).HasMaxLength(50);
  entity.Property(c => c.BodySiteSystem).HasMaxLength(200);
        entity.Property(c => c.SubSiteCode).HasMaxLength(50);
        entity.Property(c => c.SubSiteSystem).HasMaxLength(200);

        // NEW: Pricing Factors
        entity.Property(c => c.Factor).HasPrecision(5, 2);
  entity.Property(c => c.Tax).HasPrecision(18, 2);
        entity.Property(c => c.TaxRate).HasPrecision(5, 2);

        // NEW: Linkage to Other Claim Elements
        entity.Property(c => c.DiagnosisSequence).HasMaxLength(100);
        entity.Property(c => c.InformationSequence).HasMaxLength(100);
        entity.Property(c => c.ProcedureSequence).HasMaxLength(100);

        // NEW: Device and Location
        entity.Property(c => c.UDI).HasMaxLength(100);
        entity.Property(c => c.LocationId).HasMaxLength(100);

      // NEW: Program Code
     entity.Property(c => c.ProgramCode).HasMaxLength(50);
        entity.Property(c => c.ProgramCodeSystem).HasMaxLength(200);

        // Indexes
  entity.HasIndex(c => c.ClaimId);
  entity.HasIndex(c => c.Sequence);

        // NEW: Index for patient invoice
    entity.HasIndex(c => c.PatientInvoiceValue);

        // NEW: Indexes for new fields
  entity.HasIndex(c => c.BodySiteCode);
        entity.HasIndex(c => c.LocationId);
      entity.HasIndex(c => c.UDI);
   entity.HasIndex(c => c.ProgramCode);

        // Relationships
        entity.HasOne(c => c.Location)
     .WithMany()
  .HasForeignKey(c => c.LocationId)
  .OnDelete(DeleteBehavior.Restrict);
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
        entity.Property(c => c.ProductOrServiceDisplay).HasMaxLength(255);
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
       .WithMany(i => i.Details)
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
        entity.Property(c => c.DiagnosisDisplay).HasMaxLength(255);
        entity.Property(c => c.DiagnosisType).HasMaxLength(50);
        entity.Property(c => c.DiagnosisTypeSystem).HasMaxLength(500);
        entity.Property(c => c.OnAdmissionCode).HasMaxLength(10);
    entity.Property(c => c.OnAdmissionSystem).HasMaxLength(500);
        entity.Property(c => c.Notes).HasMaxLength(1000);

        // Indexes
    entity.HasIndex(c => c.ClaimId);
   entity.HasIndex(c => c.Sequence);
        entity.HasIndex(c => c.DiagnosisCode);

        // Relationships
     entity.HasOne(c => c.Claim)
 .WithMany(c => c.Diagnoses)
            .HasForeignKey(c => c.ClaimId)
       .OnDelete(DeleteBehavior.Cascade);
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
    entity.Property(c => c.Role).HasMaxLength(100);
        entity.Property(c => c.RoleSystem).HasMaxLength(500);
        entity.Property(c => c.RoleDisplay).HasMaxLength(255);
      entity.Property(c => c.Qualification).HasMaxLength(100);
        entity.Property(c => c.QualificationSystem).HasMaxLength(500);
     entity.Property(c => c.QualificationDisplay).HasMaxLength(255);
        entity.Property(c => c.Notes).HasMaxLength(1000);

        // Indexes
 entity.HasIndex(c => c.ClaimId);
entity.HasIndex(c => c.Sequence);
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
        entity.Property(c => c.CategoryDisplay).HasMaxLength(255);
        entity.Property(c => c.CodeValue).HasMaxLength(100);
   entity.Property(c => c.CodeSystem).HasMaxLength(500);
        entity.Property(c => c.StringValue).HasMaxLength(1000);
        entity.Property(c => c.QuantityValue).HasPrecision(18, 2);
        entity.Property(c => c.QuantityUnit).HasMaxLength(50);
     entity.Property(c => c.QuantitySystem).HasMaxLength(500);
  entity.Property(c => c.Notes).HasMaxLength(1000);

  // Indexes
entity.HasIndex(c => c.ClaimId);
        entity.HasIndex(c => c.Sequence);
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
        entity.Property(c => c.RelationshipDisplay).HasMaxLength(255);
   entity.Property(c => c.ReferencedClaimId).HasMaxLength(100);
        entity.Property(c => c.Notes).HasMaxLength(1000);

        // Indexes
        entity.HasIndex(c => c.ClaimId);
     entity.HasIndex(c => c.RelatedClaimIdentifierValue);

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
        entity.Property(c => c.ClaimSubTypeSystem).HasMaxLength(500);
        entity.Property(c => c.Use).HasMaxLength(50);
        entity.Property(c => c.PatientId).HasMaxLength(100);
 entity.Property(c => c.InsurerId).HasMaxLength(100);
        entity.Property(c => c.RequestorId).HasMaxLength(100);
        entity.Property(c => c.RequestIdentifierSystem).HasMaxLength(500);
        entity.Property(c => c.RequestIdentifierValue).HasMaxLength(100);
        entity.Property(c => c.PreAuthRef).HasMaxLength(100);
    entity.Property(c => c.AdvancedAuthReason).HasMaxLength(100);
     entity.Property(c => c.AdvancedAuthReasonSystem).HasMaxLength(500);
        entity.Property(c => c.ServiceProviderId).HasMaxLength(100);
        entity.Property(c => c.FhirClaimResponseJson).HasColumnType("ntext");

        // Indexes
        entity.HasIndex(c => c.ClaimId);
        entity.HasIndex(c => c.ResponseIdentifierValue);
      entity.HasIndex(c => c.ClaimResponseStatus);
        entity.HasIndex(c => c.PreAuthRef);

     // Relationships
      entity.HasOne(c => c.Claim)
     .WithMany()
            .HasForeignKey(c => c.ClaimId)
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
        entity.Property(c => c.Focal).IsRequired();
 entity.Property(c => c.CoverageId).HasMaxLength(100);
        entity.Property(c => c.PreAuthReferences).HasMaxLength(1000);

        // Indexes
        entity.HasIndex(c => c.ClaimResponseId);
        entity.HasIndex(c => c.CoverageId);

        // Relationships
        entity.HasOne(c => c.ClaimResponse)
     .WithMany(cr => cr.Insurance)
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

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.ClaimResponseId).IsRequired().HasMaxLength(100);
entity.Property(c => c.Sequence).IsRequired();
   entity.Property(c => c.ProductOrServiceCode).HasMaxLength(100);
        entity.Property(c => c.ProductOrServiceSystem).HasMaxLength(500);
     entity.Property(c => c.ProductOrServiceDisplay).HasMaxLength(255);
        entity.Property(c => c.BenefitAmount).HasPrecision(18, 2);
     entity.Property(c => c.BenefitCurrency).HasMaxLength(3);
        entity.Property(c => c.SubmittedAmount).HasPrecision(18, 2);
        entity.Property(c => c.Notes).HasMaxLength(1000);

        // Indexes
        entity.HasIndex(c => c.ClaimResponseId);
    entity.HasIndex(c => c.Sequence);

        // Relationships
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

        // Primary Key
   entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.ClaimResponseAddItemId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.AdjudicationCategory).IsRequired().HasMaxLength(100);
        entity.Property(c => c.AdjudicationSystem).HasMaxLength(500);
   entity.Property(c => c.AdjudicationDisplay).HasMaxLength(255);
        entity.Property(c => c.Amount).HasPrecision(18, 2);
        entity.Property(c => c.Currency).HasMaxLength(3);
        entity.Property(c => c.Notes).HasMaxLength(1000);

        // Indexes
   entity.HasIndex(c => c.ClaimResponseAddItemId);
        entity.HasIndex(c => c.AdjudicationCategory);

        // Relationships
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

        // Primary Key
        entity.HasKey(t => t.Id);

        // Properties
        entity.Property(t => t.ClaimResponseId).IsRequired().HasMaxLength(100);
        entity.Property(t => t.Category).IsRequired().HasMaxLength(100);
        entity.Property(t => t.CategorySystem).HasMaxLength(500);
        entity.Property(t => t.CategoryDisplay).HasMaxLength(255);
        entity.Property(t => t.Amount).HasPrecision(18, 2);
        entity.Property(t => t.Currency).HasMaxLength(3);
        entity.Property(t => t.Notes).HasMaxLength(1000);

        // Indexes
        entity.HasIndex(t => t.ClaimResponseId);
        entity.HasIndex(t => t.Category);

        // Relationships
        entity.HasOne(t => t.ClaimResponse)
            .WithMany(cr => cr.Totals)
            .HasForeignKey(t => t.ClaimResponseId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ClaimResponseDiagnosisExt entity
    /// </summary>
    private void ConfigureClaimResponseDiagnosisExtEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimResponseDiagnosisExt>();

        // Primary Key
        entity.HasKey(d => d.Id);

        // Properties
        entity.Property(d => d.ClaimResponseId).IsRequired().HasMaxLength(100);
        entity.Property(d => d.Sequence).IsRequired();
        entity.Property(d => d.DiagnosisCode).IsRequired().HasMaxLength(100);
        entity.Property(d => d.DiagnosisSystem).HasMaxLength(500);
        entity.Property(d => d.DiagnosisDisplay).HasMaxLength(255);
        entity.Property(d => d.DiagnosisType).HasMaxLength(50);
        entity.Property(d => d.DiagnosisTypeSystem).HasMaxLength(500);
        entity.Property(d => d.OnAdmissionCode).HasMaxLength(10);
        entity.Property(d => d.OnAdmissionSystem).HasMaxLength(500);
        entity.Property(d => d.Notes).HasMaxLength(1000);

        // Indexes
        entity.HasIndex(d => d.ClaimResponseId);
        entity.HasIndex(d => d.Sequence);
        entity.HasIndex(d => d.DiagnosisCode);

        // Relationships
        entity.HasOne(d => d.ClaimResponse)
            .WithMany(cr => cr.DiagnosesExt)
            .HasForeignKey(d => d.ClaimResponseId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ClaimResponseSupportingInfoExt entity
    /// </summary>
    private void ConfigureClaimResponseSupportingInfoExtEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimResponseSupportingInfoExt>();

        // Primary Key
        entity.HasKey(e => e.Id);

        // Properties
        entity.Property(e => e.ClaimResponseId).IsRequired().HasMaxLength(100);
        entity.Property(e => e.Sequence).IsRequired();
        entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
        entity.Property(e => e.CategorySystem).HasMaxLength(500);
        entity.Property(e => e.CategoryDisplay).HasMaxLength(255);
        entity.Property(e => e.CodeValue).HasMaxLength(100);
        entity.Property(e => e.CodeSystem).HasMaxLength(500);
        entity.Property(e => e.StringValue).HasMaxLength(1000);
        entity.Property(e => e.QuantityValue).HasPrecision(18, 2);
        entity.Property(e => e.QuantityUnit).HasMaxLength(50);
        entity.Property(e => e.QuantitySystem).HasMaxLength(500);
        entity.Property(e => e.Notes).HasMaxLength(1000);

        // Indexes
        entity.HasIndex(e => e.ClaimResponseId);
        entity.HasIndex(e => e.Sequence);
        entity.HasIndex(e => e.Category);

        // Relationships
        entity.HasOne(e => e.ClaimResponse)
        .WithMany(c => c.SupportingInfoExt)
        .HasForeignKey(e => e.ClaimResponseId)
        .OnDelete(DeleteBehavior.Cascade);
    }

    // ========== TASK MANAGEMENT & COMMUNICATION CONFIGURATION ==========

    /// <summary>
    /// Configure CancellationRequest entity
    /// </summary>
    private void ConfigureCancellationRequestEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<CancellationRequest>();

        // Primary Key
        entity.HasKey(c => c.Id);
        entity.Property(c => c.Id).HasMaxLength(100);

        // Properties
        entity.Property(c => c.TaskId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.IdentifierSystem).HasMaxLength(500);
        entity.Property(c => c.IdentifierValue).HasMaxLength(100);
        entity.Property(c => c.Status).IsRequired().HasMaxLength(50);
        entity.Property(c => c.Intent).IsRequired().HasMaxLength(50);
        entity.Property(c => c.Priority).HasMaxLength(50);
        entity.Property(c => c.Code).HasMaxLength(50);
        entity.Property(c => c.CodeSystem).HasMaxLength(500);
        entity.Property(c => c.FocusResourceType).HasMaxLength(100);
        entity.Property(c => c.FocusIdentifierSystem).HasMaxLength(500);
        entity.Property(c => c.FocusIdentifierValue).HasMaxLength(100);
        entity.Property(c => c.ReasonCode).HasMaxLength(50);
        entity.Property(c => c.ReasonCodeSystem).HasMaxLength(500);
        entity.Property(c => c.ReasonText).HasMaxLength(1000);
        entity.Property(c => c.Description).HasMaxLength(2000);
        entity.Property(c => c.FhirTaskJson).HasColumnType("ntext");
        entity.Property(c => c.MessageHeaderId).HasMaxLength(50);
        entity.Property(c => c.ProcessingStatus).IsRequired().HasMaxLength(50);

        // Indexes
        entity.HasIndex(c => c.TaskId).IsUnique();
        entity.HasIndex(c => c.Status);
        entity.HasIndex(c => c.ProcessingStatus);
        entity.HasIndex(c => c.FocusIdentifierValue);

        // Relationships
        entity.HasOne(c => c.Requester)
        .WithMany()
        .HasForeignKey(c => c.RequesterId)
        .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Owner)
        .WithMany()
        .HasForeignKey(c => c.OwnerId)
        .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure CancellationResponse entity
    /// </summary>
    private void ConfigureCancellationResponseEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<CancellationResponse>();

        // Primary Key
        entity.HasKey(c => c.Id);
        entity.Property(c => c.Id).HasMaxLength(100);

        // Properties
        entity.Property(c => c.TaskId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.IdentifierSystem).HasMaxLength(500);
        entity.Property(c => c.IdentifierValue).HasMaxLength(100);
        entity.Property(c => c.ReferencedRequestId).HasMaxLength(100);
        entity.Property(c => c.Status).IsRequired().HasMaxLength(50);
        entity.Property(c => c.Intent).IsRequired().HasMaxLength(50);
        entity.Property(c => c.Priority).HasMaxLength(50);
        entity.Property(c => c.Code).HasMaxLength(50);
        entity.Property(c => c.CodeSystem).HasMaxLength(500);
        entity.Property(c => c.FocusResourceType).HasMaxLength(100);
        entity.Property(c => c.FocusIdentifierSystem).HasMaxLength(500);
        entity.Property(c => c.FocusIdentifierValue).HasMaxLength(100);
        entity.Property(c => c.ResponseCode).HasMaxLength(50);
        entity.Property(c => c.ResponseMessage).HasMaxLength(2000);
        entity.Property(c => c.Description).HasMaxLength(2000);
        entity.Property(c => c.ResultText).HasMaxLength(2000);
        entity.Property(c => c.FhirTaskJson).HasColumnType("ntext");
        entity.Property(c => c.MessageHeaderId).HasMaxLength(50);
        entity.Property(c => c.ProcessingStatus).IsRequired().HasMaxLength(50);

        // Indexes
        entity.HasIndex(c => c.TaskId).IsUnique();
        entity.HasIndex(c => c.Status);
        entity.HasIndex(c => c.ProcessingStatus);
        entity.HasIndex(c => c.CancellationRequestId);

        // Relationships
        entity.HasOne(c => c.CancellationRequest)
        .WithMany()
        .HasForeignKey(c => c.CancellationRequestId)
        .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Requester)
        .WithMany()
        .HasForeignKey(c => c.RequesterId)
        .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Owner)
        .WithMany()
        .HasForeignKey(c => c.OwnerId)
        .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure Communication entity
    /// </summary>
    private void ConfigureCommunicationEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Communication>();

        // Primary Key
        entity.HasKey(c => c.Id);
        entity.Property(c => c.Id).HasMaxLength(100);

        // Properties
        entity.Property(c => c.CommunicationId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.IdentifierSystem).HasMaxLength(500);
        entity.Property(c => c.IdentifierValue).HasMaxLength(100);
        entity.Property(c => c.BasedOnResourceType).HasMaxLength(100);
        entity.Property(c => c.BasedOnIdentifierSystem).HasMaxLength(500);
        entity.Property(c => c.BasedOnIdentifierValue).HasMaxLength(100);
        entity.Property(c => c.Status).IsRequired().HasMaxLength(50);
        entity.Property(c => c.Category).HasMaxLength(100);
        entity.Property(c => c.CategorySystem).HasMaxLength(500);
        entity.Property(c => c.Priority).HasMaxLength(50);
        entity.Property(c => c.AboutResourceType).HasMaxLength(100);
        entity.Property(c => c.AboutIdentifierSystem).HasMaxLength(500);
        entity.Property(c => c.AboutIdentifierValue).HasMaxLength(100);
        entity.Property(c => c.PayloadContent).HasColumnType("ntext");
        entity.Property(c => c.PayloadAttachmentContentType).HasMaxLength(255);
        entity.Property(c => c.PayloadAttachmentTitle).HasMaxLength(255);
        entity.Property(c => c.FhirCommunicationJson).HasColumnType("ntext");
        entity.Property(c => c.MessageHeaderId).HasMaxLength(50);
        entity.Property(c => c.ProcessingStatus).IsRequired().HasMaxLength(50);

        // Indexes
        entity.HasIndex(c => c.CommunicationId).IsUnique();
        entity.HasIndex(c => c.Status);
        entity.HasIndex(c => c.ProcessingStatus);
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
    /// <summary>
    /// Configure AppealRequest entity
    /// </summary>
    private void ConfigureAppealRequestEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<AppealRequest>();

        // Primary Key
        entity.HasKey(a => a.Id);
        entity.Property(a => a.Id).HasMaxLength(100);

        // Properties
        entity.Property(a => a.AppealNumber).IsRequired().HasMaxLength(100);
        entity.Property(a => a.AppealIdentifierSystem).IsRequired().HasMaxLength(500);
        entity.Property(a => a.AppealIdentifierValue).IsRequired().HasMaxLength(100);
        entity.Property(a => a.ClaimId).IsRequired().HasMaxLength(100);
        entity.Property(a => a.ClaimResponseId).IsRequired().HasMaxLength(100);
        entity.Property(a => a.PatientId).IsRequired().HasMaxLength(100);
        entity.Property(a => a.InsurerId).IsRequired().HasMaxLength(100);
        entity.Property(a => a.ProviderId).IsRequired().HasMaxLength(100);
        entity.Property(a => a.AppealStatus).IsRequired().HasMaxLength(50);
        entity.Property(a => a.AppealLevel).IsRequired();
        entity.Property(a => a.ErrorCodeBeingAppealed).IsRequired().HasMaxLength(100);
        entity.Property(a => a.ErrorDescription).HasMaxLength(500);
        entity.Property(a => a.AppealReason).IsRequired().HasMaxLength(2000);
        entity.Property(a => a.SupportingDocumentation).HasMaxLength(4000);
        entity.Property(a => a.DenialDate).IsRequired();
        entity.Property(a => a.AppealDeadlineDate).IsRequired();
        entity.Property(a => a.AppealSubmittedDate);
        entity.Property(a => a.ReceivedDate);
        entity.Property(a => a.ReviewCompletedDate);
        entity.Property(a => a.ExpectedDecisionDate);
        entity.Property(a => a.AppealOutcome).HasMaxLength(50);
        entity.Property(a => a.ApprovedAmount).HasPrecision(18, 2);
        entity.Property(a => a.DecisionExplanation).HasMaxLength(2000);
        entity.Property(a => a.AllowsEscalation).IsRequired();
        entity.Property(a => a.EscalatedAppealId).HasMaxLength(100);
        entity.Property(a => a.IsActive).IsRequired();
        entity.Property(a => a.IsWithdrawn).IsRequired();
        entity.Property(a => a.WithdrawnDate);
        entity.Property(a => a.WithdrawalReason).HasMaxLength(1000);
        entity.Property(a => a.InternalReferenceNumber).HasMaxLength(100);
        entity.Property(a => a.Notes).HasMaxLength(2000);
        entity.Property(a => a.LastStatusUpdateDate).IsRequired();

        // Indexes
        entity.HasIndex(a => a.AppealNumber).IsUnique();
        entity.HasIndex(a => a.AppealIdentifierValue).IsUnique();
        entity.HasIndex(a => a.ClaimId);
        entity.HasIndex(a => a.ClaimResponseId);
        entity.HasIndex(a => a.PatientId);
        entity.HasIndex(a => a.InsurerId);
        entity.HasIndex(a => a.ProviderId);
        entity.HasIndex(a => a.AppealStatus);
        entity.HasIndex(a => a.AppealLevel);
        entity.HasIndex(a => a.ErrorCodeBeingAppealed);
        entity.HasIndex(a => a.AppealDeadlineDate);
        entity.HasIndex(a => a.AppealSubmittedDate);
        entity.HasIndex(a => a.IsActive);
        entity.HasIndex(a => a.IsWithdrawn);
        entity.HasIndex(a => new { a.AppealStatus, a.AppealDeadlineDate });
        entity.HasIndex(a => new { a.ClaimId, a.AppealLevel });

        // Relationships
        entity.HasOne(a => a.Claim)
            .WithMany()
            .HasForeignKey(a => a.ClaimId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(a => a.ClaimResponse)
            .WithMany()
            .HasForeignKey(a => a.ClaimResponseId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(a => a.Patient)
            .WithMany()
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(a => a.Insurer)
            .WithMany()
            .HasForeignKey(a => a.InsurerId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(a => a.Provider)
            .WithMany()
            .HasForeignKey(a => a.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(a => a.StatusHistory)
            .WithOne(h => h.Appeal)
            .HasForeignKey(h => h.AppealId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(a => a.AttachedDocuments)
            .WithOne(d => d.Appeal)
            .HasForeignKey(d => d.AppealId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure AppealStatusHistory entity
    /// </summary>
    private void ConfigureAppealStatusHistoryEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<AppealStatusHistory>();

        // Primary Key
        entity.HasKey(h => h.Id);
        entity.Property(h => h.Id).HasMaxLength(100);

        // Properties
        entity.Property(h => h.AppealId).IsRequired().HasMaxLength(100);
        entity.Property(h => h.Status).IsRequired().HasMaxLength(50);
        entity.Property(h => h.ChangedBy).IsRequired().HasMaxLength(100);
        entity.Property(h => h.ChangeReason).HasMaxLength(500);
        entity.Property(h => h.StatusChangeDate).IsRequired();
        entity.Property(h => h.Comments).HasMaxLength(1000);

        // Indexes
        entity.HasIndex(h => h.AppealId);
        entity.HasIndex(h => h.Status);
        entity.HasIndex(h => h.StatusChangeDate);
        entity.HasIndex(h => new { h.AppealId, h.StatusChangeDate });

        // Relationships
        entity.HasOne(h => h.Appeal)
            .WithMany(a => a.StatusHistory)
            .HasForeignKey(h => h.AppealId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure AppealDocument entity
    /// </summary>
    private void ConfigureAppealDocumentEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<AppealDocument>();

        // Primary Key
        entity.HasKey(d => d.Id);
        entity.Property(d => d.Id).HasMaxLength(100);

        // Properties
        entity.Property(d => d.AppealId).IsRequired().HasMaxLength(100);
        entity.Property(d => d.DocumentType).IsRequired().HasMaxLength(100);
        entity.Property(d => d.DocumentTitle).IsRequired().HasMaxLength(255);
        entity.Property(d => d.DocumentDescription).HasMaxLength(1000);
        entity.Property(d => d.FilePath).IsRequired().HasMaxLength(500);
        entity.Property(d => d.FileSizeBytes).IsRequired();
        entity.Property(d => d.MimeType).IsRequired().HasMaxLength(100);
        entity.Property(d => d.AttachedDate).IsRequired();
        entity.Property(d => d.IsVerified).IsRequired();
        entity.Property(d => d.Notes).HasMaxLength(1000);

        // Indexes
        entity.HasIndex(d => d.AppealId);
        entity.HasIndex(d => d.DocumentType);
        entity.HasIndex(d => d.AttachedDate);
        entity.HasIndex(d => d.IsVerified);

        // Relationships
        entity.HasOne(d => d.Appeal)
            .WithMany(a => a.AttachedDocuments)
            .HasForeignKey(d => d.AppealId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    /// <summary>
    /// Configure CommunicationRequest entity
    /// </summary>
    private void ConfigureCommunicationRequestEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<CommunicationRequest>();

        // Primary Key
        entity.HasKey(c => c.Id);
        entity.Property(c => c.Id).HasMaxLength(100);

        // Properties
        entity.Property(c => c.CommunicationRequestId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.IdentifierSystem).HasMaxLength(500);
        entity.Property(c => c.IdentifierValue).HasMaxLength(100);
        entity.Property(c => c.Status).IsRequired().HasMaxLength(50);
        entity.Property(c => c.Category).HasMaxLength(100);
        entity.Property(c => c.CategorySystem).HasMaxLength(500);
        entity.Property(c => c.Priority).HasMaxLength(50);
        entity.Property(c => c.AboutResourceType).HasMaxLength(100);
        entity.Property(c => c.AboutIdentifierSystem).HasMaxLength(500);
        entity.Property(c => c.AboutIdentifierValue).HasMaxLength(100);
        entity.Property(c => c.PayloadContent).HasColumnType("ntext");
        entity.Property(c => c.FhirCommunicationRequestJson).HasColumnType("ntext");

        // Indexes
        entity.HasIndex(c => c.CommunicationRequestId).IsUnique();
        entity.HasIndex(c => c.Status);
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

    /// <summary>
    /// Configure PollingRecord entity
    /// </summary>
    private void ConfigurePollingRecordEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PollingRecord>();

        // Primary Key
        entity.HasKey(p => p.Id);
        entity.Property(p => p.Id).HasMaxLength(100);

        // Properties
        entity.Property(p => p.PollingRecordId).IsRequired().HasMaxLength(100);
        entity.Property(p => p.ProviderId).IsRequired().HasMaxLength(100);
        entity.Property(p => p.RequestTaskId).HasMaxLength(100);
        entity.Property(p => p.RequestedMessageTypes).HasMaxLength(500);
        entity.Property(p => p.ResponseTaskId).HasMaxLength(100);
        entity.Property(p => p.ResponseStatus).HasMaxLength(50);
        entity.Property(p => p.ReceivedMessageTypes).HasMaxLength(500);
        entity.Property(p => p.RequestBundleJson).HasColumnType("ntext");
        entity.Property(p => p.ResponseBundleJson).HasColumnType("ntext");
        entity.Property(p => p.ProcessingStatus).IsRequired().HasMaxLength(50);
        entity.Property(p => p.ErrorMessage).HasMaxLength(2000);
        entity.Property(p => p.ErrorCode).HasMaxLength(100);
        entity.Property(p => p.CycleStatus).IsRequired().HasMaxLength(50);
        entity.Property(p => p.SourceIpAddress).HasMaxLength(50);
        entity.Property(p => p.RequestSourceId).HasMaxLength(100);
        entity.Property(p => p.Notes).HasMaxLength(2000);   

        // Indexes
        entity.HasIndex(p => p.PollingRecordId).IsUnique();
        entity.HasIndex(p => p.ProviderId);
        entity.HasIndex(p => p.ProcessingStatus);
        entity.HasIndex(p => p.CycleStatus);
        entity.HasIndex(p => p.RequestSentAt);

        // Relationships
        entity.HasOne(p => p.Provider)
        .WithMany()
        .HasForeignKey(p => p.ProviderId)
        .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(p => p.CancellationRequest)
        .WithMany()
        .HasForeignKey(p => p.CancellationRequestId)
        .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(p => p.CancellationResponse)
        .WithMany()
        .HasForeignKey(p => p.CancellationResponseId)
        .OnDelete(DeleteBehavior.Restrict);
    }
    /// <summary>
    /// Configure PreAuthorizationRequest entity
    /// </summary>
    private void ConfigurePreAuthorizationRequestEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PreAuthorizationRequest>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.RequestId).IsRequired().HasMaxLength(100);
        entity.Property(e => e.PatientId).IsRequired().HasMaxLength(100);
        entity.Property(e => e.ProviderId).IsRequired().HasMaxLength(100);
        entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
        entity.Property(e => e.RequestedDate).IsRequired();
        entity.Property(e => e.FhirRequestBundle).HasColumnType("ntext");

        entity.HasIndex(e => e.RequestId).IsUnique();
        entity.HasIndex(e => e.PatientId);
        entity.HasIndex(e => e.ProviderId);
        entity.HasIndex(e => e.Status);

        entity.HasMany(e => e.Items)
            .WithOne(i => i.PreAuthorizationRequest)
            .HasForeignKey(i => i.PreAuthorizationRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(e => e.Diagnoses)
            .WithOne(d => d.PreAuthorizationRequest)
            .HasForeignKey(d => d.PreAuthorizationRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(e => e.SupportingInfo)
            .WithOne(s => s.PreAuthorizationRequest)
            .HasForeignKey(s => s.PreAuthorizationRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Response)
            .WithOne(r => r.PreAuthorizationRequest)
            .HasForeignKey<PreAuthorizationResponse>(r => r.PreAuthorizationRequestId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure PreAuthorizationItem entity
    /// </summary>
    private void ConfigurePreAuthorizationItemEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PreAuthorizationItem>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.PreAuthorizationRequestId).IsRequired().HasMaxLength(100);
        entity.Property(e => e.Sequence).IsRequired();
        entity.Property(e => e.ServiceCode).IsRequired().HasMaxLength(100);
        entity.Property(e => e.ServiceSystem).HasMaxLength(500);
        entity.Property(e => e.Quantity).HasPrecision(18, 2);
        entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
        entity.Property(e => e.Notes).HasMaxLength(1000);

        entity.HasIndex(e => e.PreAuthorizationRequestId);
        entity.HasIndex(e => e.ServiceCode);

        entity.HasOne(e => e.PreAuthorizationRequest)
            .WithMany(r => r.Items)
            .HasForeignKey(e => e.PreAuthorizationRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure PreAuthorizationDiagnosis entity
    /// </summary>
    private void ConfigurePreAuthorizationDiagnosisEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PreAuthorizationDiagnosis>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.PreAuthorizationRequestId).IsRequired().HasMaxLength(100);
        entity.Property(e => e.Sequence).IsRequired();
        entity.Property(e => e.DiagnosisCode).IsRequired().HasMaxLength(100);
        entity.Property(e => e.DiagnosisSystem).HasMaxLength(500);
        entity.Property(e => e.DiagnosisDisplay).HasMaxLength(255);
        entity.Property(e => e.Notes).HasMaxLength(1000);

        entity.HasIndex(e => e.PreAuthorizationRequestId);
        entity.HasIndex(e => e.DiagnosisCode);

        entity.HasOne(e => e.PreAuthorizationRequest)
            .WithMany(r => r.Diagnoses)
            .HasForeignKey(e => e.PreAuthorizationRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure PreAuthorizationSupportingInfo entity
    /// </summary>
    private void ConfigurePreAuthorizationSupportingInfoEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PreAuthorizationSupportingInfo>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.PreAuthorizationRequestId).IsRequired().HasMaxLength(100);
        entity.Property(e => e.Sequence).IsRequired();
        entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
        entity.Property(e => e.CategorySystem).HasMaxLength(500);
        entity.Property(e => e.CodeValue).HasMaxLength(100);
        entity.Property(e => e.CodeSystem).HasMaxLength(500);
        entity.Property(e => e.StringValue).HasMaxLength(1000);
        entity.Property(e => e.QuantityValue).HasPrecision(18, 2);
        entity.Property(e => e.QuantityUnit).HasMaxLength(50);
        entity.Property(e => e.QuantitySystem).HasMaxLength(500);
        entity.Property(e => e.Notes).HasMaxLength(1000);

        entity.HasIndex(e => e.PreAuthorizationRequestId);
        entity.HasIndex(e => e.Category);

        entity.HasOne(e => e.PreAuthorizationRequest)
            .WithMany(r => r.SupportingInfo)
            .HasForeignKey(e => e.PreAuthorizationRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure PreAuthorizationResponse entity
    /// </summary>
    private void ConfigurePreAuthorizationResponseEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PreAuthorizationResponse>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.PreAuthorizationRequestId).IsRequired().HasMaxLength(100);
        entity.Property(e => e.ResponseId).IsRequired().HasMaxLength(100);
        entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
        entity.Property(e => e.Outcome).IsRequired().HasMaxLength(50);
        entity.Property(e => e.FhirResponseBundle).HasColumnType("ntext");

        entity.HasIndex(e => e.PreAuthorizationRequestId).IsUnique();
        entity.HasIndex(e => e.ResponseId);
        entity.HasIndex(e => e.Status);

        entity.HasOne(e => e.PreAuthorizationRequest)
            .WithOne(r => r.Response)
            .HasForeignKey<PreAuthorizationResponse>(e => e.PreAuthorizationRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(e => e.ResponseItems)
            .WithOne(i => i.PreAuthorizationResponse)
            .HasForeignKey(i => i.PreAuthorizationResponseId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(e => e.Errors)
            .WithOne(er => er.PreAuthorizationResponse)
            .HasForeignKey(er => er.PreAuthorizationResponseId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure PreAuthorizationResponseItem entity
    /// </summary>
    private void ConfigurePreAuthorizationResponseItemEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PreAuthorizationResponseItem>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.PreAuthorizationResponseId).IsRequired().HasMaxLength(100);
        entity.Property(e => e.Sequence).IsRequired();
        entity.Property(e => e.ServiceCode).IsRequired().HasMaxLength(100);
        entity.Property(e => e.ServiceSystem).HasMaxLength(500);
        entity.Property(e => e.ApprovedQuantity).HasPrecision(18, 2);
        entity.Property(e => e.ApprovedUnitPrice).HasPrecision(18, 2);
        entity.Property(e => e.Notes).HasMaxLength(1000);

        entity.HasIndex(e => e.PreAuthorizationResponseId);
        entity.HasIndex(e => e.ServiceCode);

        entity.HasOne(e => e.PreAuthorizationResponse)
            .WithMany(r => r.ResponseItems)
            .HasForeignKey(e => e.PreAuthorizationResponseId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure PreAuthorizationResponseError entity
    /// </summary>
    private void ConfigurePreAuthorizationResponseErrorEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PreAuthorizationResponseError>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.PreAuthorizationResponseId).IsRequired().HasMaxLength(100);
        entity.Property(e => e.ErrorCode).IsRequired().HasMaxLength(100);
        entity.Property(e => e.ErrorMessage).IsRequired().HasMaxLength(1000);
        entity.Property(e => e.ErrorDetails).HasMaxLength(2000);
        entity.Property(e => e.Severity).IsRequired().HasMaxLength(50);
        entity.Property(e => e.ErrorLocation).HasMaxLength(255);
        entity.Property(e => e.ErrorField).HasMaxLength(255);
        entity.Property(e => e.AdditionalContext).HasMaxLength(1000);

        entity.HasIndex(e => e.PreAuthorizationResponseId);
        entity.HasIndex(e => e.ErrorCode);
        entity.HasIndex(e => e.Severity);

        entity.HasOne(e => e.PreAuthorizationResponse)
            .WithMany(r => r.Errors)
            .HasForeignKey(e => e.PreAuthorizationResponseId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ClaimAccident entity
    /// </summary>
    private void ConfigureClaimAccidentEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimAccident>();

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.ClaimId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.AccidentDate).IsRequired();
        entity.Property(c => c.AccidentType).IsRequired().HasMaxLength(50);
        entity.Property(c => c.AccidentTypeSystem).HasMaxLength(200);
        entity.Property(c => c.AccidentLocation).HasMaxLength(500);
        entity.Property(c => c.AccidentLocationCity).HasMaxLength(100);
        entity.Property(c => c.AccidentLocationState).HasMaxLength(100);
        entity.Property(c => c.AccidentLocationCountry).HasMaxLength(2);

        // Indexes
        entity.HasIndex(c => c.ClaimId);
        entity.HasIndex(c => c.AccidentDate);
        entity.HasIndex(c => c.AccidentType);

        // Relationships
        entity.HasOne(c => c.Claim)
            .WithMany()
            .HasForeignKey(c => c.ClaimId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ClaimItemModifier entity
    /// </summary>
    private void ConfigureClaimItemModifierEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimItemModifier>();

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.ClaimItemId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.ModifierCode).IsRequired().HasMaxLength(50);
        entity.Property(c => c.ModifierSystem).HasMaxLength(200);
        entity.Property(c => c.ModifierDisplay).HasMaxLength(255);

        // Indexes
        entity.HasIndex(c => c.ClaimItemId);
        entity.HasIndex(c => c.ModifierCode);

        // Relationships
        entity.HasOne(c => c.ClaimItem)
            .WithMany()
            .HasForeignKey(c => c.ClaimItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ClaimProcedure entity
    /// </summary>
    private void ConfigureClaimProcedureEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimProcedure>();

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.ClaimId).IsRequired().HasMaxLength(100);
        entity.Property(c => c.ProcedureCode).IsRequired().HasMaxLength(100);
        entity.Property(c => c.ProcedureSystem).HasMaxLength(500);
        entity.Property(c => c.ProcedureDisplay).HasMaxLength(255);
        entity.Property(c => c.ProcedureDate);
        entity.Property(c => c.ProcedureType).HasMaxLength(50);
        entity.Property(c => c.ProcedureTypeSystem).HasMaxLength(500);

        // Indexes
        entity.HasIndex(c => c.ClaimId);
        entity.HasIndex(c => c.ProcedureCode);
        entity.HasIndex(c => c.ProcedureDate);

        // Relationships
        entity.HasOne(c => c.Claim)
            .WithMany()
            .HasForeignKey(c => c.ClaimId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    /// <summary>
    /// Configure Task entity (placeholder - using CancellationRequest/Response for task management)
    /// </summary>
    private void ConfigureTaskEntity(ModelBuilder modelBuilder)
    {
        // NOTE: Task functionality is implemented through CancellationRequest and CancellationResponse entities
        // which are already configured. This method is kept as a placeholder for potential future
        // generic Task entity if needed.

        // No configuration needed - tasks are handled by:
        // - CancellationRequest (ConfigureCancellationRequestEntity)
        // - CancellationResponse (ConfigureCancellationResponseEntity)
        // - PollingRecord (ConfigurePollingRecordEntity)
    }

    /// <summary>
    /// Configure PaymentNotice entity
    /// </summary>
    private void ConfigurePaymentNoticeEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PaymentNotice>();

        // Primary Key
        entity.HasKey(p => p.Id);
        entity.Property(p => p.Id).HasMaxLength(100);

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
        entity.Property(p => p.Currency).IsRequired().HasMaxLength(3);
        entity.Property(p => p.PaymentStatus).HasMaxLength(50);
        entity.Property(p => p.PaymentStatusSystem).HasMaxLength(500);
        entity.Property(p => p.ProviderId).HasMaxLength(100);
        entity.Property(p => p.PayeeId).HasMaxLength(100);
        entity.Property(p => p.RecipientSystem).HasMaxLength(500);
        entity.Property(p => p.RecipientValue).HasMaxLength(100);
        entity.Property(p => p.FhirPaymentNoticeJson).HasColumnType("ntext");

        // Indexes
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
    /// Configure PaymentReconciliation entity
    /// </summary>
    private void ConfigurePaymentReconciliationEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PaymentReconciliation>();

        // Primary Key
        entity.HasKey(p => p.Id);

        // Properties
        entity.Property(p => p.PaymentReconciliationId).IsRequired().HasMaxLength(100);
        entity.Property(p => p.PaymentAmount).IsRequired().HasPrecision(18, 2);
        entity.Property(p => p.FhirPaymentReconciliationJson).HasColumnType("ntext");

        // Indexes
        entity.HasIndex(p => p.PaymentReconciliationId).IsUnique();

        // Relationships
        entity.HasOne(p => p.PaymentIssuer)
            .WithMany()
            .HasForeignKey("PaymentIssuerId")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany<PaymentReconciliationDetail>()
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
        entity.HasKey(p => p.Id);

        // Properties
        entity.Property(p => p.PaymentReconciliationId).IsRequired().HasMaxLength(100);
        entity.Property(p => p.ComponentPayment).HasPrecision(18, 2);

        // Indexes
        entity.HasIndex(p => p.PaymentReconciliationId);

        // Relationships
        entity.HasOne(p => p.PaymentReconciliation)
            .WithMany()
            .HasForeignKey(p => p.PaymentReconciliationId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure VisionPrescription entity
    /// </summary>
    private void ConfigureVisionPrescriptionEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<VisionPrescription>();

        // Primary Key
        entity.HasKey(v => v.Id);

        // Properties
        entity.Property(v => v.ClaimItemId).IsRequired().HasMaxLength(100);

        // Indexes
        entity.HasIndex(v => v.ClaimItemId);

        // Relationships
        entity.HasOne(v => v.ClaimItem)
            .WithMany()
            .HasForeignKey(v => v.ClaimItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure OralDetail entity
    /// </summary>
    private void ConfigureOralDetailEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<OralDetail>();

        // Primary Key
        entity.HasKey(o => o.Id);

        // Properties
        entity.Property(o => o.ClaimItemId).IsRequired().HasMaxLength(100);

        // Indexes
        entity.HasIndex(o => o.ClaimItemId);

        // Relationships
        entity.HasOne(o => o.ClaimItem)
            .WithMany()
            .HasForeignKey(o => o.ClaimItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ClaimError entity
    /// </summary>
    private void ConfigureClaimErrorEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimError>();

        // Primary Key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.ClaimId).HasMaxLength(100);
        entity.Property(c => c.ClaimResponseId).HasMaxLength(100);
        entity.Property(c => c.ErrorCode).IsRequired().HasMaxLength(100);
        entity.Property(c => c.ErrorCodeSystem).HasMaxLength(500);
        entity.Property(c => c.ErrorSeverity).IsRequired().HasMaxLength(50);
        entity.Property(c => c.ErrorDescription).HasMaxLength(2000);
        entity.Property(c => c.ErrorDetails).HasMaxLength(2000);
        entity.Property(c => c.ErrorPath).HasMaxLength(500);
        entity.Property(c => c.ErrorExpression).HasMaxLength(500);

        // Indexes
        entity.HasIndex(c => c.ClaimId);
        entity.HasIndex(c => c.ClaimResponseId);
        entity.HasIndex(c => c.ErrorCode);
        entity.HasIndex(c => c.ErrorSeverity);

        // Relationships
        entity.HasOne(c => c.Claim)
            .WithMany()
            .HasForeignKey(c => c.ClaimId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(c => c.ClaimResponse)
            .WithMany()
            .HasForeignKey(c => c.ClaimResponseId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ServiceCodeMaster entity
    /// </summary>
    private void ConfigureServiceCodeMasterEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ServiceCodeMaster>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.ServiceCode).IsRequired().HasMaxLength(50);
        entity.Property(e => e.ServiceCategory).IsRequired().HasMaxLength(100);
        entity.Property(e => e.NphiesServiceCode).HasMaxLength(50);
        entity.Property(e => e.MappingValidationStatus).HasMaxLength(50);

        entity.HasIndex(e => e.ServiceCode).IsUnique();
        entity.HasIndex(e => e.ServiceCategory);
        entity.HasIndex(e => e.NphiesServiceCode);
    }

    /// <summary>
    /// Configure MedicationCodeMaster entity
    /// </summary>
    private void ConfigureMedicationCodeMasterEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<MedicationCodeMaster>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.MedicationCode).IsRequired().HasMaxLength(50);
        entity.Property(e => e.NphiesMedicationCode).HasMaxLength(50);

        entity.HasIndex(e => e.MedicationCode).IsUnique();
        entity.HasIndex(e => e.NphiesMedicationCode);
    }

    /// <summary>
    /// Configure MedicalDeviceCodeMaster entity
    /// </summary>
    private void ConfigureMedicalDeviceCodeMasterEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<MedicalDeviceCodeMaster>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.DeviceCode).IsRequired().HasMaxLength(50);
        entity.Property(e => e.DeviceType).HasMaxLength(100);
        entity.Property(e => e.NphiesDeviceCode).HasMaxLength(50);

        entity.HasIndex(e => e.DeviceCode).IsUnique();
        entity.HasIndex(e => e.DeviceType);
        entity.HasIndex(e => e.NphiesDeviceCode);
    }

    /// <summary>
    /// Configure DiagnosisCodeMaster entity
    /// </summary>
    private void ConfigureDiagnosisCodeMasterEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<DiagnosisCodeMaster>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.DiagnosisCode).IsRequired().HasMaxLength(20);
        entity.Property(e => e.NphiesDiagnosisCode).HasMaxLength(20);

        entity.HasIndex(e => e.DiagnosisCode).IsUnique();
        entity.HasIndex(e => e.NphiesDiagnosisCode);
    }

    /// <summary>
    /// Configure ModifierCodeMaster entity
    /// </summary>
    private void ConfigureModifierCodeMasterEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ModifierCodeMaster>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.ModifierCode).IsRequired().HasMaxLength(10);
        entity.Property(e => e.NphiesModifierCode).HasMaxLength(10);

        entity.HasIndex(e => e.ModifierCode).IsUnique();
        entity.HasIndex(e => e.NphiesModifierCode);
    }

    /// <summary>
    /// Configure BenefitCodeMaster entity
    /// </summary>
    private void ConfigureBenefitCodeMasterEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<BenefitCodeMaster>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.BenefitCode).IsRequired().HasMaxLength(50);

        entity.HasIndex(e => e.BenefitCode).IsUnique();
    }

    /// <summary>
    /// Configure PayerMaster entity
    /// </summary>
    private void ConfigurePayerMasterEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PayerMaster>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.PayerId).IsRequired().HasMaxLength(100);
        entity.Property(e => e.NphiesPayerId).HasMaxLength(100);

        entity.HasIndex(e => e.PayerId).IsUnique();
        entity.HasIndex(e => e.NphiesPayerId);
    }

    /// <summary>
    /// Configure PayerPolicyMaster entity
    /// </summary>
    private void ConfigurePayerPolicyMasterEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PayerPolicyMaster>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.PayerMasterId).IsRequired().HasMaxLength(100);
        entity.Property(e => e.PolicyCode).IsRequired().HasMaxLength(100);
        entity.Property(e => e.PolicyType).HasMaxLength(100);
        entity.Property(e => e.CoverageType).HasMaxLength(100);

        entity.HasIndex(e => e.PayerMasterId);
        entity.HasIndex(e => e.PolicyCode);

        entity.HasOne(e => e.Payer)
            .WithMany()
            .HasForeignKey(e => e.PayerMasterId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure PolicyBenefitCoverage entity
    /// </summary>
    private void ConfigurePolicyBenefitCoverageEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PolicyBenefitCoverage>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.PolicyMasterId).IsRequired().HasMaxLength(100);
        entity.Property(e => e.ServiceCodeMasterId).HasMaxLength(100);
        entity.Property(e => e.BenefitType).HasMaxLength(100);

        entity.HasIndex(e => e.PolicyMasterId);
        entity.HasIndex(e => e.ServiceCodeMasterId);

        entity.HasOne(e => e.Policy)
            .WithMany()
            .HasForeignKey(e => e.PolicyMasterId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(e => e.ServiceCode)
            .WithMany()
            .HasForeignKey(e => e.ServiceCodeMasterId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure ClaimSubmissionRules entity
    /// </summary>
    private void ConfigureClaimSubmissionRulesEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClaimSubmissionRules>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.RuleType).HasMaxLength(100);

        entity.HasIndex(e => e.RuleType);
    }

    /// <summary>
    /// Configure NphiesCodeMapping entity
    /// </summary>
    private void ConfigureNphiesCodeMappingEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<NphiesCodeMapping>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.NphiesCode).IsRequired().HasMaxLength(50);
        entity.Property(e => e.NphiesCodeSystem).IsRequired().HasMaxLength(500);
        entity.Property(e => e.CodeType).IsRequired().HasMaxLength(50);

        entity.HasIndex(e => new { e.NphiesCode, e.NphiesCodeSystem }).IsUnique();
        entity.HasIndex(e => e.CodeType);
    }

    /// <summary>
    /// Configure ClinicMaster entity
    /// </summary>
    private void ConfigureClinicMasterEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ClinicMaster>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.ClinicCode).IsRequired().HasMaxLength(50);
        entity.Property(e => e.ClinicType).HasMaxLength(100);

        entity.HasIndex(e => e.ClinicCode).IsUnique();
        entity.HasIndex(e => e.ClinicType);
    }

    /// <summary>
    /// Configure DoctorMaster entity
    /// </summary>
    private void ConfigureDoctorMasterEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<DoctorMaster>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);
        entity.Property(e => e.PractitionerId).IsRequired().HasMaxLength(100);
        entity.Property(e => e.DoctorCode).IsRequired().HasMaxLength(50);

        entity.HasIndex(e => e.PractitionerId).IsUnique();
        entity.HasIndex(e => e.DoctorCode).IsUnique();

        entity.HasOne(e => e.Practitioner)
            .WithMany()
            .HasForeignKey(e => e.PractitionerId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure DoctorQualification entity
    /// </summary>
    
    private void ConfigureDoctorQualificationEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<DoctorQualification>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);

        entity.Property(e => e.DoctorMasterId)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(e => e.QualificationType)
            .IsRequired()
            .HasMaxLength(100);

        entity.HasIndex(e => e.DoctorMasterId);
        entity.HasIndex(e => e.QualificationType);

        entity.HasOne(e => e.Doctor)                // FIXED
            .WithMany(d => d.Qualifications)
            .HasForeignKey(e => e.DoctorMasterId)
            .OnDelete(DeleteBehavior.Cascade);
    }


    /// <summary>
    /// Configure ErrorCodeMaster entity
    /// </summary>
    private void ConfigureErrorCodeMasterEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ErrorCodeMaster>();

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasMaxLength(100);

        // Additional properties will be configured based on ErrorCodeMaster entity definition
    }

    /// <summary>
    /// Configure CodeSystemEntity
    /// </summary>
    private void ConfigureCodeSystemEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<CodeSystemEntity>();

        entity.HasKey(e => e.CodeSystemId);
        entity.Property(e => e.CodeSystemId).ValueGeneratedOnAdd();

        entity.HasIndex(e => e.Url).IsUnique();
        entity.HasIndex(e => e.Name);
        entity.HasIndex(e => e.Version);
    }

    /// <summary>
    /// Configure ConceptEntity
    /// </summary>
    private void ConfigureConceptEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ConceptEntity>();

        entity.HasKey(e => e.ConceptId);
        entity.Property(e => e.ConceptId).ValueGeneratedOnAdd();
        entity.Property(e => e.Code).IsRequired().HasMaxLength(100);

        entity.HasIndex(e => new { e.CodeSystemId, e.Code }).IsUnique();
        entity.HasIndex(e => e.Code);

        entity.HasOne(e => e.CodeSystem)
            .WithMany()
            .HasForeignKey(e => e.CodeSystemId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ValueSetEntity
    /// </summary>
    private void ConfigureValueSetEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ValueSetEntity>();

        entity.HasKey(e => e.ValueSetId);
        entity.Property(e => e.ValueSetId).ValueGeneratedOnAdd();

        entity.HasIndex(e => e.Url).IsUnique();
        entity.HasIndex(e => e.Name);
        entity.HasIndex(e => e.Version);
    }

    /// <summary>
    /// Configure ValueSetCodeSystemMapEntity
    /// </summary>
    private void ConfigureValueSetCodeSystemMapEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ValueSetCodeSystemMapEntity>();

        entity.HasKey(e => e.ValueSetCodeSystemMapId);
        entity.Property(e => e.ValueSetCodeSystemMapId).ValueGeneratedOnAdd();

        entity.HasIndex(e => new { e.ValueSetId, e.CodeSystemId }).IsUnique();

        entity.HasOne(e => e.ValueSet)
            .WithMany()
            .HasForeignKey(e => e.ValueSetId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.CodeSystem)
            .WithMany()
            .HasForeignKey(e => e.CodeSystemId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ProfileElementEntity
    /// </summary>
    private void ConfigureProfileElementEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ProfileElementEntity>();

        entity.HasKey(e => e.ProfileElementId);
        entity.Property(e => e.ProfileElementId).ValueGeneratedOnAdd();
        entity.Property(e => e.ProfileName).IsRequired().HasMaxLength(100);
        entity.Property(e => e.MessageType).HasMaxLength(100);

        entity.HasIndex(e => new { e.ProfileName, e.ElementPath }).IsUnique();
        entity.HasIndex(e => e.ValueSetId);
        entity.HasIndex(e => e.MessageType);

        entity.HasOne(e => e.ValueSet)
            .WithMany()
            .HasForeignKey(e => e.ValueSetId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure ConceptCodeFilterEntity
    /// </summary>
    private void ConfigureConceptCodeFilterEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ConceptCodeFilterEntity>();

        entity.HasKey(e => e.ConceptCodeFilterId);
        entity.Property(e => e.ConceptCodeFilterId).ValueGeneratedOnAdd();
        entity.Property(e => e.Code).IsRequired().HasMaxLength(100);

        entity.HasIndex(e => new { e.ValueSetId, e.Code }).IsUnique();

        entity.HasOne(e => e.ValueSet)
            .WithMany()
            .HasForeignKey(e => e.ValueSetId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure ValidationRuleEntity
    /// </summary>
    private void ConfigureValidationRuleEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ValidationRuleEntity>();

        entity.HasKey(e => e.ValidationRuleId);
        entity.Property(e => e.ValidationRuleId).ValueGeneratedOnAdd();
        entity.Property(e => e.ErrorCode).IsRequired().HasMaxLength(50);
        entity.Property(e => e.RuleType).HasMaxLength(100);

        entity.HasIndex(e => e.ErrorCode);
        entity.HasIndex(e => e.RuleType);
        entity.HasIndex(e => e.ValueSetId);

        entity.HasOne(e => e.ValueSet)
            .WithMany()
            .HasForeignKey(e => e.ValueSetId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure NphiesMessageTypeEntity
    /// </summary>
    private void ConfigureNphiesMessageTypeEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<NphiesMessageTypeEntity>();

        entity.HasKey(e => e.NphiesMessageTypeId);
        entity.Property(e => e.NphiesMessageTypeId).ValueGeneratedOnAdd();
        entity.Property(e => e.MessageType).IsRequired().HasMaxLength(100);
        entity.Property(e => e.MessageTypeArabic).HasMaxLength(100);
        entity.Property(e => e.FhirResourceType).IsRequired().HasMaxLength(100);

        entity.HasIndex(e => e.MessageType).IsUnique();
        entity.HasIndex(e => e.FhirResourceType);
    }

    /// <summary>
    /// Configure NphiesMessageRequiredElementEntity
    /// </summary>
    private void ConfigureNphiesMessageRequiredElementEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<NphiesMessageRequiredElementEntity>();

        entity.HasKey(e => e.NphiesMessageRequiredElementId);
        entity.Property(e => e.NphiesMessageRequiredElementId).ValueGeneratedOnAdd();

        entity.HasIndex(e => new { e.NphiesMessageTypeId, e.ElementPath }).IsUnique();

        entity.HasOne(e => e.NphiesMessageType)
            .WithMany()
            .HasForeignKey(e => e.NphiesMessageTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    // ========== USER AUTHENTICATION CONFIGURATION ==========

    /// <summary>
    /// Configure User entity
    /// </summary>
    private void ConfigureUserEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<User>();

        // Primary Key
        entity.HasKey(u => u.Id);

   // Properties
  entity.Property(u => u.Id).HasMaxLength(100);
        entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
 entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
        entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
     entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
        entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(500);
        entity.Property(u => u.PasswordSalt).IsRequired().HasMaxLength(500);
   
        // Convert List<string> to comma-separated string for storage
        entity.Property(u => u.Roles).HasConversion(
  v => string.Join(",", v),
 v => v.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList()
        ).HasMaxLength(1000);

        entity.Property(u => u.IsActive).IsRequired();
     entity.Property(u => u.IsEmailVerified).IsRequired();
  entity.Property(u => u.IsMfaEnabled).IsRequired();
        entity.Property(u => u.MfaSecret).HasMaxLength(255);
        entity.Property(u => u.IsLocked).IsRequired();
        entity.Property(u => u.FailedLoginAttempts).IsRequired();
        entity.Property(u => u.LastLoginAt);
    entity.Property(u => u.LockedUntilAt);
        entity.Property(u => u.CreatedAt).IsRequired();
        entity.Property(u => u.UpdatedAt);
        entity.Property(u => u.DeletedAt);
        entity.Property(u => u.CreatedBy).HasMaxLength(100);
        entity.Property(u => u.UpdatedBy).HasMaxLength(100);
        entity.Property(u => u.OrganizationId).HasMaxLength(100);
        entity.Property(u => u.DepartmentId).HasMaxLength(100);

        // Indexes
        entity.HasIndex(u => u.Username).IsUnique();
        entity.HasIndex(u => u.Email).IsUnique();
  entity.HasIndex(u => u.IsActive);
 entity.HasIndex(u => u.IsLocked);
        entity.HasIndex(u => u.OrganizationId);
   entity.HasIndex(u => u.DepartmentId);
      entity.HasIndex(u => u.CreatedAt);

        // Relationships
        entity.HasMany(u => u.AuditLogs)
       .WithOne(a => a.User)
       .HasForeignKey(a => a.UserId)
 .OnDelete(DeleteBehavior.Restrict);

     entity.HasMany(u => u.RefreshTokens)
   .WithOne(r => r.User)
  .HasForeignKey(r => r.UserId)
         .OnDelete(DeleteBehavior.Cascade);

      entity.HasMany(u => u.LoginAttempts)
       .WithOne(l => l.User)
 .HasForeignKey(l => l.UserId)
  .OnDelete(DeleteBehavior.Restrict);

entity.HasMany(u => u.RateLimitLogs)
    .WithOne(r => r.User)
   .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure RefreshToken entity
    /// </summary>
    private void ConfigureRefreshTokenEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<RefreshToken>();

        // Primary Key
        entity.HasKey(r => r.Id);

        // Properties
entity.Property(r => r.Id).HasMaxLength(100);
        entity.Property(r => r.UserId).IsRequired().HasMaxLength(100);
        entity.Property(r => r.Token).IsRequired().HasMaxLength(500);
        entity.Property(r => r.ExpiresAt).IsRequired();
        entity.Property(r => r.CreatedAt).IsRequired();
        entity.Property(r => r.IpAddress).HasMaxLength(50);
        entity.Property(r => r.UserAgent).HasMaxLength(500);
        entity.Property(r => r.IsRevoked).IsRequired();
      entity.Property(r => r.RevokedAt);
        entity.Property(r => r.RevokedBy).HasMaxLength(100);
 entity.Property(r => r.ReplacedByToken).HasMaxLength(500);

        // Indexes
        entity.HasIndex(r => r.Token).IsUnique();
        entity.HasIndex(r => r.UserId);
        entity.HasIndex(r => r.ExpiresAt);
  entity.HasIndex(r => r.IsRevoked);
 entity.HasIndex(r => r.CreatedAt);

        // Relationships
        entity.HasOne(r => r.User)
      .WithMany(u => u.RefreshTokens)
    .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure LoginAttempt entity
    /// </summary>
    private void ConfigureLoginAttemptEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<LoginAttempt>();

        // Primary Key
   entity.HasKey(l => l.Id);

        // Properties
        entity.Property(l => l.Id).HasMaxLength(100);
        entity.Property(l => l.UserId).HasMaxLength(100);
    entity.Property(l => l.Username).IsRequired().HasMaxLength(100);
        entity.Property(l => l.IpAddress).IsRequired().HasMaxLength(50);
      entity.Property(l => l.UserAgent).HasMaxLength(500);
 entity.Property(l => l.IsSuccessful).IsRequired();
        entity.Property(l => l.FailureReason).HasMaxLength(500);
        entity.Property(l => l.AttemptAt).IsRequired();
        entity.Property(l => l.DurationMs);

        // Indexes
        entity.HasIndex(l => l.UserId);
        entity.HasIndex(l => l.Username);
        entity.HasIndex(l => l.IpAddress);
        entity.HasIndex(l => l.IsSuccessful);
   entity.HasIndex(l => l.AttemptAt);
        entity.HasIndex(l => new { l.Username, l.AttemptAt });

    // Relationships
        entity.HasOne(l => l.User)
          .WithMany(u => u.LoginAttempts)
     .HasForeignKey(l => l.UserId)
         .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure AuditLog entity
    /// </summary>
    private void ConfigureAuditLogEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<AuditLog>();

    // Primary Key
 entity.HasKey(a => a.Id);

        // Properties
        entity.Property(a => a.Id).HasMaxLength(100);
        entity.Property(a => a.UserId).HasMaxLength(100);
   entity.Property(a => a.Username).HasMaxLength(100);
      entity.Property(a => a.Action).IsRequired().HasMaxLength(100);
     entity.Property(a => a.EntityType).HasMaxLength(100);
    entity.Property(a => a.EntityId).HasMaxLength(100);
        entity.Property(a => a.OldValues).HasColumnType("ntext");
        entity.Property(a => a.NewValues).HasColumnType("ntext");
        entity.Property(a => a.ChangeDetails).HasColumnType("ntext");
   entity.Property(a => a.IpAddress).IsRequired().HasMaxLength(50);
        entity.Property(a => a.UserAgent).HasMaxLength(500);
  entity.Property(a => a.CreatedAt).IsRequired();
        entity.Property(a => a.AuditLevel).IsRequired().HasMaxLength(50);
entity.Property(a => a.Endpoint).HasMaxLength(500);
        entity.Property(a => a.HttpStatusCode);
     entity.Property(a => a.DurationMs);

        // Indexes
        entity.HasIndex(a => a.UserId);
        entity.HasIndex(a => a.Username);
        entity.HasIndex(a => a.Action);
        entity.HasIndex(a => a.EntityType);
      entity.HasIndex(a => a.EntityId);
 entity.HasIndex(a => a.CreatedAt);
     entity.HasIndex(a => a.AuditLevel);
        entity.HasIndex(a => new { a.EntityType, a.EntityId });
     entity.HasIndex(a => new { a.UserId, a.CreatedAt });

     // Relationships
     entity.HasOne(a => a.User)
            .WithMany(u => u.AuditLogs)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Configure ApiRateLimitLog entity
    /// </summary>
    private void ConfigureApiRateLimitLogEntity(ModelBuilder modelBuilder)
    {
   var entity = modelBuilder.Entity<ApiRateLimitLog>();

        // Primary Key
        entity.HasKey(r => r.Id);

        // Properties
      entity.Property(r => r.Id).HasMaxLength(100);
        entity.Property(r => r.UserId).HasMaxLength(100);
        entity.Property(r => r.IpAddress).IsRequired().HasMaxLength(50);
      entity.Property(r => r.Endpoint).IsRequired().HasMaxLength(500);
     entity.Property(r => r.HttpMethod).IsRequired().HasMaxLength(10);
        entity.Property(r => r.RequestCount).IsRequired();
        entity.Property(r => r.MaxRequests).IsRequired();
        entity.Property(r => r.WindowStart).IsRequired();
        entity.Property(r => r.WindowEnd).IsRequired();
        entity.Property(r => r.IsRateLimited).IsRequired();
      entity.Property(r => r.CreatedAt).IsRequired();
  entity.Property(r => r.ResetAt);

        // Indexes
        entity.HasIndex(r => r.UserId);
        entity.HasIndex(r => r.IpAddress);
 entity.HasIndex(r => r.Endpoint);
        entity.HasIndex(r => r.IsRateLimited);
        entity.HasIndex(r => r.CreatedAt);
        entity.HasIndex(r => new { r.IpAddress, r.Endpoint, r.WindowStart });
        entity.HasIndex(r => new { r.UserId, r.CreatedAt });

        // Relationships
        entity.HasOne(r => r.User)
     .WithMany(u => u.RateLimitLogs)
 .HasForeignKey(r => r.UserId)
  .OnDelete(DeleteBehavior.Restrict);
    }
}
