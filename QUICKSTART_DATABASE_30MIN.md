# ? QUICK START - DATABASE SETUP IN 30 MINUTES

**Goal**: Get your database up and running locally
**Time**: 30 minutes
**Prerequisites**: MS SQL Server running on localhost with SA account

---

## ?? YOUR ACTION ITEMS (Copy-Paste Commands)

### STEP 1: Update appsettings.json (2 minutes)

**File**: `NPhies_FHIR_Integration.ApiService/appsettings.json`

Replace the ConnectionStrings section with:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=NPhiesDb;User Id=sa;Password=YourActualSAPassword;Encrypt=true;TrustServerCertificate=true;Connection Timeout=30;MultipleActiveResultSets=true;"
}
```

**?? IMPORTANT**: Replace `YourActualSAPassword` with your actual SQL Server SA password!

---

### STEP 2: Open Package Manager Console (1 minute)

In Visual Studio:
1. **Tools** ? **NuGet Package Manager** ? **Package Manager Console**
2. Set "Default project" to: **NPhies_FHIR_Integration.Infrastructure**

---

### STEP 3: Create Initial Migration (3 minutes)

**Copy and paste this command into Package Manager Console:**

```powershell
Add-Migration InitialCreate -Context ApplicationDbContext -OutputDir Migrations -Verbose
```

**What you should see:**
```
Build started...
Build succeeded.
To undo this action, use Remove-Migration.
```

---

### STEP 4: Create Database (5 minutes)

**Copy and paste this command into Package Manager Console:**

```powershell
Update-Database -Context ApplicationDbContext -Verbose
```

**What you should see:**
```
Build started...
Build succeeded.
Applying migration '20240101000000_InitialCreate'.
Done.
```

---

### STEP 5: Verify Database (10 minutes)

#### 5A: Open SQL Server Management Studio

1. Launch **SQL Server Management Studio (SSMS)**
2. Connect to: `localhost`
3. Authentication: `SQL Server Authentication`
4. Username: `sa`
5. Password: Your SA password
6. Click **Connect**

#### 5B: Verify Database Created

In SSMS Object Explorer:
```
?? Databases
  ?? NPhiesDb ? (should appear here)
     ?? Tables (30+ tables)
     ?? Indexes
     ?? Keys
     ?? Stored Procedures
```

#### 5C: Run Verification Script

Open a **New Query** in SSMS and run:

```sql
USE NPhiesDb
GO

-- Count tables
SELECT COUNT(*) as 'TableCount' FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo'
GO

-- List all tables
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo' 
ORDER BY TABLE_NAME
GO

-- Verify sample tables
SELECT COUNT(*) as PatientCount FROM dbo.Patients
SELECT COUNT(*) as ClaimCount FROM dbo.Claims
SELECT COUNT(*) as OrganizationCount FROM dbo.Organizations
GO
```

**Expected Results:**
- TableCount: 30+
- PatientCount: 0-10 (depending on seeding)
- ClaimCount: 0
- OrganizationCount: 1+ (test data seeded)

---

## ? VERIFICATION CHECKLIST

After completing the above steps:

- [ ] appsettings.json updated with correct password
- [ ] Migration created successfully
- [ ] Database created successfully
- [ ] SSMS can connect to localhost
- [ ] NPhiesDb database exists
- [ ] Tables created (30+)
- [ ] Can query tables
- [ ] Test data seeded (Organizations created)

---

## ?? NEXT: Run Your Application

Once database is verified:

### Option A: Run with Visual Studio

1. Press **F5** (Debug) or **Ctrl+F5** (Run without Debug)
2. Application starts on https://localhost:5001
3. Database seeder runs automatically
4. Test data created

### Option B: Run with .NET CLI

```bash
cd NPhies_FHIR_Integration.ApiService
dotnet run
```

### Option C: Run in Development Mode

```bash
dotnet run --environment Development
```

---

## ?? TROUBLESHOOTING (If Something Goes Wrong)

### Problem: "Cannot connect to server 'localhost'"

**Solutions:**
```powershell
# 1. Verify SQL Server is running (Windows Services)
services.msc
# Look for: SQL Server (SQLEXPRESS) or similar - should show "Running"

# 2. Test connection in SSMS
# Open SSMS and manually test connection

# 3. Check firewall
# Ensure SQL Server port 1433 is open
```

### Problem: "Login failed for user 'sa'"

**Solutions:**
```sql
# 1. In SSMS, verify SA account is enabled:
# Right-click Server ? Properties ? Security
# Ensure "SQL Server and Windows Authentication mode" is selected

# 2. Reset SA password in SSMS:
# Right-click SA login ? Properties ? Change password

# 3. Restart SQL Server service
```

### Problem: "Database 'NPhiesDb' already exists"

**Solutions:**
```powershell
# Option 1: Delete and recreate
Drop-Database -Context ApplicationDbContext -Confirm:$false
Update-Database -Context ApplicationDbContext

# Option 2: Use existing database
# (The migration will succeed even if database exists)
```

### Problem: "The type 'ApplicationDbContext' could not be found"

**Solutions:**
```powershell
# Make sure Default Project is set to: NPhies_FHIR_Integration.Infrastructure
# You should see dropdown at top of Package Manager Console

# If still failing:
# Right-click Infrastructure project ? Set as Startup Project
# Then try again
```

---

## ?? Expected Database Schema (After Migration)

```
TABLES CREATED:
? dbo.Patients (PII data)
? dbo.Coverages (Insurance info)
? dbo.Organizations (Providers/Insurers)
? dbo.Locations (Service facilities)
? dbo.Practitioners (Doctors/Staff)
? dbo.MessageHeaders (FHIR messages)
? dbo.Claims (Primary claim data)
? dbo.ClaimItems (Services/procedures)
? dbo.ClaimDiagnoses (Medical conditions)
? dbo.ClaimResponses (Payer responses)
? dbo.ClaimResponseAddItems (Advanced auth items)
? dbo.ClaimResponseAdjudications (Payment adjudications)
? dbo.PaymentReconciliations (Payment matching)
? dbo.PaymentReconciliationDetails (Payment line items)
+ 16 more tables for full FHIR support

INDEXES:
? Primary keys on all tables
? Unique constraints (MRN, ClaimNumber, etc.)
? Foreign keys (referential integrity)
? Performance indexes on commonly filtered columns
```

---

## ?? CREATE ADDITIONAL MIGRATIONS (Later)

When you modify entities and want to update schema:

```powershell
# Create new migration with descriptive name
Add-Migration AddNewFeatureName -Context ApplicationDbContext -OutputDir Migrations

# Apply the migration
Update-Database -Context ApplicationDbContext
```

---

## ?? SUCCESS! WHAT'S NEXT?

Once database is running:

### ? COMPLETED
- [x] Database created locally
- [x] Schema migrated
- [x] Tables created
- [x] Ready for data persistence

### ?? NEXT PHASE: Service Integration (Week 1, Days 2-5)

1. Update ClaimResponseProcessingService to query database
2. Update DenialManagementService to use real data
3. Update AdjudicationWorkflowService
4. Implement AppealWorkflowService
5. Create integration tests

### ?? SEE ALSO
- `DATABASE_INTEGRATION_GUIDE_MSSQL.md` - Complete reference
- `SERVICE_IMPLEMENTATION_GUIDE_DATABASE.md` - Service updates
- `TESTING_IMPLEMENTATION_GUIDE.md` - Test setup

---

## ?? NEED HELP?

### Common Issues & Fixes

| Issue | Solution |
|-------|----------|
| Can't find Package Manager Console | Tools ? NuGet Package Manager ? Package Manager Console |
| Default Project not set correctly | Dropdown in PMC shows "NPhies_FHIR_Integration.Infrastructure" |
| Migration fails to build | Clean solution: Build ? Clean Solution, then try again |
| Database connection timeout | Increase timeout in connection string (already set to 30 seconds) |
| SA account locked | Use SQL Server Configuration Manager to unlock |

---

**TIME ESTIMATE**: 30 minutes total
**NEXT STEP**: Service Implementation (Week 1 Days 2-5, ~40 hours)

