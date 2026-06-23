# ?? **Quick Start: Test the API**

## **Step 1: Run the Application**

```powershell
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration
dotnet run --project NPhies_FHIR_Integration.ApiService
```

**Expected Output:**
```
Building...
Build succeeded.

info: NPhies_FHIR_Integration.Infrastructure.Seeding.DatabaseSeeder[0]
      Seeding database with test data...

info: Microsoft.AspNetCore.Hosting.Hosting[14]
      Started Kestrel server listening on https://localhost:7xxx
```

---

## **Step 2: Access Swagger UI**

Open browser and navigate to:
```
https://localhost:7xxx/swagger/index.html
```

You should see all API endpoints listed.

---

## **Step 3: Test Endpoints**

### **A. Get All Patients (Paginated)**

**Request:**
```
GET /api/patients?pageNumber=1&pageSize=10
```

**Expected Response:**
```json
{
  "success": true,
  "message": "Patients retrieved successfully",
  "data": {
    "items": [
      {
 "id": "uuid-1",
      "mrn": "MRN-2024-001",
        "firstName": "Mohammed",
        "lastName": "Al-Mutairi",
        "email": "mohammed@example.com",
        "status": "active",
        "isActive": true,
        "createdAt": "2024-06-22T..."
      },
      ...
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

### **B. Get Patient by ID**

**Request:**
```
GET /api/patients/{id}
```

**Example with response:**
```json
{
"success": true,
  "message": "Patient retrieved successfully",
  "data": {
    "id": "uuid-1",
    "mrn": "MRN-2024-001",
    "nationalId": "1234567890",
    "firstName": "Mohammed",
    "lastName": "Al-Mutairi",
  "email": "mohammed@example.com",
    "phone": "+966541234567",
    "status": "active",
    "isActive": true,
    "createdAt": "2024-06-22T..."
  },
  "statusCode": 200
}
```

---

### **C. Search Patients by Name**

**Request:**
```
GET /api/patients/search?firstName=Mohammed&lastName=
```

**Expected Response:**
```json
{
  "success": true,
  "message": "Patients found successfully",
  "data": [
 {
      "id": "uuid-1",
      "firstName": "Mohammed",
      "lastName": "Al-Mutairi",
      ...
  }
  ],
  "statusCode": 200
}
```

---

### **D. Create New Patient**

**Request:**
```
POST /api/patients
Content-Type: application/json

{
  "mrn": "MRN-2024-004",
  "nationalId": "9876543210",
"firstName": "Ahmed",
  "lastName": "Al-Rashid",
  "dateOfBirth": "1985-05-15",
  "gender": "M",
  "email": "ahmed@example.com",
  "phone": "+966501234567",
  "addressLine1": "123 Main Street",
  "city": "Riyadh",
  "state": "Riyadh",
  "postalCode": "11111",
  "country": "SA"
}
```

**Expected Response:** (201 Created)
```json
{
  "success": true,
  "message": "Patient created successfully",
  "data": {
  "id": "new-uuid",
  "mrn": "MRN-2024-004",
    "firstName": "Ahmed",
    "lastName": "Al-Rashid",
    ...
  },
  "statusCode": 201
}
```

---

### **E. Update Patient**

**Request:**
```
PUT /api/patients/{id}
Content-Type: application/json

{
  "firstName": "Ahmed Updated",
  "email": "ahmed.updated@example.com",
  "phone": "+966501111111",
  "isActive": true
}
```

**Expected Response:** (200 OK)
```json
{
  "success": true,
  "message": "Patient updated successfully",
  "data": {
    "id": "uuid",
    "firstName": "Ahmed Updated",
    "email": "ahmed.updated@example.com",
    ...
  },
  "statusCode": 200
}
```

---

### **F. Delete Patient (Soft Delete)**

**Request:**
```
DELETE /api/patients/{id}
```

**Expected Response:**
```json
{
"success": true,
  "message": "Patient deleted successfully",
  "statusCode": 200
}
```

---

## **Coverage Endpoints**

### **Get All Coverages**

```
GET /api/coverage?pageNumber=1&pageSize=10
```

### **Get Coverages for Patient**

```
GET /api/coverage/patient/{patientId}
```

### **Get Expiring Coverages**

```
GET /api/coverage/expiring?daysFromNow=30
```

### **Get Coverage by Policy Number**

```
GET /api/coverage/policy/{policyNumber}
```

### **Create Coverage**

```
POST /api/coverage
Content-Type: application/json

{
  "policyNumber": "POL-2024-004",
  "memberId": "MEM-2024-004",
  "coverageType": "employee",
  "patientId": "{patientId}",
  "insurerId": "{insurerId}",
  "coverageStartDate": "2024-01-01",
  "coverageEndDate": "2024-12-31",
  "relationToSubscriber": "Self",
  "annualDeductible": 500,
  "copay": 50,
  "coinsurancePercent": 20,
  "outOfPocketMax": 2000
}
```

---

## **Error Responses**

### **400 Bad Request**
```json
{
  "success": false,
  "message": "Pagination validation failed",
  "statusCode": 400
}
```

### **404 Not Found**
```json
{
  "success": false,
  "message": "Patient not found",
  "statusCode": 404
}
```

### **500 Internal Server Error**
```json
{
  "success": false,
  "message": "An error occurred while retrieving patients",
  "statusCode": 500
}
```

---

## **Postman Collection Template**

If you prefer using Postman, import this:

```json
{
  "info": {
    "name": "NPhies FHIR Integration API",
  "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "item": [
    {
 "name": "Patients",
      "item": [
        {
          "name": "Get All Patients",
    "request": {
  "method": "GET",
       "url": "{{base_url}}/api/patients?pageNumber=1&pageSize=10"
  }
        },
      {
          "name": "Get Patient by ID",
          "request": {
  "method": "GET",
   "url": "{{base_url}}/api/patients/{{patientId}}"
        }
        },
        {
    "name": "Create Patient",
    "request": {
     "method": "POST",
 "url": "{{base_url}}/api/patients",
            "body": {
        "mode": "raw",
        "raw": "{...}"
            }
        }
        }
      ]
    },
    {
      "name": "Coverage",
      "item": [
        {
          "name": "Get All Coverages",
        "request": {
    "method": "GET",
         "url": "{{base_url}}/api/coverage?pageNumber=1&pageSize=10"
          }
   },
        {
       "name": "Get Patient Coverages",
          "request": {
            "method": "GET",
        "url": "{{base_url}}/api/coverage/patient/{{patientId}}"
          }
        }
      ]
    }
  ],
  "variable": [
    {
      "key": "base_url",
      "value": "https://localhost:7xxx"
    }
  ]
}
```

---

## **Common Issues & Solutions**

| Issue | Solution |
|-------|----------|
| **Connection refused** | Ensure SQL Server is running and connection string is correct |
| **404 on /swagger** | Application might not be running or port is different |
| **Empty patient list** | Wait for seeding to complete (check logs) |
| **MRN already exists** | Use unique MRN when creating patients |
| **Invalid page size** | Max page size is 100, min is 1 |

---

## **Performance Tips**

- Use pagination to avoid loading all records
- Filter/search to reduce network traffic
- Cache frequently accessed data in future
- Use `pageSize=50` for optimal balance

---

## **Next Steps**

1. ? Run and test all Patient endpoints
2. ? Run and test all Coverage endpoints
3. ? Create Organization controller
4. ? Create Eligibility controller
5. ? Create Claims controller
6. ? Write integration tests

---

**Happy Testing! ??**
