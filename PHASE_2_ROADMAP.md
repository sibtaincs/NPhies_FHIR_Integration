# ?? Phase 2: Service Integration & API Development Roadmap

## ?? Current Status (Week 1, Day 1)

| Component | Status | Details |
|-----------|--------|---------|
| **Database** | ? Complete | NPhiesDb created with 20+ tables |
| **Initial Migration** | ? Applied | 20260622154910_InitialCreate |
| **Database Seeder** | ? Configured | Auto-seeds on app startup |
| **Generic Repository** | ? Implemented | Full CRUD with includes & pagination |
| **Specialized Repositories** | ? Ready | Patient, Coverage, Organization, etc. |
| **DI Container** | ? Configured | All services registered |
| **Build** | ? Successful | 0 errors |

---

## ?? Phase 2 Objectives (Next 2-3 Weeks)

### Week 1: Data Persistence & API Foundation
- ? Run application & verify seeding
- ? Test repositories with seeded data
- ? Implement API endpoints (Patients, Coverage, Organizations)
- ? Add DTOs (Data Transfer Objects)
- ? Configure AutoMapper for entity-to-DTO conversion

### Week 2: Service Layer & Business Logic
- ? Enhance EligibilityService
- ? Implement ClaimService
- ? Add PaymentService
- ? Implement RCM Services
- ? Add error handling & logging

### Week 3: API Testing & Documentation
- ? Create API endpoints for all resources
- ? Add Swagger/OpenAPI documentation
- ? Write integration tests
- ? Performance optimization

---

## ?? Detailed Task Breakdown

### Phase 2.1: Verify Database & Seeding (TODAY)

#### Command
```powershell
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration
dotnet run --project NPhies_FHIR_Integration.ApiService
```

#### Expected Results
- Application starts successfully
- DatabaseSeeder runs automatically
- Test data inserted into database
- Swagger UI available at https://localhost:port/swagger
- All repositories accessible

#### SQL Verification Queries
```sql
-- Verify test data
SELECT COUNT(*) as PatientCount FROM Patients WHERE IsActive = 1;
SELECT COUNT(*) as CoverageCount FROM Coverages WHERE Status = 'active';
SELECT COUNT(*) as OrganizationCount FROM Organizations WHERE Status = 'active';
SELECT COUNT(*) as EligibilityRequestsCount FROM CoverageEligibilityRequests;
SELECT COUNT(*) as BenefitBalancesCount FROM BenefitBalances;

-- Sample data
SELECT TOP 3 Id, FirstName, LastName, MRN FROM Patients;
SELECT TOP 3 Id, OrganizationName, LicenseNumber FROM Organizations;
SELECT TOP 3 Id, PolicyNumber, MemberID FROM Coverages;
```

---

### Phase 2.2: Implement API Endpoints

#### Endpoints to Create

**Patients Endpoints**
- `GET /api/patients` - Get all patients (paginated)
- `GET /api/patients/{id}` - Get patient details
- `GET /api/patients/search?firstName={fn}&lastName={ln}` - Search patients
- `GET /api/patients/{id}/coverage` - Get patient's coverage
- `POST /api/patients` - Create new patient
- `PUT /api/patients/{id}` - Update patient
- `DELETE /api/patients/{id}` - Soft delete patient

**Coverage Endpoints**
- `GET /api/coverage` - Get all coverage (paginated)
- `GET /api/coverage/{id}` - Get coverage details
- `GET /api/coverage/policy/{policyNumber}` - Get by policy number
- `GET /api/coverage/patient/{patientId}` - Get patient's coverage
- `GET /api/coverage/expiring` - Get expiring coverage
- `POST /api/coverage` - Create new coverage
- `PUT /api/coverage/{id}` - Update coverage

**Organization Endpoints**
- `GET /api/organizations` - Get all organizations
- `GET /api/organizations/{id}` - Get organization details
- `GET /api/organizations/providers` - Get all providers
- `GET /api/organizations/insurers` - Get all insurers
- `GET /api/organizations/search?name={name}` - Search organizations
- `POST /api/organizations` - Create organization

**Eligibility Endpoints**
- `POST /api/eligibility/request` - Submit eligibility request
- `GET /api/eligibility/request/{id}` - Get request details
- `GET /api/eligibility/response/{id}` - Get response details
- `GET /api/eligibility/patient/{patientId}` - Get patient's eligibility history

---

### Phase 2.3: Create DTOs (Data Transfer Objects)

```csharp
// PatientDto
public class PatientDto
{
    public string Id { get; set; }
    public string MRN { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
public string Phone { get; set; }
    public string Status { get; set; }
    public bool IsActive { get; set; }
}

// CoverageDto
public class CoverageDto
{
    public string Id { get; set; }
    public string PolicyNumber { get; set; }
    public string MemberID { get; set; }
    public string Status { get; set; }
    public decimal AnnualDeductible { get; set; }
    public decimal DeductibleMet { get; set; }
    public decimal Copay { get; set; }
    public decimal CoinsurancePercent { get; set; }
}

// OrganizationDto
public class OrganizationDto
{
    public string Id { get; set; }
 public string OrganizationName { get; set; }
    public string LicenseNumber { get; set; }
    public string OrganizationType { get; set; }
    public string Email { get; set; }
 public string PhoneNumber { get; set; }
    public string Status { get; set; }
}
```

---

### Phase 2.4: Implement API Controllers

Example Patient Controller:
```csharp
[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<PatientsController> _logger;

    public PatientsController(IPatientRepository patientRepository, IMapper mapper, ILogger<PatientsController> logger)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
    try
        {
        var (patients, totalCount) = await _patientRepository.GetActivePatientsPaginatedAsync(pageNumber, pageSize);
            var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(patients);
            
            return Ok(new { items = patientDtos, totalCount, pageNumber, pageSize });
        }
        catch (Exception ex)
 {
            _logger.LogError(ex, "Error fetching patients");
            return StatusCode(500, "Internal server error");
   }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientDto>> GetById(string id)
  {
        try
        {
        var patient = await _patientRepository.GetByIdAsync(id);
     if (patient == null)
              return NotFound();

      return Ok(_mapper.Map<PatientDto>(patient));
 }
 catch (Exception ex)
     {
      _logger.LogError(ex, "Error fetching patient");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
 public async Task<ActionResult<PatientDto>> Create(CreatePatientDto createPatientDto)
    {
        try
   {
            var patient = _mapper.Map<Patient>(createPatientDto);
         await _patientRepository.AddAsync(patient);
   await _patientRepository.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, _mapper.Map<PatientDto>(patient));
    }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating patient");
            return StatusCode(500, "Internal server error");
        }
    }
}
```

---

## ?? Key Technologies & Patterns

| Technology | Purpose | Status |
|-----------|---------|--------|
| Entity Framework Core 9 | ORM | ? Configured |
| Repository Pattern | Data Access | ? Implemented |
| Dependency Injection | IoC | ? Configured |
| AutoMapper | DTO Mapping | ? Configured |
| Swagger/OpenAPI | API Documentation | ? Configured |
| Serilog | Structured Logging | ? To implement |
| xUnit | Unit Testing | ? To implement |

---

## ?? Timeline

| Date | Milestone | Status |
|------|-----------|--------|
| **Today (Day 1)** | Database created & seeding verified | ? |
| **This Week (Days 2-5)** | API endpoints implemented | ? |
| **Next Week (Days 6-10)** | Service layer enhancements | ? |
| **Week 3 (Days 11-15)** | Testing & documentation | ? |

---

## ? Immediate Next Steps

### 1. Run Application (5 minutes)
```powershell
dotnet run --project NPhies_FHIR_Integration.ApiService
```

### 2. Test Swagger UI (2 minutes)
- Navigate to: https://localhost:{port}/swagger
- Verify all endpoints are listed
- Test GET endpoints with seeded data

### 3. Verify Database (3 minutes)
- Open SQL Server Management Studio
- Connect to: localhost, sa, Mahyan@123
- Run verification queries above
- Confirm test data exists

### 4. Create DTOs Folder (2 minutes)
```
NPhies_FHIR_Integration.Application/DTOs/
??? PatientDtos.cs
??? CoverageDtos.cs
??? OrganizationDtos.cs
??? EligibilityDtos.cs
??? ClaimDtos.cs
```

### 5. Implement First Endpoint (20 minutes)
Start with PatientsController:
- GET /api/patients (paginated)
- GET /api/patients/{id}
- POST /api/patients

---

## ?? Success Criteria

? Application runs without errors  
? DatabaseSeeder executes successfully  
? Test data visible in database
? Swagger UI displays all endpoints  
? All repositories working with seeded data  
? API endpoints return proper HTTP responses  
? DTOs correctly mapped from entities  
? Error handling implemented  
? Logging enabled  
? Tests passing  

---

## ?? Support & Questions

If you encounter issues:
1. Check the Application logs in `Logs/` folder
2. Verify database connection string
3. Ensure SQL Server is running
4. Check that seeding completed successfully
5. Review existing repository implementations for patterns

---

**Next Action:** Run the application and verify seeding! ??
