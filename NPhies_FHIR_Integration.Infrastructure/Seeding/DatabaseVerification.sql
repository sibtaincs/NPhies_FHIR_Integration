-- =====================================================
-- NPhies Database Verification and Seeding Check Script
-- =====================================================
-- Run this script in SQL Server Management Studio (SSMS)
-- to verify database state and seeding status
-- =====================================================

USE master;
GO

-- =====================================================
-- PART 1: DATABASE EXISTENCE CHECK
-- =====================================================
PRINT '============================================';
PRINT 'PART 1: DATABASE EXISTENCE CHECK';
PRINT '============================================';
PRINT '';

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'NPhiesDb')
BEGIN
    PRINT '? Database "NPhiesDb" EXISTS';
    
    -- Get database size
    SELECT 
      name AS DatabaseName,
        (size * 8.0 / 1024) AS SizeMB,
state_desc AS State
    FROM sys.master_files
    WHERE database_id = DB_ID('NPhiesDb') AND type_desc = 'ROWS';
END
ELSE
BEGIN
    PRINT '? Database "NPhiesDb" DOES NOT EXIST';
    PRINT 'ACTION REQUIRED: Run migrations first!';
    PRINT 'Command: dotnet ef database update --project NPhies_FHIR_Integration.Infrastructure --startup-project NPhies_FHIR_Integration.ApiService';
END
GO

USE NPhiesDb;
GO

-- =====================================================
-- PART 2: MIGRATION HISTORY CHECK
-- =====================================================
PRINT '';
PRINT '============================================';
PRINT 'PART 2: MIGRATION HISTORY CHECK';
PRINT '============================================';
PRINT '';

IF EXISTS (SELECT * FROM sys.tables WHERE name = '__EFMigrationsHistory')
BEGIN
    PRINT 'Applied Migrations:';
 SELECT 
   MigrationId,
        ProductVersion,
        CASE 
  WHEN MigrationId LIKE '%InitialCreate%' THEN '? Initial Database Creation'
      WHEN MigrationId LIKE '%PayerAndPolicyEnhancements%' THEN '? Payer & Policy Enhancements'
            WHEN MigrationId LIKE '%UpdatePayerAndPolicyMasterEntities%' THEN '? Payer & Policy Updates'
            ELSE '? ' + MigrationId
        END AS Description
    FROM __EFMigrationsHistory
    ORDER BY MigrationId;
    
    DECLARE @MigrationCount INT;
    SELECT @MigrationCount = COUNT(*) FROM __EFMigrationsHistory;
    PRINT '';
    PRINT 'Total Migrations Applied: ' + CAST(@MigrationCount AS VARCHAR(10));
END
ELSE
BEGIN
    PRINT '? No migrations table found!';
    PRINT 'ACTION REQUIRED: Run migrations first!';
END
GO

-- =====================================================
-- PART 3: MASTER TABLES EXISTENCE CHECK
-- =====================================================
PRINT '';
PRINT '============================================';
PRINT 'PART 3: MASTER TABLES EXISTENCE CHECK';
PRINT '============================================';
PRINT '';

-- Check for master tables
DECLARE @MasterTables TABLE (TableName VARCHAR(100), Exists BIT, Description VARCHAR(200));

INSERT INTO @MasterTables (TableName, Exists, Description)
VALUES
    ('ServiceCodeMasters', 0, 'Medical services and procedures'),
    ('MedicationCodeMasters', 0, 'Medications and drugs'),
    ('MedicalDeviceCodeMasters', 0, 'Medical devices and implants'),
    ('DiagnosisCodeMasters', 0, 'ICD-10 diagnosis codes'),
    ('ModifierCodeMasters', 0, 'CPT modifiers'),
    ('BenefitCodeMasters', 0, 'Insurance benefit categories'),
    ('PayerMasters', 0, 'Insurance companies'),
    ('PayerPolicyMasters', 0, 'Insurance policies'),
    ('PolicyBenefitCoverages', 0, 'Policy benefit mappings'),
    ('ClaimSubmissionRules', 0, 'Claim validation rules'),
    ('NphiesCodeMappings', 0, 'Code mappings'),
    ('ClinicMasters', 0, 'Healthcare facilities'),
    ('DoctorMasters', 0, 'Healthcare providers'),
    ('DoctorQualifications', 0, 'Doctor qualifications'),
    ('ErrorCodeMasters', 0, 'NPHIES error codes');

-- Update existence status
UPDATE @MasterTables 
SET Exists = 1
WHERE TableName IN (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE');

-- Display results
SELECT 
    CASE WHEN Exists = 1 THEN '?' ELSE '?' END AS Status,
    TableName,
    Description,
    CASE WHEN Exists = 1 THEN 'EXISTS' ELSE 'MISSING' END AS TableStatus
FROM @MasterTables
ORDER BY Exists DESC, TableName;

DECLARE @MissingTables INT;
SELECT @MissingTables = COUNT(*) FROM @MasterTables WHERE Exists = 0;

PRINT '';
IF @MissingTables = 0
    PRINT '? All master tables exist!';
ELSE
BEGIN
    PRINT '? ' + CAST(@MissingTables AS VARCHAR(10)) + ' master tables are missing!';
    PRINT 'ACTION REQUIRED: Run migrations to create missing tables!';
END
GO

-- =====================================================
-- PART 4: DATA SEEDING STATUS CHECK
-- =====================================================
PRINT '';
PRINT '============================================';
PRINT 'PART 4: DATA SEEDING STATUS CHECK';
PRINT '============================================';
PRINT '';

-- Create temp table for results
CREATE TABLE #SeedingStatus (
    TableName VARCHAR(100),
    RecordCount INT,
  ExpectedMin INT,
    Status VARCHAR(20),
    StatusIcon VARCHAR(10)
);

-- Check each master table
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'ServiceCodeMasters')
    INSERT INTO #SeedingStatus SELECT 'ServiceCodeMasters', COUNT(*), 35, '', '' FROM ServiceCodeMasters;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'MedicationCodeMasters')
    INSERT INTO #SeedingStatus SELECT 'MedicationCodeMasters', COUNT(*), 20, '', '' FROM MedicationCodeMasters;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'MedicalDeviceCodeMasters')
    INSERT INTO #SeedingStatus SELECT 'MedicalDeviceCodeMasters', COUNT(*), 11, '', '' FROM MedicalDeviceCodeMasters;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'DiagnosisCodeMasters')
    INSERT INTO #SeedingStatus SELECT 'DiagnosisCodeMasters', COUNT(*), 30, '', '' FROM DiagnosisCodeMasters;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'ModifierCodeMasters')
    INSERT INTO #SeedingStatus SELECT 'ModifierCodeMasters', COUNT(*), 12, '', '' FROM ModifierCodeMasters;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'BenefitCodeMasters')
    INSERT INTO #SeedingStatus SELECT 'BenefitCodeMasters', COUNT(*), 15, '', '' FROM BenefitCodeMasters;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PayerMasters')
    INSERT INTO #SeedingStatus SELECT 'PayerMasters', COUNT(*), 10, '', '' FROM PayerMasters;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PayerPolicyMasters')
    INSERT INTO #SeedingStatus SELECT 'PayerPolicyMasters', COUNT(*), 20, '', '' FROM PayerPolicyMasters;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PolicyBenefitCoverages')
    INSERT INTO #SeedingStatus SELECT 'PolicyBenefitCoverages', COUNT(*), 100, '', '' FROM PolicyBenefitCoverages;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'ClaimSubmissionRules')
INSERT INTO #SeedingStatus SELECT 'ClaimSubmissionRules', COUNT(*), 8, '', '' FROM ClaimSubmissionRules;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'NphiesCodeMappings')
    INSERT INTO #SeedingStatus SELECT 'NphiesCodeMappings', COUNT(*), 10, '', '' FROM NphiesCodeMappings;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'ClinicMasters')
    INSERT INTO #SeedingStatus SELECT 'ClinicMasters', COUNT(*), 3, '', '' FROM ClinicMasters;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'DoctorMasters')
    INSERT INTO #SeedingStatus SELECT 'DoctorMasters', COUNT(*), 5, '', '' FROM DoctorMasters;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'DoctorQualifications')
    INSERT INTO #SeedingStatus SELECT 'DoctorQualifications', COUNT(*), 15, '', '' FROM DoctorQualifications;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'ErrorCodeMasters')
    INSERT INTO #SeedingStatus SELECT 'ErrorCodeMasters', COUNT(*), 100, '', '' FROM ErrorCodeMasters;

-- Update status
UPDATE #SeedingStatus
SET 
    Status = CASE 
     WHEN RecordCount >= ExpectedMin THEN 'SEEDED'
        WHEN RecordCount > 0 THEN 'PARTIAL'
    ELSE 'EMPTY'
    END,
    StatusIcon = CASE 
      WHEN RecordCount >= ExpectedMin THEN '?'
        WHEN RecordCount > 0 THEN '??'
        ELSE '?'
    END;

-- Display results
SELECT 
    StatusIcon AS [Status],
TableName AS [Table Name],
    RecordCount AS [Current Count],
    ExpectedMin AS [Expected Min],
    Status AS [Seeding Status]
FROM #SeedingStatus
ORDER BY 
    CASE Status 
        WHEN 'SEEDED' THEN 1 
 WHEN 'PARTIAL' THEN 2 
        ELSE 3 
    END,
    TableName;

-- Summary
DECLARE @TotalTables INT, @SeededTables INT, @PartialTables INT, @EmptyTables INT;
SELECT @TotalTables = COUNT(*) FROM #SeedingStatus;
SELECT @SeededTables = COUNT(*) FROM #SeedingStatus WHERE Status = 'SEEDED';
SELECT @PartialTables = COUNT(*) FROM #SeedingStatus WHERE Status = 'PARTIAL';
SELECT @EmptyTables = COUNT(*) FROM #SeedingStatus WHERE Status = 'EMPTY';

PRINT '';
PRINT '?? SEEDING SUMMARY:';
PRINT '  Total Master Tables: ' + CAST(@TotalTables AS VARCHAR(10));
PRINT '  ? Fully Seeded: ' + CAST(@SeededTables AS VARCHAR(10));
PRINT '  ?? Partially Seeded: ' + CAST(@PartialTables AS VARCHAR(10));
PRINT '  ? Empty Tables: ' + CAST(@EmptyTables AS VARCHAR(10));

DROP TABLE #SeedingStatus;
GO

-- =====================================================
-- PART 5: SAMPLE DATA PREVIEW
-- =====================================================
PRINT '';
PRINT '============================================';
PRINT 'PART 5: SAMPLE DATA PREVIEW';
PRINT '============================================';
PRINT '';

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'ServiceCodeMasters')
BEGIN
    PRINT '--- Sample Services (Top 5) ---';
    SELECT TOP 5 
      ServiceCode AS Code, 
        ServiceName AS Name, 
        ServiceCategory AS Category,
        DefaultPrice AS Price,
        CASE WHEN IsNphiesMapped = 1 THEN '?' ELSE '?' END AS [NPHIES Mapped]
    FROM ServiceCodeMasters 
  ORDER BY DefaultPrice DESC;
END

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PayerMasters')
BEGIN
    PRINT '';
    PRINT '--- Insurance Companies ---';
    SELECT 
 PayerId AS [Payer ID],
        PayerName AS [Payer Name],
        City,
      CASE WHEN IsActive = 1 THEN '?' ELSE '?' END AS Active
    FROM PayerMasters
    WHERE IsActive = 1;
END

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'DoctorMasters')
BEGIN
    PRINT '';
    PRINT '--- Healthcare Providers ---';
    SELECT 
        DoctorCode AS Code,
        DoctorName AS Name,
        Specialization,
     ConsultationFee AS [Fee (SAR)]
    FROM DoctorMasters
    WHERE IsActive = 1;
END

-- =====================================================
-- PART 6: ACTION ITEMS
-- =====================================================
PRINT '';
PRINT '============================================';
PRINT 'PART 6: RECOMMENDED ACTIONS';
PRINT '============================================';
PRINT '';

-- Check overall status
DECLARE @AllTablesExist BIT = 1;
DECLARE @AllTablesSeeded BIT = 1;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ServiceCodeMasters')
  SET @AllTablesExist = 0;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'ServiceCodeMasters')
BEGIN
    IF (SELECT COUNT(*) FROM ServiceCodeMasters) = 0
     SET @AllTablesSeeded = 0;
END

IF @AllTablesExist = 0
BEGIN
    PRINT '? MIGRATION REQUIRED:';
    PRINT '   1. Open terminal in Visual Studio';
    PRINT '   2. Run: dotnet ef database update --project NPhies_FHIR_Integration.Infrastructure --startup-project NPhies_FHIR_Integration.ApiService';
    PRINT '';
END

IF @AllTablesExist = 1 AND @AllTablesSeeded = 0
BEGIN
    PRINT '?? SEEDING REQUIRED:';
    PRINT '   1. Ensure you are in Development mode';
    PRINT '   2. Run the ApiService project';
  PRINT '   3. Seeding will happen automatically on startup';
    PRINT '   OR';
    PRINT '   4. Run: dotnet run --project NPhies_FHIR_Integration.ApiService';
    PRINT '';
END

IF @AllTablesExist = 1 AND @AllTablesSeeded = 1
BEGIN
    PRINT '? SYSTEM READY!';
    PRINT ' Database is fully migrated and seeded.';
    PRINT '   You can now use the API endpoints.';
    PRINT '';
END

PRINT '============================================';
PRINT 'END OF VERIFICATION SCRIPT';
PRINT '============================================';
GO
