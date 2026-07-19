# ?? **ANGULAR RCM PORTAL - QUICK SETUP COMMANDS**

## ? **COPY-PASTE READY COMMANDS FOR WINDOWS POWERSHELL**

---

## ?? **COMPLETE SETUP - ONE SCRIPT**

Copy and paste this entire block into PowerShell:

```powershell
# ===== STEP 1: Create Angular Project =====
Write-Host "Creating Angular Project..." -ForegroundColor Green
ng new rcm-portal-antd --routing --style=less --package-manager=npm

# ===== STEP 2: Navigate to Project =====
Write-Host "Navigating to project..." -ForegroundColor Green
cd rcm-portal-antd

# ===== STEP 3: Add ng-zorro (Ant Design) =====
Write-Host "Adding Ant Design (ng-zorro)..." -ForegroundColor Green
ng add ng-zorro-antd

# ===== STEP 4: Install Additional Dependencies =====
Write-Host "Installing additional dependencies..." -ForegroundColor Green
npm install chart.js ng2-charts moment ngx-moment lodash-es axios

# ===== STEP 5: Create Project Structure =====
Write-Host "Creating project structure..." -ForegroundColor Green

# Create directories
New-Item -ItemType Directory -Force -Path "src\app\core\services"
New-Item -ItemType Directory -Force -Path "src\app\core\interceptors"
New-Item -ItemType Directory -Force -Path "src\app\core\guards"
New-Item -ItemType Directory -Force -Path "src\app\core\models"
New-Item -ItemType Directory -Force -Path "src\app\shared\components"
New-Item -ItemType Directory -Force -Path "src\app\shared\pipes"
New-Item -ItemType Directory -Force -Path "src\app\shared\directives"
New-Item -ItemType Directory -Force -Path "src\app\features\dashboard\components"
New-Item -ItemType Directory -Force -Path "src\app\features\claims\components"
New-Item -ItemType Directory -Force -Path "src\app\features\eligibility\components"
New-Item -ItemType Directory -Force -Path "src\app\features\pre-auth\components"
New-Item -ItemType Directory -Force -Path "src\app\features\reports\components"
New-Item -ItemType Directory -Force -Path "src\app\features\settings\components"
New-Item -ItemType Directory -Force -Path "src\app\auth\components"

# ===== STEP 6: Generate Modules Using Angular CLI =====
Write-Host "Generating modules..." -ForegroundColor Green

# Core modules
ng generate module core --routing
ng generate module core/services
ng generate module core/interceptors
ng generate module core/guards

# Shared module
ng generate module shared

# Feature modules
ng generate module features/dashboard --routing
ng generate module features/claims --routing
ng generate module features/eligibility --routing
ng generate module features/pre-auth --routing
ng generate module features/reports --routing
ng generate module features/settings --routing

# Auth module
ng generate module auth --routing

# ===== STEP 7: Initialize Git =====
Write-Host "Initializing Git repository..." -ForegroundColor Green
git init
git add .
git commit -m "feat: Initial Angular RCM Portal setup with Ant Design (ng-zorro)"

# ===== STEP 8: Start Development Server =====
Write-Host "Starting development server..." -ForegroundColor Green
Write-Host "Open browser at http://localhost:4200" -ForegroundColor Cyan
ng serve --open
```

---

## ?? **STEP-BY-STEP EXECUTION**

### **Step 1: Install Angular CLI (If Not Installed)**

```powershell
npm install -g @angular/cli@18

# Verify installation
ng version
```

### **Step 2: Create New Project**

```powershell
ng new rcm-portal-antd --routing --style=less --package-manager=npm

# Navigate to project
cd rcm-portal-antd
```

### **Step 3: Add Ant Design**

```powershell
# This single command configures everything!
ng add ng-zorro-antd

# Just press Enter for all defaults
```

### **Step 4: Install Dependencies**

```powershell
npm install chart.js ng2-charts moment ngx-moment lodash-es axios
```

### **Step 5: Create Project Structure (Directories)**

```powershell
# Core directory structure
New-Item -ItemType Directory -Force -Path "src\app\core\services"
New-Item -ItemType Directory -Force -Path "src\app\core\interceptors"
New-Item -ItemType Directory -Force -Path "src\app\core\guards"
New-Item -ItemType Directory -Force -Path "src\app\core\models"

# Shared directory structure
New-Item -ItemType Directory -Force -Path "src\app\shared\components"
New-Item -ItemType Directory -Force -Path "src\app\shared\pipes"
New-Item -ItemType Directory -Force -Path "src\app\shared\directives"

# Features directory structure
New-Item -ItemType Directory -Force -Path "src\app\features\dashboard\components"
New-Item -ItemType Directory -Force -Path "src\app\features\claims\components"
New-Item -ItemType Directory -Force -Path "src\app\features\eligibility\components"
New-Item -ItemType Directory -Force -Path "src\app\features\pre-auth\components"
New-Item -ItemType Directory -Force -Path "src\app\features\reports\components"
New-Item -ItemType Directory -Force -Path "src\app\features\settings\components"

# Auth directory structure
New-Item -ItemType Directory -Force -Path "src\app\auth\components"
```

### **Step 6: Create Modules**

```powershell
# Core modules
ng generate module core --routing
ng generate module core/services
ng generate module core/interceptors
ng generate module core/guards

# Shared module
ng generate module shared

# Feature modules
ng generate module features/dashboard --routing
ng generate module features/claims --routing
ng generate module features/eligibility --routing
ng generate module features/pre-auth --routing
ng generate module features/reports --routing
ng generate module features/settings --routing

# Auth module
ng generate module auth --routing
```

### **Step 7: Generate Components (Optional - Next Phase)**

```powershell
# Dashboard components
ng generate component features/dashboard/dashboard

# Claims components
ng generate component features/claims/components/claim-list
ng generate component features/claims/components/claim-detail
ng generate component features/claims/components/claim-submit

# Eligibility components
ng generate component features/eligibility/components/eligibility-check
ng generate component features/eligibility/components/eligibility-result

# Pre-auth components
ng generate component features/pre-auth/components/pre-auth-list
ng generate component features/pre-auth/components/pre-auth-submit

# Reports components
ng generate component features/reports/components/reports-dashboard

# Auth components
ng generate component auth/components/login
ng generate component auth/components/logout

# Shared components
ng generate component shared/components/navbar
ng generate component shared/components/sidebar
ng generate component shared/components/footer
```

### **Step 8: Generate Services**

```powershell
# Core services
ng generate service core/services/api
ng generate service core/services/auth
ng generate service core/services/claims
ng generate service core/services/eligibility
ng generate service core/services/preauth
ng generate service core/services/reports

# Interceptors
ng generate service core/interceptors/jwt
ng generate service core/interceptors/error
ng generate service core/interceptors/http

# Guards
ng generate guard core/guards/auth
```

### **Step 9: Initialize Git**

```powershell
# Initialize git repository
git init

# Add all files
git add .

# Initial commit
git commit -m "feat: Initial Angular RCM Portal setup with Ant Design (ng-zorro)"

# Set branch name
git branch -M main

# Add remote (replace with your repo)
git remote add origin https://github.com/yourusername/rcm-portal-antd.git

# Push to GitHub
git push -u origin main
```

### **Step 10: Start Development Server**

```powershell
# Start with auto-open browser
ng serve --open

# Or with specific port
ng serve --port 4200 --open

# With polling (if changes not detected)
ng serve --poll 2000 --open
```

---

## ?? **VERIFY INSTALLATION**

### **Check Node Version**

```powershell
node --version
# Expected: v18.0.0 or higher
```

### **Check npm Version**

```powershell
npm --version
# Expected: 9.0.0 or higher
```

### **Check Angular CLI Version**

```powershell
ng version
# Expected: Angular CLI version 18.0.0 or higher
```

### **Check Installed Packages**

```powershell
npm ls ng-zorro-antd
npm ls @angular/core
npm ls chart.js
```

### **Check Project Structure**

```powershell
# List all files
ls -R src/app

# Or use tree command
tree src/app /L 3
```

---

## ?? **SUPER FAST SETUP (5 MIN)**

```powershell
# Copy-paste this entire block and hit Enter

# 1. Create project (1 min)
ng new rcm-portal-antd --routing --style=less --package-manager=npm; cd rcm-portal-antd

# 2. Add Ant Design (2 min)
ng add ng-zorro-antd

# 3. Install deps (1 min)
npm install chart.js ng2-charts moment ngx-moment lodash-es axios

# 4. Start server (1 min)
ng serve --open
```

**Result**: Application running at http://localhost:4200 in 5 minutes!

---

## ?? **TROUBLESHOOTING**

### **Issue: `ng` command not found**

**Solution:**
```powershell
npm install -g @angular/cli@18
```

### **Issue: ng add ng-zorro-antd fails**

**Solution:**
```powershell
# Clear npm cache
npm cache clean --force

# Try again
ng add ng-zorro-antd --force
```

### **Issue: Port 4200 already in use**

**Solution:**
```powershell
# Use different port
ng serve --port 3000 --open
```

### **Issue: Changes not detected in browser**

**Solution:**
```powershell
# Run with polling
ng serve --poll 2000
```

### **Issue: Module errors after ng add**

**Solution:**
```powershell
# Delete node_modules and reinstall
rm -r node_modules
npm install
ng serve --open
```

---

## ?? **EXPECTED OUTPUT**

### **After `ng serve --open`**

```
? Compiled successfully.

Application bundle generated successfully. 15.23 seconds

Initial chunk sizes:
 bundle.js   5.32 MB
 polyfills.js  849.12 kB
 styles.css         248.34 kB

? Build succeeded. The application is now running.

Watch mode enabled. Watching for file changes.

?  Local:   http://localhost:4200/
```

### **Browser Window**

You should see:
- ? Welcome message
- ? Ant Design styling
- ? No errors in console
- ? Page auto-reloads on file save

---

## ? **FINAL VERIFICATION CHECKLIST**

- [ ] Node 18+ installed: `node --version`
- [ ] npm 9+ installed: `npm --version`
- [ ] Angular CLI 18+ installed: `ng version`
- [ ] Project created: `ng new rcm-portal-antd`
- [ ] Ant Design added: `ng add ng-zorro-antd`
- [ ] Dependencies installed: `npm install`
- [ ] Development server running: `ng serve`
- [ ] Browser opens at localhost:4200
- [ ] No errors in browser console
- [ ] Ant Design styling visible

---

## ?? **NEXT STEPS**

After setup, create:

1. **API Service** - Connect to .NET 9 backend
2. **Auth Service** - Handle authentication
3. **Core Components** - Navbar, Sidebar, Footer
4. **Dashboard** - Main dashboard with metrics
5. **Claims Module** - Claims management
6. **Eligibility Module** - Real-time checks
7. **Reports Module** - Reporting and analytics

---

## ?? **SAVE THESE COMMANDS**

Create a file named `setup-commands.ps1` in your project root and save all these commands for future reference.

---

**Status**: ? **SETUP GUIDE COMPLETE**

**Time**: ~30-45 minutes (First time setup)

?? **Ready to build your RCM portal!** ??

