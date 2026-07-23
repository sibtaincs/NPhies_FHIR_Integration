# ?? **BACKEND SETUP & LOGIN API TESTING GUIDE**

**Status**: ? **Backend Ready**  
**Framework**: .NET 9  
**Authentication**: JWT  

---

## ?? **QUICK START - 5 MINUTES**

### **Step 1: Setup Database**
```bash
# Navigate to backend project
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration

# Create database (using EF migrations)
dotnet ef database update -p NPhies_FHIR_Integration.Infrastructure
```

### **Step 2: Start Backend Server**
```bash
# Run the API
dotnet run --project NPhies_FHIR_Integration.ApiService
```

**Expected Output**:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7001
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### **Step 3: Test Login Endpoint**
```bash
# Open another terminal and test the API
curl -X POST https://localhost:7001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"test.reviewer","password":"TestPassword123!"}'
```

**Expected Response**:
```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "accessToken": "eyJ...",
    "refreshToken": "refresh...",
    "expiresIn": 3600,
    "user": {
      "id": "user-123",
"username": "test.reviewer",
   "email": "test.reviewer@example.com",
      "roles": ["TECHNICAL_REVIEWER"],
    "isActive": true
    }
  }
}
```

### **Step 4: Update Frontend API Endpoint**
Update your login component to call the real API:

```typescript
// In login.component.ts, replace the mock login with real API call
onSubmit(): void {
  this.submitted = true;
  this.errorMsg = '';
  this.successMsg = '';

  if (this.loginForm.invalid) {
    this.errorMsg = 'Please fill in all fields';
    return;
  }

  this.isLoading = true;

  // REAL API CALL (replace mock setTimeout)
  const loginData = {
    username: this.loginForm.value.username,
    password: this.loginForm.value.password
  };

  this.http.post<any>('/api/auth/login', loginData).subscribe({
    next: (response) => {
   if (response.success && response.data) {
        // Store tokens
        localStorage.setItem('accessToken', response.data.accessToken);
        localStorage.setItem('refreshToken', response.data.refreshToken);
        localStorage.setItem('currentUser', JSON.stringify(response.data.user));

        this.successMsg = 'Login successful! Redirecting...';
 this.isLoading = false;

        // Redirect
   setTimeout(() => {
   const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/dashboard';
 this.router.navigateByUrl(returnUrl);
        }, 1000);
 }
    },
    error: (error) => {
      this.errorMsg = error?.error?.message || 'Login failed. Please try again.';
      this.isLoading = false;
    }
  });
}
```

### **Step 5: Test Frontend + Backend Together**
```bash
# Terminal 1: Backend
cd NPhies_FHIR_Integration
dotnet run --project NPhies_FHIR_Integration.ApiService

# Terminal 2: Frontend
cd rcm-portal-antd
ng serve --port 4200
```

**Test URL**: http://localhost:4200/auth/login  
**Test User**: test.reviewer / TestPassword123!

---

## ?? **TEST CREDENTIALS**

These are created automatically when the database seeds:

```
??????????????????????????????????????????????????????????????
?  TEST CREDENTIALS   ?
??????????????????????????????????????????????????????????????
?  User 1:          ?
?    Username: test.reviewer       ?
?    Email: test.reviewer@example.com      ?
?    Password: TestPassword123!        ?
?    Role: TECHNICAL_REVIEWER    ?
?    ?
?  User 2:      ?
?    Username: admin.manager    ?
?    Email: admin.manager@example.com ?
?    Password: AdminPassword123!          ?
?    Role: TECHNICAL_REVIEW_MANAGER              ?
?     ?
?  User 3:              ?
?    Username: john.reviewer       ?
?    Email: john.reviewer@example.com    ?
?    Password: TestPassword123!    ?
?    Role: TECHNICAL_REVIEWER           ?
??????????????????????????????????????????????????????????????
```

---

## ?? **API ENDPOINTS**

### **Authentication**

| Method | Endpoint | Request | Response |
|--------|----------|---------|----------|
| POST | `/api/auth/login` | `{username, password}` | `{accessToken, refreshToken, user}` |
| POST | `/api/auth/refresh` | `{refreshToken}` | `{accessToken, refreshToken}` |
| POST | `/api/auth/logout` | (Bearer token) | `{success}` |
| GET | `/api/auth/me` | (Bearer token) | `{user, roles, permissions}` |
| GET | `/api/auth/health` | - | `{status: 'healthy'}` |

---

## ?? **TESTING WITH POSTMAN**

### **1. Setup Postman Collection**

Create a new Postman environment with variables:

```json
{
  "baseUrl": "https://localhost:7001",
  "accessToken": "",
  "refreshToken": ""
}
```

### **2. Login Request**

```http
POST {{baseUrl}}/api/auth/login
Content-Type: application/json

{
  "username": "test.reviewer",
  "password": "TestPassword123!"
}
```

**Post-request Script** (save tokens):
```javascript
var jsonData = pm.response.json();
if (jsonData.success) {
  pm.environment.set("accessToken", jsonData.data.accessToken);
  pm.environment.set("refreshToken", jsonData.data.refreshToken);
}
```

### **3. Get Current User**

```http
GET {{baseUrl}}/api/auth/me
Authorization: Bearer {{accessToken}}
```

### **4. Refresh Token**

```http
POST {{baseUrl}}/api/auth/refresh
Content-Type: application/json

{
  "refreshToken": "{{refreshToken}}"
}
```

### **5. Logout**

```http
POST {{baseUrl}}/api/auth/logout
Authorization: Bearer {{accessToken}}
```

---

## ?? **TROUBLESHOOTING**

### **Issue 1: "Connection refused" / Cannot connect to backend**

**Solution**:
```bash
# Check if backend is running
# Terminal should show: "Now listening on: https://localhost:7001"

# If not running, start it:
dotnet run --project NPhies_FHIR_Integration.ApiService

# Check firewall
# Make sure port 7001 is not blocked
```

### **Issue 2: "Database does not exist"**

**Solution**:
```bash
# Create and seed database
dotnet ef database update -p NPhies_FHIR_Integration.Infrastructure

# Or if migrations are missing:
dotnet ef migrations add "InitialCreate" -p NPhies_FHIR_Integration.Infrastructure
dotnet ef database update -p NPhies_FHIR_Integration.Infrastructure
```

### **Issue 3: "401 Unauthorized" / "Invalid credentials"**

**Causes & Solutions**:
1. **User doesn't exist**
   - Make sure database was seeded
   - Run: `dotnet ef database update`
   - Check test users exist in DB

2. **Wrong password**
   - Use correct credentials from test data
   - Default: `TestPassword123!`

3. **JWT not configured**
- Check `appsettings.json` has JwtSettings
   - Verify Secret is set (at least 32 chars)

### **Issue 4: "CORS error" / "No Access-Control-Allow-Origin"**

**Solution** in `appsettings.Development.json`:
```json
{
  "App": {
    "IsDevelopment": true
  },
  "SecuritySettings": {
    "Cors": {
      "AllowedOrigins": [
        "http://localhost:4200",
        "http://localhost:3000"
      ]
    }
  }
}
```

### **Issue 5: Frontend login still fails after backend starts**

**Check these**:
1. Backend running on `https://localhost:7001`? 
2. Frontend making request to correct URL?
3. Database has test users?
4. CORS enabled for `http://localhost:4200`?

---

## ?? **CONFIGURATION**

### **appsettings.Development.json**

Create this file in the ApiService project root:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NPhiesDb;Trusted_Connection=true;"
  },
  "JwtSettings": {
    "Secret": "your-super-secret-key-that-is-at-least-32-characters-long-for-security-jwt",
    "Issuer": "NPhiesIssuer",
    "Audience": "NPhiesAudience",
    "ExpirationMinutes": 60,
  "RefreshTokenExpirationDays": 7
  },
  "App": {
 "IsDevelopment": true
  },
  "SecuritySettings": {
    "Cors": {
      "AllowedOrigins": [
        "http://localhost:4200",
   "http://localhost:3000",
        "http://localhost:4300"
 ]
    },
    "RateLimiting": {
 "Enabled": true,
      "MaxRequestsPerMinute": 100
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

---

## ? **VERIFICATION CHECKLIST**

```
Backend Setup:
[  ] SQL Server is running
[  ] Project builds without errors
[  ] Database migrations applied
[  ] Test data seeded
[  ] API starts on https://localhost:7001

API Testing:
[  ] POST /api/auth/login returns 200
[  ] Response has accessToken
[  ] Response has refreshToken
[  ] Response has user object
[  ] Token can be used for authenticated requests
[  ] Logout revokes token

Frontend Testing:
[  ] Frontend runs on http://localhost:4200
[  ] Login page loads
[  ] Can enter credentials
[  ] Login button works
[  ] Connects to backend API
[  ] Stores tokens in localStorage
[  ] Redirects to dashboard on success
[  ] Shows error on failed login

Integration:
[  ] Frontend + Backend communicate
[  ] CORS working (no browser errors)
[  ] Tokens valid and storable
[  ] Can make authenticated API calls
[  ] Can refresh expired tokens
```

---

## ?? **SECURITY FEATURES ENABLED**

```
[?] JWT Token Authentication
[?] Password Hashing (PBKDF2)
[?] CORS Configuration
[?] Rate Limiting
[?] Audit Logging
[?] Brute Force Protection
[?] Token Expiration
[?] Refresh Token Rotation
[?] HTTPS Support
[?] Authorization Policies
```

---

## ?? **ARCHITECTURE**

```
Frontend (Angular)
    ? (HTTP POST /api/auth/login)
Login Component
 ?
Http Client
    ?
API Service (localhost:7001)
    ?
AuthController
    ?
AuthenticationService
    ?
Database (SQL Server)
    ?
Users Table
    ?
Returns: {accessToken, refreshToken, user}
    ?
Frontend stores tokens
    ?
Used in JWT Interceptor for subsequent requests
```

---

## ?? **RUNNING BOTH SIMULTANEOUSLY**

### **Option 1: Two Terminal Windows**

**Terminal 1 - Backend**:
```bash
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration
dotnet run --project NPhies_FHIR_Integration.ApiService
```

**Terminal 2 - Frontend**:
```bash
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd
ng serve
```

### **Option 2: VS Code Tasks**

Create `.vscode/tasks.json`:
```json
{
  "version": "2.0.0",
  "tasks": [
    {
      "label": "Backend",
      "type": "shell",
 "command": "dotnet",
      "args": [
        "run",
  "--project",
        "NPhies_FHIR_Integration.ApiService"
      ],
      "isBackground": true
    },
    {
      "label": "Frontend",
      "type": "shell",
      "command": "ng",
      "args": ["serve"],
      "isBackground": true
    },
{
      "label": "Start All",
   "dependsOn": ["Backend", "Frontend"],
      "problemMatcher": []
    }
  ]
}
```

Then run: `Ctrl+Shift+B` ? Select "Start All"

---

# **? BACKEND IS READY TO USE!** ??

**Next**: Start both servers and test login!

```bash
# Terminal 1
dotnet run --project NPhies_FHIR_Integration.ApiService

# Terminal 2
ng serve

# Browser
http://localhost:4200/auth/login
```

**Test User**: test.reviewer / TestPassword123!
