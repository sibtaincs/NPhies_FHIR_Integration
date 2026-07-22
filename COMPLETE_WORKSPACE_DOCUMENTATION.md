# ?? **COMPLETE WORKSPACE SETUP & DOCUMENTATION**

**Last Updated**: January 2024  
**Status**: ? Phase 1 Complete - Ready for Phase 2 Development

---

## ?? **TABLE OF CONTENTS**

1. [Workspace Overview](#workspace-overview)
2. [Frontend Setup (Angular)](#frontend-setup-angular)
3. [Backend Setup (.NET 9)](#backend-setup-net-9)
4. [Project Structure](#project-structure)
5. [Quick Start Commands](#quick-start-commands)
6. [Development Workflow](#development-workflow)
7. [Integration Points](#integration-points)
8. [Database Configuration](#database-configuration)
9. [API Endpoints](#api-endpoints)
10. [Deployment Guide](#deployment-guide)

---

## ?? **WORKSPACE OVERVIEW**

### **Workspace Location**

```
C:\Users\sibtain.alimadad\source\repos\
??? NPhies_FHIR_Integration/  (Backend - .NET 9)
?   ??? NPhies_FHIR_Integration.ApiService/
?   ??? NPhies_FHIR_Integration.Application/
?   ??? NPhies_FHIR_Integration.Domain/
?   ??? NPhies_FHIR_Integration.Infrastructure/
?   ??? NPhies_FHIR_Integration.Common/
?   ??? NPhies_FHIR_Integration.ServiceDefaults/
?
??? rcm-portal-antd/              (Frontend - Angular 18)
    ??? src/
    ??? node_modules/
    ??? package.json
    ??? angular.json
```

### **Project Technologies**

| Component | Technology | Version |
|-----------|-----------|---------|
| **Backend** | .NET | 9.0 |
| **Backend Framework** | ASP.NET Core | Latest |
| **Frontend** | Angular | 18.2.14 |
| **UI Library** | Ant Design | 18.0.0 |
| **Package Manager (Node)** | npm | 10.8.2 |
| **Node.js** | Node | 18.20.8 |
| **TypeScript** | TypeScript | 5.5.4 |

### **Git Repository**

```
Repository: NPhies_FHIR_Integration
URL: https://github.com/sibtaincs/NPhies_FHIR_Integration
Branch: main
Location: C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration
```

---

## ?? **FRONTEND SETUP (ANGULAR)**

### **Project Details**

```
Project Name: rcm-portal-antd
Location: C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd\
Framework: Angular 18.2.14
UI Library: Ant Design (ng-zorro-antd 18.0.0)
Dev Server: http://localhost:4300
```

### **Key Dependencies**

```json
{
  "dependencies": {
    "@angular/animations": "^18.2.0",
    "@angular/common": "^18.2.0",
    "@angular/compiler": "^18.2.0",
    "@angular/core": "^18.2.0",
    "@angular/forms": "^18.2.0",
    "@angular/platform-browser": "^18.2.0",
    "@angular/platform-browser-dynamic": "^18.2.0",
    "@angular/router": "^18.2.0",
    "ng-zorro-antd": "^18.0.0",
    "chart.js": "^4.5.1",
    "ng2-charts": "^4.1.1",
    "moment": "^2.30.1",
    "ngx-moment": "^6.0.2",
  "lodash-es": "^4.18.1",
    "rxjs": "~7.8.0",
    "tslib": "^2.3.0",
    "zone.js": "~0.14.2"
  }
}
```

### **Frontend Project Structure**

```
src/app/
??? core/    # Singleton services, guards, interceptors
?   ??? services/    (API, Auth, etc.)
?   ??? interceptors/(HTTP, Error handling)
?   ??? guards/     (Route guards, Auth guard)
?   ??? models/             (TypeScript interfaces)
?   ??? constants/   (App constants)
?
??? shared/       # Reusable components
?   ??? components/
?   ?   ??? navbar/
?   ?   ??? sidebar/
?   ?   ??? footer/
?   ??? pipes/  (Custom pipes)
?   ??? directives/         (Custom directives)
?
??? features/    # Feature modules
?   ??? dashboard/
?   ??? claims/
?   ??? eligibility/
?   ??? pre-auth/
?   ??? reports/
?
??? auth/               # Authentication
?   ??? components/
?   ?   ??? login/
?   ?   ??? logout/
?   ??? services/
?
??? app.component.ts
??? app.component.html
??? app.component.less
??? app-routing.module.ts
??? app.module.ts

src/
??? assets/       # Static files
?   ??? images/
?   ??? styles/
??? environments/    # Environment config
?   ??? environment.ts
?   ??? environment.prod.ts
??? theme.less         # Ant Design theme
??? styles.less             # Global styles
??? main.ts        # Entry point
```

### **Running Frontend**

```powershell
# Navigate to project
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd

# Open in VS Code
code .

# Start dev server (in terminal Ctrl + `)
npm start

# Access app
# http://localhost:4300
```

---

## ?? **BACKEND SETUP (.NET 9)**

### **Backend Projects**

| Project | Purpose | Path |
|---------|---------|------|
| **ApiService** | Main API entry point | `NPhies_FHIR_Integration.ApiService` |
| **Application** | Business logic layer | `NPhies_FHIR_Integration.Application` |
| **Domain** | Domain entities & interfaces | `NPhies_FHIR_Integration.Domain` |
| **Infrastructure** | Data access & external services | `NPhies_FHIR_Integration.Infrastructure` |
| **Common** | Shared utilities & constants | `NPhies_FHIR_Integration.Common` |
| **ServiceDefaults** | Service configuration & defaults | `NPhies_FHIR_Integration.ServiceDefaults` |

### **Backend Architecture**

```
NPhies_FHIR_Integration/
??? NPhies_FHIR_Integration.ApiService/
?   ??? Controllers/    # API endpoints
? ??? Program.cs         # App entry point
?   ??? appsettings.json   # Configuration
?
??? NPhies_FHIR_Integration.Application/
?   ??? Services/          # Business logic
?   ??? DTOs/        # Data transfer objects
?   ??? Interfaces/        # Service contracts
?
??? NPhies_FHIR_Integration.Domain/
?   ??? Entities/          # Domain models
?   ??? Enums/        # Domain enumerations
?   ??? Interfaces/        # Domain contracts
?
??? NPhies_FHIR_Integration.Infrastructure/
?   ??? Data/              # Database context
?   ??? Repositories/      # Data access
?   ??? ExternalServices/  # Third-party integrations
?
??? NPhies_FHIR_Integration.Common/
?   ??? Constants/         # App constants
?   ??? Exceptions/   # Custom exceptions
?   ??? Utilities/         # Helper functions
?
??? NPhies_FHIR_Integration.ServiceDefaults/
    ??? Extensions/        # Service configuration
```

### **Backend Technologies**

- **.NET Version**: 9.0
- **Framework**: ASP.NET Core
- **API Type**: RESTful API
- **Authentication**: JWT Bearer
- **Database**: SQL Server / Entity Framework Core
- **CORS**: Enabled for frontend (localhost:4300)

### **Running Backend**

```powershell
# Navigate to backend
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration

# Build solution
dotnet build

# Run API service
dotnet run --project NPhies_FHIR_Integration.ApiService

# Access API
# http://localhost:5000/api
```

---

## ?? **PROJECT STRUCTURE**

### **Complete Workspace Tree**

```
C:\Users\sibtain.alimadad\source\repos\
?
?? NPhies_FHIR_Integration/         [Backend - .NET 9]
?  ?? NPhies_FHIR_Integration.ApiService/
?  ?  ?? Controllers/
?  ?  ?  ?? AuthController.cs
?  ??  ?? ClaimsController.cs
?  ?  ?  ?? EligibilityController.cs
?  ?  ?  ?? ReportsController.cs
?  ?  ?? Properties/
?  ?  ?? appsettings.json
?  ?  ?? Program.cs
?  ?  ?? NPhies_FHIR_Integration.ApiService.csproj
?  ?
?  ?? NPhies_FHIR_Integration.Application/
?  ?  ?? Services/
?  ?  ?? DTOs/
?  ?  ?? NPhies_FHIR_Integration.Application.csproj
?  ?
?  ?? NPhies_FHIR_Integration.Domain/
?  ?  ?? Entities/
?  ?  ?? Enums/
?  ?  ?? NPhies_FHIR_Integration.Domain.csproj
?  ?
?  ?? NPhies_FHIR_Integration.Infrastructure/
?  ?  ?? Data/
?  ?  ?? Repositories/
?  ?  ?? NPhies_FHIR_Integration.Infrastructure.csproj
?  ?
?  ?? NPhies_FHIR_Integration.Common/
?  ?  ?? Constants/
?  ?  ?? Exceptions/
?  ?  ?? NPhies_FHIR_Integration.Common.csproj
?  ?
?  ?? NPhies_FHIR_Integration.ServiceDefaults/
?  ?  ?? NPhies_FHIR_Integration.ServiceDefaults.csproj
?  ?
?  ?? .gitignore
?  ?? README.md
?  ?? NPhies_FHIR_Integration.sln
?
?? rcm-portal-antd/     [Frontend - Angular 18]
   ?? src/
   ?  ?? app/
   ?  ?  ?? core/
   ?  ?  ?  ?? services/
   ?  ?  ??? interceptors/
   ?  ?  ?  ?? guards/
   ?  ?  ?  ?? models/
   ?  ?  ?  ?? constants/
   ?  ?  ?? shared/
   ?  ?  ?  ?? components/
   ?  ?  ?  ?? pipes/
   ?  ?  ?  ?? directives/
   ?  ?  ?? features/
 ?  ?  ?  ?? dashboard/
   ?  ?  ?  ?? claims/
   ?  ?  ?  ?? eligibility/
   ?  ?  ?  ?? pre-auth/
?  ?  ?  ?? reports/
   ?  ?  ?? auth/
   ?  ?  ?? app.component.ts
 ?  ?  ?? app.component.html
   ?  ?  ?? app-routing.module.ts
   ?  ?  ?? app.module.ts
   ?  ?? assets/
   ?  ?? environments/
   ?  ?? theme.less
   ?  ?? styles.less
   ?  ?? main.ts
 ?? node_modules/     [967 packages]
   ?? angular.json
   ?? package.json
   ?? tsconfig.json
   ?? .npmrc
   ?? .nvmrc
   ?? start-server.bat
   ?? README.md
```

---

## ? **QUICK START COMMANDS**

### **Frontend Development**

```powershell
# Start frontend dev server
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd
npm start
# Access: http://localhost:4300

# Or using batch file
.\start-server.bat

# Create new component
ng generate component features/my-feature/components/my-component

# Create new service
ng generate service core/services/my-service

# Build for production
npm run build

# Run tests
npm test

# Check code style
npm run lint
```

### **Backend Development**

```powershell
# Navigate to backend
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration

# Build solution
dotnet build

# Run API
dotnet run --project NPhies_FHIR_Integration.ApiService

# Run specific project
dotnet run --project NPhies_FHIR_Integration.ApiService
# Access: http://localhost:5000/api

# Apply migrations
dotnet ef database update --project NPhies_FHIR_Integration.Infrastructure

# Create migration
dotnet ef migrations add MigrationName --project NPhies_FHIR_Integration.Infrastructure

# Run tests
dotnet test

# Clean solution
dotnet clean
```

### **Git Commands**

```powershell
# Check status
git status

# View changes
git diff

# Stage changes
git add .

# Commit
git commit -m "feat: Your feature description"

# Push to remote
git push origin main

# Pull latest
git pull origin main

# Create feature branch
git checkout -b feature/new-feature
```

---

## ?? **DEVELOPMENT WORKFLOW**

### **Full Stack Development Setup**

#### **Terminal 1: Start Frontend**

```powershell
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd
npm start
# Frontend running on: http://localhost:4300
```

#### **Terminal 2: Start Backend**

```powershell
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration
dotnet run --project NPhies_FHIR_Integration.ApiService
# Backend API running on: http://localhost:5000/api
```

#### **Terminal 3: VS Code (Frontend Development)**

```powershell
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd
code .
# Edit and develop frontend
```

### **Daily Development Checklist**

- [ ] Start frontend: `npm start` (localhost:4300)
- [ ] Start backend: `dotnet run` (localhost:5000)
- [ ] Open VS Code for frontend
- [ ] Open Visual Studio for backend
- [ ] Create feature branch: `git checkout -b feature/name`
- [ ] Make changes
- [ ] Test locally
- [ ] Commit: `git commit -m "feat: description"`
- [ ] Push: `git push origin feature/name`
- [ ] Create pull request

---

## ?? **INTEGRATION POINTS**

### **Frontend-Backend Communication**

```
????????????????????????????????????????????????
?        Angular Frontend (Port 4300)?
?  ??????????????????????????????????????????? ?
?  ?         HTTP Client (HttpClient)   ? ?
?  ?  ????????????????????????????????????  ? ?
?  ?  ?   Auth Interceptor (JWT Token)  ?  ? ?
?  ?  ?   Error Interceptor     ?  ? ?
?  ?  ????????????????????????????????????  ? ?
?  ??????????????????????????????????????????? ?
????????????????????????????????????????????????
   ? HTTP/HTTPS
              ?
????????????????????????????????????????????????
?     ASP.NET Core API (Port 5000)             ?
?  ??????????????????????????????????????????? ?
?  ?    Controllers          ? ?
?  ?  - AuthController       ? ?
?  ?  - ClaimsController    ? ?
?  ?  - EligibilityController     ? ?
?  ?  - ReportsController         ? ?
?  ???????????????????????????????????????????? ?
?  ??????????????????????????????????????????? ?
?  ?        Business Logic (Services)       ? ?
?  ?  - ClaimsService              ? ?
?  ?  - EligibilityService      ? ?
?  ?  - AuthService  ? ?
?  ???????????????????????????????????????????? ?
?  ??????????????????????????????????????????? ?
?  ?     Data Access (Repositories)   ? ?
?  ?  - ClaimsRepository ? ?
?  ?  - EligibilityRepository       ? ?
?  ???????????????????????????????????????????? ?
?  ??????????????????????????????????????????? ?
?  ?        Database (SQL Server)           ? ?
?  ?  - Claims Table           ? ?
?  ?  - Eligibility Table   ? ?
?  ?  - Users Table     ? ?
?  ???????????????????????????????????????????? ?
????????????????????????????????????????????????
```

### **Environment Configuration**

**Frontend** (`src/environments/environment.ts`):
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api',
  endpoints: {
    auth: { login: '/auth/login', logout: '/auth/logout' },
    claims: { list: '/claims', create: '/claims' },
    eligibility: { check: '/v1/eligibility/check' }
  }
};
```

**Backend** (`appsettings.json`):
```json
{
  "Logging": { "LogLevel": { "Default": "Information" } },
  "AllowedHosts": "*",
  "Cors": {
    "AllowedOrigins": "http://localhost:4300",
    "AllowedMethods": "GET,POST,PUT,DELETE",
    "AllowedHeaders": "Content-Type,Authorization"
  }
}
```

---

## ??? **DATABASE CONFIGURATION**

### **Connection String**

Location: `appsettings.json` in ApiService

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NPhiesDb;Trusted_Connection=true;"
  }
}
```

### **Entity Framework Migrations**

```powershell
# Create initial migration
dotnet ef migrations add InitialCreate --project NPhies_FHIR_Integration.Infrastructure

# Update database
dotnet ef database update --project NPhies_FHIR_Integration.Infrastructure

# List migrations
dotnet ef migrations list --project NPhies_FHIR_Integration.Infrastructure

# Remove last migration
dotnet ef migrations remove --project NPhies_FHIR_Integration.Infrastructure
```

### **Database Tables**

- **Users** - User accounts
- **Claims** - Insurance claims
- **Eligibility** - Eligibility records
- **PreAuthorizations** - Pre-auth requests
- **AuditLogs** - System audit trails

---

## ?? **API ENDPOINTS**

### **Authentication**

```
POST   /api/auth/login              Login user
POST   /api/auth/logout       Logout user
POST   /api/auth/refresh            Refresh token
GET    /api/auth/me          Get current user
```

### **Claims**

```
GET  /api/claims      List all claims
GET    /api/claims/{id}     Get claim by ID
POST   /api/claims         Create new claim
PUT  /api/claims/{id}     Update claim
DELETE /api/claims/{id}   Delete claim
GET    /api/claims/patient/{id}     Get claims by patient
GET    /api/claims/status/{status}  Get claims by status
```

### **Eligibility**

```
GET    /api/v1/eligibility/check    Check eligibility
GET    /api/v1/eligibility/requests List eligibility requests
GET    /api/v1/eligibility/responses List eligibility responses
GET    /api/v1/eligibility/requests/pending  Get pending requests
```

### **Reports**

```
GET    /api/reports       List all reports
GET    /api/reports/{id}   Get report by ID
POST   /api/reports/generate     Generate new report
GET    /api/reports/export/{id}Export report (PDF/Excel)
```

---

## ?? **DEPLOYMENT GUIDE**

### **Frontend Deployment**

#### **1. Build Production Bundle**

```powershell
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd
ng build --configuration production

# Output in: dist/rcm-portal-antd/
```

#### **2. Deploy to Web Server**

```powershell
# Copy dist folder to hosting server
# Configure for SPA routing
# Update API_URL in environment config for production
```

#### **3. Environment Configuration for Production**

Update `src/environments/environment.prod.ts`:

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://your-api-domain.com/api',
  apiTimeout: 30000,
  // ... other config
};
```

### **Backend Deployment**

#### **1. Build Release**

```powershell
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration
dotnet publish -c Release -o ./publish

# Output in: ./publish/
```

#### **2. Configure for Production**

Update `appsettings.Production.json`:

```json
{
  "Logging": { "LogLevel": { "Default": "Error" } },
  "ConnectionStrings": {
    "DefaultConnection": "Production connection string"
},
  "Cors": {
    "AllowedOrigins": "https://your-frontend-domain.com"
  }
}
```

#### **3. Deploy to Server**

```powershell
# Deploy published files to server
# Configure IIS or Linux hosting
# Run migrations on production database
# Setup SSL certificates
```

---

## ?? **DOCUMENTATION FILES**

### **In Repository Root**

- `ANGULAR_FRONTEND_DEVELOPMENT_MASTER_GUIDE.md` - Complete Angular guide
- `DOCUMENTATION_CONSOLIDATION_SUMMARY.md` - Documentation overview
- `README.md` - Project overview (in each project)

### **Key Configuration Files**

**Frontend:**
- `angular.json` - Angular build config
- `package.json` - Dependencies
- `tsconfig.json` - TypeScript config
- `.npmrc` - npm config
- `.nvmrc` - Node version

**Backend:**
- `*.csproj` - Project configuration
- `appsettings.json` - App configuration
- `Program.cs` - Startup configuration

---

## ?? **NEXT STEPS**

### **Immediate (This Week)**

1. **Phase 2: Core Architecture**
   - [ ] Create TypeScript models
   - [ ] Build API service layer
   - [ ] Implement authentication
   - [ ] Setup interceptors

2. **Backend Setup**
   - [ ] Configure database connection
   - [ ] Create EF Core migrations
   - [ ] Build API controllers
   - [ ] Implement authentication

### **Short Term (Next 2 Weeks)**

1. **Phase 3: Feature Development**
   - [ ] Build dashboard module
   - [ ] Implement claims management
   - [ ] Add eligibility checking
   - [ ] Create pre-auth module

2. **Backend Features**
   - [ ] Build business logic services
   - [ ] Implement data access layer
   - [ ] Add validation and error handling
   - [ ] Setup logging and monitoring

### **Testing & Deployment**

- [ ] Unit tests
- [ ] Integration tests
- [ ] End-to-end testing
- [ ] Performance optimization
- [ ] Security audit
- [ ] Deployment preparation

---

## ?? **USEFUL RESOURCES**

### **Documentation**

| Resource | URL |
|----------|-----|
| Angular | https://angular.io |
| Ant Design | https://ng.ant.design |
| .NET 9 | https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9 |
| ASP.NET Core | https://learn.microsoft.com/en-us/aspnet/core |
| Entity Framework | https://learn.microsoft.com/en-us/ef/core |
| TypeScript | https://www.typescriptlang.org |
| RxJS | https://rxjs.dev |
| RESTful API Design | https://restfulapi.net |

### **Tools**

- **VS Code** - Frontend IDE
- **Visual Studio** - Backend IDE
- **Postman** - API testing
- **SQL Server Management Studio** - Database management
- **Git** - Version control

---

## ? **DEVELOPMENT CHECKLIST**

### **Setup Phase**
- [ ] Node.js 18+ installed
- [ ] npm 10+ installed
- [ ] .NET 9 SDK installed
- [ ] Visual Studio or VS Code installed
- [ ] Git configured
- [ ] Frontend dependencies installed (`npm install`)
- [ ] Backend dependencies resolved

### **Development Phase**
- [ ] Frontend dev server running (localhost:4300)
- [ ] Backend API running (localhost:5000)
- [ ] Database connection working
- [ ] Authentication implemented
- [ ] Interceptors configured
- [ ] API endpoints tested

### **Testing Phase**
- [ ] Unit tests passing
- [ ] Integration tests passing
- [ ] E2E tests passing
- [ ] Performance acceptable
- [ ] No security vulnerabilities

### **Deployment Phase**
- [ ] Production build created
- [ ] Configuration updated
- [ ] Database migrations applied
- [ ] SSL certificates configured
- [ ] Monitoring setup
- [ ] Deployment tested

---

## ?? **SUMMARY**

### **Current State**
- ? Frontend: Angular 18 project created and running
- ? Backend: .NET 9 project structure setup
- ? Both projects in single repository
- ? Development environment configured
- ? Ready for feature development

### **Architecture**
- Frontend: Angular 18 + Ant Design (Port 4300)
- Backend: ASP.NET Core 9 (Port 5000)
- Database: SQL Server
- Communication: HTTP/REST with JWT authentication

### **Next Action**
Begin Phase 2: Implement core architecture and APIs

```powershell
# Terminal 1: Start Frontend
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd
npm start

# Terminal 2: Start Backend
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration
dotnet run --project NPhies_FHIR_Integration.ApiService

# Then: Start building features! ??
```

---

**Happy Coding! ??**

For more detailed information, refer to:
- `ANGULAR_FRONTEND_DEVELOPMENT_MASTER_GUIDE.md` (Frontend)
- Backend documentation in each project's README.md

