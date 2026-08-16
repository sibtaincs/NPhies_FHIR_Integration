-- =====================================================
-- DIAGNOSTIC: Check Seeding Issue
-- =====================================================
-- Run this to see what's actually in the database
-- =====================================================

USE NPhiesDb;
GO

PRINT '============================================';
PRINT 'DIAGNOSTIC REPORT - SEEDING ISSUE';
PRINT '============================================';
PRINT '';

-- Check if tables exist
PRINT '1. TABLE EXISTENCE CHECK:';
PRINT '============================================';
IF OBJECT_ID('ServiceCodeMasters', 'U') IS NOT NULL
    PRINT '? ServiceCodeMasters table EXISTS'
ELSE
    PRINT '? ServiceCodeMasters table MISSING - Run migrations!';

IF OBJECT_ID('PayerMasters', 'U') IS NOT NULL
    PRINT '? PayerMasters table EXISTS'
ELSE
    PRINT '? PayerMasters table MISSING - Run migrations!';

IF OBJECT_ID('Organizations', 'U') IS NOT NULL
    PRINT '? Organizations table EXISTS'
ELSE
    PRINT '? Organizations table MISSING - Run migrations!';

PRINT '';

-- Check record counts
PRINT '2. RECORD COUNT CHECK:';
PRINT '============================================';

DECLARE @ServiceCount INT, @PayerCount INT, @OrgCount INT;

SELECT @ServiceCount = COUNT(*) FROM ServiceCodeMasters;
SELECT @PayerCount = COUNT(*) FROM PayerMasters;
SELECT @OrgCount = COUNT(*) FROM Organizations;

PRINT 'ServiceCodeMasters: ' + CAST(@ServiceCount AS VARCHAR(10)) + ' records';
PRINT 'PayerMasters: ' + CAST(@PayerCount AS VARCHAR(10)) + ' records';
PRINT 'Organizations: ' + CAST(@OrgCount AS VARCHAR(10)) + ' records';

PRINT '';

-- Identify the issue
PRINT '3. ISSUE IDENTIFICATION:';
PRINT '============================================';

IF @ServiceCount = 0 AND @PayerCount = 0 AND @OrgCount = 0
BEGIN
    PRINT '? ISSUE: No data at all - Seeding never ran or failed';
    PRINT '';
 PRINT 'SOLUTION:';
    PRINT '  1. Check if application is in Development mode';
  PRINT '  2. Check application logs for errors';
    PRINT '  3. Run: dotnet run --project NPhies_FHIR_Integration.ApiService';
    PRINT '  4. Watch for "Database seeding completed successfully!" message';
END
ELSE IF @ServiceCount > 0
BEGIN
    PRINT '?? ISSUE: ServiceCodeMasters has data - Seeder is skipping due to AnyAsync() check';
    PRINT '';
  PRINT 'This means seeding was attempted but incomplete!';
    PRINT '';
    PRINT 'SOLUTION Options:';
    PRINT '  OPTION A - Clear specific table and re-run:';
    PRINT '    DELETE FROM ServiceCodeMasters;';
    PRINT '    -- Then run application again';
    PRINT '';
    PRINT '  OPTION B - Clear all master tables:';
    PRINT '    EXEC sp_MSforeachtable "DELETE FROM ? WHERE OBJECTPROPERTY(OBJECT_ID(''?''), ''IsMSShipped'') = 0"';
    PRINT '    -- Then run application again';
    PRINT '';
    PRINT '  OPTION C - Drop and recreate database:';
    PRINT '    dotnet ef database drop --force';
    PRINT '    dotnet ef database update';
PRINT '    dotnet run --project NPhies_FHIR_Integration.ApiService';
END
ELSE IF @PayerCount > 0 BUT @ServiceCount = 0
BEGIN
    PRINT '?? ISSUE: Partial seeding - PayerMasterSeeder ran but EnhancedDatabaseSeeder did not';
    PRINT '';
    PRINT 'SOLUTION:';
  PRINT '  Check application logs for errors between payer seeding and master data seeding';
END

PRINT '';

-- Check for foreign key issues
IF @OrgCount = 0
BEGIN
    PRINT '4. FOREIGN KEY ISSUE:';
    PRINT '============================================';
    PRINT '?? WARNING: No Organizations exist!';
    PRINT 'ClinicMasters and DoctorMasters need Organizations as parent records.';
    PRINT '';
    PRINT 'SOLUTION:';
 PRINT '  Organizations are seeded by DatabaseSeeder (not EnhancedDatabaseSeeder)';
    PRINT '  Enable DatabaseSeeder in Program.cs OR create sample organizations first.';
END

PRINT '';

-- Show sample data if any
PRINT '5. SAMPLE DATA (if any):';
PRINT '============================================';

IF @ServiceCount > 0
BEGIN
    PRINT 'Sample Services:';
    SELECT TOP 3 ServiceCode, ServiceName, DefaultPrice FROM ServiceCodeMasters;
END

IF @PayerCount > 0
BEGIN
    PRINT '';
    PRINT 'Sample Payers:';
    SELECT TOP 3 PayerId, PayerName, City FROM PayerMasters;
END

IF @OrgCount > 0
BEGIN
    PRINT '';
    PRINT 'Sample Organizations:';
    SELECT TOP 3 OrganizationName, OrganizationType, City FROM Organizations;
END

PRINT '';
PRINT '============================================';
PRINT 'END OF DIAGNOSTIC';
PRINT '============================================';
GO
