# ?? **ENVIRONMENT VERIFICATION CHECKLIST**

## ? **COMPLETE SYSTEM VERIFICATION GUIDE**

Before starting your RCM Portal development, verify your entire environment setup.

---

## ?? **VERIFICATION STEPS**

### **Step 1: Check Node Version Manager (NVM)**

```bash
# Verify NVM is installed
nvm --version

# Expected: 1.1.x or similar
```

**If error:**
```bash
# NVM not installed, follow NVM_SETUP_GUIDE.md
```

### **Step 2: List Installed Node Versions**

```bash
# Show all installed Node versions
nvm list
```

**Expected output:**
```
    16.20.2
    18.18.2
->  18.18.2 (Currently using 64-bit executable)
    default -> 18 (-> 18.18.2)
```

### **Step 3: Verify Current Node Version**

```bash
# Check active Node version
node --version

# Expected: v18.x.x (for new RCM portal)
```

### **Step 4: Verify npm Version**

```bash
# Check npm version
npm --version

# Expected: 9.x.x (for Node 18)
```

### **Step 5: Check Global Angular CLI**

```bash
# Check Angular CLI version
ng version

# Expected: Angular 18.x.x
# Or: @angular/cli: 18.x.x
```

### **Step 6: Verify All Global Packages**

```bash
# List all global packages
npm list -g --depth=0

# Should show:
# ??? @angular/cli@18.x.x
# ??? npm@9.x.x
# ??? other packages
```

### **Step 7: Check Git Installation**

```bash
# Verify Git is installed
git --version

# Expected: git version 2.x.x or higher
```

### **Step 8: Check Project Git Status**

```bash
# Navigate to project directory
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration

# Check git status
git status

# Expected: On branch main
```

---

## ?? **QUICK VERIFICATION SCRIPT**

### **Windows PowerShell - Save as `verify-setup.ps1`**

```powershell
# Verification Script for RCM Portal Setup
Write-Host "=== Environment Verification ===" -ForegroundColor Green
Write-Host ""

# 1. NVM
Write-Host "1. Checking NVM..." -ForegroundColor Yellow
$nvm = nvm --version
if ($nvm) {
    Write-Host "? NVM: $nvm" -ForegroundColor Green
} else {
  Write-Host "   ? NVM not installed" -ForegroundColor Red
}

# 2. Node
Write-Host "2. Checking Node..." -ForegroundColor Yellow
$node = node --version
if ($node) {
    Write-Host "   ? Node: $node" -ForegroundColor Green
} else {
    Write-Host "   ? Node not installed" -ForegroundColor Red
}

# 3. npm
Write-Host "3. Checking npm..." -ForegroundColor Yellow
$npm = npm --version
if ($npm) {
    Write-Host "   ? npm: $npm" -ForegroundColor Green
} else {
    Write-Host "   ? npm not installed" -ForegroundColor Red
}

# 4. Angular CLI
Write-Host "4. Checking Angular CLI..." -ForegroundColor Yellow
try {
  $ng = ng version --minimal
    Write-Host "   ? Angular CLI installed" -ForegroundColor Green
} catch {
    Write-Host "   ? Angular CLI not installed" -ForegroundColor Red
}

# 5. Git
Write-Host "5. Checking Git..." -ForegroundColor Yellow
$git = git --version
if ($git) {
    Write-Host "   ? Git: $git" -ForegroundColor Green
} else {
    Write-Host "   ? Git not installed" -ForegroundColor Red
}

Write-Host ""
Write-Host "=== Verification Complete ===" -ForegroundColor Green
```

**Run the script:**
```bash
.\verify-setup.ps1
```

### **Bash/Git Bash - Save as `verify-setup.sh`**

```bash
#!/bin/bash

echo "=== Environment Verification ==="
echo ""

# 1. NVM
echo "1. Checking NVM..."
if command -v nvm &> /dev/null; then
    echo "   ? NVM: $(nvm --version)"
else
    echo "   ? NVM not installed"
fi

# 2. Node
echo "2. Checking Node..."
if command -v node &> /dev/null; then
 echo "   ? Node: $(node --version)"
else
    echo "   ? Node not installed"
fi

# 3. npm
echo "3. Checking npm..."
if command -v npm &> /dev/null; then
    echo " ? npm: $(npm --version)"
else
    echo "   ? npm not installed"
fi

# 4. Angular CLI
echo "4. Checking Angular CLI..."
if command -v ng &> /dev/null; then
    echo "   ? Angular CLI installed"
else
echo "   ? Angular CLI not installed"
fi

# 5. Git
echo "5. Checking Git..."
if command -v git &> /dev/null; then
 echo "   ? Git: $(git --version)"
else
    echo "   ? Git not installed"
fi

echo ""
echo "=== Verification Complete ==="
```

**Run the script:**
```bash
chmod +x verify-setup.sh
./verify-setup.sh
```

---

## ? **COMPLETE VERIFICATION CHECKLIST**

### **Before Starting Development**

- [ ] NVM installed (`nvm --version`)
- [ ] Node 18.x installed (`nvm list`)
- [ ] Node 18.x active (`node --version` shows v18.x.x)
- [ ] npm 9.x installed (`npm --version` shows 9.x.x)
- [ ] Angular CLI 18 installed (`ng version` shows 18.x.x)
- [ ] Git installed (`git --version`)
- [ ] Repository cloned (`git status` shows main branch)
- [ ] .nvmrc file ready (for project auto-switching)

---

## ?? **FIXING COMMON VERIFICATION ISSUES**

### **Issue 1: NVM not found**

```bash
# Solution: Restart PowerShell/Terminal after NVM installation
# OR add NVM to PATH manually
# Refer to: NVM_SETUP_GUIDE.md
```

### **Issue 2: Wrong Node version active**

```bash
# Solution: Switch to correct version
nvm use 18
node --version  # Should show v18.x.x
```

### **Issue 3: Angular CLI not found**

```bash
# Solution: Install Angular CLI globally
npm install -g @angular/cli@18
ng version
```

### **Issue 4: npm packages issue**

```bash
# Solution: Clear npm cache and reinstall
npm cache clean --force
npm install -g @angular/cli@18
```

### **Issue 5: Multiple Node versions installed, but wrong one active**

```bash
# Solution: Set correct default and use it
nvm alias default 18
nvm use default
node --version
```

---

## ?? **EXPECTED VERIFICATION OUTPUT**

### **Successful Verification**

```
=== Environment Verification ===

1. Checking NVM...
   ? NVM: 1.1.11

2. Checking Node...
   ? Node: v18.18.2

3. Checking npm...
   ? npm: 9.8.1

4. Checking Angular CLI...
? Angular CLI installed

5. Checking Git...
   ? Git: git version 2.42.0

=== Verification Complete ===
```

### **Failed Verification (Example)**

```
=== Environment Verification ===

1. Checking NVM...
   ? NVM: 1.1.11

2. Checking Node...
   ? Node: v16.20.2  ? WRONG VERSION!

3. Checking npm...
   ? npm: 8.19.4

4. Checking Angular CLI...
   ? Angular CLI installed

5. Checking Git...
   ? Git: git version 2.42.0

?? ISSUE: Node version is v16, should be v18
SOLUTION: Run 'nvm use 18'
```

---

## ?? **MANUAL VERIFICATION COMMANDS**

Run these commands one by one to verify setup:

```bash
# 1. NVM
nvm --version

# 2. List all Node versions
nvm list

# 3. Current Node version
node -v

# 4. Current npm version
npm -v

# 5. Angular version
ng version

# 6. Global packages
npm list -g --depth=0

# 7. Git status
git status

# 8. Node modules in current directory
npm list --depth=0
```

---

## ?? **VERIFICATION TEMPLATE**

### **Copy and Fill In Your System Info**

```
=== YOUR SYSTEM VERIFICATION ===

NVM Version: _______________
Node Version: _______________
npm Version: _______________
Angular CLI Version: _______________
Git Version: _______________

Project Status: _______________
Branch: _______________

=== NOTES ===
[Add any issues found]
```

---

## ? **YOU'RE READY IF:**

? All checks pass  
? Node version is 18.x  
? npm version is 9.x  
? Angular CLI is version 18  
? Git shows main branch  
? No error messages shown  

---

## ?? **NEXT STEPS**

1. ? Run verification script
2. ? Fix any issues (use NVM_SETUP_GUIDE.md)
3. ? Verify all checks pass
4. ? Open RCM_PORTAL_DEVELOPMENT_GUIDE.md
5. ? Create your RCM Portal project!

---

**Status**: ? VERIFICATION READY  
**Last Updated**: January 2024

?? **Your environment is ready for development!** ??

