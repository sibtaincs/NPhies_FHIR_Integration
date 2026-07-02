using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NPhies_FHIR_Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMasterDataTables : Migration
    {
     /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
   {
   // Create all master data tables without modifying existing tables
  
            migrationBuilder.CreateTable(
      name: "BenefitCodeMaster",
  columns: table => new
                {
          Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
   BenefitCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
    BenefitName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
  BenefitCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
      IsActive = table.Column<bool>(type: "bit", nullable: false),
         CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
         ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
 CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
 },
constraints: table =>
  {
    table.PrimaryKey("PK_BenefitCodeMaster", x => x.Id);
      });

            migrationBuilder.CreateTable(
        name: "DiagnosisCodeMaster",
                columns: table => new
 {
    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
           DiagnosisCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
         DiagnosisName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
  DiagnosisCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DiagnosisType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
    IsOnAdmission = table.Column<bool>(type: "bit", nullable: false),
   NphiesDiagnosisCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
        IsNphiesMapped = table.Column<bool>(type: "bit", nullable: false),
Severity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
    RequiresDocumentation = table.Column<bool>(type: "bit", nullable: false),
     IsActive = table.Column<bool>(type: "bit", nullable: false),
         CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
             ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
          CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
           },
         constraints: table =>
    {
     table.PrimaryKey("PK_DiagnosisCodeMaster", x => x.Id);
             });

            migrationBuilder.CreateTable(
           name: "MedicalDeviceCodeMaster",
         columns: table => new
    {
           Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
   DeviceCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
          DeviceName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
         DeviceDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
   DeviceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
        Manufacturer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
        NphiesDeviceCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
      IsNphiesMapped = table.Column<bool>(type: "bit", nullable: false),
             UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
      CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
      IsImplantable = table.Column<bool>(type: "bit", nullable: false),
     IsReusable = table.Column<bool>(type: "bit", nullable: false),
         IsActive = table.Column<bool>(type: "bit", nullable: false),
         CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
      ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
              CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
     },
     constraints: table =>
       {
    table.PrimaryKey("PK_MedicalDeviceCodeMaster", x => x.Id);
                });

         migrationBuilder.CreateTable(
           name: "MedicationCodeMaster",
          columns: table => new
  {
 Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
        MedicationCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
         MedicationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ActiveIngredient = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
        Strength = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
           Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
          Form = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        Manufacturer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
     NphiesMedicationCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
          IsNphiesMapped = table.Column<bool>(type: "bit", nullable: false),
    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
    IsControlledSubstance = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
          CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
         ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
               CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
       UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
            {
     table.PrimaryKey("PK_MedicationCodeMaster", x => x.Id);
           });

        migrationBuilder.CreateTable(
        name: "ModifierCodeMaster",
     columns: table => new
     {
      Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
  ModifierCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
     ModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
 ModifierDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
       ModifierType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
    ImpactOnCharges = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
ChargePercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
      NphiesModifierCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
     IsActive = table.Column<bool>(type: "bit", nullable: false),
          CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
         ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
     CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
         },
  constraints: table =>
      {
        table.PrimaryKey("PK_ModifierCodeMaster", x => x.Id);
      });

         migrationBuilder.CreateTable(
                name: "NphiesCodeMapping",
       columns: table => new
     {
   Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
        LocalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
           LocalCodeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
        LocalDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
       NphiesCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
             NphiesCodeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
     NphiesDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
    CodeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
         IsMappingValid = table.Column<bool>(type: "bit", nullable: false),
     MappingValidationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
             Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
      ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
               CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
  UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
     IsActive = table.Column<bool>(type: "bit", nullable: false)
     },
      constraints: table =>
          {
table.PrimaryKey("PK_NphiesCodeMapping", x => x.Id);
     });

            migrationBuilder.CreateTable(
    name: "PayerMaster",
        columns: table => new
       {
       Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
         PayerId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
 PayerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
      PayerType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
               NphiesConnectionStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
     NphiesApiEndpoint = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
         IsNphiesMember = table.Column<bool>(type: "bit", nullable: false),
          SupportedClaimTypes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
             SupportedEligibilityTypes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
  MaxClaimsPerDay = table.Column<int>(type: "int", nullable: false),
      MaxClaimAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
        CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
    IsActive = table.Column<bool>(type: "bit", nullable: false),
          CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
         ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
         CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
       UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
      constraints: table =>
      {
          table.PrimaryKey("PK_PayerMaster", x => x.Id);
  });

            migrationBuilder.CreateTable(
  name: "ServiceCodeMaster",
      columns: table => new
     {
      Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
ServiceCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
ServiceName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
       ServiceDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
 ServiceCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
             NphiesServiceCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
         NphiesServiceName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
  NphiesCategoryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
         IsNphiesMapped = table.Column<bool>(type: "bit", nullable: false),
            MappingValidationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
         DefaultPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
     CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
          IsRequiresAuthorization = table.Column<bool>(type: "bit", nullable: false),
    DefaultAuthorizationDays = table.Column<int>(type: "int", nullable: true),
        IsActive = table.Column<bool>(type: "bit", nullable: false),
           CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
     ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
       CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
       UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
   },
           constraints: table =>
       {
table.PrimaryKey("PK_ServiceCodeMaster", x => x.Id);
           });

     migrationBuilder.CreateTable(
      name: "ClinicMaster",
             columns: table => new
                {
  Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
       OrganizationId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
             ClinicName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ClinicCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
  ClinicType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
  SpecializedServices = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
          NumberOfBeds = table.Column<int>(type: "int", nullable: true),
         NumberOfDoctors = table.Column<int>(type: "int", nullable: true),
   NumberOfNurses = table.Column<int>(type: "int", nullable: true),
          IsCertifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
           AccreditationLevel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
          WorkingHoursFrom = table.Column<TimeOnly>(type: "time", nullable: true),
         WorkingHoursTo = table.Column<TimeOnly>(type: "time", nullable: true),
       IsEmergencyAvailable = table.Column<bool>(type: "bit", nullable: false),
      PharmacyAvailable = table.Column<bool>(type: "bit", nullable: false),
      LabAvailable = table.Column<bool>(type: "bit", nullable: false),
    ImagingAvailable = table.Column<bool>(type: "bit", nullable: false),
                    AcceptsCashPayment = table.Column<bool>(type: "bit", nullable: false),
      AcceptsInsurance = table.Column<bool>(type: "bit", nullable: false),
          AcceptsCardPayment = table.Column<bool>(type: "bit", nullable: false),
          AvailableBeds = table.Column<int>(type: "int", nullable: true),
 IsActive = table.Column<bool>(type: "bit", nullable: false),
 CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
     ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
         CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
     UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
        },
      constraints: table =>
      {
          table.PrimaryKey("PK_ClinicMaster", x => x.Id);
        table.ForeignKey(
 name: "FK_ClinicMaster_Organizations_OrganizationId",
  column: x => x.OrganizationId,
     principalTable: "Organizations",
       principalColumn: "Id",
 onDelete: ReferentialAction.Cascade);
                });

   migrationBuilder.CreateTable(
                name: "PayerPolicyMaster",
         columns: table => new
        {
    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
          PayerMasterId = table.Column<string>(type: "nvarchar(100)", nullable: false),
        PolicyCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
         PolicyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
      PolicyType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
       CoverageType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                 AnnualPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
 CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
    AnnualDeductible = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
      MaxOutOfPocket = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
           Copay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
    CoinsurancePercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
    CoverageLimitPerVisit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
       CoverageLimitPerYear = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
      PreAuthRequiredForAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
         EffectiveFromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
       EffectiveToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
           IsPolicyActive = table.Column<bool>(type: "bit", nullable: false),
      Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
        CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
     ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
           CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
     UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
          IsActive = table.Column<bool>(type: "bit", nullable: false)
 },
       constraints: table =>
     {
       table.PrimaryKey("PK_PayerPolicyMaster", x => x.Id);
              table.ForeignKey(
     name: "FK_PayerPolicyMaster_PayerMaster_PayerMasterId",
     column: x => x.PayerMasterId,
             principalTable: "PayerMaster",
     principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
 });

    migrationBuilder.CreateTable(
          name: "DoctorMaster",
       columns: table => new
  {
         Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            PractitionerId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
       DoctorCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
     DoctorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
          Specialization = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
        SubSpecialization = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
             QualificationDegree = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        UniversityName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            YearsOfExperience = table.Column<int>(type: "int", nullable: true),
           IsConsultant = table.Column<bool>(type: "bit", nullable: false),
     ConsultationFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
               FollowupFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
     CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
         ClinicMasterId = table.Column<string>(type: "nvarchar(100)", nullable: true),
     IsAvailableForAppointments = table.Column<bool>(type: "bit", nullable: false),
          AvailableSlotsPerDay = table.Column<int>(type: "int", nullable: true),
Board = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
        BoardLicenseNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
      BoardLicenseExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
            ResearchPapers = table.Column<int>(type: "int", nullable: true),
      IsTeachingMember = table.Column<bool>(type: "bit", nullable: false),
   IsActive = table.Column<bool>(type: "bit", nullable: false),
     CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
        ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
           CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
     UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
       },
    constraints: table =>
     {
     table.PrimaryKey("PK_DoctorMaster", x => x.Id);
         table.ForeignKey(
                name: "FK_DoctorMaster_ClinicMaster_ClinicMasterId",
    column: x => x.ClinicMasterId,
       principalTable: "ClinicMaster",
       principalColumn: "Id",
      onDelete: ReferentialAction.Restrict);
          table.ForeignKey(
         name: "FK_DoctorMaster_Practitioners_PractitionerId",
          column: x => x.PractitionerId,
         principalTable: "Practitioners",
   principalColumn: "Id",
         onDelete: ReferentialAction.Cascade);
                });

   migrationBuilder.CreateTable(
 name: "PolicyBenefitCoverage",
            columns: table => new
       {
   Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
    PolicyMasterId = table.Column<string>(type: "nvarchar(100)", nullable: false),
              ServiceCodeMasterId = table.Column<string>(type: "nvarchar(100)", nullable: true),
        ServiceCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
    BenefitType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
         CoveragePercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
      MaxCoverageAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
    RequiresPreAuth = table.Column<bool>(type: "bit", nullable: false),
        RequiresReferral = table.Column<bool>(type: "bit", nullable: false),
             PreAuthValidityDays = table.Column<int>(type: "int", nullable: true),
          CoverageLimitPerYear = table.Column<int>(type: "int", nullable: true),
        CoverageLimitPerLifetime = table.Column<int>(type: "int", nullable: true),
        IsExcluded = table.Column<bool>(type: "bit", nullable: false),
     ExclusionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
      IsWaitingPeriodApplicable = table.Column<bool>(type: "bit", nullable: false),
    WaitingPeriodDays = table.Column<int>(type: "int", nullable: true),
        Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
           CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
   ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
         CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
   UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
          IsActive = table.Column<bool>(type: "bit", nullable: false)
         },
 constraints: table =>
  {
   table.PrimaryKey("PK_PolicyBenefitCoverage", x => x.Id);
        table.ForeignKey(
          name: "FK_PolicyBenefitCoverage_PayerPolicyMaster_PolicyMasterId",
  column: x => x.PolicyMasterId,
            principalTable: "PayerPolicyMaster",
      principalColumn: "Id",
     onDelete: ReferentialAction.Cascade);
        table.ForeignKey(
            name: "FK_PolicyBenefitCoverage_ServiceCodeMaster_ServiceCodeMasterId",
   column: x => x.ServiceCodeMasterId,
  principalTable: "ServiceCodeMaster",
        principalColumn: "Id");
   });

      migrationBuilder.CreateTable(
       name: "DoctorQualification",
           columns: table => new
       {
           Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
      DoctorMasterId = table.Column<string>(type: "nvarchar(100)", nullable: false),
      QualificationType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
        QualificationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
  UniversityName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
         IsExpiring = table.Column<bool>(type: "bit", nullable: false),
             ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
  CertificateNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
         VerificationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
       CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
          CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
     UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
       IsActive = table.Column<bool>(type: "bit", nullable: false)
     },
        constraints: table =>
      {
            table.PrimaryKey("PK_DoctorQualification", x => x.Id);
           table.ForeignKey(
 name: "FK_DoctorQualification_DoctorMaster_DoctorMasterId",
  column: x => x.DoctorMasterId,
      principalTable: "DoctorMaster",
     principalColumn: "Id",
         onDelete: ReferentialAction.Cascade);
    });

       migrationBuilder.CreateTable(
          name: "ClaimSubmissionRules",
    columns: table => new
  {
          Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
        PayerMasterId = table.Column<string>(type: "nvarchar(100)", nullable: true),
         PolicyMasterId = table.Column<string>(type: "nvarchar(100)", nullable: true),
          RuleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
  RuleType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
 RuleCondition = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
             MaxClaimAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
     MaxItemsPerClaim = table.Column<int>(type: "int", nullable: true),
        RequiresInvoice = table.Column<bool>(type: "bit", nullable: false),
         RequiresMedicalReport = table.Column<bool>(type: "bit", nullable: false),
         RequiresPhotos = table.Column<bool>(type: "bit", nullable: false),
            MaxDaysForSubmission = table.Column<int>(type: "int", nullable: true),
           IsActive = table.Column<bool>(type: "bit", nullable: false),
          Priority = table.Column<int>(type: "int", nullable: false),
          CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
         ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
              CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
  UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
      },
         constraints: table =>
      {
         table.PrimaryKey("PK_ClaimSubmissionRules", x => x.Id);
     table.ForeignKey(
         name: "FK_ClaimSubmissionRules_PayerMaster_PayerMasterId",
      column: x => x.PayerMasterId,
     principalTable: "PayerMaster",
   principalColumn: "Id");
     table.ForeignKey(
        name: "FK_ClaimSubmissionRules_PayerPolicyMaster_PolicyMasterId",
column: x => x.PolicyMasterId,
    principalTable: "PayerPolicyMaster",
         principalColumn: "Id");
                });

        // Create all indexes
         migrationBuilder.CreateIndex("IX_BenefitCodeMaster_BenefitCode", "BenefitCodeMaster", "BenefitCode", unique: true);
            migrationBuilder.CreateIndex("IX_BenefitCodeMaster_IsActive", "BenefitCodeMaster", "IsActive");
      migrationBuilder.CreateIndex("IX_DiagnosisCodeMaster_DiagnosisCode", "DiagnosisCodeMaster", "DiagnosisCode", unique: true);
            migrationBuilder.CreateIndex("IX_DiagnosisCodeMaster_DiagnosisCategory", "DiagnosisCodeMaster", "DiagnosisCategory");
         migrationBuilder.CreateIndex("IX_DiagnosisCodeMaster_IsActive", "DiagnosisCodeMaster", "IsActive");
            migrationBuilder.CreateIndex("IX_MedicalDeviceCodeMaster_DeviceCode", "MedicalDeviceCodeMaster", "DeviceCode", unique: true);
            migrationBuilder.CreateIndex("IX_MedicalDeviceCodeMaster_IsActive", "MedicalDeviceCodeMaster", "IsActive");
    migrationBuilder.CreateIndex("IX_MedicationCodeMaster_MedicationCode", "MedicationCodeMaster", "MedicationCode", unique: true);
            migrationBuilder.CreateIndex("IX_MedicationCodeMaster_IsActive", "MedicationCodeMaster", "IsActive");
       migrationBuilder.CreateIndex("IX_ModifierCodeMaster_ModifierCode", "ModifierCodeMaster", "ModifierCode", unique: true);
migrationBuilder.CreateIndex("IX_ModifierCodeMaster_IsActive", "ModifierCodeMaster", "IsActive");
          migrationBuilder.CreateIndex("IX_NphiesCodeMapping_LocalCode_LocalCodeSystem", "NphiesCodeMapping", new[] { "LocalCode", "LocalCodeSystem" }, unique: true, filter: "[LocalCodeSystem] IS NOT NULL");
   migrationBuilder.CreateIndex("IX_NphiesCodeMapping_NphiesCode", "NphiesCodeMapping", "NphiesCode");
          migrationBuilder.CreateIndex("IX_NphiesCodeMapping_CodeType", "NphiesCodeMapping", "CodeType");
            migrationBuilder.CreateIndex("IX_NphiesCodeMapping_IsMappingValid", "NphiesCodeMapping", "IsMappingValid");
    migrationBuilder.CreateIndex("IX_PayerMaster_PayerId", "PayerMaster", "PayerId", unique: true);
            migrationBuilder.CreateIndex("IX_PayerMaster_IsActive", "PayerMaster", "IsActive");
   migrationBuilder.CreateIndex("IX_ServiceCodeMaster_ServiceCode", "ServiceCodeMaster", "ServiceCode", unique: true);
            migrationBuilder.CreateIndex("IX_ServiceCodeMaster_ServiceCategory", "ServiceCodeMaster", "ServiceCategory");
            migrationBuilder.CreateIndex("IX_ServiceCodeMaster_IsNphiesMapped", "ServiceCodeMaster", "IsNphiesMapped");
    migrationBuilder.CreateIndex("IX_ServiceCodeMaster_IsActive", "ServiceCodeMaster", "IsActive");
            migrationBuilder.CreateIndex("IX_ClinicMaster_ClinicCode", "ClinicMaster", "ClinicCode", unique: true);
  migrationBuilder.CreateIndex("IX_ClinicMaster_OrganizationId", "ClinicMaster", "OrganizationId");
migrationBuilder.CreateIndex("IX_ClinicMaster_IsActive", "ClinicMaster", "IsActive");
         migrationBuilder.CreateIndex("IX_PayerPolicyMaster_PayerMasterId", "PayerPolicyMaster", "PayerMasterId");
            migrationBuilder.CreateIndex("IX_PayerPolicyMaster_PolicyCode", "PayerPolicyMaster", "PolicyCode", unique: true);
         migrationBuilder.CreateIndex("IX_PayerPolicyMaster_IsPolicyActive", "PayerPolicyMaster", "IsPolicyActive");
            migrationBuilder.CreateIndex("IX_DoctorMaster_PractitionerId", "DoctorMaster", "PractitionerId");
      migrationBuilder.CreateIndex("IX_DoctorMaster_ClinicMasterId", "DoctorMaster", "ClinicMasterId");
          migrationBuilder.CreateIndex("IX_DoctorMaster_DoctorCode", "DoctorMaster", "DoctorCode", unique: true);
   migrationBuilder.CreateIndex("IX_DoctorMaster_BoardLicenseNumber", "DoctorMaster", "BoardLicenseNumber");
            migrationBuilder.CreateIndex("IX_DoctorMaster_IsActive", "DoctorMaster", "IsActive");
            migrationBuilder.CreateIndex("IX_PolicyBenefitCoverage_PolicyMasterId", "PolicyBenefitCoverage", "PolicyMasterId");
            migrationBuilder.CreateIndex("IX_PolicyBenefitCoverage_ServiceCodeMasterId", "PolicyBenefitCoverage", "ServiceCodeMasterId");
            migrationBuilder.CreateIndex("IX_DoctorQualification_DoctorMasterId", "DoctorQualification", "DoctorMasterId");
            migrationBuilder.CreateIndex("IX_ClaimSubmissionRules_PayerMasterId", "ClaimSubmissionRules", "PayerMasterId");
     migrationBuilder.CreateIndex("IX_ClaimSubmissionRules_PolicyMasterId", "ClaimSubmissionRules", "PolicyMasterId");
            migrationBuilder.CreateIndex("IX_ClaimSubmissionRules_IsActive", "ClaimSubmissionRules", "IsActive");
        }

        /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
        {
          migrationBuilder.DropTable("ClaimSubmissionRules");
  migrationBuilder.DropTable("DoctorQualification");
            migrationBuilder.DropTable("PolicyBenefitCoverage");
     migrationBuilder.DropTable("DoctorMaster");
            migrationBuilder.DropTable("PayerPolicyMaster");
            migrationBuilder.DropTable("ClinicMaster");
         migrationBuilder.DropTable("BenefitCodeMaster");
          migrationBuilder.DropTable("DiagnosisCodeMaster");
            migrationBuilder.DropTable("MedicalDeviceCodeMaster");
     migrationBuilder.DropTable("MedicationCodeMaster");
            migrationBuilder.DropTable("ModifierCodeMaster");
            migrationBuilder.DropTable("NphiesCodeMapping");
            migrationBuilder.DropTable("PayerMaster");
            migrationBuilder.DropTable("ServiceCodeMaster");
     }
    }
}
