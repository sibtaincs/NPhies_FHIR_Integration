# ?? **READY TO RUN: ACTION GUIDE**

## ? **Current Status**

| Item | Status |
|------|--------|
| **Connection String** | ? Fixed (Windows Auth) |
| **Build** | ? Successful (0 errors) |
| **DLLs Locked Issue** | ? Resolved (processes stopped & cleaned) |
| **Ready to Run** | ? YES |

---

## **?? IMMEDIATE ACTION: Run Application**

### **Step 1: Open PowerShell**
```powershell
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration
```

### **Step 2: Run the Application**
```powershell
dotnet run --project NPhies_FHIR_Integration.ApiService
```

### **Step 3: Wait for Console Output**

You should see:
```
Build succeeded.

info: NPhies_FHIR_Integration.Infrastructure.Seeding.DatabaseSeeder[0]
      Seeding database with test data...

info: Microsoft.AspNetCore.Hosting.Hosting[14]
   Kestrel server listening on https://localhost:7xxx
```

**?? This takes about 30-60 seconds**

### **Step 4: Open Swagger UI in Browser**

Once the application is running, open:
```
https://localhost:7xxx/swagger
```

You should see the Swagger UI with all API endpoints listed.

---

## **?? First Test: Get All Patients**

### **In Swagger UI:**

1. Find **GET /api/patients**
2. Click **Try it out** button
3. Click **Execute** button
4. Scroll down to see **Response**

### **Expected Response:**
```json
{
  "success": true,
  "message": "Patients retrieved successfully",
  "data": {
"items": [
      {
      "id": "uuid-value",
        "mrn": "MRN-2024-001",
        "firstName": "Mohammed",
        "lastName": "Al-Mutairi",
 "email": "mohammed@example.com",
  "status": "active",
        "isActive": true
    },
      // ... more patients (3 total seeded)
    ],
    "pageNumber": 1,
    "pageSize": 10,
 "totalCount": 3,
    "totalPages": 1
  },
  "statusCode": 200,
  "timestamp": "2024-06-22T..."
}
```

**? If you see this ? Everything is working!**

---

## **?? Quick Test Sequence**

### **Test 1: Get All Patients (READ)**
```
GET /api/patients
Expected: 200 OK with 3 patients
```

### **Test 2: Get Specific Patient (READ)**
```
GET /api/patients/{id}
(Use an ID from Test 1)
Expected: 200 OK with patient details
```

### **Test 3: Create New Patient (CREATE)**
```
POST /api/patients
Body:
{
  "mrn": "MRN-2024-999",
  "nationalId": "9999999999",
  "firstName": "Test",
  "lastName": "User",
  "dateOfBirth": "1990-01-01",
  "gender": "M",
  "email": "test@example.com",
  "phone": "+966501234567",
  "addressLine1": "123 Test Street",
  "city": "Riyadh",
  "state": "Riyadh",
  "postalCode": "11111",
  "country": "SA"
}
Expected: 201 Created with new patient ID
```

### **Test 4: Get Coverage (READ)**
```
GET /api/coverage
Expected: 200 OK with coverage data
```

### **Test 5: Delete Patient (DELETE)**
```
DELETE /api/patients/{id}
(Use ID from Test 3)
Expected: 200 OK
```

---

## **?? Endpoints Summary**

### **Patients (7 endpoints)**
- ? `GET /api/patients` - List all
- ? `GET /api/patients/{id}` - Get by ID
- ? `GET /api/patients/mrn/{mrn}` - Get by MRN
- ? `GET /api/patients/search` - Search by name
- ? `POST /api/patients` - Create new
- ? `PUT /api/patients/{id}` - Update
- ? `DELETE /api/patients/{id}` - Delete

### **Coverage (8 endpoints)**
- ? `GET /api/coverage` - List all
- ? `GET /api/coverage/{id}` - Get by ID
- ? `GET /api/coverage/policy/{p}` - Get by policy
- ? `GET /api/coverage/patient/{p}` - Get by patient
- ? `GET /api/coverage/expiring` - Expiring soon
- ? `POST /api/coverage` - Create new
- ? `PUT /api/coverage/{id}` - Update
- ? `DELETE /api/coverage/{id}` - Delete

---

## **?? If Something Goes Wrong**

### **Application won't start?**
1. Check SQL Server is running: `Get-Service -Name MSSQLSERVER`
2. See: `DATABASE_CONNECTION_TROUBLESHOOTING.md`

### **Can't connect to database?**
1. Verify connection string has: `Integrated Security=true`
2. Check file: `appsettings.json`

### **Swagger UI won't load?**
1. Wait longer (30-60 seconds)
2. Refresh browser (Ctrl+F5)
3. Try: `https://localhost:7000/swagger`

### **DLL locked error?**
```powershell
# Stop running dotnet processes
Stop-Process -Name dotnet -Force
# Clean build
dotnet clean
# Retry
dotnet run --project NPhies_FHIR_Integration.ApiService
```

---

## **? Success Checklist**

- [ ] PowerShell open in project directory
- [ ] Application running (console shows "Listening on")
- [ ] Browser can access Swagger UI
- [ ] GET /api/patients returns 200 OK
- [ ] Response has 3 seeded patients
- [ ] Can create new patient (POST)
- [ ] Can update patient (PUT)
- [ ] Can delete patient (DELETE)

---

## **?? You're Ready!**

**Everything is configured and ready to go. Just run the application and test!**

```powershell
dotnet run --project NPhies_FHIR_Integration.ApiService
```

Then open: `https://localhost:7xxx/swagger`

**Happy testing! ??**

---

**Questions?** See these guides:
- `QUICK_RUN_APPLICATION.md` - Detailed setup
- `API_TESTING_GUIDE.md` - Complete testing examples
- `DATABASE_CONNECTION_TROUBLESHOOTING.md` - Troubleshooting
