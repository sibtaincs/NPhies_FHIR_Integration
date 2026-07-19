# ?? **PHASE 1: PROJECT FOUNDATION - COMPLETE SETUP GUIDE**

## ? **STEP-BY-STEP SETUP FOR RCM PORTAL ANGULAR PROJECT**

This is your **hands-on guide** for Phase 1 - getting your Angular 18 + Ant Design project up and running.

---

## ?? **PHASE 1 CHECKLIST**

- [ ] Verify Node.js version (v18.x or v20.x)
- [ ] Install Angular CLI globally
- [ ] Create new Angular project
- [ ] Navigate to project directory
- [ ] Add Ant Design
- [ ] Install additional dependencies
- [ ] Setup folder structure
- [ ] Create project modules
- [ ] Configure environment files
- [ ] Create theme configuration
- [ ] Make first commit
- [ ] Start dev server
- [ ] Verify in browser

---

## ?? **STEP 1: VERIFY YOUR ENVIRONMENT**

### **Check Node.js Version**

Your system has:
```
? Node: v20.19.6
? npm: 10.8.2
? NVM: 1.2.2
? Available Node versions: v20, v18, v14
```

### **Switch to Node 18 (if needed)**

```bash
# Switch to Node 18
nvm use 18

# Verify
node --version  # Should show v18.x.x
npm --version   # Should show 9.x.x
```

### **For This Project - Use Node 20**

```bash
# Keep Node 20 (already using it)
node --version  # v20.19.6
npm --version   # 10.8.2
```

---

## ? **STEP 2: ANGULAR CLI IS INSTALLED**

You already have:
```
? Angular CLI: 18.2.21
? @angular/core: Ready to use
? ng command: Available
```

**Verify it's working:**
```bash
ng version
# Should show Angular CLI 18.2.21
```

---

## ?? **STEP 3: CREATE ANGULAR PROJECT**

### **Option 1: Using Terminal/PowerShell (Recommended)**

Open PowerShell and run:

```powershell
# Navigate to your repos directory
cd C:\Users\sibtain.alimadad\source\repos

# Create the Angular project
ng new rcm-portal-antd `
  --routing `
  --style=less `
  --package-manager=npm `
  --skip-git

# This will take 3-5 minutes
# It will prompt for routing and styling options
```

### **What Each Flag Does**

| Flag | Purpose |
|------|---------|
| `--routing` | Adds routing module for navigation |
| `--style=less` | Uses LESS instead of CSS (better for theming) |
| `--package-manager=npm` | Use npm (not yarn or pnpm) |
| `--skip-git` | Don't initialize git (you already have repo) |

### **Option 2: Interactive Setup**

If you want an interactive prompt:

```bash
ng new rcm-portal-antd
# Answer the prompts:
# - Routing: Yes (y)
# - Stylesheet: LESS (l)
# - Package manager: npm
```

### **What Gets Created**

```
rcm-portal-antd/
??? src/
?   ??? app/
?   ?   ??? app.component.ts
?   ?   ??? app.component.html
?   ?   ??? app.component.less
?   ?   ??? app-routing.module.ts
?   ??? assets/
?   ??? environments/
?   ??? styles.less
?   ??? main.ts
??? angular.json
??? package.json
??? tsconfig.json
??? ... (other config files)
```

---

## ?? **STEP 4: NAVIGATE TO PROJECT**

Once project is created:

```bash
# Enter project directory
cd rcm-portal-antd

# Verify you're in the right place
ls -la
# Should show: angular.json, package.json, src/, etc.
```

---

## ?? **STEP 5: ADD ANT DESIGN**

### **Install Ant Design for Angular**

```bash
# While in rcm-portal-antd directory
ng add ng-zorro-antd

# This will:
# - Install ng-zorro-antd package
# - Add Ant Design styles to angular.json
# - Create theme.less file
# - Update app.module.ts
```

This takes about 2-3 minutes.

### **Expected Output**

```
? Packages installed successfully.
? Add ng-zorro-antd theme to styles.
? Updated angular.json
```

---

## ?? **STEP 6: INSTALL ADDITIONAL DEPENDENCIES**

All at once:

```bash
npm install chart.js ng2-charts moment ngx-moment lodash-es
```

Or one by one if you prefer:

```bash
npm install chart.js      # For charts
npm install ng2-charts    # Angular wrapper for charts
npm install moment # Date/time handling
npm install ngx-moment    # Angular wrapper for moment
npm install lodash-es   # Utility library
```

### **What These Are For**

| Package | Purpose |
|---------|---------|
| `chart.js` | Chart library |
| `ng2-charts` | Angular integration for charts |
| `moment` | Date/time manipulation |
| `ngx-moment` | Angular pipes for moment |
| `lodash-es` | Utility functions (ES6 modules) |

---

## ??? **STEP 7: CREATE FOLDER STRUCTURE**

Once everything is installed, create the recommended folder structure:

### **Using PowerShell**

```powershell
# Create all folders in one go
mkdir -p src/app/core/services
mkdir -p src/app/core/interceptors
mkdir -p src/app/core/guards
mkdir -p src/app/core/models
mkdir -p src/app/core/constants

mkdir -p src/app/shared/components/navbar
mkdir -p src/app/shared/components/sidebar
mkdir -p src/app/shared/components/footer
mkdir -p src/app/shared/pipes
mkdir -p src/app/shared/directives

mkdir -p src/app/features/dashboard
mkdir -p src/app/features/claims/components
mkdir -p src/app/features/claims/services
mkdir -p src/app/features/eligibility/components
mkdir -p src/app/features/eligibility/services
mkdir -p src/app/features/pre-auth/components
mkdir -p src/app/features/pre-auth/services
mkdir -p src/app/features/reports/components
mkdir -p src/app/features/reports/services

mkdir -p src/app/auth/components
mkdir -p src/app/auth/services

mkdir -p src/environments
mkdir -p src/assets/images
mkdir -p src/assets/styles
```

### **Folder Structure Explanation**

```
src/app/
??? core/        # Core services & logic
?   ??? services/ # API, Auth, etc.
?   ??? interceptors/       # HTTP interceptors
?   ??? guards/         # Route guards
?   ??? models/     # TypeScript interfaces
?   ??? constants/      # App constants
?
??? shared/             # Shared across modules
?   ??? components/     # Reusable components
?   ??? pipes/        # Custom pipes
?   ??? directives/         # Custom directives
?
??? features/           # Feature modules
?   ??? dashboard/   # Dashboard module
?   ??? claims/       # Claims module
?   ??? eligibility/        # Eligibility module
?   ??? pre-auth/     # Pre-auth module
?   ??? reports/     # Reports module
?
??? auth/       # Authentication module
    ??? components/         # Login/logout
    ??? services/      # Auth service
```

---

## ?? **STEP 8: CREATE CORE MODULES**

Generate Angular modules:

```bash
# Generate feature modules
ng generate module core --routing
ng generate module shared
ng generate module features/dashboard --routing
ng generate module features/claims --routing
ng generate module features/eligibility --routing
ng generate module features/pre-auth --routing
ng generate module features/reports --routing
ng generate module auth --routing

# Generate initial components
ng generate component shared/components/navbar --skip-tests
ng generate component shared/components/sidebar --skip-tests
ng generate component shared/components/footer --skip-tests
ng generate component auth/components/login --skip-tests
ng generate component auth/components/logout --skip-tests
ng generate component features/dashboard/dashboard --skip-tests
```

### **What These Commands Do**

- `ng generate module` - Creates an Angular module
- `--routing` - Adds routing configuration
- `ng generate component` - Creates a component
- `--skip-tests` - Skip test file generation (for now)

---

## ?? **STEP 9: CONFIGURE ENVIRONMENT FILES**

### **Update `src/environments/environment.ts`**

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api',
  apiTimeout: 30000,
  
  endpoints: {
    auth: {
      login: '/auth/login',
      logout: '/auth/logout',
    refresh: '/auth/refresh',
      me: '/auth/me'
    },
    claims: {
      list: '/claims',
 create: '/claims',
      getById: '/claims',
      update: '/claims',
      details: '/claims',
      byPatient: '/claims/patient',
      byStatus: '/claims/status'
    },
    eligibility: {
      requests: '/v1/eligibility/requests',
      check: '/v1/eligibility/check',
      responses: '/v1/eligibility/responses',
      pending: '/v1/eligibility/requests/pending'
    }
  },
  
  app: {
    name: 'RCM Portal',
    version: '1.0.0',
    tokenKey: 'rcm_token',
    userKey: 'rcm_user'
  }
};
```

### **Create `src/environments/environment.prod.ts`**

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://api.yourdomain.com/api',
  apiTimeout: 30000,
  
  endpoints: {
    // Same structure as development
    auth: { /* ... */ },
    claims: { /* ... */ },
    eligibility: { /* ... */ }
  },
  
  app: {
    name: 'RCM Portal',
  version: '1.0.0',
    tokenKey: 'rcm_token',
    userKey: 'rcm_user'
  }
};
```

---

## ?? **STEP 10: THEME CONFIGURATION**

### **Update `src/theme.less`**

```less
// Ant Design Color Customization for RCM Healthcare

// Primary Colors
@primary-color: #0066cc;     // Medical blue
@success-color: #00a854;     // Approved green
@warning-color: #faad14;     // Pending yellow
@error-color: #ff4d4f;  // Denied red
@info-color: #1890ff;     // Info blue

// RCM Specific Colors
@claims-approved: #00a854;
@claims-pending: #faad14;
@claims-denied: #ff4d4f;
@claims-submitted: #1890ff;

// Text Colors
@text-color: rgba(0, 0, 0, 0.85);
@text-color-secondary: rgba(0, 0, 0, 0.65);

// Border
@border-radius-base: 2px;
@border-color-base: #d9d9d9;

// Fonts
@font-family-base: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
@font-size-base: 14px;

// Breakpoints
@screen-xs: 480px;
@screen-sm: 576px;
@screen-md: 768px;
@screen-lg: 992px;
@screen-xl: 1200px;
@screen-xxl: 1600px;
```

### **Update `src/styles.less`**

```less
// Import theme variables
@import './theme.less';

// Global styles
* {
  box-sizing: border-box;
}

body {
  margin: 0;
  padding: 0;
  font-family: @font-family-base;
  font-size: @font-size-base;
  color: @text-color;
  background-color: #fafafa;
}

// Utility classes
.mb-8 {
  margin-bottom: 8px;
}

.mb-16 {
  margin-bottom: 16px;
}

.mb-24 {
  margin-bottom: 24px;
}

.ml-8 {
  margin-left: 8px;
}

.ml-16 {
  margin-left: 16px;
}

.mt-16 {
  margin-top: 16px;
}

.text-center {
  text-align: center;
}

.text-right {
  text-align: right;
}

// Scrollbar styling
::-webkit-scrollbar {
  width: 8px;
  height: 8px;
}

::-webkit-scrollbar-track {
  background: #f1f1f1;
}

::-webkit-scrollbar-thumb {
  background: #888;
  border-radius: 4px;
}

::-webkit-scrollbar-thumb:hover {
  background: #555;
}
```

---

## ?? **STEP 11: UPDATE angular.json FOR THEME**

Make sure your `angular.json` includes the LESS file. It should look like:

```json
{
  "projects": {
    "rcm-portal-antd": {
      "architect": {
        "build": {
          "options": {
            "styles": [
     "src/theme.less",
         "src/styles.less"
]
       }
    }
      }
    }
  }
}
```

---

## ?? **STEP 12: FIRST GIT COMMIT**

Once everything is set up:

```bash
# Check git status
git status

# Add all files
git add .

# Commit
git commit -m "feat: Phase 1 - Project foundation with Angular 18 + Ant Design"

# Push to repository
git push origin main
```

---

## ?? **STEP 13: START DEVELOPMENT SERVER**

```bash
# Start the dev server
ng serve

# Or with open browser
ng serve --open

# Expected output:
# ? Compiled successfully.
# ? Application bundle generated successfully. [4.2 MB]
# Local:     http://localhost:4200/
# Use Ctrl+C to stop the server
```

---

## ?? **STEP 14: VERIFY IN BROWSER**

Navigate to: **http://localhost:4200**

You should see:
- ? Angular welcome page
- ? No errors in console (F12)
- ? Development server running

---

## ?? **PROJECT STRUCTURE AFTER PHASE 1**

```
rcm-portal-antd/
??? src/
?   ??? app/
?   ?   ??? core/
?   ?   ?   ??? services/
?   ?   ?   ??? interceptors/
?   ?   ?   ??? guards/
?   ?   ?   ??? models/
?   ?   ?   ??? constants/
?   ?   ??? shared/
?   ?   ?   ??? components/
?   ?   ?   ?   ??? navbar/
?   ?   ?   ?   ??? sidebar/
?   ?   ?   ?   ??? footer/
?   ?   ?   ??? pipes/
?   ?   ?   ??? directives/
?   ?   ??? features/
?   ?   ?   ??? dashboard/
?   ?   ?   ??? claims/
?   ?   ?   ??? eligibility/
?   ?   ?   ??? pre-auth/
?   ?   ?   ??? reports/
?   ?   ??? auth/
?   ?   ?   ??? components/
?   ?   ? ??? services/
?   ?   ??? app.component.ts
?   ?   ??? app-routing.module.ts
?   ??? assets/
?   ??? environments/
?   ?   ??? environment.ts
?   ?   ??? environment.prod.ts
?   ??? theme.less
?   ??? styles.less
?   ??? main.ts
??? angular.json
??? package.json
??? tsconfig.json
??? ... (config files)
```

---

## ? **PHASE 1 COMPLETION CHECKLIST**

- [ ] Node.js v18+ or v20 verified
- [ ] Angular CLI 18 installed globally
- [ ] Angular project created (`rcm-portal-antd`)
- [ ] Navigated to project directory
- [ ] Ant Design added with `ng add ng-zorro-antd`
- [ ] Additional dependencies installed (chart.js, moment, lodash-es)
- [ ] Folder structure created
- [ ] Modules generated (core, shared, auth, features)
- [ ] Environment files configured
- [ ] Theme configuration created
- [ ] Global styles added
- [ ] First commit made
- [ ] Dev server started with `ng serve`
- [ ] Browser shows Angular app (no errors)

---

## ?? **WHAT'S NEXT (Phase 2)**

Once Phase 1 is complete:

1. ? You'll move to **Phase 2: Architecture & Core Setup**
2. ? Create TypeScript models/interfaces
3. ? Build API service layer
4. ? Implement authentication service
5. ? Setup JWT interceptor
6. ? Setup error handling

---

## ?? **PHASE 1 SUMMARY**

| Task | Status | Time |
|------|--------|------|
| Environment Setup | ? Done | 5 min |
| Angular CLI Install | ? Done | 2 min |
| Create Project | ? To Do | 5 min |
| Add Ant Design | ? To Do | 3 min |
| Install Dependencies | ? To Do | 2 min |
| Folder Structure | ? To Do | 2 min |
| Generate Modules | ? To Do | 5 min |
| Configure Environments | ? To Do | 3 min |
| Theme Setup | ? To Do | 3 min |
| Git Commit | ? To Do | 2 min |
| Start Dev Server | ? To Do | 2 min |
| **TOTAL** | ? | **38 min** |

---

## ?? **LET'S START!**

### **Now:**

1. Open PowerShell
2. Navigate to: `C:\Users\sibtain.alimadad\source\repos`
3. Run: `ng new rcm-portal-antd --routing --style=less --package-manager=npm --skip-git`
4. Follow the prompts
5. Wait for installation (3-5 minutes)

### **Then:**

6. `cd rcm-portal-antd`
7. `ng add ng-zorro-antd`
8. `npm install chart.js ng2-charts moment ngx-moment lodash-es`
9. Follow Steps 7-14 above for folder structure, modules, config, etc.

### **Finally:**

10. `ng serve --open`
11. See your app at http://localhost:4200

---

## ?? **TIPS**

? **Internet Required**: npm install downloads packages
? **Disk Space**: ~1-2 GB free (node_modules is large)
? **Time**: Total ~45 minutes first time
? **Terminal**: Keep it open, don't close during installation
? **Errors**: If error occurs, can usually retry with `npm install`

---

**Status**: ? READY TO EXECUTE  
**Next**: Run `ng new rcm-portal-antd...` command

?? **Phase 1 starts NOW!** ??

