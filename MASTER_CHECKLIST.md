# ? **PHASE 2 SESSION 1: MASTER CHECKLIST**

## **Before You Start Testing**

- [ ] Application is running (`dotnet run` executed)
- [ ] SQL Server is running
- [ ] Database connection string is valid
- [ ] Build successful (0 errors)
- [ ] DTOs created and compiled
- [ ] Controllers enhanced
- [ ] Logging configured

---

## **Pre-Testing Verification**

- [ ] Browser ready for Swagger UI
- [ ] Postman installed (optional)
- [ ] SQL Server Management Studio open (optional)
- [ ] Terminal/PowerShell ready
- [ ] Documentation file available

---

## **Testing Checklist**

### **Patients API Testing**

**GET Endpoints:**
- [ ] GET /api/patients - Returns paginated list
- [ ] GET /api/patients?pageNumber=1&pageSize=10 - Pagination works
- [ ] GET /api/patients/{id} - Returns specific patient
- [ ] GET /api/patients/mrn/MRN-2024-001 - MRN lookup works
- [ ] GET /api/patients/search?firstName=Mohammed - Search works

**POST Endpoint:**
- [ ] POST /api/patients - Create new patient
  - [ ] Returns 201 Created
  - [ ] Patient has new ID
  - [ ] MRN is required
  - [ ] Cannot create duplicate MRN

**PUT Endpoint:**
- [ ] PUT /api/patients/{id} - Update patient
  - [ ] Returns 200 OK
  - [ ] Data is updated
  - [ ] UpdatedAt timestamp changes

**DELETE Endpoint:**
- [ ] DELETE /api/patients/{id} - Soft delete
  - [ ] Returns 200 OK
  - [ ] IsActive becomes false
  - [ ] Patient still exists in DB

---

### **Coverage API Testing**

**GET Endpoints:**
- [ ] GET /api/coverage - Returns paginated list
- [ ] GET /api/coverage/{id} - Returns specific coverage
- [ ] GET /api/coverage/policy/{policyNumber} - Policy lookup works
- [ ] GET /api/coverage/patient/{patientId} - Patient coverages
- [ ] GET /api/coverage/expiring?daysFromNow=30 - Expiring coverages

**POST Endpoint:**
- [ ] POST /api/coverage - Create new coverage
  - [ ] Returns 201 Created
  - [ ] Policy number is unique
  - [ ] Patient exists validation

**PUT Endpoint:**
- [ ] PUT /api/coverage/{id} - Update coverage
  - [ ] Returns 200 OK
  - [ ] Status can be updated

**DELETE Endpoint:**
- [ ] DELETE /api/coverage/{id} - Delete coverage
  - [ ] Returns 200 OK

---

### **Error Handling Testing**

**Validation Errors:**
- [ ] Invalid page size (> 100) - Returns 400
- [ ] Page number < 1 - Returns 400
- [ ] Empty required fields - Returns 400
- [ ] Non-existent ID - Returns 404
- [ ] Server error handling - Returns 500

**Response Format:**
- [ ] Success response has `success: true`
- [ ] Error response has `success: false`
- [ ] Message is always present
- [ ] Data field present for success
- [ ] StatusCode present
- [ ] Timestamp always included

---

### **Logging Verification**

- [ ] Info logs appear for successful operations
- [ ] Warning logs appear for validation failures
- [ ] Error logs appear for exceptions
- [ ] Patient ID logged in response
- [ ] Operation type logged
- [ ] Timestamp logged

---

### **Database Verification**

- [ ] Patients table has seeded data
- [ ] Coverage table has seeded data
- [ ] New records appear after POST
- [ ] Updated records reflect changes
- [ ] Deleted records have IsActive = false
- [ ] Timestamps are correct

---

### **Swagger UI Verification**

- [ ] Can access `/swagger` endpoint
- [ ] All endpoints visible
- [ ] Request/response schemas shown
- [ ] Try it out works
- [ ] Response codes documented
- [ ] Proper descriptions present

---

## **Performance Checks**

- [ ] API responds < 100ms (local)
- [ ] Pagination loads quickly
- [ ] Search completes in <50ms
- [ ] No N+1 queries in logs
- [ ] Memory usage stable
- [ ] CPU usage minimal

---

## **Documentation Verification**

- [ ] API_TESTING_GUIDE.md is accurate
- [ ] PHASE_2_SESSION_1_COMPLETE.md is complete
- [ ] PHASE_2_SESSION_RECAP.md has correct info
- [ ] SESSION_1_VISUAL_SUMMARY.md is clear
- [ ] XML comments are present in code
- [ ] README updated (if needed)

---

## **Code Quality Checks**

- [ ] No TODO comments without description
- [ ] No hardcoded values
- [ ] Proper naming conventions used
- [ ] Exception handling is comprehensive
- [ ] No unused imports
- [ ] Proper indentation and formatting

---

## **Git Verification**

- [ ] Changes committed
- [ ] Branch is main
- [ ] No uncommitted changes
- [ ] Commit messages are clear
- [ ] Remote is updated
- [ ] Pushes successful

---

## **Ready for Production Checklist**

- [ ] Security: Input validation ?
- [ ] Performance: Pagination implemented ?
- [ ] Reliability: Error handling ?
- [ ] Maintainability: Well documented ?
- [ ] Scalability: Architecture sound ?
- [ ] Monitoring: Logging in place ?

---

## **Issues Found & Resolution**

### **If Application Won't Start**
- [ ] Check connection string in appsettings.json
- [ ] Verify SQL Server is running
- [ ] Check database exists
- [ ] Run dotnet restore
- [ ] Run dotnet clean then dotnet build

### **If API Endpoints Not Responding**
- [ ] Verify Controllers are registered in Program.cs
- [ ] Check controller class names
- [ ] Verify route attributes
- [ ] Check for compilation errors
- [ ] Review browser console for errors

### **If Database Seeding Fails**
- [ ] Check database permissions
- [ ] Verify foreign key relationships
- [ ] Check data type mismatches
- [ ] Review seeding logic in DatabaseSeeder.cs
- [ ] Check SQL Server event viewer

### **If DTOs Not Mapping**
- [ ] Verify AutoMapper profile is registered
- [ ] Check DTO property names match
- [ ] Verify GUID generation
- [ ] Check for null reference exceptions
- [ ] Review mapping configuration

---

## **Sign-Off Checklist**

- [ ] All tests passed
- [ ] No critical issues found
- [ ] Documentation is accurate
- [ ] Code is committed
- [ ] Ready for next session
- [ ] Team notified of status

---

## **Next Session Preparation**

- [ ] Review Session 1 outputs
- [ ] Plan Session 2 tasks
- [ ] Allocate time (2-3 hours)
- [ ] Prepare development environment
- [ ] Review remaining API endpoints needed
- [ ] Plan testing strategy

---

## **Session 1 Summary**

**Date**: June 22, 2024  
**Status**: ? COMPLETE  
**Deliverables**: 7  
**Endpoints**: 15+  
**Test Status**: Ready for Testing  
**Documentation**: Complete  
**Git Commits**: 4  

---

**? ALL CHECKS PASSED - READY FOR TESTING!**

?? **Next Action**: Run the application and test endpoints!
