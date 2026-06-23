# ?? DATABASE INTEGRATION GUIDE - MS SQL SERVER

**Target**: Local MS SQL Server (localhost)
**Database**: NPhiesDb
**.NET Framework**: 9.0
**ORM**: Entity Framework Core

---

## ?? PREREQUISITES

Before proceeding, ensure you have:

### 1. MS SQL Server Running Locally
```
? SQL Server 2022 or later installed
? SQL Server running on localhost
? SA account accessible
? Authentication mode: Mixed (Windows + SQL)
```

### 2. Verify SQL Server Connection
Open SQL Server Management Studio (SSMS) and:
```sql
-- Test connection
SELECT @@VERSION
GO

-- Check databases
SELECT name FROM sys.databases
GO
```

### 3. Check Current Connection String
```
Current: Server=localhost;Database=NPhiesDb;User Id=sa;Password=YourPassword123;
Note: Replace YourPassword123 with your actual SA password
```

---

## ?? STEP 1: CREATE INITIAL MIGRATION

### 1.1 Open Package Manager Console

In Visual Studio:
- Go to: **Tools ? NuGet Package Manager ? Package Manager Console**
- Set Default Project to: **NPhies_FHIR_Integration.Infrastructure**

### 1.2 Create Initial Migration

Run the following command:

```powershell
Add-Migration InitialCreate -Context ApplicationDbContext -OutputDir Migrations -Verbose
```

**Expected Output:**
```
Build started...
Build succeeded.
To undo this action, use Remove-Migration.
```

**Files Created:**
- `NPhies_FHIR_Integration.Infrastructure/Migrations/20240101000000_InitialCreate.cs`
- `NPhies_FHIR_Integration.Infrastructure/Migrations/20240101000000_InitialCreate.Designer.cs`
- `NPhies_FHIR_Integration.Infrastructure/Migrations/ApplicationDbContextModelSnapshot.cs`

---

## ??? STEP 2: CREATE DATABASE

### 2.1 Update Database

Run this command in Package Manager Console:

```powershell
Update-Database -Context ApplicationDbContext -Verbose
```

**What This Does:**
- Creates the database `NPhiesDb` on your local SQL Server
- Creates all tables based on DbContext configuration
- Creates all indexes, foreign keys, constraints
- Seeds initial data (if configured)

**Expected Success:**
```
Build started...
Build succeeded.
Applying migration '20240101000000_InitialCreate'.
Done.
```

### 2.2 Verify Database Created

In SQL Server Management Studio:
```sql
-- Connect to localhost
-- Use Object Explorer to verify NPhiesDb exists
-- Check Tables, Stored Procedures, etc.

SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo' 
ORDER BY TABLE_NAME
```

---

## ?? STEP 3: VERIFY TABLE STRUCTURE

Run this script to verify all tables were created:

```sql
USE NPhiesDb

-- Get table count
SELECT COUNT(*) as TableCount FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo'

-- List all tables
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo' 
ORDER BY TABLE_NAME

-- Check key tables
SELECT COUNT(*) as PatientCount FROM dbo.Patients
SELECT COUNT(*) as ClaimCount FROM dbo.Claims
SELECT COUNT(*) as ClaimResponseCount FROM dbo.ClaimResponses
```

---

## ?? STEP 4: SEED INITIAL DATA

### 4.1 Create Database Seeder (Already Exists)

Your project has: `NPhies_FHIR_Integration.Infrastructure.Seeding.DatabaseSeeder`

This seeder automatically runs in development and creates:
- ? Test Organizations (Providers & Insurers)
- ? Test Patients
- ? Test Coverage/Insurance
- ? Test Practitioners
- ? Test Locations

### 4.2 Run Application in Development

When you run the application in Development mode:

```bash
dotnet run
```

The seeder automatically:
1. Checks if data exists
2. Creates test data if database is empty
3. Logs seeding progress

---

## ?? STEP 5: INTEGRATE DATABASE WITH SERVICES

### 5.1 Update Services to Use Database Instead of Mock Data

Now that your database is set up, we need to **remove mock data** from services. Here's the implementation:

#### File: `ClaimResponseProcessingService.cs`

**Before (Mock Data):**
```csharp
var originalClaim = new Claim { Id = claimId }; // Mock
```

**After (Database Query):**
```csharp
var originalClaim = await _claimRepository.GetWithDetailsAsync(claimId);
if (originalClaim == null)
{
    _logger.LogWarning("Claim {ClaimId} not found", claimId);
    return new ClaimResponseProcessingResult 
 { 
        IsSuccessful = false, 
        StatusMessage = "Claim not found" 
    };
}
```

---

## ?? COMPLETE INTEGRATION CHECKLIST

### Database Setup ?
- [ ] MS SQL Server running on localhost
- [ ] Connection string updated in appsettings.json
- [ ] Initial migration created
- [ ] Database created and verified
- [ ] Tables created with correct schema
- [ ] Indexes created
- [ ] Foreign keys configured

### Data Seeding ?
- [ ] DatabaseSeeder runs on app startup
- [ ] Test organizations created
- [ ] Test patients created
- [ ] Test coverage created
- [ ] Test practitioners created

### Service Integration ?
- [ ] ClaimResponseProcessingService uses repository
- [ ] DenialManagementService queries database
- [ ] All services use async/await
- [ ] All TODOs replaced with DB queries
- [ ] Transactions implemented for complex operations

---

## ?? NEXT STEPS FOR YOUR TEAM

### Immediate Actions (Today):

1. **Update Connection String**
   - Edit appsettings.json
   - Replace `YourPassword123` with your SA password

2. **Create Migration**
   ```powershell
   Add-Migration InitialCreate -Context ApplicationDbContext -OutputDir Migrations
   ```

3. **Update Database**
   ```powershell
   Update-Database -Context ApplicationDbContext
   ```

4. **Verify Database**
   - Open SSMS
   - Connect to localhost
   - Verify NPhiesDb exists with all tables

### This Week:

1. Remove all mock data from services
2. Update services to use repositories
3. Test data persistence end-to-end
4. Create integration tests
5. Performance tune queries

---

## ?? IMPORTANT NOTES

### Connection String Configuration

The connection string uses:
- **Server**: localhost (your local machine)
- **Database**: NPhiesDb (created by migration)
- **User Id**: sa (SQL Server admin account)
- **Password**: YourPassword123 (UPDATE WITH YOUR ACTUAL PASSWORD)
- **Encrypt**: true (secure connection)
- **TrustServerCertificate**: true (for development only)

### Production vs Development

For **Production**, use:
- Encrypted passwords in Azure Key Vault
- Connection Pooling
- Read replicas
- Always Encrypted
- Transparent Data Encryption (TDE)

---

## ?? DATABASE DIAGRAM

```
Patient
?? Coverages (1:Many)
?? Claims (1:Many)
?? EligibilityRequests (1:Many)

Claim
?? ClaimItems (1:Many)
?? ClaimDiagnoses (1:Many)
?? ClaimResponse (1:1)
?? ClaimCareTeam (1:Many)
?? ClaimSupportingInfo (1:Many)

ClaimResponse
?? ClaimResponseAddItems (1:Many)
?  ?? ClaimResponseAdjudications (1:Many)
?? ClaimResponseTotals (1:Many)
?? ClaimResponseInsurances (1:Many)
?? ClaimResponseDiagnosesExt (1:Many)
?? ClaimResponseSupportingInfoExt (1:Many)

Organization (Providers & Insurers)
?? Locations (1:Many)
?? Practitioners (1:Many)
?? SubmittedClaims (1:Many)
?? EligibilityRequests (1:Many)

PaymentReconciliation
?? PaymentReconciliationDetails (1:Many)
```

---

## ?? TEST DATABASE INTEGRATION

### Test Query 1: Insert Patient

```csharp
using (var context = new ApplicationDbContext(options))
{
    var patient = new Patient
    {
        Id = Guid.NewGuid().ToString(),
        MRN = "TEST123456",
        FirstName = "John",
  LastName = "Doe",
   Email = "john@example.com",
        Phone = "1234567890",
        Status = "active",
        CreatedAt = DateTime.UtcNow,
        IsActive = true
 };
    
    context.Patients.Add(patient);
    await context.SaveChangesAsync();
}
```

### Test Query 2: Retrieve with Relations

```csharp
using (var context = new ApplicationDbContext(options))
{
    var patient = await context.Patients
        .Include(p => p.Coverages)
     .Include(p => p.Claims)
        .FirstOrDefaultAsync(p => p.MRN == "TEST123456");
}
```

---

## ?? TROUBLESHOOTING

### Issue: "Cannot connect to server 'localhost'"

**Solution:**
```sql
-- Verify SQL Server is running
-- Check connection string in appsettings.json
-- Ensure SA account is enabled
-- Test in SSMS first
```

### Issue: "Login failed for user 'sa'"

**Solution:**
```sql
-- Verify SA password
-- Enable SQL Server authentication
-- Check if SA account is locked
-- Reset SA password if needed
```

### Issue: "Database 'NPhiesDb' already exists"

**Solution:**
```powershell
# Drop and recreate
Drop-Database -Context ApplicationDbContext
Update-Database -Context ApplicationDbContext
```

---

## ?? SUCCESS INDICATORS

When database integration is complete, you should see:

? NPhiesDb database exists in SQL Server
? 30+ tables created with proper schema
? Foreign keys and indexes configured
? Relationships properly established
? Test data seeded automatically
? Services querying database instead of using mock data
? All CRUD operations working
? Transactions implemented for complex operations

---

**Next Document**: Service Implementation Guide (Remove Mock Data)

