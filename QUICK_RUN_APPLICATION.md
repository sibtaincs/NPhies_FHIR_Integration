# ?? **Quick Start: Run the Application Now**

## **1?? Verify Prerequisites**

### **SQL Server Status**
```powershell
Get-Service -Name MSSQLSERVER
```

**Expected**: `Status = Running` ?

---

## **2?? Run the Application**

```powershell
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration

dotnet run --project NPhies_FHIR_Integration.ApiService
```

### **Expected Console Output:**
```
Build succeeded.

info: NPhies_FHIR_Integration.Infrastructure.Seeding.DatabaseSeeder[0]
      Seeding database with test data...

info: Microsoft.AspNetCore.Hosting.Hosting[14]
 Kestrel server listening on https://localhost:7xxx
```

---

## **3?? Access Swagger UI**

Open your browser and navigate to:
```
https://localhost:7xxx/swagger
```

### **What You Should See:**
- ? Swagger UI loads
- ? List of all endpoints
- ? Patient endpoints
- ? Coverage endpoints
- ? Try it out buttons

---

## **4?? Test First Endpoint**

### **GET /api/patients** (Get all patients)

In Swagger UI:
1. Click **GET** `/api/patients`
2. Click **Try it out**
3. Click **Execute**

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
   // ... more patients
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

---

## **5?? Test Another Endpoint**

### **GET /api/patients/{id}** (Get specific patient)

In Swagger UI:
1. Copy a patient **id** from previous response
2. Click **GET** `/api/patients/{id}`
3. Click **Try it out**
4. Paste the id in the **id** field
5. Click **Execute**

### **Expected**: Single patient details

---

## **6?? Test Create Endpoint**

### **POST /api/patients** (Create new patient)

In Swagger UI:
1. Click **POST** `/api/patients`
2. Click **Try it out**
3. Enter this in the request body:

```json
{
  "mrn": "MRN-2024-099",
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
```

4. Click **Execute**

### **Expected**: Returns 201 Created with new patient

---

## **Common Endpoints to Test**

### **Patients**
```
? GET    /api/patients  (List all)
? GET    /api/patients/{id}                (Get one)
? GET    /api/patients/mrn/{mrn}     (Get by MRN)
? GET    /api/patients/search?firstName=  (Search)
? POST   /api/patients              (Create)
? PUT    /api/patients/{id}              (Update)
? DELETE /api/patients/{id}         (Delete)
```

### **Coverage**
```
? GET    /api/coverage         (List all)
? GET    /api/coverage/{id}     (Get one)
? GET    /api/coverage/policy/{num}        (Get by policy)
? GET  /api/coverage/patient/{id}        (Get by patient)
? GET    /api/coverage/expiring        (Expiring soon)
? POST   /api/coverage         (Create)
? PUT    /api/coverage/{id} (Update)
? DELETE /api/coverage/{id}    (Delete)
```

---

## **Verify Database Changes**

After creating/updating data:

### **Option 1: SQL Server Management Studio**
1. Connect to `localhost`
2. Expand **Databases** ? **NPhiesDb**
3. Expand **Tables**
4. Right-click **dbo.Patients** ? **Select Top 1000 Rows**
5. Should see your new patient

### **Option 2: Command Line Query**
```powershell
sqlcmd -S localhost -E -d NPhiesDb -Q "SELECT * FROM Patients;"
```

---

## **Stop the Application**

When done testing:
```
Press Ctrl+C in the terminal
```

---

## **Troubleshooting**

### **If Application Won't Start**

1. Check SQL Server is running
   ```powershell
   Get-Service -Name MSSQLSERVER
   ```

2. Check database connection
   - See: `DATABASE_CONNECTION_TROUBLESHOOTING.md`

3. Clear build cache
   ```powershell
   dotnet clean
   dotnet build
   ```

### **If Swagger UI Won't Load**

1. Wait for "Listening on" message
2. Refresh browser (F5)
3. Try: `https://localhost:7xxx/swagger/index.html`

### **If Endpoints Return Errors**

1. Check error message in response
2. Check application console for detailed errors
3. Verify request format matches examples
4. Check required fields are provided

---

## **Next Steps**

- ? Test all 15+ endpoints
- ? Follow `API_TESTING_GUIDE.md` for comprehensive testing
- ? Review seeded data
- ? Prepare for Session 2 development

---

## **Success Checklist**

- [ ] SQL Server running
- [ ] Application started without errors
- [ ] Swagger UI accessible
- [ ] GET /api/patients returns data
- [ ] POST /api/patients creates new record
- [ ] PUT /api/patients updates record
- [ ] DELETE /api/patients soft deletes record
- [ ] Database shows changes in SSMS

---

?? **Application is ready! Start testing now!**
