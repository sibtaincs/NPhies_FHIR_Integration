IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Organizations] (
    [Id] nvarchar(450) NOT NULL,
    [OrganizationName] nvarchar(255) NOT NULL,
    [LicenseNumber] nvarchar(100) NOT NULL,
    [LicenseSystem] nvarchar(500) NOT NULL,
    [LicenseTypeSystem] nvarchar(max) NULL,
    [LicenseUse] nvarchar(max) NULL,
    [FhirId] nvarchar(max) NULL,
    [OrganizationType] nvarchar(50) NOT NULL,
    [ProviderTypeCode] nvarchar(max) NULL,
    [ProviderTypeSystem] nvarchar(max) NULL,
    [SpecializationType] nvarchar(100) NOT NULL,
    [AddressText] nvarchar(max) NULL,
    [AddressCountry] nvarchar(max) NULL,
    [Website] nvarchar(500) NOT NULL,
    [Email] nvarchar(255) NOT NULL,
    [PhoneNumber] nvarchar(20) NOT NULL,
    [ContactPhone] nvarchar(max) NULL,
    [ContactPhoneUse] nvarchar(max) NULL,
    [AddressLine1] nvarchar(255) NOT NULL,
    [AddressLine2] nvarchar(255) NOT NULL,
    [City] nvarchar(100) NOT NULL,
    [State] nvarchar(100) NOT NULL,
    [PostalCode] nvarchar(20) NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Organizations] PRIMARY KEY ([Id])
);

CREATE TABLE [Patients] (
    [Id] nvarchar(100) NOT NULL,
    [MRN] nvarchar(50) NOT NULL,
    [IdentifierSystem] nvarchar(500) NOT NULL,
    [NationalId] nvarchar(50) NOT NULL,
    [NationalIdSystem] nvarchar(max) NULL,
    [NationalIdType] nvarchar(max) NULL,
    [FirstName] nvarchar(100) NOT NULL,
    [LastName] nvarchar(100) NOT NULL,
    [FullName] nvarchar(max) NULL,
    [DateOfBirth] datetime2 NOT NULL,
    [Gender] nvarchar(1) NOT NULL,
    [MaritalStatus] nvarchar(max) NULL,
    [Occupation] nvarchar(max) NULL,
    [OccupationSystem] nvarchar(max) NULL,
    [Deceased] bit NOT NULL,
    [Active] bit NOT NULL,
    [Email] nvarchar(255) NOT NULL,
    [Phone] nvarchar(20) NOT NULL,
    [PhoneSystem] nvarchar(max) NULL,
    [PhoneUse] nvarchar(max) NULL,
    [AddressLine1] nvarchar(255) NOT NULL,
    [AddressLine2] nvarchar(255) NOT NULL,
    [City] nvarchar(100) NOT NULL,
    [State] nvarchar(100) NOT NULL,
    [PostalCode] nvarchar(20) NOT NULL,
    [Country] nvarchar(2) NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Patients] PRIMARY KEY ([Id])
);

CREATE TABLE [Locations] (
    [Id] nvarchar(100) NOT NULL,
    [LocationName] nvarchar(255) NOT NULL,
    [LocationLicense] nvarchar(100) NOT NULL,
    [LicenseSystem] nvarchar(500) NOT NULL,
    [OrganizationId] nvarchar(450) NOT NULL,
    [FacilityType] nvarchar(50) NOT NULL,
    [FacilityTypeDescription] nvarchar(255) NOT NULL,
    [AddressLine1] nvarchar(255) NOT NULL,
    [AddressLine2] nvarchar(255) NOT NULL,
    [City] nvarchar(100) NOT NULL,
    [State] nvarchar(100) NOT NULL,
    [PostalCode] nvarchar(20) NOT NULL,
    [Country] nvarchar(2) NOT NULL,
    [Phone] nvarchar(20) NOT NULL,
    [Email] nvarchar(255) NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Locations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Locations_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [MessageHeaders] (
    [Id] nvarchar(50) NOT NULL,
    [MessageUUID] nvarchar(50) NOT NULL,
    [CorrelationId] nvarchar(50) NOT NULL,
    [EventCode] nvarchar(100) NOT NULL,
    [EventSystem] nvarchar(500) NOT NULL,
    [SenderOrganizationId] nvarchar(450) NOT NULL,
    [SenderLicenseSystem] nvarchar(max) NULL,
    [SenderOrganizationLicense] nvarchar(max) NULL,
    [DestinationName] nvarchar(255) NOT NULL,
    [DestinationEndpoint] nvarchar(500) NOT NULL,
    [DestinationOrganizationId] nvarchar(max) NULL,
    [DestinationLicenseSystem] nvarchar(max) NULL,
    [FocusResourceType] nvarchar(100) NOT NULL,
    [FocusResourceId] nvarchar(100) NOT NULL,
    [SourceName] nvarchar(255) NOT NULL,
    [SourceEndpoint] nvarchar(500) NOT NULL,
    [MessageTimestamp] datetime2 NOT NULL,
    [ResponseTimestamp] datetime2 NULL,
    [Status] nvarchar(50) NOT NULL,
    [ResponseStatus] nvarchar(50) NOT NULL,
    [ResponseIdentifier] nvarchar(max) NULL,
    [ResponseCode] nvarchar(max) NULL,
    [ErrorCode] nvarchar(100) NOT NULL,
    [ErrorMessage] nvarchar(1000) NOT NULL,
    [BundleContent] ntext NOT NULL,
    [ResponseBundleContent] ntext NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_MessageHeaders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MessageHeaders_Organizations_SenderOrganizationId] FOREIGN KEY ([SenderOrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Practitioners] (
    [Id] nvarchar(100) NOT NULL,
    [FirstName] nvarchar(100) NOT NULL,
    [LastName] nvarchar(100) NOT NULL,
    [LicenseNumber] nvarchar(100) NOT NULL,
    [LicenseSystem] nvarchar(500) NOT NULL,
    [Specialization] nvarchar(100) NOT NULL,
    [Qualification] nvarchar(255) NOT NULL,
    [Title] nvarchar(50) NOT NULL,
    [Email] nvarchar(255) NOT NULL,
    [Phone] nvarchar(20) NOT NULL,
    [OrganizationId] nvarchar(450) NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Practitioners] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Practitioners_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [TaskRequests] (
    [Id] nvarchar(100) NOT NULL,
    [TaskId] nvarchar(100) NOT NULL,
    [IdentifierSystem] nvarchar(500) NULL,
    [IdentifierValue] nvarchar(100) NULL,
    [Status] nvarchar(50) NOT NULL,
    [Intent] nvarchar(50) NOT NULL,
    [Priority] nvarchar(50) NULL,
    [Code] nvarchar(50) NULL,
    [CodeSystem] nvarchar(500) NULL,
    [FocusResourceType] nvarchar(100) NULL,
    [FocusIdentifierSystem] nvarchar(500) NULL,
    [FocusIdentifierValue] nvarchar(100) NULL,
    [ReasonCode] nvarchar(50) NULL,
    [ReasonCodeSystem] nvarchar(500) NULL,
    [ReasonText] nvarchar(1000) NULL,
    [AuthoredOn] datetime2 NULL,
    [LastModified] datetime2 NULL,
    [RequesterId] nvarchar(450) NULL,
    [OwnerId] nvarchar(450) NULL,
    [Description] nvarchar(max) NULL,
    [FhirTaskJson] ntext NULL,
    [MessageHeaderId] nvarchar(50) NULL,
    [ProcessingStatus] nvarchar(50) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_TaskRequests] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TaskRequests_Organizations_OwnerId] FOREIGN KEY ([OwnerId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TaskRequests_Organizations_RequesterId] FOREIGN KEY ([RequesterId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Coverages] (
    [Id] nvarchar(100) NOT NULL,
    [PolicyNumber] nvarchar(100) NOT NULL,
    [MemberID] nvarchar(50) NOT NULL,
    [CoverageIdentifierSystem] nvarchar(max) NULL,
    [CoverageIdentifierValue] nvarchar(max) NULL,
    [CoverageType] nvarchar(50) NOT NULL,
    [CoverageTypeSystem] nvarchar(max) NULL,
    [PlanCode] nvarchar(max) NULL,
    [PlanCodeSystem] nvarchar(max) NULL,
    [CoverageClass] nvarchar(max) NULL,
    [Status] nvarchar(50) NOT NULL,
    [SubscriberRelationship] nvarchar(max) NOT NULL,
    [SubscriberId] nvarchar(max) NULL,
    [SubscriberPatientId] nvarchar(max) NULL,
    [Dependent] nvarchar(max) NULL,
    [RelationToSubscriber] nvarchar(50) NOT NULL,
    [PolicyHolderOrganizationId] nvarchar(max) NULL,
    [SubscriberMRN] nvarchar(50) NOT NULL,
    [Subrogation] bit NOT NULL,
    [MaxCopay] decimal(18,2) NULL,
    [MaxCopayCurrency] nvarchar(max) NULL,
    [CoinsurancePercentValue] decimal(18,2) NULL,
    [CoverageClassType] nvarchar(max) NULL,
    [CoverageClassValue] nvarchar(max) NULL,
    [PatientId] nvarchar(100) NOT NULL,
    [InsurerId] nvarchar(450) NOT NULL,
    [CoverageStartDate] datetime2 NOT NULL,
    [CoverageEndDate] datetime2 NOT NULL,
    [AnnualDeductible] decimal(18,2) NOT NULL,
    [DeductibleMet] decimal(18,2) NOT NULL,
    [Copay] decimal(18,2) NOT NULL,
    [CoinsurancePercent] decimal(5,2) NOT NULL,
    [OutOfPocketMax] decimal(18,2) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Coverages] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Coverages_Organizations_InsurerId] FOREIGN KEY ([InsurerId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Coverages_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Encounters] (
    [Id] nvarchar(100) NOT NULL,
    [EncounterId] nvarchar(100) NOT NULL,
    [IdentifierSystem] nvarchar(500) NULL,
    [IdentifierValue] nvarchar(100) NULL,
    [Status] nvarchar(50) NOT NULL,
    [Class] nvarchar(50) NOT NULL,
    [ServiceType] nvarchar(100) NULL,
    [ServiceTypeSystem] nvarchar(500) NULL,
    [PatientId] nvarchar(100) NOT NULL,
    [PeriodStart] datetime2 NOT NULL,
    [PeriodEnd] datetime2 NULL,
    [AdmitSource] nvarchar(50) NULL,
    [AdmitSourceSystem] nvarchar(500) NULL,
    [ServiceEventType] nvarchar(100) NULL,
    [ServiceEventTypeSystem] nvarchar(max) NULL,
    [IntendedLengthOfStay] nvarchar(100) NULL,
    [IntendedLengthOfStaySystem] nvarchar(max) NULL,
    [ServiceProviderId] nvarchar(450) NULL,
    [FhirEncounterJson] ntext NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Encounters] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Encounters_Organizations_ServiceProviderId] FOREIGN KEY ([ServiceProviderId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Encounters_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [TaskResponses] (
    [Id] nvarchar(100) NOT NULL,
    [TaskId] nvarchar(100) NOT NULL,
    [IdentifierSystem] nvarchar(500) NULL,
    [IdentifierValue] nvarchar(100) NULL,
    [ReferencedRequestId] nvarchar(100) NULL,
    [TaskRequestId] nvarchar(100) NULL,
    [Status] nvarchar(50) NOT NULL,
    [Intent] nvarchar(50) NOT NULL,
    [Priority] nvarchar(50) NULL,
    [Code] nvarchar(50) NULL,
    [CodeSystem] nvarchar(500) NULL,
    [FocusResourceType] nvarchar(100) NULL,
    [FocusIdentifierSystem] nvarchar(500) NULL,
    [FocusIdentifierValue] nvarchar(100) NULL,
    [ResponseCode] nvarchar(50) NULL,
    [ResponseMessage] nvarchar(1000) NULL,
    [ResponseStatusCode] int NULL,
    [AuthoredOn] datetime2 NULL,
    [LastModified] datetime2 NULL,
    [RequesterId] nvarchar(450) NULL,
    [OwnerId] nvarchar(450) NULL,
    [Description] nvarchar(max) NULL,
    [ResultText] nvarchar(1000) NULL,
    [FhirTaskJson] ntext NULL,
    [MessageHeaderId] nvarchar(50) NULL,
    [ProcessingStatus] nvarchar(50) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_TaskResponses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TaskResponses_Organizations_OwnerId] FOREIGN KEY ([OwnerId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TaskResponses_Organizations_RequesterId] FOREIGN KEY ([RequesterId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TaskResponses_TaskRequests_TaskRequestId] FOREIGN KEY ([TaskRequestId]) REFERENCES [TaskRequests] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [CoverageEligibilityRequests] (
    [Id] nvarchar(100) NOT NULL,
    [MessageUUID] nvarchar(50) NOT NULL,
    [RequestId] nvarchar(100) NOT NULL,
    [RequestIdentifierSystem] nvarchar(max) NULL,
    [RequestIdentifierValue] nvarchar(max) NULL,
    [MessageHeaderId] nvarchar(50) NOT NULL,
    [RequestType] nvarchar(50) NOT NULL,
    [PrioritySystem] nvarchar(max) NULL,
    [PurposeJson] nvarchar(max) NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [Priority] nvarchar(50) NOT NULL,
    [CreatedDate] datetime2 NULL,
    [ServiceDate] datetime2 NOT NULL,
    [ServicedPeriodStart] datetime2 NOT NULL,
    [ServicedPeriodEnd] datetime2 NOT NULL,
    [ServiceType] nvarchar(100) NOT NULL,
    [PatientId] nvarchar(100) NOT NULL,
    [CoverageId] nvarchar(100) NOT NULL,
    [ProviderId] nvarchar(450) NOT NULL,
    [InsurerId] nvarchar(450) NOT NULL,
    [EntererPractitionerId] nvarchar(100) NOT NULL,
    [RequestCreatedAt] datetime2 NOT NULL,
    [SubmittedAt] datetime2 NOT NULL,
    [RespondedAt] datetime2 NULL,
    [ResponseId] nvarchar(max) NOT NULL,
    [EligibilityStatus] nvarchar(50) NOT NULL,
    [MessageStatus] nvarchar(50) NOT NULL,
    [FhirRequestBundle] ntext NOT NULL,
    [FhirResponseBundle] ntext NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_CoverageEligibilityRequests] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CoverageEligibilityRequests_Coverages_CoverageId] FOREIGN KEY ([CoverageId]) REFERENCES [Coverages] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CoverageEligibilityRequests_MessageHeaders_MessageHeaderId] FOREIGN KEY ([MessageHeaderId]) REFERENCES [MessageHeaders] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CoverageEligibilityRequests_Organizations_InsurerId] FOREIGN KEY ([InsurerId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CoverageEligibilityRequests_Organizations_ProviderId] FOREIGN KEY ([ProviderId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CoverageEligibilityRequests_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CoverageEligibilityRequests_Practitioners_EntererPractitionerId] FOREIGN KEY ([EntererPractitionerId]) REFERENCES [Practitioners] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Claims] (
    [Id] nvarchar(100) NOT NULL,
    [ClaimNumber] nvarchar(100) NOT NULL,
    [ClaimIdentifierSystem] nvarchar(500) NULL,
    [ClaimIdentifierValue] nvarchar(100) NULL,
    [Status] nvarchar(50) NOT NULL,
    [ClaimType] nvarchar(50) NOT NULL,
    [ClaimTypeSystem] nvarchar(500) NULL,
    [ClaimSubType] nvarchar(50) NULL,
    [ClaimSubTypeSystem] nvarchar(max) NULL,
    [Use] nvarchar(50) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [ServicedPeriodStart] datetime2 NULL,
    [ServicedPeriodEnd] datetime2 NULL,
    [Priority] nvarchar(50) NOT NULL,
    [PrioritySystem] nvarchar(500) NULL,
    [PatientId] nvarchar(100) NOT NULL,
    [CoverageId] nvarchar(100) NOT NULL,
    [ProviderId] nvarchar(450) NOT NULL,
    [InsurerId] nvarchar(450) NOT NULL,
    [PractitionerId] nvarchar(100) NULL,
    [LocationId] nvarchar(100) NULL,
    [MessageHeaderId] nvarchar(50) NULL,
    [EncounterId] nvarchar(100) NULL,
    [PayeeType] nvarchar(50) NULL,
    [PayeeTypeSystem] nvarchar(500) NULL,
    [Total] decimal(18,2) NOT NULL,
    [TotalCurrency] nvarchar(3) NOT NULL,
    [FhirClaimBundle] ntext NULL,
    [EpisodeIdentifierSystem] nvarchar(500) NULL,
    [EpisodeIdentifierValue] nvarchar(100) NULL,
    [EligibilityOfflineReference] nvarchar(100) NULL,
    [EligibilityOfflineDate] datetime2 NULL,
    [AuthorizationOfflineDate] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Claims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Claims_Coverages_CoverageId] FOREIGN KEY ([CoverageId]) REFERENCES [Coverages] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Claims_Encounters_EncounterId] FOREIGN KEY ([EncounterId]) REFERENCES [Encounters] ([Id]),
    CONSTRAINT [FK_Claims_Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Locations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Claims_MessageHeaders_MessageHeaderId] FOREIGN KEY ([MessageHeaderId]) REFERENCES [MessageHeaders] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Claims_Organizations_InsurerId] FOREIGN KEY ([InsurerId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Claims_Organizations_ProviderId] FOREIGN KEY ([ProviderId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Claims_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Claims_Practitioners_PractitionerId] FOREIGN KEY ([PractitionerId]) REFERENCES [Practitioners] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [CoverageEligibilityResponses] (
    [Id] nvarchar(100) NOT NULL,
    [ResponseUUID] nvarchar(50) NOT NULL,
    [RequestId] nvarchar(100) NOT NULL,
    [RequestIdentifierSystem] nvarchar(max) NULL,
    [RequestIdentifierValue] nvarchar(max) NULL,
    [ResponseIdentifierSystem] nvarchar(max) NULL,
    [ResponseIdentifierValue] nvarchar(max) NULL,
    [EligibilityRequestId] nvarchar(100) NOT NULL,
    [MessageHeaderId] nvarchar(50) NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [Outcome] nvarchar(50) NOT NULL,
    [ProcessingStatus] nvarchar(255) NOT NULL,
    [ResponsePurpose] nvarchar(max) NULL,
    [Disposition] nvarchar(max) NULL,
    [SiteEligibility] nvarchar(max) NULL,
    [SiteEligibilitySystem] nvarchar(max) NULL,
    [ResponseCreatedAt] datetime2 NOT NULL,
    [ResponseReceivedAt] datetime2 NOT NULL,
    [ServicedDate] datetime2 NULL,
    [BenefitPeriodStart] datetime2 NULL,
    [BenefitPeriodEnd] datetime2 NULL,
    [EligibilityStatus] nvarchar(50) NOT NULL,
    [IsInForce] bit NOT NULL,
    [ServicedPeriodStart] datetime2 NOT NULL,
    [ServicedPeriodEnd] datetime2 NOT NULL,
    [InsurerId] nvarchar(450) NOT NULL,
    [PatientId] nvarchar(100) NOT NULL,
    [CoverageId] nvarchar(100) NOT NULL,
    [IsInNetwork] bit NOT NULL,
    [NetworkStatus] nvarchar(50) NOT NULL,
    [NetworkName] nvarchar(255) NOT NULL,
    [CoveredServicesJson] nvarchar(max) NOT NULL,
    [ExcludedServicesJson] nvarchar(max) NOT NULL,
    [LimitationsJson] nvarchar(max) NOT NULL,
    [FhirResponseContent] ntext NOT NULL,
    [ExplanationOfBenefits] nvarchar(1000) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_CoverageEligibilityResponses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CoverageEligibilityResponses_CoverageEligibilityRequests_EligibilityRequestId] FOREIGN KEY ([EligibilityRequestId]) REFERENCES [CoverageEligibilityRequests] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CoverageEligibilityResponses_Coverages_CoverageId] FOREIGN KEY ([CoverageId]) REFERENCES [Coverages] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CoverageEligibilityResponses_MessageHeaders_MessageHeaderId] FOREIGN KEY ([MessageHeaderId]) REFERENCES [MessageHeaders] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CoverageEligibilityResponses_Organizations_InsurerId] FOREIGN KEY ([InsurerId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CoverageEligibilityResponses_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [EligibilityItems] (
    [Id] nvarchar(100) NOT NULL,
    [EligibilityRequestId] nvarchar(100) NOT NULL,
    [SequenceNumber] int NOT NULL,
    [Category] nvarchar(50) NOT NULL,
    [CategorySystem] nvarchar(500) NOT NULL,
    [CategoryDescription] nvarchar(255) NOT NULL,
    [ProductOrServiceCode] nvarchar(100) NOT NULL,
    [ProductOrServiceSystem] nvarchar(500) NOT NULL,
    [ProductOrServiceDescription] nvarchar(255) NOT NULL,
    [DiagnosisCodes] nvarchar(max) NOT NULL,
    [Notes] nvarchar(1000) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_EligibilityItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EligibilityItems_CoverageEligibilityRequests_EligibilityRequestId] FOREIGN KEY ([EligibilityRequestId]) REFERENCES [CoverageEligibilityRequests] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ClaimCareTeams] (
    [Id] nvarchar(100) NOT NULL,
    [ClaimId] nvarchar(100) NOT NULL,
    [Sequence] int NOT NULL,
    [PractitionerId] nvarchar(100) NOT NULL,
    [Role] nvarchar(50) NULL,
    [RoleSystem] nvarchar(500) NULL,
    [RoleDisplay] nvarchar(max) NULL,
    [Qualification] nvarchar(100) NULL,
    [QualificationSystem] nvarchar(500) NULL,
    [QualificationDisplay] nvarchar(max) NULL,
    [Notes] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_ClaimCareTeams] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClaimCareTeams_Claims_ClaimId] FOREIGN KEY ([ClaimId]) REFERENCES [Claims] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ClaimCareTeams_Practitioners_PractitionerId] FOREIGN KEY ([PractitionerId]) REFERENCES [Practitioners] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [ClaimDiagnoses] (
    [Id] nvarchar(100) NOT NULL,
    [ClaimId] nvarchar(100) NOT NULL,
    [ClaimId1] nvarchar(100) NULL,
    [Sequence] int NOT NULL,
    [DiagnosisCode] nvarchar(100) NOT NULL,
    [DiagnosisSystem] nvarchar(500) NULL,
    [DiagnosisDisplay] nvarchar(max) NULL,
    [DiagnosisType] nvarchar(50) NULL,
    [DiagnosisTypeSystem] nvarchar(max) NULL,
    [OnAdmissionCode] nvarchar(10) NULL,
    [OnAdmissionSystem] nvarchar(max) NULL,
    [Notes] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_ClaimDiagnoses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClaimDiagnoses_Claims_ClaimId] FOREIGN KEY ([ClaimId]) REFERENCES [Claims] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ClaimDiagnoses_Claims_ClaimId1] FOREIGN KEY ([ClaimId1]) REFERENCES [Claims] ([Id])
);

CREATE TABLE [ClaimItems] (
    [Id] nvarchar(100) NOT NULL,
    [ClaimId] nvarchar(100) NOT NULL,
    [ClaimId1] nvarchar(100) NULL,
    [Sequence] int NOT NULL,
    [CareTeamSequence] int NULL,
    [ProductOrServiceCode] nvarchar(100) NOT NULL,
    [ProductOrServiceSystem] nvarchar(500) NULL,
    [ProductOrServiceDisplay] nvarchar(max) NULL,
    [AltProductOrServiceCode] nvarchar(100) NULL,
    [AltProductOrServiceSystem] nvarchar(500) NULL,
    [ServicedDate] datetime2 NULL,
    [ServicedPeriodStart] datetime2 NULL,
    [ServicedPeriodEnd] datetime2 NULL,
    [Quantity] decimal(18,2) NULL,
    [UnitPrice] decimal(18,2) NULL,
    [Net] decimal(18,2) NULL,
    [PatientShare] decimal(18,2) NULL,
    [PatientShareCurrency] nvarchar(max) NULL,
    [IsPackage] bit NOT NULL,
    [IsMaternity] bit NOT NULL,
    [Notes] nvarchar(1000) NULL,
    [PatientInvoiceSystem] nvarchar(500) NULL,
    [PatientInvoiceValue] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_ClaimItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClaimItems_Claims_ClaimId] FOREIGN KEY ([ClaimId]) REFERENCES [Claims] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ClaimItems_Claims_ClaimId1] FOREIGN KEY ([ClaimId1]) REFERENCES [Claims] ([Id])
);

CREATE TABLE [ClaimRelatedClaims] (
    [Id] nvarchar(100) NOT NULL,
    [ClaimId] nvarchar(100) NOT NULL,
    [RelatedClaimIdentifierSystem] nvarchar(500) NULL,
    [RelatedClaimIdentifierValue] nvarchar(100) NULL,
    [Relationship] nvarchar(50) NULL,
    [RelationshipSystem] nvarchar(500) NULL,
    [RelationshipDisplay] nvarchar(max) NULL,
    [ReferencedClaimId] nvarchar(max) NULL,
    [Notes] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_ClaimRelatedClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClaimRelatedClaims_Claims_ClaimId] FOREIGN KEY ([ClaimId]) REFERENCES [Claims] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ClaimResponses] (
    [Id] nvarchar(100) NOT NULL,
    [ClaimId] nvarchar(100) NOT NULL,
    [ResponseIdentifierSystem] nvarchar(500) NULL,
    [ResponseIdentifierValue] nvarchar(100) NULL,
    [ClaimResponseStatus] nvarchar(50) NULL,
    [ClaimType] nvarchar(50) NULL,
    [ClaimTypeSystem] nvarchar(500) NULL,
    [ClaimSubType] nvarchar(50) NULL,
    [ClaimSubTypeSystem] nvarchar(max) NULL,
    [Use] nvarchar(50) NULL,
    [PatientId] nvarchar(100) NULL,
    [InsurerId] nvarchar(450) NULL,
    [RequestorId] nvarchar(450) NULL,
    [RequestIdentifierSystem] nvarchar(500) NULL,
    [RequestIdentifierValue] nvarchar(100) NULL,
    [PreAuthRef] nvarchar(100) NULL,
    [PreAuthPeriodStart] datetime2 NULL,
    [PreAuthPeriodEnd] datetime2 NULL,
    [AdvancedAuthReason] nvarchar(max) NULL,
    [AdvancedAuthReasonSystem] nvarchar(max) NULL,
    [ServiceProviderId] nvarchar(450) NULL,
    [FhirClaimResponseJson] ntext NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_ClaimResponses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClaimResponses_Claims_ClaimId] FOREIGN KEY ([ClaimId]) REFERENCES [Claims] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ClaimResponses_Organizations_InsurerId] FOREIGN KEY ([InsurerId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ClaimResponses_Organizations_RequestorId] FOREIGN KEY ([RequestorId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ClaimResponses_Organizations_ServiceProviderId] FOREIGN KEY ([ServiceProviderId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ClaimResponses_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [ClaimSupportingInfos] (
    [Id] nvarchar(100) NOT NULL,
    [ClaimId] nvarchar(100) NOT NULL,
    [Sequence] int NOT NULL,
    [Category] nvarchar(100) NOT NULL,
    [CategorySystem] nvarchar(500) NULL,
    [CategoryDisplay] nvarchar(max) NULL,
    [CodeValue] nvarchar(100) NULL,
    [CodeSystem] nvarchar(max) NULL,
    [StringValue] nvarchar(4000) NULL,
    [QuantityValue] decimal(18,2) NULL,
    [QuantityUnit] nvarchar(50) NULL,
    [QuantitySystem] nvarchar(max) NULL,
    [DateValue] datetime2 NULL,
    [BooleanValue] bit NULL,
    [Notes] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_ClaimSupportingInfos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClaimSupportingInfos_Claims_ClaimId] FOREIGN KEY ([ClaimId]) REFERENCES [Claims] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [BenefitBalances] (
    [Id] nvarchar(100) NOT NULL,
    [EligibilityResponseId] nvarchar(100) NOT NULL,
    [SequenceNumber] int NOT NULL,
    [Category] nvarchar(50) NOT NULL,
    [CategorySystem] nvarchar(500) NOT NULL,
    [CategoryDescription] nvarchar(255) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_BenefitBalances] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BenefitBalances_CoverageEligibilityResponses_EligibilityResponseId] FOREIGN KEY ([EligibilityResponseId]) REFERENCES [CoverageEligibilityResponses] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [EligibilityErrors] (
    [Id] nvarchar(100) NOT NULL,
    [EligibilityRequestId] nvarchar(100) NULL,
    [EligibilityResponseId] nvarchar(100) NULL,
    [ErrorCode] nvarchar(100) NOT NULL,
    [ErrorCodeSystem] nvarchar(500) NOT NULL,
    [ErrorMessage] nvarchar(1000) NOT NULL,
    [ErrorDetails] nvarchar(2000) NOT NULL,
    [Severity] nvarchar(50) NOT NULL,
    [ErrorLocation] nvarchar(255) NOT NULL,
    [ErrorField] nvarchar(255) NOT NULL,
    [HttpStatusCode] int NULL,
    [ErrorOccurredAt] datetime2 NOT NULL,
    [AdditionalContext] nvarchar(1000) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_EligibilityErrors] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EligibilityErrors_CoverageEligibilityRequests_EligibilityRequestId] FOREIGN KEY ([EligibilityRequestId]) REFERENCES [CoverageEligibilityRequests] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_EligibilityErrors_CoverageEligibilityResponses_EligibilityResponseId] FOREIGN KEY ([EligibilityResponseId]) REFERENCES [CoverageEligibilityResponses] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [EligibilityItemModifiers] (
    [Id] nvarchar(100) NOT NULL,
    [EligibilityItemId] nvarchar(100) NOT NULL,
    [ModifierCode] nvarchar(100) NOT NULL,
    [ModifierSystem] nvarchar(500) NOT NULL,
    [ModifierDescription] nvarchar(255) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_EligibilityItemModifiers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EligibilityItemModifiers_EligibilityItems_EligibilityItemId] FOREIGN KEY ([EligibilityItemId]) REFERENCES [EligibilityItems] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ClaimItemDetails] (
    [Id] nvarchar(100) NOT NULL,
    [ClaimItemId] nvarchar(100) NOT NULL,
    [Sequence] int NOT NULL,
    [ProductOrServiceCode] nvarchar(100) NOT NULL,
    [ProductOrServiceSystem] nvarchar(500) NULL,
    [ProductOrServiceDisplay] nvarchar(max) NULL,
    [AltProductOrServiceCode] nvarchar(100) NULL,
    [AltProductOrServiceSystem] nvarchar(500) NULL,
    [Quantity] decimal(18,2) NULL,
    [UnitPrice] decimal(18,2) NULL,
    [Net] decimal(18,2) NULL,
    [Notes] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_ClaimItemDetails] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClaimItemDetails_ClaimItems_ClaimItemId] FOREIGN KEY ([ClaimItemId]) REFERENCES [ClaimItems] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ClaimResponseAddItems] (
    [Id] nvarchar(100) NOT NULL,
    [ClaimResponseId] nvarchar(100) NOT NULL,
    [Sequence] int NOT NULL,
    [ProductOrServiceCode] nvarchar(100) NULL,
    [ProductOrServiceSystem] nvarchar(500) NULL,
    [ProductOrServiceDisplay] nvarchar(max) NULL,
    [IsApproved] bit NOT NULL,
    [ApprovedQuantity] int NULL,
    [BenefitAmount] decimal(18,2) NULL,
    [BenefitCurrency] nvarchar(max) NULL,
    [SubmittedAmount] decimal(18,2) NULL,
    [DiagnosisSequence] int NULL,
    [InformationSequence] int NULL,
    [IsMaternity] bit NOT NULL,
    [Notes] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_ClaimResponseAddItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClaimResponseAddItems_ClaimResponses_ClaimResponseId] FOREIGN KEY ([ClaimResponseId]) REFERENCES [ClaimResponses] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ClaimResponseDiagnosesExt] (
    [Id] nvarchar(100) NOT NULL,
    [ClaimResponseId] nvarchar(100) NOT NULL,
    [Sequence] int NOT NULL,
    [DiagnosisCode] nvarchar(100) NOT NULL,
    [DiagnosisSystem] nvarchar(500) NULL,
    [DiagnosisDisplay] nvarchar(max) NULL,
    [DiagnosisType] nvarchar(50) NULL,
    [DiagnosisTypeSystem] nvarchar(max) NULL,
    [Notes] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_ClaimResponseDiagnosesExt] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClaimResponseDiagnosesExt_ClaimResponses_ClaimResponseId] FOREIGN KEY ([ClaimResponseId]) REFERENCES [ClaimResponses] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ClaimResponseInsurances] (
    [Id] nvarchar(100) NOT NULL,
    [ClaimResponseId] nvarchar(100) NOT NULL,
    [Sequence] int NOT NULL,
    [Focal] bit NOT NULL,
    [CoverageId] nvarchar(100) NULL,
    [PreAuthReferences] nvarchar(500) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_ClaimResponseInsurances] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClaimResponseInsurances_ClaimResponses_ClaimResponseId] FOREIGN KEY ([ClaimResponseId]) REFERENCES [ClaimResponses] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ClaimResponseInsurances_Coverages_CoverageId] FOREIGN KEY ([CoverageId]) REFERENCES [Coverages] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [ClaimResponseSupportingInfosExt] (
    [Id] nvarchar(100) NOT NULL,
    [ClaimResponseId] nvarchar(100) NOT NULL,
    [Sequence] int NOT NULL,
    [Category] nvarchar(100) NOT NULL,
    [CategorySystem] nvarchar(500) NULL,
    [CategoryDisplay] nvarchar(max) NULL,
    [CodeValue] nvarchar(100) NULL,
    [CodeSystem] nvarchar(max) NULL,
    [StringValue] nvarchar(4000) NULL,
    [QuantityValue] decimal(18,2) NULL,
    [QuantityUnit] nvarchar(50) NULL,
    [QuantitySystem] nvarchar(max) NULL,
    [Notes] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_ClaimResponseSupportingInfosExt] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClaimResponseSupportingInfosExt_ClaimResponses_ClaimResponseId] FOREIGN KEY ([ClaimResponseId]) REFERENCES [ClaimResponses] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ClaimResponseTotals] (
    [Id] nvarchar(100) NOT NULL,
    [ClaimResponseId] nvarchar(100) NOT NULL,
    [Category] nvarchar(50) NOT NULL,
    [CategorySystem] nvarchar(500) NULL,
    [CategoryDisplay] nvarchar(max) NULL,
    [Amount] decimal(18,2) NOT NULL,
    [Currency] nvarchar(3) NOT NULL,
    [Sequence] int NULL,
    [Notes] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_ClaimResponseTotals] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClaimResponseTotals_ClaimResponses_ClaimResponseId] FOREIGN KEY ([ClaimResponseId]) REFERENCES [ClaimResponses] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Benefits] (
    [Id] nvarchar(100) NOT NULL,
    [BenefitBalanceId] nvarchar(100) NOT NULL,
    [SequenceNumber] int NOT NULL,
    [BenefitType] nvarchar(50) NOT NULL,
    [BenefitTypeSystem] nvarchar(500) NOT NULL,
    [BenefitTypeDescription] nvarchar(255) NOT NULL,
    [AllowedAmount] decimal(18,2) NOT NULL,
    [AllowedCurrency] nvarchar(3) NOT NULL,
    [AllowedUnit] nvarchar(50) NOT NULL,
    [UsedAmount] decimal(18,2) NOT NULL,
    [PercentageAmount] decimal(5,2) NULL,
    [Description] nvarchar(1000) NOT NULL,
    [BenefitStartDate] datetime2 NULL,
    [BenefitEndDate] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Benefits] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Benefits_BenefitBalances_BenefitBalanceId] FOREIGN KEY ([BenefitBalanceId]) REFERENCES [BenefitBalances] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ClaimResponseAdjudications] (
    [Id] nvarchar(100) NOT NULL,
    [ClaimResponseAddItemId] nvarchar(100) NOT NULL,
    [AdjudicationCategory] nvarchar(50) NOT NULL,
    [AdjudicationSystem] nvarchar(500) NULL,
    [AdjudicationDisplay] nvarchar(max) NULL,
    [Amount] decimal(18,2) NULL,
    [Currency] nvarchar(max) NULL,
    [QuantityValue] int NULL,
    [Notes] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_ClaimResponseAdjudications] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClaimResponseAdjudications_ClaimResponseAddItems_ClaimResponseAddItemId] FOREIGN KEY ([ClaimResponseAddItemId]) REFERENCES [ClaimResponseAddItems] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_BenefitBalances_Category] ON [BenefitBalances] ([Category]);

CREATE INDEX [IX_BenefitBalances_EligibilityResponseId] ON [BenefitBalances] ([EligibilityResponseId]);

CREATE INDEX [IX_Benefits_BenefitBalanceId] ON [Benefits] ([BenefitBalanceId]);

CREATE INDEX [IX_Benefits_BenefitType] ON [Benefits] ([BenefitType]);

CREATE INDEX [IX_ClaimCareTeams_ClaimId] ON [ClaimCareTeams] ([ClaimId]);

CREATE INDEX [IX_ClaimCareTeams_PractitionerId] ON [ClaimCareTeams] ([PractitionerId]);

CREATE INDEX [IX_ClaimDiagnoses_ClaimId] ON [ClaimDiagnoses] ([ClaimId]);

CREATE INDEX [IX_ClaimDiagnoses_ClaimId1] ON [ClaimDiagnoses] ([ClaimId1]);

CREATE INDEX [IX_ClaimDiagnoses_DiagnosisCode] ON [ClaimDiagnoses] ([DiagnosisCode]);

CREATE INDEX [IX_ClaimDiagnoses_OnAdmissionCode] ON [ClaimDiagnoses] ([OnAdmissionCode]);

CREATE INDEX [IX_ClaimItemDetails_ClaimItemId] ON [ClaimItemDetails] ([ClaimItemId]);

CREATE INDEX [IX_ClaimItemDetails_Sequence] ON [ClaimItemDetails] ([Sequence]);

CREATE INDEX [IX_ClaimItems_ClaimId] ON [ClaimItems] ([ClaimId]);

CREATE INDEX [IX_ClaimItems_ClaimId1] ON [ClaimItems] ([ClaimId1]);

CREATE INDEX [IX_ClaimItems_PatientInvoiceValue] ON [ClaimItems] ([PatientInvoiceValue]);

CREATE INDEX [IX_ClaimItems_Sequence] ON [ClaimItems] ([Sequence]);

CREATE INDEX [IX_ClaimRelatedClaims_ClaimId] ON [ClaimRelatedClaims] ([ClaimId]);

CREATE INDEX [IX_ClaimResponseAddItems_ClaimResponseId] ON [ClaimResponseAddItems] ([ClaimResponseId]);

CREATE INDEX [IX_ClaimResponseAdjudications_ClaimResponseAddItemId] ON [ClaimResponseAdjudications] ([ClaimResponseAddItemId]);

CREATE INDEX [IX_ClaimResponseDiagnosesExt_ClaimResponseId] ON [ClaimResponseDiagnosesExt] ([ClaimResponseId]);

CREATE INDEX [IX_ClaimResponseInsurances_ClaimResponseId] ON [ClaimResponseInsurances] ([ClaimResponseId]);

CREATE INDEX [IX_ClaimResponseInsurances_CoverageId] ON [ClaimResponseInsurances] ([CoverageId]);

CREATE INDEX [IX_ClaimResponseInsurances_Sequence] ON [ClaimResponseInsurances] ([Sequence]);

CREATE UNIQUE INDEX [IX_ClaimResponses_ClaimId] ON [ClaimResponses] ([ClaimId]);

CREATE INDEX [IX_ClaimResponses_InsurerId] ON [ClaimResponses] ([InsurerId]);

CREATE INDEX [IX_ClaimResponses_PatientId] ON [ClaimResponses] ([PatientId]);

CREATE INDEX [IX_ClaimResponses_PreAuthRef] ON [ClaimResponses] ([PreAuthRef]);

CREATE INDEX [IX_ClaimResponses_RequestorId] ON [ClaimResponses] ([RequestorId]);

CREATE INDEX [IX_ClaimResponses_ServiceProviderId] ON [ClaimResponses] ([ServiceProviderId]);

CREATE INDEX [IX_ClaimResponseSupportingInfosExt_ClaimResponseId] ON [ClaimResponseSupportingInfosExt] ([ClaimResponseId]);

CREATE INDEX [IX_ClaimResponseTotals_ClaimResponseId] ON [ClaimResponseTotals] ([ClaimResponseId]);

CREATE UNIQUE INDEX [IX_Claims_ClaimNumber] ON [Claims] ([ClaimNumber]);

CREATE INDEX [IX_Claims_CoverageId] ON [Claims] ([CoverageId]);

CREATE INDEX [IX_Claims_EligibilityOfflineReference] ON [Claims] ([EligibilityOfflineReference]);

CREATE INDEX [IX_Claims_EncounterId] ON [Claims] ([EncounterId]);

CREATE INDEX [IX_Claims_EpisodeIdentifierValue] ON [Claims] ([EpisodeIdentifierValue]);

CREATE INDEX [IX_Claims_InsurerId] ON [Claims] ([InsurerId]);

CREATE INDEX [IX_Claims_LocationId] ON [Claims] ([LocationId]);

CREATE INDEX [IX_Claims_MessageHeaderId] ON [Claims] ([MessageHeaderId]);

CREATE INDEX [IX_Claims_PatientId] ON [Claims] ([PatientId]);

CREATE INDEX [IX_Claims_PractitionerId] ON [Claims] ([PractitionerId]);

CREATE INDEX [IX_Claims_ProviderId] ON [Claims] ([ProviderId]);

CREATE INDEX [IX_Claims_Status] ON [Claims] ([Status]);

CREATE INDEX [IX_Claims_Use] ON [Claims] ([Use]);

CREATE INDEX [IX_ClaimSupportingInfos_Category] ON [ClaimSupportingInfos] ([Category]);

CREATE INDEX [IX_ClaimSupportingInfos_ClaimId] ON [ClaimSupportingInfos] ([ClaimId]);

CREATE INDEX [IX_CoverageEligibilityRequests_CoverageId] ON [CoverageEligibilityRequests] ([CoverageId]);

CREATE INDEX [IX_CoverageEligibilityRequests_EntererPractitionerId] ON [CoverageEligibilityRequests] ([EntererPractitionerId]);

CREATE INDEX [IX_CoverageEligibilityRequests_InsurerId] ON [CoverageEligibilityRequests] ([InsurerId]);

CREATE INDEX [IX_CoverageEligibilityRequests_MessageHeaderId] ON [CoverageEligibilityRequests] ([MessageHeaderId]);

CREATE INDEX [IX_CoverageEligibilityRequests_PatientId] ON [CoverageEligibilityRequests] ([PatientId]);

CREATE INDEX [IX_CoverageEligibilityRequests_ProviderId] ON [CoverageEligibilityRequests] ([ProviderId]);

CREATE UNIQUE INDEX [IX_CoverageEligibilityRequests_RequestId] ON [CoverageEligibilityRequests] ([RequestId]);

CREATE INDEX [IX_CoverageEligibilityRequests_Status] ON [CoverageEligibilityRequests] ([Status]);

CREATE INDEX [IX_CoverageEligibilityResponses_CoverageId] ON [CoverageEligibilityResponses] ([CoverageId]);

CREATE UNIQUE INDEX [IX_CoverageEligibilityResponses_EligibilityRequestId] ON [CoverageEligibilityResponses] ([EligibilityRequestId]);

CREATE INDEX [IX_CoverageEligibilityResponses_InsurerId] ON [CoverageEligibilityResponses] ([InsurerId]);

CREATE INDEX [IX_CoverageEligibilityResponses_MessageHeaderId] ON [CoverageEligibilityResponses] ([MessageHeaderId]);

CREATE INDEX [IX_CoverageEligibilityResponses_Outcome] ON [CoverageEligibilityResponses] ([Outcome]);

CREATE INDEX [IX_CoverageEligibilityResponses_PatientId] ON [CoverageEligibilityResponses] ([PatientId]);

CREATE INDEX [IX_CoverageEligibilityResponses_RequestId] ON [CoverageEligibilityResponses] ([RequestId]);

CREATE UNIQUE INDEX [IX_CoverageEligibilityResponses_ResponseUUID] ON [CoverageEligibilityResponses] ([ResponseUUID]);

CREATE INDEX [IX_Coverages_InsurerId] ON [Coverages] ([InsurerId]);

CREATE INDEX [IX_Coverages_MemberID] ON [Coverages] ([MemberID]);

CREATE INDEX [IX_Coverages_PatientId] ON [Coverages] ([PatientId]);

CREATE UNIQUE INDEX [IX_Coverages_PolicyNumber] ON [Coverages] ([PolicyNumber]);

CREATE INDEX [IX_Coverages_Status] ON [Coverages] ([Status]);

CREATE INDEX [IX_EligibilityErrors_EligibilityRequestId] ON [EligibilityErrors] ([EligibilityRequestId]);

CREATE INDEX [IX_EligibilityErrors_EligibilityResponseId] ON [EligibilityErrors] ([EligibilityResponseId]);

CREATE INDEX [IX_EligibilityErrors_ErrorCode] ON [EligibilityErrors] ([ErrorCode]);

CREATE INDEX [IX_EligibilityErrors_Severity] ON [EligibilityErrors] ([Severity]);

CREATE INDEX [IX_EligibilityItemModifiers_EligibilityItemId] ON [EligibilityItemModifiers] ([EligibilityItemId]);

CREATE INDEX [IX_EligibilityItems_Category] ON [EligibilityItems] ([Category]);

CREATE INDEX [IX_EligibilityItems_EligibilityRequestId] ON [EligibilityItems] ([EligibilityRequestId]);

CREATE INDEX [IX_Encounters_Class] ON [Encounters] ([Class]);

CREATE UNIQUE INDEX [IX_Encounters_EncounterId] ON [Encounters] ([EncounterId]);

CREATE INDEX [IX_Encounters_PatientId] ON [Encounters] ([PatientId]);

CREATE INDEX [IX_Encounters_ServiceProviderId] ON [Encounters] ([ServiceProviderId]);

CREATE INDEX [IX_Encounters_Status] ON [Encounters] ([Status]);

CREATE UNIQUE INDEX [IX_Locations_LocationLicense] ON [Locations] ([LocationLicense]);

CREATE INDEX [IX_Locations_OrganizationId] ON [Locations] ([OrganizationId]);

CREATE INDEX [IX_Locations_Status] ON [Locations] ([Status]);

CREATE INDEX [IX_MessageHeaders_EventCode] ON [MessageHeaders] ([EventCode]);

CREATE UNIQUE INDEX [IX_MessageHeaders_MessageUUID] ON [MessageHeaders] ([MessageUUID]);

CREATE INDEX [IX_MessageHeaders_SenderOrganizationId] ON [MessageHeaders] ([SenderOrganizationId]);

CREATE INDEX [IX_MessageHeaders_Status] ON [MessageHeaders] ([Status]);

CREATE UNIQUE INDEX [IX_Organizations_LicenseNumber] ON [Organizations] ([LicenseNumber]);

CREATE INDEX [IX_Organizations_OrganizationType] ON [Organizations] ([OrganizationType]);

CREATE INDEX [IX_Organizations_Status] ON [Organizations] ([Status]);

CREATE INDEX [IX_Patients_IsActive] ON [Patients] ([IsActive]);

CREATE UNIQUE INDEX [IX_Patients_MRN] ON [Patients] ([MRN]);

CREATE INDEX [IX_Patients_Status] ON [Patients] ([Status]);

CREATE UNIQUE INDEX [IX_Practitioners_LicenseNumber] ON [Practitioners] ([LicenseNumber]);

CREATE INDEX [IX_Practitioners_OrganizationId] ON [Practitioners] ([OrganizationId]);

CREATE INDEX [IX_Practitioners_Status] ON [Practitioners] ([Status]);

CREATE INDEX [IX_TaskRequests_Code] ON [TaskRequests] ([Code]);

CREATE INDEX [IX_TaskRequests_FocusIdentifierValue] ON [TaskRequests] ([FocusIdentifierValue]);

CREATE INDEX [IX_TaskRequests_OwnerId] ON [TaskRequests] ([OwnerId]);

CREATE INDEX [IX_TaskRequests_ReasonCode] ON [TaskRequests] ([ReasonCode]);

CREATE INDEX [IX_TaskRequests_RequesterId] ON [TaskRequests] ([RequesterId]);

CREATE INDEX [IX_TaskRequests_Status] ON [TaskRequests] ([Status]);

CREATE INDEX [IX_TaskRequests_TaskId] ON [TaskRequests] ([TaskId]);

CREATE INDEX [IX_TaskResponses_Code] ON [TaskResponses] ([Code]);

CREATE INDEX [IX_TaskResponses_FocusIdentifierValue] ON [TaskResponses] ([FocusIdentifierValue]);

CREATE INDEX [IX_TaskResponses_OwnerId] ON [TaskResponses] ([OwnerId]);

CREATE INDEX [IX_TaskResponses_RequesterId] ON [TaskResponses] ([RequesterId]);

CREATE INDEX [IX_TaskResponses_ResponseCode] ON [TaskResponses] ([ResponseCode]);

CREATE INDEX [IX_TaskResponses_Status] ON [TaskResponses] ([Status]);

CREATE INDEX [IX_TaskResponses_TaskId] ON [TaskResponses] ([TaskId]);

CREATE INDEX [IX_TaskResponses_TaskRequestId] ON [TaskResponses] ([TaskRequestId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260623131839_AddTaskRequestAndTaskResponseEntities', N'9.0.0');

CREATE TABLE [CommunicationRequests] (
    [Id] nvarchar(100) NOT NULL,
    [CommunicationRequestId] nvarchar(100) NOT NULL,
    [IdentifierSystem] nvarchar(500) NULL,
    [IdentifierValue] nvarchar(100) NULL,
    [Status] nvarchar(50) NOT NULL,
    [Category] nvarchar(100) NULL,
    [CategorySystem] nvarchar(500) NULL,
    [Priority] nvarchar(50) NULL,
    [SubjectPatientId] nvarchar(100) NULL,
    [AboutResourceType] nvarchar(100) NULL,
    [AboutIdentifierSystem] nvarchar(500) NULL,
    [AboutIdentifierValue] nvarchar(100) NULL,
    [PayloadContent] nvarchar(max) NULL,
    [RecipientId] nvarchar(450) NULL,
    [SenderId] nvarchar(450) NULL,
    [FhirCommunicationRequestJson] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_CommunicationRequests] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CommunicationRequests_Organizations_RecipientId] FOREIGN KEY ([RecipientId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CommunicationRequests_Organizations_SenderId] FOREIGN KEY ([SenderId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CommunicationRequests_Patients_SubjectPatientId] FOREIGN KEY ([SubjectPatientId]) REFERENCES [Patients] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Communications] (
    [Id] nvarchar(100) NOT NULL,
    [CommunicationId] nvarchar(100) NOT NULL,
    [IdentifierSystem] nvarchar(500) NULL,
    [IdentifierValue] nvarchar(100) NULL,
    [BasedOnResourceType] nvarchar(100) NULL,
    [BasedOnIdentifierSystem] nvarchar(500) NULL,
    [BasedOnIdentifierValue] nvarchar(100) NULL,
    [Status] nvarchar(50) NOT NULL,
    [Category] nvarchar(100) NULL,
    [CategorySystem] nvarchar(500) NULL,
    [Priority] nvarchar(50) NULL,
    [SubjectPatientId] nvarchar(100) NULL,
    [AboutResourceType] nvarchar(100) NULL,
    [AboutIdentifierSystem] nvarchar(500) NULL,
    [AboutIdentifierValue] nvarchar(100) NULL,
    [PayloadContent] nvarchar(max) NULL,
    [RecipientId] nvarchar(450) NULL,
    [SenderId] nvarchar(450) NULL,
    [PayloadAttachmentContentType] nvarchar(100) NULL,
    [PayloadAttachmentData] varbinary(max) NULL,
    [PayloadAttachmentTitle] nvarchar(500) NULL,
    [PayloadAttachmentCreation] datetime2 NULL,
    [FhirCommunicationJson] nvarchar(max) NULL,
    [MessageHeaderId] nvarchar(max) NULL,
    [ProcessingStatus] nvarchar(max) NOT NULL,
    [ProcessedAt] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Communications] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Communications_Organizations_RecipientId] FOREIGN KEY ([RecipientId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Communications_Organizations_SenderId] FOREIGN KEY ([SenderId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Communications_Patients_SubjectPatientId] FOREIGN KEY ([SubjectPatientId]) REFERENCES [Patients] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_CommunicationRequests_CommunicationRequestId] ON [CommunicationRequests] ([CommunicationRequestId]);

CREATE INDEX [IX_CommunicationRequests_RecipientId] ON [CommunicationRequests] ([RecipientId]);

CREATE INDEX [IX_CommunicationRequests_SenderId] ON [CommunicationRequests] ([SenderId]);

CREATE INDEX [IX_CommunicationRequests_Status] ON [CommunicationRequests] ([Status]);

CREATE INDEX [IX_CommunicationRequests_SubjectPatientId] ON [CommunicationRequests] ([SubjectPatientId]);

CREATE INDEX [IX_Communications_CommunicationId] ON [Communications] ([CommunicationId]);

CREATE INDEX [IX_Communications_RecipientId] ON [Communications] ([RecipientId]);

CREATE INDEX [IX_Communications_SenderId] ON [Communications] ([SenderId]);

CREATE INDEX [IX_Communications_Status] ON [Communications] ([Status]);

CREATE INDEX [IX_Communications_SubjectPatientId] ON [Communications] ([SubjectPatientId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260623145559_AddCommunicationTables', N'9.0.0');

CREATE TABLE [PollingRecords] (
    [Id] nvarchar(100) NOT NULL,
    [PollingRecordId] nvarchar(100) NOT NULL,
    [ProviderId] nvarchar(450) NOT NULL,
    [RequestTaskId] nvarchar(100) NULL,
    [TaskRequestId] nvarchar(100) NULL,
    [RequestedMessageTypes] nvarchar(500) NULL,
    [RequestSentAt] datetime2 NULL,
    [ResponseTaskId] nvarchar(100) NULL,
    [TaskResponseId] nvarchar(100) NULL,
    [ResponseStatus] nvarchar(50) NULL,
    [ResponseReceivedAt] datetime2 NULL,
    [MessagesReceived] int NOT NULL,
    [ReceivedMessageTypes] nvarchar(500) NULL,
    [RequestBundleJson] ntext NULL,
    [ResponseBundleJson] ntext NULL,
    [ProcessingStatus] nvarchar(50) NOT NULL,
    [HttpStatusCode] int NULL,
    [ErrorMessage] nvarchar(1000) NULL,
    [ErrorCode] nvarchar(100) NULL,
    [DurationMs] bigint NULL,
    [CycleStatus] nvarchar(50) NOT NULL,
    [IsAcknowledged] bit NOT NULL,
    [AcknowledgedAt] datetime2 NULL,
    [RetryCount] int NOT NULL,
    [MaxRetries] int NULL,
    [NextRetryAt] datetime2 NULL,
    [SourceIpAddress] nvarchar(50) NULL,
    [RequestSourceId] nvarchar(100) NULL,
    [Notes] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_PollingRecords] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PollingRecords_Organizations_ProviderId] FOREIGN KEY ([ProviderId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PollingRecords_TaskRequests_TaskRequestId] FOREIGN KEY ([TaskRequestId]) REFERENCES [TaskRequests] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_PollingRecords_TaskResponses_TaskResponseId] FOREIGN KEY ([TaskResponseId]) REFERENCES [TaskResponses] ([Id]) ON DELETE SET NULL
);

CREATE INDEX [IX_PollingRecords_CycleStatus] ON [PollingRecords] ([CycleStatus]);

CREATE INDEX [IX_PollingRecords_IsAcknowledged] ON [PollingRecords] ([IsAcknowledged]);

CREATE INDEX [IX_PollingRecords_PollingRecordId] ON [PollingRecords] ([PollingRecordId]);

CREATE INDEX [IX_PollingRecords_ProcessingStatus] ON [PollingRecords] ([ProcessingStatus]);

CREATE INDEX [IX_PollingRecords_ProviderId] ON [PollingRecords] ([ProviderId]);

CREATE INDEX [IX_PollingRecords_RequestSentAt] ON [PollingRecords] ([RequestSentAt]);

CREATE INDEX [IX_PollingRecords_ResponseReceivedAt] ON [PollingRecords] ([ResponseReceivedAt]);

CREATE INDEX [IX_PollingRecords_ResponseStatus] ON [PollingRecords] ([ResponseStatus]);

CREATE INDEX [IX_PollingRecords_TaskRequestId] ON [PollingRecords] ([TaskRequestId]);

CREATE INDEX [IX_PollingRecords_TaskResponseId] ON [PollingRecords] ([TaskResponseId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260623152236_AddPollingRecordTable', N'9.0.0');

CREATE TABLE [BenefitCodeMaster] (
    [Id] nvarchar(100) NOT NULL,
    [BenefitCode] nvarchar(50) NOT NULL,
    [BenefitName] nvarchar(255) NOT NULL,
    [BenefitCategory] nvarchar(100) NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] nvarchar(100) NULL,
    [ModifiedBy] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_BenefitCodeMaster] PRIMARY KEY ([Id])
);

CREATE TABLE [DiagnosisCodeMaster] (
    [Id] nvarchar(100) NOT NULL,
    [DiagnosisCode] nvarchar(20) NOT NULL,
    [DiagnosisName] nvarchar(255) NOT NULL,
    [DiagnosisCategory] nvarchar(100) NULL,
    [DiagnosisType] nvarchar(50) NULL,
    [IsOnAdmission] bit NOT NULL,
    [NphiesDiagnosisCode] nvarchar(20) NULL,
    [IsNphiesMapped] bit NOT NULL,
    [Severity] nvarchar(50) NULL,
    [RequiresDocumentation] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] nvarchar(100) NULL,
    [ModifiedBy] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_DiagnosisCodeMaster] PRIMARY KEY ([Id])
);

CREATE TABLE [MedicalDeviceCodeMaster] (
    [Id] nvarchar(100) NOT NULL,
    [DeviceCode] nvarchar(50) NOT NULL,
    [DeviceName] nvarchar(255) NOT NULL,
    [DeviceDescription] nvarchar(1000) NULL,
    [DeviceType] nvarchar(100) NULL,
    [Manufacturer] nvarchar(255) NULL,
    [NphiesDeviceCode] nvarchar(50) NULL,
    [IsNphiesMapped] bit NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    [CurrencyCode] nvarchar(3) NOT NULL,
    [IsImplantable] bit NOT NULL,
    [IsReusable] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] nvarchar(100) NULL,
    [ModifiedBy] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_MedicalDeviceCodeMaster] PRIMARY KEY ([Id])
);

CREATE TABLE [MedicationCodeMaster] (
    [Id] nvarchar(100) NOT NULL,
    [MedicationCode] nvarchar(50) NOT NULL,
    [MedicationName] nvarchar(255) NOT NULL,
    [ActiveIngredient] nvarchar(255) NULL,
    [Strength] nvarchar(100) NULL,
    [Unit] nvarchar(50) NULL,
    [Form] nvarchar(50) NULL,
    [Manufacturer] nvarchar(255) NULL,
    [NphiesMedicationCode] nvarchar(50) NULL,
    [IsNphiesMapped] bit NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    [CurrencyCode] nvarchar(3) NOT NULL,
    [IsControlledSubstance] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] nvarchar(100) NULL,
    [ModifiedBy] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_MedicationCodeMaster] PRIMARY KEY ([Id])
);

CREATE TABLE [ModifierCodeMaster] (
    [Id] nvarchar(100) NOT NULL,
    [ModifierCode] nvarchar(10) NOT NULL,
    [ModifierName] nvarchar(255) NOT NULL,
    [ModifierDescription] nvarchar(500) NULL,
    [ModifierType] nvarchar(50) NULL,
    [ImpactOnCharges] nvarchar(100) NULL,
    [ChargePercentage] decimal(5,2) NULL,
    [NphiesModifierCode] nvarchar(10) NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] nvarchar(100) NULL,
    [ModifiedBy] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_ModifierCodeMaster] PRIMARY KEY ([Id])
);

CREATE TABLE [NphiesCodeMapping] (
    [Id] nvarchar(100) NOT NULL,
    [LocalCode] nvarchar(50) NOT NULL,
    [LocalCodeSystem] nvarchar(500) NULL,
    [LocalDescription] nvarchar(255) NULL,
    [NphiesCode] nvarchar(50) NOT NULL,
    [NphiesCodeSystem] nvarchar(500) NOT NULL,
    [NphiesDescription] nvarchar(255) NULL,
    [CodeType] nvarchar(50) NOT NULL,
    [IsMappingValid] bit NOT NULL,
    [MappingValidationDate] datetime2 NULL,
    [Notes] nvarchar(1000) NULL,
    [CreatedBy] nvarchar(100) NULL,
    [ModifiedBy] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_NphiesCodeMapping] PRIMARY KEY ([Id])
);

CREATE TABLE [PayerMaster] (
    [Id] nvarchar(100) NOT NULL,
    [PayerId] nvarchar(100) NOT NULL,
    [PayerName] nvarchar(255) NOT NULL,
    [PayerType] nvarchar(50) NULL,
    [NphiesConnectionStatus] nvarchar(50) NULL,
    [NphiesApiEndpoint] nvarchar(500) NULL,
    [IsNphiesMember] bit NOT NULL,
    [SupportedClaimTypes] nvarchar(500) NULL,
    [SupportedEligibilityTypes] nvarchar(500) NULL,
    [MaxClaimsPerDay] int NOT NULL,
    [MaxClaimAmount] decimal(18,2) NULL,
    [CurrencyCode] nvarchar(3) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] nvarchar(100) NULL,
    [ModifiedBy] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_PayerMaster] PRIMARY KEY ([Id])
);

CREATE TABLE [ServiceCodeMaster] (
    [Id] nvarchar(100) NOT NULL,
    [ServiceCode] nvarchar(50) NOT NULL,
    [ServiceName] nvarchar(255) NOT NULL,
    [ServiceDescription] nvarchar(1000) NULL,
    [ServiceCategory] nvarchar(100) NOT NULL,
    [NphiesServiceCode] nvarchar(50) NULL,
    [NphiesServiceName] nvarchar(255) NULL,
    [NphiesCategoryCode] nvarchar(50) NULL,
    [IsNphiesMapped] bit NOT NULL,
    [MappingValidationStatus] nvarchar(50) NULL,
    [DefaultPrice] decimal(18,2) NOT NULL,
    [CurrencyCode] nvarchar(3) NOT NULL,
    [IsRequiresAuthorization] bit NOT NULL,
    [DefaultAuthorizationDays] int NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] nvarchar(100) NULL,
    [ModifiedBy] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_ServiceCodeMaster] PRIMARY KEY ([Id])
);

CREATE TABLE [ClinicMaster] (
    [Id] nvarchar(100) NOT NULL,
    [OrganizationId] nvarchar(450) NOT NULL,
    [ClinicName] nvarchar(255) NOT NULL,
    [ClinicCode] nvarchar(50) NOT NULL,
    [ClinicType] nvarchar(100) NULL,
    [SpecializedServices] nvarchar(500) NULL,
    [NumberOfBeds] int NULL,
    [NumberOfDoctors] int NULL,
    [NumberOfNurses] int NULL,
    [IsCertifiedBy] nvarchar(255) NULL,
    [AccreditationLevel] nvarchar(100) NULL,
    [WorkingHoursFrom] time NULL,
    [WorkingHoursTo] time NULL,
    [IsEmergencyAvailable] bit NOT NULL,
    [PharmacyAvailable] bit NOT NULL,
    [LabAvailable] bit NOT NULL,
    [ImagingAvailable] bit NOT NULL,
    [AcceptsCashPayment] bit NOT NULL,
    [AcceptsInsurance] bit NOT NULL,
    [AcceptsCardPayment] bit NOT NULL,
    [AvailableBeds] int NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] nvarchar(100) NULL,
    [ModifiedBy] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_ClinicMaster] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClinicMaster_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [PayerPolicyMaster] (
    [Id] nvarchar(100) NOT NULL,
    [PayerMasterId] nvarchar(100) NOT NULL,
    [PolicyCode] nvarchar(100) NOT NULL,
    [PolicyName] nvarchar(255) NOT NULL,
    [PolicyType] nvarchar(100) NULL,
    [CoverageType] nvarchar(100) NULL,
    [AnnualPremium] decimal(18,2) NOT NULL,
    [CurrencyCode] nvarchar(3) NOT NULL,
    [AnnualDeductible] decimal(18,2) NOT NULL,
    [MaxOutOfPocket] decimal(18,2) NULL,
    [Copay] decimal(18,2) NULL,
    [CoinsurancePercentage] decimal(5,2) NULL,
    [CoverageLimitPerVisit] decimal(18,2) NULL,
    [CoverageLimitPerYear] decimal(18,2) NULL,
    [PreAuthRequiredForAmount] decimal(18,2) NULL,
    [EffectiveFromDate] datetime2 NOT NULL,
    [EffectiveToDate] datetime2 NULL,
    [IsPolicyActive] bit NOT NULL,
    [Notes] nvarchar(1000) NULL,
    [CreatedBy] nvarchar(100) NULL,
    [ModifiedBy] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_PayerPolicyMaster] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PayerPolicyMaster_PayerMaster_PayerMasterId] FOREIGN KEY ([PayerMasterId]) REFERENCES [PayerMaster] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [DoctorMaster] (
    [Id] nvarchar(100) NOT NULL,
    [PractitionerId] nvarchar(100) NOT NULL,
    [DoctorCode] nvarchar(50) NOT NULL,
    [DoctorName] nvarchar(255) NOT NULL,
    [Specialization] nvarchar(100) NULL,
    [SubSpecialization] nvarchar(100) NULL,
    [QualificationDegree] nvarchar(50) NULL,
    [UniversityName] nvarchar(255) NULL,
    [YearsOfExperience] int NULL,
    [IsConsultant] bit NOT NULL,
    [ConsultationFee] decimal(18,2) NULL,
    [FollowupFee] decimal(18,2) NULL,
    [CurrencyCode] nvarchar(3) NOT NULL,
    [ClinicMasterId] nvarchar(100) NULL,
    [IsAvailableForAppointments] bit NOT NULL,
    [AvailableSlotsPerDay] int NULL,
    [Board] nvarchar(100) NULL,
    [BoardLicenseNumber] nvarchar(100) NULL,
    [BoardLicenseExpiry] datetime2 NULL,
    [ResearchPapers] int NULL,
    [IsTeachingMember] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] nvarchar(100) NULL,
    [ModifiedBy] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_DoctorMaster] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DoctorMaster_ClinicMaster_ClinicMasterId] FOREIGN KEY ([ClinicMasterId]) REFERENCES [ClinicMaster] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_DoctorMaster_Practitioners_PractitionerId] FOREIGN KEY ([PractitionerId]) REFERENCES [Practitioners] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [PolicyBenefitCoverage] (
    [Id] nvarchar(100) NOT NULL,
    [PolicyMasterId] nvarchar(100) NOT NULL,
    [ServiceCodeMasterId] nvarchar(100) NULL,
    [ServiceCategory] nvarchar(100) NULL,
    [BenefitType] nvarchar(100) NULL,
    [CoveragePercentage] decimal(5,2) NOT NULL,
    [MaxCoverageAmount] decimal(18,2) NULL,
    [RequiresPreAuth] bit NOT NULL,
    [RequiresReferral] bit NOT NULL,
    [PreAuthValidityDays] int NULL,
    [CoverageLimitPerYear] int NULL,
    [CoverageLimitPerLifetime] int NULL,
    [IsExcluded] bit NOT NULL,
    [ExclusionReason] nvarchar(500) NULL,
    [IsWaitingPeriodApplicable] bit NOT NULL,
    [WaitingPeriodDays] int NULL,
    [Notes] nvarchar(1000) NULL,
    [CreatedBy] nvarchar(100) NULL,
    [ModifiedBy] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_PolicyBenefitCoverage] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PolicyBenefitCoverage_PayerPolicyMaster_PolicyMasterId] FOREIGN KEY ([PolicyMasterId]) REFERENCES [PayerPolicyMaster] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PolicyBenefitCoverage_ServiceCodeMaster_ServiceCodeMasterId] FOREIGN KEY ([ServiceCodeMasterId]) REFERENCES [ServiceCodeMaster] ([Id])
);

CREATE TABLE [DoctorQualification] (
    [Id] nvarchar(100) NOT NULL,
    [DoctorMasterId] nvarchar(100) NOT NULL,
    [QualificationType] nvarchar(100) NOT NULL,
    [QualificationName] nvarchar(255) NOT NULL,
    [UniversityName] nvarchar(255) NOT NULL,
    [IssuedDate] datetime2 NOT NULL,
    [IsExpiring] bit NOT NULL,
    [ExpiryDate] datetime2 NULL,
    [CertificateNumber] nvarchar(100) NULL,
    [VerificationStatus] nvarchar(50) NULL,
    [CreatedBy] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_DoctorQualification] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DoctorQualification_DoctorMaster_DoctorMasterId] FOREIGN KEY ([DoctorMasterId]) REFERENCES [DoctorMaster] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ClaimSubmissionRules] (
    [Id] nvarchar(100) NOT NULL,
    [PayerMasterId] nvarchar(100) NULL,
    [PolicyMasterId] nvarchar(100) NULL,
    [RuleName] nvarchar(255) NOT NULL,
    [RuleType] nvarchar(100) NULL,
    [RuleCondition] nvarchar(1000) NULL,
    [MaxClaimAmount] decimal(18,2) NULL,
    [MaxItemsPerClaim] int NULL,
    [RequiresInvoice] bit NOT NULL,
    [RequiresMedicalReport] bit NOT NULL,
    [RequiresPhotos] bit NOT NULL,
    [MaxDaysForSubmission] int NULL,
    [IsActive] bit NOT NULL,
    [Priority] int NOT NULL,
    [CreatedBy] nvarchar(100) NULL,
    [ModifiedBy] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_ClaimSubmissionRules] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClaimSubmissionRules_PayerMaster_PayerMasterId] FOREIGN KEY ([PayerMasterId]) REFERENCES [PayerMaster] ([Id]),
    CONSTRAINT [FK_ClaimSubmissionRules_PayerPolicyMaster_PolicyMasterId] FOREIGN KEY ([PolicyMasterId]) REFERENCES [PayerPolicyMaster] ([Id])
);

CREATE UNIQUE INDEX [IX_BenefitCodeMaster_BenefitCode] ON [BenefitCodeMaster] ([BenefitCode]);

CREATE INDEX [IX_BenefitCodeMaster_IsActive] ON [BenefitCodeMaster] ([IsActive]);

CREATE UNIQUE INDEX [IX_DiagnosisCodeMaster_DiagnosisCode] ON [DiagnosisCodeMaster] ([DiagnosisCode]);

CREATE INDEX [IX_DiagnosisCodeMaster_DiagnosisCategory] ON [DiagnosisCodeMaster] ([DiagnosisCategory]);

CREATE INDEX [IX_DiagnosisCodeMaster_IsActive] ON [DiagnosisCodeMaster] ([IsActive]);

CREATE UNIQUE INDEX [IX_MedicalDeviceCodeMaster_DeviceCode] ON [MedicalDeviceCodeMaster] ([DeviceCode]);

CREATE INDEX [IX_MedicalDeviceCodeMaster_IsActive] ON [MedicalDeviceCodeMaster] ([IsActive]);

CREATE UNIQUE INDEX [IX_MedicationCodeMaster_MedicationCode] ON [MedicationCodeMaster] ([MedicationCode]);

CREATE INDEX [IX_MedicationCodeMaster_IsActive] ON [MedicationCodeMaster] ([IsActive]);

CREATE UNIQUE INDEX [IX_ModifierCodeMaster_ModifierCode] ON [ModifierCodeMaster] ([ModifierCode]);

CREATE INDEX [IX_ModifierCodeMaster_IsActive] ON [ModifierCodeMaster] ([IsActive]);

CREATE UNIQUE INDEX [IX_NphiesCodeMapping_LocalCode_LocalCodeSystem] ON [NphiesCodeMapping] ([LocalCode], [LocalCodeSystem]) WHERE [LocalCodeSystem] IS NOT NULL;

CREATE INDEX [IX_NphiesCodeMapping_NphiesCode] ON [NphiesCodeMapping] ([NphiesCode]);

CREATE INDEX [IX_NphiesCodeMapping_CodeType] ON [NphiesCodeMapping] ([CodeType]);

CREATE INDEX [IX_NphiesCodeMapping_IsMappingValid] ON [NphiesCodeMapping] ([IsMappingValid]);

CREATE UNIQUE INDEX [IX_PayerMaster_PayerId] ON [PayerMaster] ([PayerId]);

CREATE INDEX [IX_PayerMaster_IsActive] ON [PayerMaster] ([IsActive]);

CREATE UNIQUE INDEX [IX_ServiceCodeMaster_ServiceCode] ON [ServiceCodeMaster] ([ServiceCode]);

CREATE INDEX [IX_ServiceCodeMaster_ServiceCategory] ON [ServiceCodeMaster] ([ServiceCategory]);

CREATE INDEX [IX_ServiceCodeMaster_IsNphiesMapped] ON [ServiceCodeMaster] ([IsNphiesMapped]);

CREATE INDEX [IX_ServiceCodeMaster_IsActive] ON [ServiceCodeMaster] ([IsActive]);

CREATE UNIQUE INDEX [IX_ClinicMaster_ClinicCode] ON [ClinicMaster] ([ClinicCode]);

CREATE INDEX [IX_ClinicMaster_OrganizationId] ON [ClinicMaster] ([OrganizationId]);

CREATE INDEX [IX_ClinicMaster_IsActive] ON [ClinicMaster] ([IsActive]);

CREATE INDEX [IX_PayerPolicyMaster_PayerMasterId] ON [PayerPolicyMaster] ([PayerMasterId]);

CREATE UNIQUE INDEX [IX_PayerPolicyMaster_PolicyCode] ON [PayerPolicyMaster] ([PolicyCode]);

CREATE INDEX [IX_PayerPolicyMaster_IsPolicyActive] ON [PayerPolicyMaster] ([IsPolicyActive]);

CREATE INDEX [IX_DoctorMaster_PractitionerId] ON [DoctorMaster] ([PractitionerId]);

CREATE INDEX [IX_DoctorMaster_ClinicMasterId] ON [DoctorMaster] ([ClinicMasterId]);

CREATE UNIQUE INDEX [IX_DoctorMaster_DoctorCode] ON [DoctorMaster] ([DoctorCode]);

CREATE INDEX [IX_DoctorMaster_BoardLicenseNumber] ON [DoctorMaster] ([BoardLicenseNumber]);

CREATE INDEX [IX_DoctorMaster_IsActive] ON [DoctorMaster] ([IsActive]);

CREATE INDEX [IX_PolicyBenefitCoverage_PolicyMasterId] ON [PolicyBenefitCoverage] ([PolicyMasterId]);

CREATE INDEX [IX_PolicyBenefitCoverage_ServiceCodeMasterId] ON [PolicyBenefitCoverage] ([ServiceCodeMasterId]);

CREATE INDEX [IX_DoctorQualification_DoctorMasterId] ON [DoctorQualification] ([DoctorMasterId]);

CREATE INDEX [IX_ClaimSubmissionRules_PayerMasterId] ON [ClaimSubmissionRules] ([PayerMasterId]);

CREATE INDEX [IX_ClaimSubmissionRules_PolicyMasterId] ON [ClaimSubmissionRules] ([PolicyMasterId]);

CREATE INDEX [IX_ClaimSubmissionRules_IsActive] ON [ClaimSubmissionRules] ([IsActive]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260624120000_AddMasterDataTables', N'9.0.0');

CREATE TABLE [Users] (
    [Id] nvarchar(100) NOT NULL,
    [Username] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [PasswordSalt] nvarchar(max) NOT NULL,
    [Roles] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [IsEmailVerified] bit NOT NULL,
    [IsMfaEnabled] bit NOT NULL,
    [MfaSecret] nvarchar(max) NULL,
    [IsLocked] bit NOT NULL,
    [FailedLoginAttempts] int NOT NULL,
    [LastLoginAt] datetime2 NULL,
    [LockedUntilAt] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [OrganizationId] nvarchar(max) NULL,
    [DepartmentId] nvarchar(max) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

CREATE TABLE [AuditLogs] (
    [Id] nvarchar(100) NOT NULL,
    [UserId] nvarchar(100) NULL,
    [Username] nvarchar(max) NULL,
    [Action] nvarchar(max) NOT NULL,
    [EntityType] nvarchar(max) NULL,
    [EntityId] nvarchar(max) NULL,
    [OldValues] nvarchar(max) NULL,
    [NewValues] nvarchar(max) NULL,
    [ChangeDetails] nvarchar(max) NULL,
    [IpAddress] nvarchar(max) NOT NULL,
    [UserAgent] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [AuditLevel] nvarchar(max) NOT NULL,
    [Endpoint] nvarchar(max) NULL,
    [HttpStatusCode] int NULL,
    [DurationMs] float NULL,
    CONSTRAINT [PK_AuditLogs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AuditLogs_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);

CREATE TABLE [LoginAttempts] (
    [Id] nvarchar(100) NOT NULL,
    [UserId] nvarchar(100) NULL,
    [Username] nvarchar(max) NOT NULL,
    [IpAddress] nvarchar(max) NOT NULL,
    [UserAgent] nvarchar(max) NULL,
    [IsSuccessful] bit NOT NULL,
    [FailureReason] nvarchar(max) NULL,
    [AttemptAt] datetime2 NOT NULL,
    [DurationMs] float NULL,
    CONSTRAINT [PK_LoginAttempts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_LoginAttempts_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);

CREATE TABLE [RateLimitLogs] (
    [Id] nvarchar(100) NOT NULL,
    [UserId] nvarchar(100) NULL,
    [IpAddress] nvarchar(max) NOT NULL,
    [Endpoint] nvarchar(max) NOT NULL,
    [HttpMethod] nvarchar(max) NOT NULL,
    [RequestCount] int NOT NULL,
    [MaxRequests] int NOT NULL,
    [WindowStart] datetime2 NOT NULL,
    [WindowEnd] datetime2 NOT NULL,
    [IsRateLimited] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [ResetAt] datetime2 NULL,
    CONSTRAINT [PK_RateLimitLogs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RateLimitLogs_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);

CREATE TABLE [RefreshTokens] (
    [Id] nvarchar(100) NOT NULL,
    [UserId] nvarchar(100) NOT NULL,
    [Token] nvarchar(max) NOT NULL,
    [ExpiresAt] datetime2 NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [IpAddress] nvarchar(max) NULL,
    [UserAgent] nvarchar(max) NULL,
    [IsRevoked] bit NOT NULL,
    [RevokedAt] datetime2 NULL,
    [RevokedBy] nvarchar(max) NULL,
    [ReplacedByToken] nvarchar(max) NULL,
    CONSTRAINT [PK_RefreshTokens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RefreshTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AuditLogs_UserId] ON [AuditLogs] ([UserId]);

CREATE INDEX [IX_LoginAttempts_UserId] ON [LoginAttempts] ([UserId]);

CREATE INDEX [IX_RateLimitLogs_UserId] ON [RateLimitLogs] ([UserId]);

CREATE INDEX [IX_RefreshTokens_UserId] ON [RefreshTokens] ([UserId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260624134139_AddUserAuthenticationEntities', N'9.0.0');

COMMIT;
GO

