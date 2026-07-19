# ?? **ANGULAR RCM PORTAL - COMPLETE FRONTEND SETUP GUIDE**

## ? **STEP-BY-STEP FRONTEND ENVIRONMENT SETUP**

**Status**: Ready for Implementation  
**Technology**: Angular 18 + Ant Design (ng-zorro)  
**Target**: Production-Ready RCM Portal  
**Estimated Time**: 30-45 minutes  

---

## ?? **PREREQUISITES**

Before starting, ensure you have:

```bash
? Node.js 18.x or higher
? npm 9.x or higher
? Angular CLI 18.x
? Git (for version control)
? VS Code or any modern IDE
? Terminal/Command Prompt access
```

### **Verify Prerequisites**

```bash
# Check Node.js version
node --version
# Expected: v18.0.0 or higher

# Check npm version
npm --version
# Expected: 9.0.0 or higher

# Check if Angular CLI is installed
ng version
# If not installed, continue to Step 1
```

---

## ??? **STEP 1: INSTALL ANGULAR CLI**

### **Command**

```bash
npm install -g @angular/cli@18
```

### **Verify Installation**

```bash
ng version
```

**Expected Output:**
```
     _ _         ____ _     ___
    / \   _ __   __ _ _   _| | __ _ _ __ / __ \| |   |_ _|
   / ? \  | '_ \ / _` | | | | |/ _` | '__|   / / _` | |    | |
  / ___ \ | | | | (_| | | |_| | (_| | |      \ \__/ | |    | |
 /_/   \_\|_| |_|\__, |_|\__,_|\__,_|_|       \____|_|_|___|___|
  |___/

Angular CLI: 18.0.0
Node: 18.x.x
OS: win32 x64

Angular: 18.0.0
...
```

---

## ?? **STEP 2: CREATE NEW ANGULAR PROJECT**

### **Command**

```bash
ng new rcm-portal-antd --routing --style=less
```

### **Parameters Explained**

| Parameter | Description |
|-----------|-------------|
| `rcm-portal-antd` | Project name |
| `--routing` | Enable Angular routing |
| `--style=less` | Use LESS for styling (Ant Design native) |

### **What This Creates**

```
rcm-portal-antd/
??? src/
?   ??? app/
?   ??? assets/
?   ??? index.html
?   ??? main.ts
?   ??? styles.less
??? angular.json
??? package.json
??? tsconfig.json
??? ...
```

### **Verify Creation**

```bash
cd rcm-portal-antd
npm install
```

---

## ?? **STEP 3: ADD ANT DESIGN (ng-zorro)**

### **Command**

```bash
ng add ng-zorro-antd
```

### **What This Installs**

The `ng add` command automatically:

- ? Installs `ng-zorro-antd` package
- ? Installs `@ant-design/icons-angular`
- ? Installs `@angular/animations`
- ? Updates `app.module.ts`
- ? Creates theme configuration files
- ? Adds Ant Design icons
- ? Configures Angular CLI

### **Files Created/Modified**

```
src/
??? app/
?   ??? app.module.ts (Updated with Ant Design modules)
??? theme.less (NEW - For theme customization)
??? styles.less (Updated with Ant Design styles)
??? main.ts (Updated)

angular.json (Updated with theme configuration)
package.json (Updated with dependencies)
```

---

## ?? **STEP 4: VERIFY INSTALLATION**

### **Check package.json**

```bash
cat package.json | grep -A5 "dependencies"
```

**Should contain:**
```json
"dependencies": {
  "@angular/animations": "^18.0.0",
  "@angular/common": "^18.0.0",
  "@angular/core": "^18.0.0",
  "@angular/forms": "^18.0.0",
  "@angular/platform-browser": "^18.0.0",
  "@ng-bootstrap/ng-bootstrap": "^14.0.0",
  "@ant-design/icons-angular": "^18.0.0",
  "ng-zorro-antd": "^18.0.0",
  ...
}
```

### **Verify Ant Design Installation**

```bash
npm ls ng-zorro-antd
```

**Expected Output:**
```
rcm-portal-antd@ 
??? ng-zorro-antd@18.0.0
```

---

## ?? **STEP 5: START DEVELOPMENT SERVER**

### **Command**

```bash
ng serve --open
```

### **What This Does**

- ? Compiles Angular project
- ? Starts development server
- ? Opens http://localhost:4200 in browser
- ? Watches for file changes
- ? Hot reloads on save

### **Expected Output**

```
? Compiled successfully.

Application bundle generated successfully. 13.25 seconds
Watch mode enabled. Watching for file changes.
NOTE: Raw file sizes are shown and do not include HTTP compression.

Initial chunk sizes:
bundle.js   4.87 MB
polyfills.js  843.21 kB
styles.css  234.15 kB
```

### **Browser Output**

You should see:
```
Welcome to rcm-portal-antd!
This is your first application.
```

---

## ?? **STEP 6: CREATE PROJECT STRUCTURE**

### **Command Sequence**

```bash
# Create directory structure
mkdir -p src/app/core/services
mkdir -p src/app/core/interceptors
mkdir -p src/app/core/guards
mkdir -p src/app/core/models
mkdir -p src/app/shared/components
mkdir -p src/app/shared/pipes
mkdir -p src/app/shared/directives
mkdir -p src/app/features/dashboard
mkdir -p src/app/features/claims/components
mkdir -p src/app/features/eligibility/components
mkdir -p src/app/features/pre-auth/components
mkdir -p src/app/features/reports/components
mkdir -p src/app/features/settings/components
```

### **Using Angular CLI (Better)**

```bash
# Core modules
ng generate module core
ng generate module core/services
ng generate module core/interceptors
ng generate module core/guards

# Shared module
ng generate module shared

# Feature modules
ng generate module features/dashboard
ng generate module features/claims
ng generate module feature/eligibility
ng generate module features/pre-auth
ng generate module features/reports
ng generate module features/settings

# Auth module
ng generate module auth
```

### **Final Structure**

```
src/app/
??? core/
?   ??? services/
?   ?   ??? api.service.ts
?   ?   ??? auth.service.ts
?   ?   ??? claims.service.ts
?   ?   ??? eligibility.service.ts
?   ??? interceptors/
?   ?   ??? http.interceptor.ts
?   ?   ??? error.interceptor.ts
?   ?   ??? jwt.interceptor.ts
?   ??? guards/
? ?   ??? auth.guard.ts
?   ??? models/
?   ?   ??? claim.model.ts
?   ?   ??? user.model.ts
?   ?   ??? eligibility.model.ts
?   ??? core.module.ts
?
??? shared/
?   ??? components/
?   ?   ??? navbar/
?   ?   ??? sidebar/
?   ?   ??? footer/
?   ??? pipes/
?   ??? directives/
?   ??? shared.module.ts
?
??? features/
?   ??? dashboard/
?   ?   ??? dashboard.component.ts
?   ?   ??? dashboard.component.html
?   ?   ??? dashboard.component.less
?   ?   ??? dashboard.module.ts
?   ??? claims/
?   ?   ??? components/
?   ?   ??? claims.module.ts
?   ??? eligibility/
?   ?   ??? components/
?   ?   ??? eligibility.module.ts
?   ??? pre-auth/
?   ?   ??? components/
?   ?   ??? pre-auth.module.ts
?   ??? reports/
?   ?   ??? components/
?   ?   ??? reports.module.ts
?   ??? settings/
?       ??? components/
?       ??? settings.module.ts
?
??? auth/
?   ??? login/
?   ??? logout/
?   ??? auth.module.ts
?
??? app.component.ts
??? app.component.html
??? app.module.ts
??? app.routing.module.ts
??? app-routing.module.ts
```

---

## ?? **STEP 7: CONFIGURE ENVIRONMENT**

### **Create environments/environment.ts**

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api',
  apiTimeout: 30000,
  appVersion: '1.0.0',
  features: {
    analytics: true,
    reporting: true,
    preAuth: true
  }
};
```

### **Create environments/environment.prod.ts**

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://api.nphies-rcm.health/api',
  apiTimeout: 30000,
  appVersion: '1.0.0',
  features: {
    analytics: true,
    reporting: true,
    preAuth: true
  }
};
```

---

## ?? **STEP 8: CONFIGURE APP MODULE**

### **app.module.ts**

```typescript
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// Ant Design Modules
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzMessageModule } from 'ng-zorro-antd/message';
import { NzNotificationModule } from 'ng-zorro-antd/notification';

// Application Modules
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';

// Interceptors
import { JwtInterceptor } from './core/interceptors/jwt.interceptor';
import { ErrorInterceptor } from './core/interceptors/error.interceptor';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    CoreModule,
    SharedModule,
    
    // Ant Design Modules
    NzLayoutModule,
    NzMenuModule,
    NzBreadCrumbModule,
    NzIconModule,
    NzTableModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzCardModule,
  NzStatisticModule,
    NzGridModule,
    NzSelectModule,
    NzDatePickerModule,
    NzModalModule,
    NzDrawerModule,
    NzTabsModule,
    NzAlertModule,
    NzPaginationModule,
    NzBadgeModule,
    NzTagModule,
    NzSpinModule,
    NzMessageModule,
    NzNotificationModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
```

---

## ?? **STEP 9: CUSTOMIZE THEME**

### **src/theme.less**

```less
// Ant Design Theme Customization

// Primary Colors
@primary-color: #1890ff;
@success-color: #52c41a;
@warning-color: #faad14;
@error-color: #ff4d4f;
@info-color: #1890ff;

// Background & Text
@body-background: #f5f5f5;
@component-background: #fff;
@text-color: rgba(0, 0, 0, 0.85);
@text-color-secondary: rgba(0, 0, 0, 0.65);

// Border
@border-radius-base: 2px;
@border-color-base: #d9d9d9;

// Typography
@font-size-base: 14px;
@line-height-base: 1.5715;
@font-family-base: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;

// Spacing
@margin-xs: 8px;
@margin-sm: 12px;
@margin-md: 16px;
@margin-lg: 24px;
@margin-xl: 32px;

// Healthcare Specific
@healthcare-primary: #0066cc;
@healthcare-success: #00a854;
@healthcare-warning: #ff7a45;
@healthcare-danger: #ff4d4f;

// Shadows
@shadow-1-up: 0 -2px 8px 0 rgba(0, 0, 0, 0.06);
@shadow-1-down: 0 2px 8px 0 rgba(0, 0, 0, 0.06);
@shadow-1-left: -2px 0 8px 0 rgba(0, 0, 0, 0.06);
@shadow-1-right: 2px 0 8px 0 rgba(0, 0, 0, 0.06);
```

---

## ? **STEP 10: INSTALL ADDITIONAL DEPENDENCIES**

### **Optional but Recommended**

```bash
# Chart library for reports
npm install chart.js ng2-charts

# Date utilities
npm install moment ngx-moment

# HTTP utilities
npm install axios

# Reactive programming
npm install rxjs

# Data formatting
npm install lodash-es

# Excel export
npm install xlsx

# PDF export
npm install pdfmake
```

### **Updated package.json (Dependencies)**

```json
{
  "dependencies": {
    "@angular/animations": "^18.0.0",
    "@angular/common": "^18.0.0",
    "@angular/compiler": "^18.0.0",
  "@angular/core": "^18.0.0",
    "@angular/forms": "^18.0.0",
    "@angular/platform-browser": "^18.0.0",
    "@angular/platform-browser-dynamic": "^18.0.0",
    "@angular/router": "^18.0.0",
    "@ant-design/icons-angular": "^18.0.0",
    "@ng-bootstrap/ng-bootstrap": "^14.0.0",
    "ng-zorro-antd": "^18.0.0",
    "chart.js": "^4.4.0",
    "ng2-charts": "^4.1.0",
    "moment": "^2.29.4",
    "ngx-moment": "^6.0.2",
    "lodash-es": "^4.17.21",
    "rxjs": "^7.8.1",
    "tslib": "^2.6.2",
    "zone.js": "^0.14.2"
  },
  "devDependencies": {
    "@angular-devkit/build-angular": "^18.0.0",
    "@angular/cli": "^18.0.0",
    "@angular/compiler-cli": "^18.0.0",
    "@types/jasmine": "~5.1.0",
    "@types/lodash-es": "^4.17.12",
    "jasmine-core": "~5.1.0",
    "karma": "~6.4.0",
    "karma-chrome-launcher": "~3.2.0",
    "karma-coverage": "~2.2.0",
    "karma-jasmine": "~5.1.0",
    "karma-jasmine-html-reporter": "~2.1.0",
    "typescript": "~5.2.2"
  }
}
```

---

## ?? **STEP 11: RUN DEVELOPMENT SERVER**

### **Start Server**

```bash
ng serve --open
```

### **Expected Output**

```
? Compiled successfully.

Application bundle generated successfully. 15.23 seconds

Initial chunk sizes:
 bundle.js       5.32 MB
 polyfills.js  849.12 kB
 styles.css         248.34 kB

? Build succeeded. The application is now running.

Watch mode enabled. Watching for file changes.

?  Local:   http://localhost:4200/
```

### **Browser Opens**

You should see:
- ? "Welcome to rcm-portal-antd!" message
- ? Ant Design styling applied
- ? No errors in console

---

## ?? **STEP 12: VERIFY ENVIRONMENT**

### **Create test-environment component**

```bash
ng generate component test-environment
```

### **test-environment.component.ts**

```typescript
import { Component, OnInit } from '@angular/core';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-test-environment',
  templateUrl: './test-environment.component.html',
  styleUrls: ['./test-environment.component.less']
})
export class TestEnvironmentComponent implements OnInit {
  environment = environment;
  
  systemInfo = {
    nodeVersion: process.version,
    platform: navigator.platform,
 userAgent: navigator.userAgent,
    timestamp: new Date()
  };

ngOnInit(): void {
  console.log('Environment:', environment);
    console.log('System Info:', this.systemInfo);
  }
}
```

### **test-environment.component.html**

```html
<nz-card nzTitle="Environment Information">
  <div nz-row [nzGutter]="16">
    <div nz-col [nzSpan]="12">
      <h3>Application Environment</h3>
      <p><strong>API URL:</strong> {{ environment.apiUrl }}</p>
      <p><strong>Production:</strong> {{ environment.production }}</p>
      <p><strong>Version:</strong> {{ environment.appVersion }}</p>
    </div>
    <div nz-col [nzSpan]="12">
      <h3>System Information</h3>
      <p><strong>Platform:</strong> {{ systemInfo.platform }}</p>
      <p><strong>Timestamp:</strong> {{ systemInfo.timestamp | date:'medium' }}</p>
    </div>
  </div>
</nz-card>
```

---

## ?? **STEP 13: SETUP VERSION CONTROL**

### **Initialize Git**

```bash
git init
git remote add origin https://github.com/yourusername/rcm-portal-antd.git
```

### **Create .gitignore**

```
# Dependencies
node_modules/
package-lock.json
yarn.lock

# Build
dist/
build/
out/

# IDE
.vscode/
.idea/
*.swp
*.swo
*~
.DS_Store

# Angular
.angular/
.angular-cli.json

# Environment
src/environments/environment.prod.ts (optional - keep local)

# Logs
*.log
npm-debug.log*

# Misc
.env
.env.local
```

### **Initial Commit**

```bash
git add .
git commit -m "feat: Initial Angular RCM Portal setup with Ant Design (ng-zorro)"
git branch -M main
git push -u origin main
```

---

## ?? **STEP 14: BUILD CONFIGURATION**

### **angular.json - Development Configuration**

```json
{
  "projects": {
    "rcm-portal-antd": {
  "architect": {
        "build": {
          "options": {
            "optimization": true,
            "outputHashing": "all",
    "sourceMap": false,
         "namedChunks": false,
     "aot": true,
  "extractLicenses": true,
    "vendorChunk": false,
       "buildOptimizer": true
          }
 },
        "serve": {
          "options": {
   "browserTarget": "rcm-portal-antd:build",
            "port": 4200,
        "open": true
      },
          "configurations": {
   "production": {
    "browserTarget": "rcm-portal-antd:build:production"
 }
          }
        }
      }
    }
  }
}
```

---

## ? **QUICK COMMANDS REFERENCE**

```bash
# Development
ng serve# Start dev server
ng serve --open        # Start with auto-open browser
ng serve --port 3000     # Use different port

# Building
ng build      # Development build
ng build --configuration production  # Production build

# Components
ng generate component <name>   # Generate component
ng generate module <name>  # Generate module
ng generate service <name>     # Generate service
ng generate guard <name>     # Generate guard

# Testing
ng test       # Run unit tests
ng test --code-coverage    # Generate coverage report
ng e2e         # Run E2E tests

# Linting
ng lint      # Run linter

# Help
ng help         # Show all commands
```

---

## ? **VERIFICATION CHECKLIST**

- [ ] Node.js 18+ installed
- [ ] npm 9+ installed
- [ ] Angular CLI 18+ installed
- [ ] Angular project created
- [ ] ng-zorro-antd installed
- [ ] Development server running
- [ ] Browser opens at localhost:4200
- [ ] No errors in browser console
- [ ] Ant Design styling visible
- [ ] Project structure created
- [ ] Environment files configured
- [ ] App module configured
- [ ] Theme customization file created
- [ ] Additional dependencies installed
- [ ] Git initialized and committed
- [ ] Build configuration verified

---

## ?? **SETUP COMPLETE!**

Your Angular RCM Portal frontend environment is now fully configured!

### **What You Have**

? **Angular 18 + Ant Design (ng-zorro)**  
? **Complete project structure**  
? **Development server running**  
? **Environment configuration**  
? **Theme customization setup**  
? **Build configuration ready**  
? **Version control initialized**  

### **Next Steps**

1. ? Create core services (API, Auth, Claims)
2. ? Build shared components (Navbar, Sidebar)
3. ? Implement features (Dashboard, Claims, Eligibility)
4. ? Add authentication
5. ? Connect to .NET 9 backend API

---

**Status**: ? **FRONTEND ENVIRONMENT SETUP COMPLETE**

**Time Taken**: 30-45 minutes  
**Next Phase**: Create Core Services & Components  

?? **Ready to build amazing features!** ??

