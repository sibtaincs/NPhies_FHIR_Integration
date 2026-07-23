# ?? **COMPLETE SETUP - START BACKEND & FRONTEND**

**Status**: ? **Ready to Run**  
**Time**: ~5 minutes setup  

---

## ? **FASTEST WAY TO RUN (3 STEPS)**

### **Step 1: Start Backend** (Terminal 1)
```bash
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration
dotnet run --project NPhies_FHIR_Integration.ApiService
```

**Wait for**:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7001
```

### **Step 2: Start Frontend** (Terminal 2)
```bash
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd
ng serve
```

**Wait for**:
```
Application bundle generation complete.
```

### **Step 3: Open Browser**
```
URL: http://localhost:4200/auth/login
```

---

## ?? **TEST LOGIN CREDENTIALS**

```
Option 1 - Reviewer:
  Username: test.reviewer
  Password: TestPassword123!

Option 2 - Manager:
  Username: admin.manager
  Password: AdminPassword123!

Option 3 - Another Reviewer:
  Username: john.reviewer
  Password: TestPassword123!
```

---

## ?? **WHAT HAPPENS**

```
1. You enter credentials ? Frontend
2. Frontend sends POST /api/auth/login ? Backend
3. Backend validates credentials ? Database
4. Returns accessToken + refreshToken ? Frontend
5. Frontend stores tokens in localStorage
6. Redirects to /dashboard
7. All future requests include token in header
```

---

## ? **VERIFICATION**

### **Backend Running?**
- Check Terminal 1 for "Now listening on: https://localhost:7001"

### **Frontend Running?**
- Check Terminal 2 for "Application bundle generation complete"

### **Login Works?**
- Enter `test.reviewer` / `TestPassword123!`
- Click "Sign In"
- Should see: "Login successful! Redirecting..."
- Should redirect to `/dashboard`

### **Check Storage**
- Press F12 in browser
- Go to Application tab
- Check localStorage for:
  - `accessToken` ?
  - `refreshToken` ?
  - `currentUser` ?

---

## ?? **TROUBLESHOOTING**

### **"Cannot connect to server"** ?
```
? Check backend is running on https://localhost:7001
? Check no errors in backend terminal
? Check firewall allows port 7001
```

### **"Invalid username or password"** ?
```
? Use exactly: test.reviewer / TestPassword123!
? Check database was seeded
? Check users exist in database
```

### **"CORS error"** ?
```
? Backend should auto-allow http://localhost:4200
? If custom URL, update appsettings.Development.json
? Check browser console for exact error
```

---

## ?? **PORTS**

| Service | Port | URL |
|---------|------|-----|
| Backend API | 7001 | https://localhost:7001 |
| Frontend | 4200 | http://localhost:4200 |
| Database | (local) | (localdb)\mssqllocaldb |

---

## ?? **FULL WORKFLOW**

```
???????????????????
?   Browser    ?
?   :4200         ?
???????????????????
    ?
    ????????????
    ? Frontend  ?
    ? Angular   ?
    ? :4200     ?
    ?????????????
         ? HTTP POST
         ? /api/auth/login
       ?
    ?????????????????????
 ?  Backend API      ?
    ?  .NET 9           ?
  ?  :7001            ?
    ?  AuthController   ?
    ????????????????????
         ?
    ????????????????
    ?  Database    ?
?  SQL Server  ?
    ?  LocalDB     ?
    ????????????????
```

---

## ?? **STEP-BY-STEP DETAILED**

### **Detailed Step 1: Start Backend**

```bash
# Navigate to solution
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration

# Option A: Using dotnet CLI
dotnet run --project NPhies_FHIR_Integration.ApiService

# Option B: Using Visual Studio (if available)
# 1. Open NPhies_FHIR_Integration.sln
# 2. Set NPhies_FHIR_Integration.ApiService as startup project
# 3. Press F5 or Ctrl+F5
```

**Expected Terminal Output**:
```
info: Microsoft.EntityFrameworkCore.Infrastructure[10403]
      Entity Framework Core 8.0.11 initialized 'ApplicationDbContext'...
info: NPhies_FHIR_Integration.Infrastructure.Seeding.DatabaseSeeder[0]
      Seeding database with initial data...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7001
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### **Detailed Step 2: Start Frontend**

```bash
# Navigate to Angular project
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd

# Install dependencies (first time only)
npm install

# Start development server
ng serve --port 4200
```

**Expected Terminal Output**:
```
? Compiled successfully.
? Bundle generation complete.

Local:        http://localhost:4200/
Browser:      ready
...
```

### **Detailed Step 3: Open in Browser**

```
URL: http://localhost:4200/auth/login
```

**You should see**:
- Beautiful login form
- Purple-blue gradient background
- Username and password inputs
- Demo buttons (?? and ?????)
- Loading spinner (when submitting)

### **Detailed Step 4: Login**

```
1. Click demo button [??] to auto-fill reviewer credentials
 Username: test.reviewer
   Password: TestPassword123!

2. Click "Sign In" button

3. Watch for:
   - Loading spinner appears
   - Success message shows
   - Redirects after 1 second to /dashboard
   - localStorage has accessToken
```

---

## ?? **SECURITY ENABLED**

```
? JWT Authentication (1 hour expiration)
? Password Hashing (PBKDF2)
? CORS Enabled
? Rate Limiting
? Audit Logging
? Token Refresh
? Brute Force Protection
? HTTPS Ready
```

---

## ?? **GUIDES**

| Guide | Purpose |
|-------|---------|
| **BACKEND_SETUP_LOGIN_API_TESTING.md** | Backend setup & API testing |
| **LOGIN_PAGE_SIMPLIFIED_WORKING.md** | Frontend login page |
| **LOGIN_PAGE_QUICK_REFERENCE.md** | Quick reference card |
| **LOGIN_PAGE_TROUBLESHOOTING_GUIDE.md** | Troubleshooting |

---

## ?? **ARCHITECTURE**

```
Frontend (Angular):
- Login Component
- Auth Service
- JWT Interceptor
- Auth Guard
- localStorage for tokens

Backend (.NET 9):
- AuthController
- AuthenticationService
- JwtService
- PasswordHashingService
- Database (SQL Server)

Communication:
- POST /api/auth/login ? returns tokens
- GET /api/auth/me ? returns user
- POST /api/auth/refresh ? refresh token
- POST /api/auth/logout ? revoke token
```

---

## ? **FEATURES READY**

```
Frontend:
? Beautiful login UI
? Form validation
? Password visibility toggle
? Demo buttons
? Error alerts
? Loading states
? Token storage
? Auto-redirect

Backend:
? User authentication
? JWT token generation
? Token refresh mechanism
? User validation
? CORS support
? Rate limiting
? Audit logging
? Error handling
```

---

## ?? **IMPORTANT NOTES**

1. **Backend must run FIRST**
   - Database needs to seed
   - Port 7001 must be available

2. **HTTPS/SSL Certificate**
   - LocalDB on localhost:7001 uses HTTPS
   - Browser may show security warning
   - This is normal for local development
   - Click "Advanced" ? "Proceed"

3. **Database**
   - Uses (localdb)\mssqllocaldb
   - Creates NPhiesDb automatically
   - Test users auto-seeded

4. **Browser Console**
   - Press F12 to open DevTools
   - Check Console tab for errors
   - Check Network tab for API calls

---

# **YOU'RE ALL SET!** ?

```bash
# Terminal 1
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration
dotnet run --project NPhies_FHIR_Integration.ApiService

# Terminal 2
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd
ng serve

# Browser
http://localhost:4200/auth/login

# Login
Username: test.reviewer
Password: TestPassword123!
```

---

## ?? **NEED HELP?**

| Issue | Check |
|-------|-------|
| Backend won't start | Port 7001 available? SQL Server running? |
| Frontend won't start | npm install ran? Port 4200 free? |
| Login fails | Backend running? Users in database? |
| CORS error | Backend CORS config? Frontend URL correct? |
| Tokens not stored | Browser localStorage enabled? |

---

# **LET'S GO!** ??
