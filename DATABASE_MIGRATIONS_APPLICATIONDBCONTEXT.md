# ?? **DATABASE MIGRATIONS - APPLICATION DBCONTEXT**

**Status**: ? **Ready to Apply**  
**DbContext**: `ApplicationDbContext`  
**Migrations**: Pre-existing and ready  

---

## ?? **QUICK COMMANDS**

### **Apply Migrations**

```bash
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration

# Option 1: Using Package Manager Console (Visual Studio)
Update-Database -Context ApplicationDbContext -Project NPhies_FHIR_Integration.Infrastructure -StartupProject NPhies_FHIR_Integration.ApiService

# Option 2: Using dotnet CLI
dotnet ef database update --context ApplicationDbContext --project NPhies_FHIR_Integration.Infrastructure --startup-project NPhies_FHIR_Integration.ApiService
```

### **Verify Migrations**

```bash
# List all migrations
dotnet ef migrations list --context ApplicationDbContext

# Expected Output:
# 20260715154547_InitialCreate (Applied)
```

---

## ?? **WHAT'S INCLUDED**

### **Initial Migration: `InitialCreate`**

The migration includes creation of all tables for:

? **Authentication & Authorization**
- `Users` - User accounts
- `Roles` - System roles
- `Permissions` - Granular permissions
- `UserRoles` - User-role mappings
- `RolePermissions` - Role-permission mappings

? **RBAC & Supervision**
- `SupervisorAssignments` - Manager-subordinate relationships
- `PermissionAuditLogs` - Audit trail

? **Claims Processing**
- `Claims` - Master claim records
- `ClaimItems` - Individual items in claims
- `ClaimDiagnosis` - Diagnosis codes
- `ClaimResponses` - Claim responses from payer
- `ClaimAssignments` - Assignment to reviewers
- `QAReviews` - Quality assurance reviews

? **Eligibility & Coverage**
- `CoverageEligibilityRequests` - Eligibility queries
- `CoverageEligibilityResponses` - Eligibility results
- `Coverage` - Coverage details
- `BenefitBalance` - Benefit information

? **Master Data**
- `Patients` - Patient records
- `Organizations` - Healthcare organizations
- `ErrorCodes` - NPHIES error codes
- And many more...

---

## ?? **DATABASE CONFIGURATION**

### **Connection String** (appsettings.json or appsettings.Development.json)

```json
{
  "ConnectionStrings": {
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NPhiesDb;Trusted_Connection=true;"
  }
}
```

### **Alternative Connection Strings**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=NPhiesDb;Integrated Security=true;Encrypt=true;TrustServerCertificate=true;"
  }
}
```

**For SQL Server (not LocalDB)**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server.database.windows.net,1433;Initial Catalog=NPhiesDb;Persist Security Info=False;User ID=your-user;Password=your-password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}
```

---

## ? **MIGRATION STATUS**

### **Existing Migrations**

| Migration | File | Status |
|-----------|------|--------|
| **InitialCreate** | `20260715154547_InitialCreate.cs` | ? Ready |
| **Designer Snapshot** | `20260715154547_InitialCreate.Designer.cs` | ? Ready |
| **Model Snapshot** | `ApplicationDbContextModelSnapshot.cs` | ? Ready |

All migrations are **pre-created** and ready to apply!

---

## ?? **STEP-BY-STEP GUIDE**

### **Step 1: Verify Database Connection**

```bash
# Test connection string
dotnet ef dbcontext info --context ApplicationDbContext
```

**Expected Output**:
```
Provider name: Microsoft.EntityFrameworkCore.SqlServer
Database name: NPhiesDb
DataSource: (localdb)\mssqllocaldb
```

### **Step 2: Apply Migrations**

```bash
# Apply all pending migrations
dotnet ef database update --context ApplicationDbContext
```

**Expected Output**:
```
Applying migration '20260715154547_InitialCreate'
Done.
```

### **Step 3: Verify Database Created**

```sql
-- In SQL Server Management Studio or Azure Data Studio:
-- Check if database exists
SELECT name FROM sys.databases WHERE name = 'NPhiesDb';

-- Check tables
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo';
```

### **Step 4: Check Data**

```bash
# Run application to seed test data
dotnet run --project NPhies_FHIR_Integration.ApiService
```

---

## ?? **TROUBLESHOOTING**

### **Issue: "More than one DbContext was found"**

**Solution**: Specify the correct context:
```bash
dotnet ef database update --context ApplicationDbContext
```

### **Issue: "Database already exists"**

This is fine! The migration will:
1. Check current state
2. Apply only new migrations
3. Skip already-applied migrations

### **Issue: "Connection refused"**

Check:
1. LocalDB is running: `sqllocaldb info mssqllocaldb`
2. Connection string is correct
3. Firewall allows connections

### **Issue: "Login failed for user"**

If using SQL Server (not LocalDB):
1. Verify username/password
2. Check SQL Server authentication enabled
3. Verify user has database create permissions

### **Issue: "The model backing the context has changed"**

Solution: Create a new migration for any pending changes:
```bash
dotnet ef migrations add "UpdateModelChanges" --context ApplicationDbContext
dotnet ef database update --context ApplicationDbContext
```

---

## ?? **WHAT TO DO NEXT**

### **1. Apply Migrations** ?
```bash
dotnet ef database update --context ApplicationDbContext
```

### **2. Start Backend**
```bash
dotnet run --project NPhies_FHIR_Integration.ApiService
```

### **3. Test Login**
```
URL: http://localhost:4200/auth/login
Username: test.reviewer
Password: TestPassword123!
```

### **4. Verify Data**

Check that test users are created in the database:

```sql
SELECT * FROM Users;
```

---

## ?? **MIGRATION FILES**

### **Location**
```
NPhies_FHIR_Integration.Infrastructure/Migrations/
??? 20260715154547_InitialCreate.cs          (Migration up/down)
??? 20260715154547_InitialCreate.Designer.cs (Designer metadata)
??? ApplicationDbContextModelSnapshot.cs     (Current model state)
```

### **What Each File Does**

1. **InitialCreate.cs** - Contains Up() and Down() methods
   - Up() - Creates all tables during migration
   - Down() - Removes all tables during rollback

2. **InitialCreate.Designer.cs** - Metadata for Visual Studio
   - Used by EF Core tools
   - Auto-generated, don't edit

3. **ApplicationDbContextModelSnapshot.cs** - Current model state
   - Used to detect changes
   - Auto-generated after each migration

---

## ?? **FULL INTEGRATION SETUP**

```bash
# 1. Navigate to solution directory
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration

# 2. Restore NuGet packages (already done)
dotnet restore

# 3. Apply migrations
dotnet ef database update --context ApplicationDbContext

# 4. Build solution
dotnet build

# 5. Start backend
dotnet run --project NPhies_FHIR_Integration.ApiService

# 6. (In another terminal) Start frontend
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd
ng serve

# 7. Test in browser
# http://localhost:4200/auth/login
```

---

## ?? **DATABASE SCHEMA OVERVIEW**

```
???????????????????????????????????????????????????????????
?         NPhiesDb Database  ?
???????????????????????????????????????????????????????????
? Users, Roles, Permissions, UserRoles, RolePermissions  ?
? SupervisorAssignments, PermissionAuditLogs             ?
? Claims, ClaimItems, ClaimDiagnosis, ClaimResponses     ?
? ClaimAssignments, QAReviews       ?
? CoverageEligibilityRequests, CoverageEligibilityResp.  ?
? Coverage, BenefitBalance, Patients, Organizations      ?
? ErrorCodes, ServiceCodeMasters, And Many More...   ?
???????????????????????????????????????????????????????????
```

---

## ? **POST-MIGRATION**

After applying migrations:

1. ? **Database created** with all tables
2. ? **Relationships configured** with FK constraints
3. ? **Indexes created** for performance
4. ? **Seeding ready** (happens on app startup in Development)

---

## ?? **RECOMMENDED APPROACH**

Since migrations are pre-created and ready:

```bash
# Just run this one command:
dotnet ef database update --context ApplicationDbContext

# That's it! Database will be created with all tables, relationships, and indexes.
```

---

# **READY TO APPLY MIGRATIONS!** ?

Execute the migration command above and your database will be ready!

```bash
dotnet ef database update --context ApplicationDbContext
```

**Next**: Start the backend and test login!
