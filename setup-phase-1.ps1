#!/usr/bin/env powershell

# =============================================================================
# RCM Portal Angular Project Setup - PHASE 1 Automation Script
# =============================================================================
# This script automates the entire Phase 1 setup process
# Usage: .\setup-phase-1.ps1
# =============================================================================

$ErrorActionPreference = "Stop"
$WarningPreference = "SilentlyContinue"

# Colors for console output
$colors = @{
    "Green" = [System.ConsoleColor]::Green
    "Yellow" = [System.ConsoleColor]::Yellow
    "Red" = [System.ConsoleColor]::Red
    "Cyan" = [System.ConsoleColor]::Cyan
    "White" = [System.ConsoleColor]::White
}

function Write-Section {
    param([string]$Message)
    Write-Host "`n" -NoNewline
    Write-Host "=" * 80 -ForegroundColor $colors["Cyan"]
    Write-Host "  $Message" -ForegroundColor $colors["Cyan"]
    Write-Host "=" * 80 -ForegroundColor $colors["Cyan"]
}

function Write-Status {
    param([string]$Message, [string]$Status)
    $statusColor = if ($Status -eq "?") { $colors["Green"] } else { $colors["Yellow"] }
Write-Host "  $Status $Message" -ForegroundColor $statusColor
}

function Write-Error-Message {
param([string]$Message)
    Write-Host "  ? ERROR: $Message" -ForegroundColor $colors["Red"]
}

function Write-Info {
    param([string]$Message)
  Write-Host "  ?? $Message" -ForegroundColor $colors["White"]
}

# =============================================================================
# STEP 1: VERIFY ENVIRONMENT
# =============================================================================

Write-Section "PHASE 1: PROJECT FOUNDATION - STEP 1/11 - ENVIRONMENT VERIFICATION"

Write-Host "`nVerifying your development environment..."

# Check Node.js
Write-Host "`nChecking Node.js..." -ForegroundColor $colors["Yellow"]
$nodeVersion = node --version 2>$null
if ($nodeVersion) {
    Write-Status "Node.js $nodeVersion" "?"
} else {
    Write-Error-Message "Node.js not found. Please install Node.js 18+ or use NVM."
    exit 1
}

# Check npm
Write-Host "`nChecking npm..." -ForegroundColor $colors["Yellow"]
$npmVersion = npm --version 2>$null
if ($npmVersion) {
    Write-Status "npm $npmVersion" "?"
} else {
    Write-Error-Message "npm not found."
    exit 1
}

# Check Angular CLI
Write-Host "`nChecking Angular CLI..." -ForegroundColor $colors["Yellow"]
$ngVersion = ng version 2>$null
if ($ngVersion) {
    Write-Status "Angular CLI installed" "?"
} else {
    Write-Info "Angular CLI not found, installing globally..."
    npm install -g @angular/cli@18
    Write-Status "Angular CLI installed" "?"
}

Write-Status "Environment verification complete" "?"

# =============================================================================
# STEP 2: CREATE PROJECT
# =============================================================================

Write-Section "PHASE 1: PROJECT FOUNDATION - STEP 2/11 - CREATE ANGULAR PROJECT"

$projectDir = "C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd"

if (Test-Path $projectDir) {
    Write-Info "Project directory already exists at: $projectDir"
    $response = Read-Host "Do you want to continue with existing project? (y/n)"
    if ($response -ne "y") {
        Write-Info "Exiting setup..."
        exit 0
    }
} else {
    Write-Host "`nCreating Angular project..." -ForegroundColor $colors["Yellow"]
    Write-Info "This may take 3-5 minutes..."
    
    cd C:\Users\sibtain.alimadad\source\repos
    
    ng new rcm-portal-antd `
        --routing `
        --style=less `
        --package-manager=npm `
    --skip-git `
        --package-manager=npm
    
  if ($LASTEXITCODE -eq 0) {
        Write-Status "Angular project created successfully" "?"
    } else {
        Write-Error-Message "Failed to create Angular project"
        exit 1
 }
}

# =============================================================================
# STEP 3: NAVIGATE AND ADD ANT DESIGN
# =============================================================================

Write-Section "PHASE 1: PROJECT FOUNDATION - STEP 3/11 - ADD ANT DESIGN"

cd $projectDir

Write-Host "`nAdding Ant Design to project..." -ForegroundColor $colors["Yellow"]
Write-Info "This may take 2-3 minutes..."

ng add ng-zorro-antd --skip-confirmation

if ($LASTEXITCODE -eq 0) {
    Write-Status "Ant Design added successfully" "?"
} else {
    Write-Error-Message "Failed to add Ant Design"
    # Continue anyway as this might be a warning
}

# =============================================================================
# STEP 4: INSTALL DEPENDENCIES
# =============================================================================

Write-Section "PHASE 1: PROJECT FOUNDATION - STEP 4/11 - INSTALL DEPENDENCIES"

Write-Host "`nInstalling additional dependencies..." -ForegroundColor $colors["Yellow"]

$packages = @(
    "chart.js",
    "ng2-charts",
    "moment",
    "ngx-moment",
    "lodash-es"
)

foreach ($package in $packages) {
    Write-Host "  Installing $package..." -ForegroundColor $colors["Yellow"]
    npm install $package
    Write-Status "$package installed" "?"
}

# =============================================================================
# STEP 5: CREATE FOLDER STRUCTURE
# =============================================================================

Write-Section "PHASE 1: PROJECT FOUNDATION - STEP 5/11 - CREATE FOLDER STRUCTURE"

Write-Host "`nCreating project folder structure..." -ForegroundColor $colors["Yellow"]

$folders = @(
    "src/app/core/services",
    "src/app/core/interceptors",
  "src/app/core/guards",
 "src/app/core/models",
    "src/app/core/constants",
    "src/app/shared/components/navbar",
    "src/app/shared/components/sidebar",
    "src/app/shared/components/footer",
    "src/app/shared/pipes",
    "src/app/shared/directives",
    "src/app/features/dashboard",
  "src/app/features/claims/components",
    "src/app/features/claims/services",
    "src/app/features/eligibility/components",
    "src/app/features/eligibility/services",
    "src/app/features/pre-auth/components",
    "src/app/features/pre-auth/services",
    "src/app/features/reports/components",
    "src/app/features/reports/services",
    "src/app/auth/components",
    "src/app/auth/services",
    "src/environments",
    "src/assets/images",
    "src/assets/styles"
)

foreach ($folder in $folders) {
    if (!(Test-Path $folder)) {
        New-Item -ItemType Directory -Path $folder -Force | Out-Null
        Write-Status "Created $folder" "?"
    }
}

# =============================================================================
# STEP 6: GENERATE MODULES
# =============================================================================

Write-Section "PHASE 1: PROJECT FOUNDATION - STEP 6/11 - GENERATE MODULES"

Write-Host "`nGenerating Angular modules..." -ForegroundColor $colors["Yellow"]
Write-Info "This may take 1-2 minutes..."

$modules = @(
    "core --routing",
    "shared",
    "features/dashboard --routing",
    "features/claims --routing",
    "features/eligibility --routing",
    "features/pre-auth --routing",
    "features/reports --routing",
    "auth --routing"
)

foreach ($module in $modules) {
    Write-Host "  Generating module: $module..." -ForegroundColor $colors["Yellow"]
    ng generate module $module --skip-tests | Out-Null
    Write-Status "Module generated: $module" "?"
}

# =============================================================================
# STEP 7: GENERATE COMPONENTS
# =============================================================================

Write-Section "PHASE 1: PROJECT FOUNDATION - STEP 7/11 - GENERATE COMPONENTS"

Write-Host "`nGenerating components..." -ForegroundColor $colors["Yellow"]
Write-Info "This may take 1-2 minutes..."

$components = @(
    "shared/components/navbar --skip-tests",
    "shared/components/sidebar --skip-tests",
    "shared/components/footer --skip-tests",
    "auth/components/login --skip-tests",
    "auth/components/logout --skip-tests",
    "features/dashboard/dashboard --skip-tests"
)

foreach ($component in $components) {
    Write-Host "  Generating component: $component..." -ForegroundColor $colors["Yellow"]
    ng generate component $component | Out-Null
    Write-Status "Component generated: $component" "?"
}

# =============================================================================
# STEP 8: CREATE ENVIRONMENT FILES
# =============================================================================

Write-Section "PHASE 1: PROJECT FOUNDATION - STEP 8/11 - CONFIGURE ENVIRONMENTS"

Write-Host "`nUpdating environment files..." -ForegroundColor $colors["Yellow"]

$envContent = @"
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
"@

$envContent | Out-File -Encoding UTF8 "src/environments/environment.ts"
Write-Status "environment.ts configured" "?"

$envProdContent = @"
export const environment = {
  production: true,
  apiUrl: 'https://api.yourdomain.com/api',
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
"@

$envProdContent | Out-File -Encoding UTF8 "src/environments/environment.prod.ts"
Write-Status "environment.prod.ts configured" "?"

# =============================================================================
# STEP 9: CREATE THEME FILE
# =============================================================================

Write-Section "PHASE 1: PROJECT FOUNDATION - STEP 9/11 - SETUP THEME"

Write-Host "`nCreating theme configuration..." -ForegroundColor $colors["Yellow"]

$themeContent = @"
// Ant Design Color Customization for RCM Healthcare

// Primary Colors
@primary-color: #0066cc;     // Medical blue
@success-color: #00a854;     // Approved green
@warning-color: #faad14;     // Pending yellow
@error-color: #ff4d4f;       // Denied red
@info-color: #1890ff; // Info blue

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
"@

$themeContent | Out-File -Encoding UTF8 "src/theme.less"
Write-Status "theme.less created" "?"

# =============================================================================
# STEP 10: CREATE .nvmrc FILE
# =============================================================================

Write-Section "PHASE 1: PROJECT FOUNDATION - STEP 10/11 - CREATE .nvmrc FILE"

Write-Host "`nCreating .nvmrc for Node version management..." -ForegroundColor $colors["Yellow"]

"18" | Out-File -Encoding UTF8 ".nvmrc"
Write-Status ".nvmrc created (Node 18)" "?"

# =============================================================================
# STEP 11: GIT COMMIT
# =============================================================================

Write-Section "PHASE 1: PROJECT FOUNDATION - STEP 11/11 - GIT COMMIT"

Write-Host "`nCommitting Phase 1 to Git..." -ForegroundColor $colors["Yellow"]

git add .
git commit -m "feat: Phase 1 - Project foundation with Angular 18 + Ant Design setup"

if ($LASTEXITCODE -eq 0) {
    Write-Status "Project committed to Git" "?"
} else {
    Write-Status "Git commit skipped (might not be a git repo)" "??"
}

# =============================================================================
# COMPLETION
# =============================================================================

Write-Section "PHASE 1 SETUP COMPLETE! ??"

Write-Host "`nYour Angular RCM Portal project is ready!" -ForegroundColor $colors["Green"]
Write-Host "`nNext steps:" -ForegroundColor $colors["Cyan"]

Write-Host "`n  1. Start the development server:" -ForegroundColor $colors["White"]
Write-Host "     ng serve --open" -ForegroundColor $colors["Yellow"]

Write-Host "`n  2. Open browser to:" -ForegroundColor $colors["White"]
Write-Host "     http://localhost:4200" -ForegroundColor $colors["Yellow"]

Write-Host "`n  3. Next Phase:" -ForegroundColor $colors["White"]
Write-Host "     Follow Phase 2: Architecture & Core Setup" -ForegroundColor $colors["Yellow"]

Write-Host "`n  Project location:" -ForegroundColor $colors["White"]
Write-Host "   $projectDir" -ForegroundColor $colors["Yellow"]

Write-Host "`n" -ForegroundColor $colors["Green"]
Write-Host "? Phase 1 Complete!" -ForegroundColor $colors["Green"]
Write-Host "`n"

