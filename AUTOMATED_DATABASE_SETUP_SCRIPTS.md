# ?? AUTOMATED DATABASE SETUP SCRIPTS

**Purpose**: Automate database creation and configuration
**For Teams**: Share these scripts across development team
**.NET Version**: 9.0

---

## ?? SCRIPT 1: PowerShell Database Setup

**File**: `setup-database.ps1`

Create this file in the root of your solution and run it:

```powershell
# NPhies Database Setup Script
# Run: .\setup-database.ps1

param(
    [string]$SqlServer = "localhost",
    [string]$DatabaseName = "NPhiesDb",
    [string]$SaPassword = ""
)

Write-Host "========================================" -ForegroundColor Green
Write-Host "NPhies Database Setup" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green

# Validate parameters
if ([string]::IsNullOrEmpty($SaPassword)) {
    Write-Host "Error: SA password required" -ForegroundColor Red
    Write-Host "Usage: .\setup-database.ps1 -SaPassword 'YourPassword'" -ForegroundColor Yellow
 exit 1
}

try {
    Write-Host ""
Write-Host "1. Testing SQL Server connection..." -ForegroundColor Cyan
    
    $connectionString = "Server=$SqlServer;User Id=sa;Password=$SaPassword;Connection Timeout=5;"
    $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $connection.Open()
    $connection.Close()
    
    Write-Host "   ? Connected to $SqlServer" -ForegroundColor Green
    
    Write-Host ""
    Write-Host "2. Creating migration..." -ForegroundColor Cyan
 
    $env:ASPNETCORE_ENVIRONMENT = "Development"
    
    # From NPhies_FHIR_Integration.Infrastructure directory
    Push-Location "NPhies_FHIR_Integration.Infrastructure"
    
    dotnet ef migrations add InitialCreate `
      --context ApplicationDbContext `
        --output-dir Migrations `
   --project .
  
    Pop-Location
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ? Migration created" -ForegroundColor Green
    } else {
    Write-Host "   ? Migration creation failed" -ForegroundColor Red
        exit 1
    }
    
    Write-Host ""
  Write-Host "3. Updating database..." -ForegroundColor Cyan
    
    Push-Location "NPhies_FHIR_Integration.ApiService"
    
    dotnet ef database update `
        --context ApplicationDbContext `
    --project ../NPhies_FHIR_Integration.Infrastructure
    
    Pop-Location
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ? Database updated" -ForegroundColor Green
    } else {
        Write-Host "   ? Database update failed" -ForegroundColor Red
        exit 1
    }
    
    Write-Host ""
    Write-Host "4. Verifying database..." -ForegroundColor Cyan
    
    $verifyQuery = @"
SELECT COUNT(*) as TableCount FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo'
"@

    $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $connection.ConnectionString = "Server=$SqlServer;Database=$DatabaseName;User Id=sa;Password=$SaPassword;"
    $connection.Open()
    
    $command = $connection.CreateCommand()
    $command.CommandText = $verifyQuery
    $tableCount = $command.ExecuteScalar()
    
    $connection.Close()
    
    Write-Host "   ? Database verified - $tableCount tables created" -ForegroundColor Green
    
    Write-Host ""
  Write-Host "========================================" -ForegroundColor Green
    Write-Host "? Setup Complete!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "Database: $DatabaseName" -ForegroundColor Cyan
    Write-Host "Server: $SqlServer" -ForegroundColor Cyan
    Write-Host "Tables: $tableCount" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Next Steps:" -ForegroundColor Yellow
    Write-Host "1. Run the application: dotnet run" -ForegroundColor White
    Write-Host "2. Seeding will run automatically in Development mode" -ForegroundColor White
    Write-Host "3. Database is ready for queries" -ForegroundColor White
    Write-Host ""
}
catch {
    Write-Host ""
 Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Troubleshooting:" -ForegroundColor Yellow
    Write-Host "1. Verify SQL Server is running" -ForegroundColor White
    Write-Host "2. Check SA password is correct" -ForegroundColor White
    Write-Host "3. Ensure SQL Server is listening on localhost:1433" -ForegroundColor White
    exit 1
}
```

**Usage**:
```powershell
# Navigate to solution root
cd C:\path\to\NPhies_FHIR_Integration

# Run setup (replace with your SA password)
.\setup-database.ps1 -SaPassword "YourActualPassword"
```

---

## ?? SCRIPT 2: Batch File (Windows)

**File**: `setup-database.bat`

For Windows command prompt:

```batch
@echo off
REM NPhies Database Setup Script
REM Run: setup-database.bat

setlocal enabledelayedexpansion

echo.
echo ========================================
echo NPhies Database Setup
echo ========================================
echo.

REM Check if NPhies_FHIR_Integration.Infrastructure exists
if not exist "NPhies_FHIR_Integration.Infrastructure" (
    echo Error: Must run from solution root directory
    exit /b 1
)

echo 1. Setting environment variables...
set ASPNETCORE_ENVIRONMENT=Development

echo 2. Creating migration...
cd NPhies_FHIR_Integration.Infrastructure
dotnet ef migrations add InitialCreate --context ApplicationDbContext --output-dir Migrations

if !errorlevel! neq 0 (
    echo Error creating migration
    cd ..
    exit /b 1
)

cd ..

echo 3. Updating database...
cd NPhies_FHIR_Integration.ApiService

dotnet ef database update --context ApplicationDbContext --project ../NPhies_FHIR_Integration.Infrastructure

if !errorlevel! neq 0 (
    echo Error updating database
    cd ..
    exit /b 1
)

cd ..

echo.
echo ========================================
echo ^? Setup Complete!
echo ========================================
echo.
echo Database created successfully
echo Ready for application startup
echo.

pause
```

**Usage**:
```batch
cd C:\path\to\NPhies_FHIR_Integration
setup-database.bat
```

---

## ?? SCRIPT 3: Docker Setup (Optional)

**File**: `Dockerfile` (for containerized SQL Server)

```dockerfile
# SQL Server with NPhies Database
# Build: docker build -t nphies-sqlserver .
# Run: docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123" -p 1433:1433 -d nphies-sqlserver

FROM mcr.microsoft.com/mssql/server:2022-latest

ENV SA_PASSWORD=YourPassword123
ENV ACCEPT_EULA=Y

WORKDIR /scripts

COPY ./setup.sql /scripts/

CMD /opt/mssql/bin/sqlservr & \
    sleep 10 && \
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -i /scripts/setup.sql
```

**File**: `docker-compose.yml`

```yaml
version: '3.8'

services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
SA_PASSWORD: "YourPassword123"
      ACCEPT_EULA: "Y"
    ports:
      - "1433:1433"
    volumes:
   - sqlserver_data:/var/opt/mssql

  api:
    build:
      context: .
 dockerfile: NPhies_FHIR_Integration.ApiService/Dockerfile
    environment:
      ConnectionStrings__DefaultConnection: "Server=sqlserver;Database=NPhiesDb;User Id=sa;Password=YourPassword123;"
      ASPNETCORE_ENVIRONMENT: "Development"
    ports:
      - "5000:5000"
      - "5001:5001"
    depends_on:
      - sqlserver

volumes:
  sqlserver_data:
```

**Usage**:
```bash
docker-compose up -d
# Waits for SQL Server to start, creates database, runs API
```

---

## ?? SCRIPT 4: Database Seeding Script

**File**: `NPhies_FHIR_Integration.Infrastructure/SeedData.sql`

Manual seeding if automatic seeding doesn't work:

```sql
USE NPhiesDb
GO

-- Create test organizations
INSERT INTO dbo.Organizations (
    Id, OrganizationName, LicenseNumber, LicenseSystem, 
    OrganizationType, SpecializationType, Email, PhoneNumber, 
    AddressLine1, City, State, PostalCode, Status, 
    CreatedAt, UpdatedAt, IsActive
) VALUES
(
    'ORG-001', 'Test Healthcare Provider LLC', 'LIC-001', 'https://example.com/license',
    'provider', 'hospital', 'info@provider.com', '555-0001',
    '123 Health St', 'Medical City', 'State', '12345', 'active',
    GETUTCDATE(), GETUTCDATE(), 1
),
(
    'ORG-002', 'Test Insurance Company', 'INS-001', 'https://example.com/license',
    'insurer', 'health insurance', 'claims@insurer.com', '555-0002',
    '456 Insurance Ave', 'Insurance City', 'State', '54321', 'active',
    GETUTCDATE(), GETUTCDATE(), 1
)
GO

-- Create test patient
INSERT INTO dbo.Patients (
    Id, MRN, FirstName, LastName, IdentifierSystem,
    Email, Phone, Gender, AddressLine1, City, State, PostalCode,
    Status, CreatedAt, UpdatedAt, IsActive
) VALUES
(
    'PAT-001', 'MRN123456', 'John', 'Doe', 'https://example.com/mrn',
    'john.doe@example.com', '555-1234', 'M', '789 Patient St',
    'Patient City', 'State', '67890', 'active',
    GETUTCDATE(), GETUTCDATE(), 1
)
GO

-- Create test coverage
INSERT INTO dbo.Coverages (
    Id, PolicyNumber, MemberID, CoverageType, Status,
    AnnualDeductible, DeductibleMet, Copay, CoinsurancePercent,
    OutOfPocketMax, PatientId, InsurerId,
    CreatedAt, UpdatedAt, IsActive
) VALUES
(
    'COV-001', 'POL-123456', 'MEM-001', 'health', 'active',
    1000.00, 0.00, 25.00, 20.00, 5000.00,
    'PAT-001', 'ORG-002',
    GETUTCDATE(), GETUTCDATE(), 1
)
GO

PRINT 'Seeding complete!'
```

---

## ? VERIFICATION SCRIPT

**File**: `verify-database.sql`

Check if database is properly set up:

```sql
USE NPhiesDb
GO

PRINT 'NPhies Database Verification'
PRINT '=================================='
PRINT ''

-- Table count
DECLARE @TableCount INT
SELECT @TableCount = COUNT(*) FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo'
PRINT 'Total Tables: ' + CAST(@TableCount AS VARCHAR(3))

-- Key table verification
PRINT ''
PRINT 'Key Tables:'
IF OBJECT_ID('dbo.Patients', 'U') IS NOT NULL PRINT '  ? dbo.Patients'
IF OBJECT_ID('dbo.Claims', 'U') IS NOT NULL PRINT '? dbo.Claims'
IF OBJECT_ID('dbo.ClaimResponses', 'U') IS NOT NULL PRINT '  ? dbo.ClaimResponses'
IF OBJECT_ID('dbo.Coverages', 'U') IS NOT NULL PRINT '  ? dbo.Coverages'
IF OBJECT_ID('dbo.Organizations', 'U') IS NOT NULL PRINT '  ? dbo.Organizations'

-- Row counts
PRINT ''
PRINT 'Data Counts:'
DECLARE @PatientCount INT = (SELECT COUNT(*) FROM dbo.Patients)
DECLARE @OrgCount INT = (SELECT COUNT(*) FROM dbo.Organizations)
DECLARE @ClaimCount INT = (SELECT COUNT(*) FROM dbo.Claims)

PRINT '  Patients: ' + CAST(@PatientCount AS VARCHAR(10))
PRINT '  Organizations: ' + CAST(@OrgCount AS VARCHAR(10))
PRINT '  Claims: ' + CAST(@ClaimCount AS VARCHAR(10))

PRINT ''
PRINT 'Verification Complete!'
```

---

## ?? USAGE SUMMARY

### For Individual Developer:
```powershell
.\setup-database.ps1 -SaPassword "your-password"
```

### For Team (CI/CD Pipeline):
```bash
dotnet ef database update --context ApplicationDbContext
```

### For Docker Environment:
```bash
docker-compose up -d
```

### For Manual Setup:
1. Execute `QUICKSTART_DATABASE_30MIN.md` steps
2. Use SSMS to verify
3. Run `verify-database.sql` to check

---

## ?? CHECKLIST AFTER SETUP

- [ ] SQL Server running on localhost
- [ ] Database `NPhiesDb` exists
- [ ] 30+ tables created
- [ ] Primary keys on all tables
- [ ] Foreign keys configured
- [ ] Indexes created
- [ ] Test data seeded (Organizations)
- [ ] Can query data successfully
- [ ] Application starts without errors
- [ ] Ready for service integration

---

**Next Step**: `SERVICE_IMPLEMENTATION_GUIDE_DATABASE.md` - Update services to use database

