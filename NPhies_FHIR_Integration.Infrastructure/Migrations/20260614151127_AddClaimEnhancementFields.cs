using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NPhies_FHIR_Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClaimEnhancementFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrganizationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LicenseSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LicenseTypeSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseUse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FhirId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProviderTypeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderTypeSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecializationType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPhoneUse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressLine1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MRN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NationalIdSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalIdType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OccupationSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deceased = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhoneSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneUse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressLine1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LocationLicense = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LicenseSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OrganizationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FacilityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FacilityTypeDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageHeaders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MessageUUID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CorrelationId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EventCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SenderOrganizationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SenderLicenseSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderOrganizationLicense = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DestinationEndpoint = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DestinationOrganizationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationLicenseSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FocusResourceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FocusResourceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SourceName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SourceEndpoint = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MessageTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponseTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResponseStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResponseIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    BundleContent = table.Column<string>(type: "ntext", nullable: false),
                    ResponseBundleContent = table.Column<string>(type: "ntext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageHeaders_Organizations_SenderOrganizationId",
                        column: x => x.SenderOrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentNotices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentNoticeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PaymentIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentStatusSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProviderId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RecipientSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RecipientValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FhirPaymentNoticeJson = table.Column<string>(type: "ntext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentNotices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentNotices_Organizations_PayeeId",
                        column: x => x.PayeeId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentNotices_Organizations_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentReconciliations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentReconciliationId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Outcome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Disposition = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    PaymentMethodType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentMethodSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PaymentIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PaymentIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentIssuerId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RequestorId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FhirPaymentReconciliationJson = table.Column<string>(type: "ntext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentReconciliations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentReconciliations_Organizations_PaymentIssuerId",
                        column: x => x.PaymentIssuerId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentReconciliations_Organizations_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Practitioners",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LicenseSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Qualification = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OrganizationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Practitioners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Practitioners_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommunicationRequestId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CategorySystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubjectPatientId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AboutResourceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AboutIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AboutIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayloadContent = table.Column<string>(type: "nvarchar(2000)", nullable: true),
                    RecipientId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SenderId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FhirCommunicationRequestJson = table.Column<string>(type: "ntext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunicationRequests_Organizations_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommunicationRequests_Organizations_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommunicationRequests_Patients_SubjectPatientId",
                        column: x => x.SubjectPatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Coverages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PolicyNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MemberID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CoverageIdentifierSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverageIdentifierValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverageType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CoverageTypeSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanCodeSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverageClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubscriberRelationship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriberId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubscriberPatientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dependent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelationToSubscriber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PolicyHolderOrganizationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubscriberMRN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Subrogation = table.Column<bool>(type: "bit", nullable: false),
                    MaxCopay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxCopayCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoinsurancePercentValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CoverageClassType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverageClassValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InsurerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CoverageStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CoverageEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnnualDeductible = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DeductibleMet = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Copay = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CoinsurancePercent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    OutOfPocketMax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coverages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coverages_Organizations_InsurerId",
                        column: x => x.InsurerId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Coverages_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Encounters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EncounterId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Class = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ServiceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ServiceTypeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdmitSource = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AdmitSourceSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ServiceEventType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ServiceEventTypeSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntendedLengthOfStay = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IntendedLengthOfStaySystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceProviderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FhirEncounterJson = table.Column<string>(type: "ntext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encounters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Encounters_Organizations_ServiceProviderId",
                        column: x => x.ServiceProviderId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Encounters_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TaskId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TaskIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TaskIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Intent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CodeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CodeDisplay = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AuthoredOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequesterId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PollInputType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PollInputValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FocusResourceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FocusIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FocusIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReasonCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReasonCodeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OutputType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OutputTypeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OutputBundleId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OutputBundleReference = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResponseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ResponseIdentifier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MetaTag = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FhirTaskJson = table.Column<string>(type: "ntext", nullable: true),
                    MessageHeaderId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_MessageHeaders_MessageHeaderId",
                        column: x => x.MessageHeaderId,
                        principalTable: "MessageHeaders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tasks_Organizations_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Organizations_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentReconciliationDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentReconciliationId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DetailType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequestIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequestReference = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResponseIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResponseIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ResponseReference = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DetailDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AmountCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    SubmitterId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ComponentPayment = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    EarlyFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    NphiesFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentReconciliationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentReconciliationDetails_Organizations_PayeeId",
                        column: x => x.PayeeId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentReconciliationDetails_Organizations_SubmitterId",
                        column: x => x.SubmitterId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentReconciliationDetails_PaymentReconciliations_PaymentReconciliationId",
                        column: x => x.PaymentReconciliationId,
                        principalTable: "PaymentReconciliations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoverageEligibilityRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MessageUUID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RequestIdentifierSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestIdentifierValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageHeaderId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PrioritySystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurposeJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServicedPeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServicedPeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CoverageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InsurerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntererPractitionerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequestCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponseId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EligibilityStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MessageStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FhirRequestBundle = table.Column<string>(type: "ntext", nullable: false),
                    FhirResponseBundle = table.Column<string>(type: "ntext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageEligibilityRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoverageEligibilityRequests_Coverages_CoverageId",
                        column: x => x.CoverageId,
                        principalTable: "Coverages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoverageEligibilityRequests_MessageHeaders_MessageHeaderId",
                        column: x => x.MessageHeaderId,
                        principalTable: "MessageHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoverageEligibilityRequests_Organizations_InsurerId",
                        column: x => x.InsurerId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoverageEligibilityRequests_Organizations_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoverageEligibilityRequests_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoverageEligibilityRequests_Practitioners_EntererPractitionerId",
                        column: x => x.EntererPractitionerId,
                        principalTable: "Practitioners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClaimIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ClaimIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClaimTypeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ClaimSubType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ClaimSubTypeSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Use = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServicedPeriodStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServicedPeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PrioritySystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CoverageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InsurerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PractitionerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LocationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MessageHeaderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EncounterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PayeeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PayeeTypeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    FhirClaimBundle = table.Column<string>(type: "ntext", nullable: true),
                    EpisodeIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EpisodeIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EligibilityOfflineReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EligibilityOfflineDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthorizationOfflineDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Claims_Coverages_CoverageId",
                        column: x => x.CoverageId,
                        principalTable: "Coverages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Claims_Encounters_EncounterId",
                        column: x => x.EncounterId,
                        principalTable: "Encounters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Claims_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Claims_MessageHeaders_MessageHeaderId",
                        column: x => x.MessageHeaderId,
                        principalTable: "MessageHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Claims_Organizations_InsurerId",
                        column: x => x.InsurerId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Claims_Organizations_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Claims_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Claims_Practitioners_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Practitioners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CoverageEligibilityResponses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ResponseUUID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RequestIdentifierSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestIdentifierValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseIdentifierSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseIdentifierValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EligibilityRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MessageHeaderId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Outcome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProcessingStatus = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ResponsePurpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disposition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteEligibility = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteEligibilitySystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponseReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServicedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BenefitPeriodStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BenefitPeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EligibilityStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsInForce = table.Column<bool>(type: "bit", nullable: false),
                    ServicedPeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServicedPeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsurerId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CoverageId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsInNetwork = table.Column<bool>(type: "bit", nullable: false),
                    NetworkStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NetworkName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CoveredServicesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExcludedServicesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LimitationsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FhirResponseContent = table.Column<string>(type: "ntext", nullable: false),
                    ExplanationOfBenefits = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageEligibilityResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoverageEligibilityResponses_CoverageEligibilityRequests_EligibilityRequestId",
                        column: x => x.EligibilityRequestId,
                        principalTable: "CoverageEligibilityRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoverageEligibilityResponses_Coverages_CoverageId",
                        column: x => x.CoverageId,
                        principalTable: "Coverages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoverageEligibilityResponses_MessageHeaders_MessageHeaderId",
                        column: x => x.MessageHeaderId,
                        principalTable: "MessageHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoverageEligibilityResponses_Organizations_InsurerId",
                        column: x => x.InsurerId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoverageEligibilityResponses_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EligibilityItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EligibilityRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SequenceNumber = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategorySystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CategoryDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProductOrServiceCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProductOrServiceSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ProductOrServiceDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DiagnosisCodes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EligibilityItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EligibilityItems_CoverageEligibilityRequests_EligibilityRequestId",
                        column: x => x.EligibilityRequestId,
                        principalTable: "CoverageEligibilityRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimCareTeams",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    PractitionerId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RoleSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RoleDisplay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qualification = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QualificationSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    QualificationDisplay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimCareTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimCareTeams_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimCareTeams_Practitioners_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Practitioners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClaimDiagnoses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    DiagnosisCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DiagnosisSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DiagnosisDisplay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnosisType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DiagnosisTypeSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnAdmissionCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    OnAdmissionSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimDiagnoses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimDiagnoses_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    CareTeamSequence = table.Column<int>(type: "int", nullable: true),
                    ProductOrServiceCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProductOrServiceSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProductOrServiceDisplay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AltProductOrServiceCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AltProductOrServiceSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ServicedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServicedPeriodStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServicedPeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Net = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PatientShare = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PatientShareCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPackage = table.Column<bool>(type: "bit", nullable: false),
                    IsMaternity = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PatientInvoiceSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PatientInvoiceValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimItems_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimRelatedClaims",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RelatedClaimIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RelatedClaimIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Relationship = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RelationshipSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RelationshipDisplay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferencedClaimId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimRelatedClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimRelatedClaims_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimResponses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ResponseIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResponseIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClaimResponseStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ClaimTypeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ClaimSubType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ClaimSubTypeSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Use = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InsurerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequestIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PreAuthRef = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PreAuthPeriodStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PreAuthPeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdvancedAuthReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdvancedAuthReasonSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceProviderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FhirClaimResponseJson = table.Column<string>(type: "ntext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimResponses_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimResponses_Organizations_InsurerId",
                        column: x => x.InsurerId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimResponses_Organizations_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimResponses_Organizations_ServiceProviderId",
                        column: x => x.ServiceProviderId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimResponses_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClaimSupportingInfos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategorySystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CategoryDisplay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CodeSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StringValue = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    QuantityValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    QuantityUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    QuantitySystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateValue = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BooleanValue = table.Column<bool>(type: "bit", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimSupportingInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimSupportingInfos_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BenefitBalances",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EligibilityResponseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SequenceNumber = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategorySystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CategoryDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenefitBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BenefitBalances_CoverageEligibilityResponses_EligibilityResponseId",
                        column: x => x.EligibilityResponseId,
                        principalTable: "CoverageEligibilityResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EligibilityErrors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EligibilityRequestId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EligibilityResponseId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ErrorCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ErrorCodeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ErrorDetails = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ErrorLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ErrorField = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HttpStatusCode = table.Column<int>(type: "int", nullable: true),
                    ErrorOccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdditionalContext = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EligibilityErrors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EligibilityErrors_CoverageEligibilityRequests_EligibilityRequestId",
                        column: x => x.EligibilityRequestId,
                        principalTable: "CoverageEligibilityRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EligibilityErrors_CoverageEligibilityResponses_EligibilityResponseId",
                        column: x => x.EligibilityResponseId,
                        principalTable: "CoverageEligibilityResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EligibilityItemModifiers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EligibilityItemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModifierCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ModifierDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EligibilityItemModifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EligibilityItemModifiers_EligibilityItems_EligibilityItemId",
                        column: x => x.EligibilityItemId,
                        principalTable: "EligibilityItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimItemDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimItemId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    ProductOrServiceCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProductOrServiceSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProductOrServiceDisplay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AltProductOrServiceCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AltProductOrServiceSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Net = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimItemDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimItemDetails_ClaimItems_ClaimItemId",
                        column: x => x.ClaimItemId,
                        principalTable: "ClaimItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimResponseAddItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimResponseId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    ProductOrServiceCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProductOrServiceSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProductOrServiceDisplay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedQuantity = table.Column<int>(type: "int", nullable: true),
                    BenefitAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    BenefitCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DiagnosisSequence = table.Column<int>(type: "int", nullable: true),
                    InformationSequence = table.Column<int>(type: "int", nullable: true),
                    IsMaternity = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimResponseAddItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimResponseAddItems_ClaimResponses_ClaimResponseId",
                        column: x => x.ClaimResponseId,
                        principalTable: "ClaimResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimResponseDiagnosesExt",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimResponseId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    DiagnosisCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DiagnosisSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DiagnosisDisplay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnosisType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DiagnosisTypeSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimResponseDiagnosesExt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimResponseDiagnosesExt_ClaimResponses_ClaimResponseId",
                        column: x => x.ClaimResponseId,
                        principalTable: "ClaimResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimResponseInsurances",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimResponseId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Focal = table.Column<bool>(type: "bit", nullable: false),
                    CoverageId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PreAuthReferences = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimResponseInsurances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimResponseInsurances_ClaimResponses_ClaimResponseId",
                        column: x => x.ClaimResponseId,
                        principalTable: "ClaimResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimResponseInsurances_Coverages_CoverageId",
                        column: x => x.CoverageId,
                        principalTable: "Coverages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClaimResponseSupportingInfosExt",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimResponseId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategorySystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CategoryDisplay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CodeSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StringValue = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    QuantityValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    QuantityUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    QuantitySystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimResponseSupportingInfosExt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimResponseSupportingInfosExt_ClaimResponses_ClaimResponseId",
                        column: x => x.ClaimResponseId,
                        principalTable: "ClaimResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimResponseTotals",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimResponseId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategorySystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CategoryDisplay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimResponseTotals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimResponseTotals_ClaimResponses_ClaimResponseId",
                        column: x => x.ClaimResponseId,
                        principalTable: "ClaimResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Benefits",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BenefitBalanceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SequenceNumber = table.Column<int>(type: "int", nullable: false),
                    BenefitType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BenefitTypeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BenefitTypeDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AllowedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AllowedCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    AllowedUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UsedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PercentageAmount = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    BenefitStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BenefitEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benefits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Benefits_BenefitBalances_BenefitBalanceId",
                        column: x => x.BenefitBalanceId,
                        principalTable: "BenefitBalances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimResponseAdjudications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimResponseAddItemId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AdjudicationCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AdjudicationSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdjudicationDisplay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityValue = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimResponseAdjudications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimResponseAdjudications_ClaimResponseAddItems_ClaimResponseAddItemId",
                        column: x => x.ClaimResponseAddItemId,
                        principalTable: "ClaimResponseAddItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BenefitBalances_Category",
                table: "BenefitBalances",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitBalances_EligibilityResponseId",
                table: "BenefitBalances",
                column: "EligibilityResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_Benefits_BenefitBalanceId",
                table: "Benefits",
                column: "BenefitBalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Benefits_BenefitType",
                table: "Benefits",
                column: "BenefitType");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimCareTeams_ClaimId",
                table: "ClaimCareTeams",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimCareTeams_PractitionerId",
                table: "ClaimCareTeams",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimDiagnoses_ClaimId",
                table: "ClaimDiagnoses",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimDiagnoses_DiagnosisCode",
                table: "ClaimDiagnoses",
                column: "DiagnosisCode");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimDiagnoses_OnAdmissionCode",
                table: "ClaimDiagnoses",
                column: "OnAdmissionCode");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimItemDetails_ClaimItemId",
                table: "ClaimItemDetails",
                column: "ClaimItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimItemDetails_Sequence",
                table: "ClaimItemDetails",
                column: "Sequence");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimItems_ClaimId",
                table: "ClaimItems",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimItems_PatientInvoiceValue",
                table: "ClaimItems",
                column: "PatientInvoiceValue");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimItems_Sequence",
                table: "ClaimItems",
                column: "Sequence");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimRelatedClaims_ClaimId",
                table: "ClaimRelatedClaims",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimResponseAddItems_ClaimResponseId",
                table: "ClaimResponseAddItems",
                column: "ClaimResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimResponseAdjudications_ClaimResponseAddItemId",
                table: "ClaimResponseAdjudications",
                column: "ClaimResponseAddItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimResponseDiagnosesExt_ClaimResponseId",
                table: "ClaimResponseDiagnosesExt",
                column: "ClaimResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimResponseInsurances_ClaimResponseId",
                table: "ClaimResponseInsurances",
                column: "ClaimResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimResponseInsurances_CoverageId",
                table: "ClaimResponseInsurances",
                column: "CoverageId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimResponseInsurances_Sequence",
                table: "ClaimResponseInsurances",
                column: "Sequence");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimResponses_ClaimId",
                table: "ClaimResponses",
                column: "ClaimId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClaimResponses_InsurerId",
                table: "ClaimResponses",
                column: "InsurerId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimResponses_PatientId",
                table: "ClaimResponses",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimResponses_PreAuthRef",
                table: "ClaimResponses",
                column: "PreAuthRef");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimResponses_RequestorId",
                table: "ClaimResponses",
                column: "RequestorId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimResponses_ServiceProviderId",
                table: "ClaimResponses",
                column: "ServiceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimResponseSupportingInfosExt_ClaimResponseId",
                table: "ClaimResponseSupportingInfosExt",
                column: "ClaimResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimResponseTotals_ClaimResponseId",
                table: "ClaimResponseTotals",
                column: "ClaimResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ClaimNumber",
                table: "Claims",
                column: "ClaimNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Claims_CoverageId",
                table: "Claims",
                column: "CoverageId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_EligibilityOfflineReference",
                table: "Claims",
                column: "EligibilityOfflineReference");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_EncounterId",
                table: "Claims",
                column: "EncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_EpisodeIdentifierValue",
                table: "Claims",
                column: "EpisodeIdentifierValue");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_InsurerId",
                table: "Claims",
                column: "InsurerId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_LocationId",
                table: "Claims",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_MessageHeaderId",
                table: "Claims",
                column: "MessageHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_PatientId",
                table: "Claims",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_PractitionerId",
                table: "Claims",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ProviderId",
                table: "Claims",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_Status",
                table: "Claims",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_Use",
                table: "Claims",
                column: "Use");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimSupportingInfos_Category",
                table: "ClaimSupportingInfos",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimSupportingInfos_ClaimId",
                table: "ClaimSupportingInfos",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRequests_AboutIdentifierValue",
                table: "CommunicationRequests",
                column: "AboutIdentifierValue");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRequests_Category",
                table: "CommunicationRequests",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRequests_CommunicationRequestId",
                table: "CommunicationRequests",
                column: "CommunicationRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRequests_Priority",
                table: "CommunicationRequests",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRequests_RecipientId",
                table: "CommunicationRequests",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRequests_SenderId",
                table: "CommunicationRequests",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRequests_Status",
                table: "CommunicationRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRequests_SubjectPatientId",
                table: "CommunicationRequests",
                column: "SubjectPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageEligibilityRequests_CoverageId",
                table: "CoverageEligibilityRequests",
                column: "CoverageId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageEligibilityRequests_EntererPractitionerId",
                table: "CoverageEligibilityRequests",
                column: "EntererPractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageEligibilityRequests_InsurerId",
                table: "CoverageEligibilityRequests",
                column: "InsurerId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageEligibilityRequests_MessageHeaderId",
                table: "CoverageEligibilityRequests",
                column: "MessageHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageEligibilityRequests_PatientId",
                table: "CoverageEligibilityRequests",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageEligibilityRequests_ProviderId",
                table: "CoverageEligibilityRequests",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageEligibilityRequests_RequestId",
                table: "CoverageEligibilityRequests",
                column: "RequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoverageEligibilityRequests_Status",
                table: "CoverageEligibilityRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageEligibilityResponses_CoverageId",
                table: "CoverageEligibilityResponses",
                column: "CoverageId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageEligibilityResponses_EligibilityRequestId",
                table: "CoverageEligibilityResponses",
                column: "EligibilityRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoverageEligibilityResponses_InsurerId",
                table: "CoverageEligibilityResponses",
                column: "InsurerId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageEligibilityResponses_MessageHeaderId",
                table: "CoverageEligibilityResponses",
                column: "MessageHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageEligibilityResponses_Outcome",
                table: "CoverageEligibilityResponses",
                column: "Outcome");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageEligibilityResponses_PatientId",
                table: "CoverageEligibilityResponses",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageEligibilityResponses_RequestId",
                table: "CoverageEligibilityResponses",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageEligibilityResponses_ResponseUUID",
                table: "CoverageEligibilityResponses",
                column: "ResponseUUID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coverages_InsurerId",
                table: "Coverages",
                column: "InsurerId");

            migrationBuilder.CreateIndex(
                name: "IX_Coverages_MemberID",
                table: "Coverages",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Coverages_PatientId",
                table: "Coverages",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Coverages_PolicyNumber",
                table: "Coverages",
                column: "PolicyNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coverages_Status",
                table: "Coverages",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityErrors_EligibilityRequestId",
                table: "EligibilityErrors",
                column: "EligibilityRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityErrors_EligibilityResponseId",
                table: "EligibilityErrors",
                column: "EligibilityResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityErrors_ErrorCode",
                table: "EligibilityErrors",
                column: "ErrorCode");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityErrors_Severity",
                table: "EligibilityErrors",
                column: "Severity");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityItemModifiers_EligibilityItemId",
                table: "EligibilityItemModifiers",
                column: "EligibilityItemId");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityItems_Category",
                table: "EligibilityItems",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityItems_EligibilityRequestId",
                table: "EligibilityItems",
                column: "EligibilityRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_Class",
                table: "Encounters",
                column: "Class");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_EncounterId",
                table: "Encounters",
                column: "EncounterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_PatientId",
                table: "Encounters",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_ServiceProviderId",
                table: "Encounters",
                column: "ServiceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_Status",
                table: "Encounters",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LocationLicense",
                table: "Locations",
                column: "LocationLicense",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_OrganizationId",
                table: "Locations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Status",
                table: "Locations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MessageHeaders_EventCode",
                table: "MessageHeaders",
                column: "EventCode");

            migrationBuilder.CreateIndex(
                name: "IX_MessageHeaders_MessageUUID",
                table: "MessageHeaders",
                column: "MessageUUID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessageHeaders_SenderOrganizationId",
                table: "MessageHeaders",
                column: "SenderOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageHeaders_Status",
                table: "MessageHeaders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_LicenseNumber",
                table: "Organizations",
                column: "LicenseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OrganizationType",
                table: "Organizations",
                column: "OrganizationType");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Status",
                table: "Organizations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_IsActive",
                table: "Patients",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_MRN",
                table: "Patients",
                column: "MRN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Status",
                table: "Patients",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentNotices_PayeeId",
                table: "PaymentNotices",
                column: "PayeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentNotices_PaymentDate",
                table: "PaymentNotices",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentNotices_PaymentNoticeId",
                table: "PaymentNotices",
                column: "PaymentNoticeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentNotices_PaymentStatus",
                table: "PaymentNotices",
                column: "PaymentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentNotices_ProviderId",
                table: "PaymentNotices",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentNotices_Status",
                table: "PaymentNotices",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReconciliationDetails_Amount",
                table: "PaymentReconciliationDetails",
                column: "Amount");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReconciliationDetails_DetailDate",
                table: "PaymentReconciliationDetails",
                column: "DetailDate");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReconciliationDetails_DetailType",
                table: "PaymentReconciliationDetails",
                column: "DetailType");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReconciliationDetails_PayeeId",
                table: "PaymentReconciliationDetails",
                column: "PayeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReconciliationDetails_PaymentReconciliationId",
                table: "PaymentReconciliationDetails",
                column: "PaymentReconciliationId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReconciliationDetails_SubmitterId",
                table: "PaymentReconciliationDetails",
                column: "SubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReconciliations_Outcome",
                table: "PaymentReconciliations",
                column: "Outcome");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReconciliations_PaymentDate",
                table: "PaymentReconciliations",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReconciliations_PaymentIssuerId",
                table: "PaymentReconciliations",
                column: "PaymentIssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReconciliations_PaymentReconciliationId",
                table: "PaymentReconciliations",
                column: "PaymentReconciliationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReconciliations_PeriodEnd",
                table: "PaymentReconciliations",
                column: "PeriodEnd");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReconciliations_PeriodStart",
                table: "PaymentReconciliations",
                column: "PeriodStart");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReconciliations_RequestorId",
                table: "PaymentReconciliations",
                column: "RequestorId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReconciliations_Status",
                table: "PaymentReconciliations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Practitioners_LicenseNumber",
                table: "Practitioners",
                column: "LicenseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Practitioners_OrganizationId",
                table: "Practitioners",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Practitioners_Status",
                table: "Practitioners",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Code",
                table: "Tasks",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_FocusIdentifierValue",
                table: "Tasks",
                column: "FocusIdentifierValue");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_MessageHeaderId",
                table: "Tasks",
                column: "MessageHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_MetaTag",
                table: "Tasks",
                column: "MetaTag");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OutputBundleId",
                table: "Tasks",
                column: "OutputBundleId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OwnerId",
                table: "Tasks",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Priority",
                table: "Tasks",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ReasonCode",
                table: "Tasks",
                column: "ReasonCode");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_RequesterId",
                table: "Tasks",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ResponseCode",
                table: "Tasks",
                column: "ResponseCode");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Status",
                table: "Tasks",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskId",
                table: "Tasks",
                column: "TaskId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Benefits");

            migrationBuilder.DropTable(
                name: "ClaimCareTeams");

            migrationBuilder.DropTable(
                name: "ClaimDiagnoses");

            migrationBuilder.DropTable(
                name: "ClaimItemDetails");

            migrationBuilder.DropTable(
                name: "ClaimRelatedClaims");

            migrationBuilder.DropTable(
                name: "ClaimResponseAdjudications");

            migrationBuilder.DropTable(
                name: "ClaimResponseDiagnosesExt");

            migrationBuilder.DropTable(
                name: "ClaimResponseInsurances");

            migrationBuilder.DropTable(
                name: "ClaimResponseSupportingInfosExt");

            migrationBuilder.DropTable(
                name: "ClaimResponseTotals");

            migrationBuilder.DropTable(
                name: "ClaimSupportingInfos");

            migrationBuilder.DropTable(
                name: "CommunicationRequests");

            migrationBuilder.DropTable(
                name: "EligibilityErrors");

            migrationBuilder.DropTable(
                name: "EligibilityItemModifiers");

            migrationBuilder.DropTable(
                name: "PaymentNotices");

            migrationBuilder.DropTable(
                name: "PaymentReconciliationDetails");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "BenefitBalances");

            migrationBuilder.DropTable(
                name: "ClaimItems");

            migrationBuilder.DropTable(
                name: "ClaimResponseAddItems");

            migrationBuilder.DropTable(
                name: "EligibilityItems");

            migrationBuilder.DropTable(
                name: "PaymentReconciliations");

            migrationBuilder.DropTable(
                name: "CoverageEligibilityResponses");

            migrationBuilder.DropTable(
                name: "ClaimResponses");

            migrationBuilder.DropTable(
                name: "CoverageEligibilityRequests");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "Coverages");

            migrationBuilder.DropTable(
                name: "Encounters");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "MessageHeaders");

            migrationBuilder.DropTable(
                name: "Practitioners");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
