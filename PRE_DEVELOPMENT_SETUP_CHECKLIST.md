# ? **PRE-DEVELOPMENT SETUP CHECKLIST**

## ?? **COMPLETE CHECKLIST BEFORE STARTING RCM PORTAL DEVELOPMENT**

This guide ensures you have EVERYTHING ready before creating your Angular project.

---

## ?? **PHASE 1: SYSTEM REQUIREMENTS (5 minutes)**

### **Hardware Requirements**

- [ ] **RAM**: Minimum 8GB (16GB recommended)
  - Check: Right-click "This PC" ? Properties ? Installed RAM
  
- [ ] **Disk Space**: Minimum 20GB free
  - Check: File Explorer ? Right-click C: drive ? Properties
  
- [ ] **Processor**: Modern multi-core processor
  - Check: Task Manager ? Performance tab

### **Internet Connection**

- [ ] **Speed**: At least 5 Mbps download
  - Why? Downloading Node, npm packages, Angular CLI
  
- [ ] **Stability**: Stable connection (no frequent disconnects)
  - Why? npm install can take 5-10 minutes

---

## ?? **PHASE 2: INSTALL PREREQUISITES (30 minutes)**

### **Windows Requirements**

- [ ] **Windows 10/11** (64-bit recommended)
  - Check: Settings ? System ? About
  
- [ ] **Administrator Access**
  - Why? NVM and global npm packages need admin rights
  
- [ ] **PowerShell 5.0+** (for scripts)
  - Check: `$PSVersionTable.PSVersion`

### **Essential Software**

- [ ] **Git for Windows** (https://git-scm.com/download/win)
  - Check: `git --version`
  - Install: Default options, use Git Bash
  
- [ ] **NVM for Windows** (https://github.com/coreybutler/nvm-windows)
  - Check: `nvm --version`
  - Install: nvm-setup.exe
  - **Restart computer after install**
  
- [ ] **Visual Studio Code** (Optional but recommended)
  - Download: https://code.visualstudio.com/
  - Extensions: Angular Language Service, Prettier, ESLint

---

## ?? **PHASE 3: NODE SETUP (20 minutes)**

### **Install Node Versions with NVM**

- [ ] **Verify NVM Installation**
  ```bash
  nvm --version
  # Should show: 1.1.x
  ```

- [ ] **Install Node 18 LTS** (for RCM Portal)
  ```bash
  nvm install 18
  nvm list
  ```

- [ ] **Install Node 16 LTS** (for existing projects)
  ```bash
  nvm install 16
  nvm list
  # Should show both 16 and 18
  ```

- [ ] **Set Node 18 as Default**
  ```bash
  nvm alias default 18
  nvm use 18
  ```

- [ ] **Verify Current Node Setup**
  ```bash
  node --version      # Should be v18.x.x
  npm --version       # Should be 9.x.x
  ```

---

## ?? **PHASE 4: ANGULAR CLI SETUP (10 minutes)**

- [ ] **Ensure Node 18 Active**
  ```bash
  nvm use 18
  ```

- [ ] **Install Angular CLI 18 Globally**
  ```bash
  npm install -g @angular/cli@18
  ```

- [ ] **Verify Angular CLI Installation**
  ```bash
  ng version
  # Should show: Angular CLI: 18.x.x
  ```

- [ ] **Check Global Packages**
  ```bash
  npm list -g --depth=0
  # Should include @angular/cli@18.x.x
  ```

---

## ?? **PHASE 5: PROJECT DIRECTORY SETUP (5 minutes)**

### **Create Project Directory Structure**

- [ ] **Create Base Directory**
  ```bash
  mkdir C:\Users\sibtain.alimadad\source\repos\angular-projects
  cd C:\Users\sibtain.alimadad\source\repos\angular-projects
  ```

- [ ] **Create Project Directory**
  ```bash
  # This is where your RCM Portal will go
  # We'll create it in Phase 7
  ```

- [ ] **Create .nvmrc Files for Each Project**
  ```bash
  # For existing projects (if applicable)
  echo "16" > .nvmrc
  
  # For new RCM Portal (will do this during creation)
  echo "18" > .nvmrc
  ```

---

## ?? **PHASE 6: ENVIRONMENT VERIFICATION (5 minutes)**

### **Run Complete Verification**

- [ ] **Manual Verification Commands**
  ```bash
  # Test each command
  nvm --version
  node --version
  npm --version
  ng version
  git --version
  ```

- [ ] **Run Verification Script** (from ENVIRONMENT_VERIFICATION.md)
  ```bash
  # Download and run verify-setup.ps1 or verify-setup.sh
  .\verify-setup.ps1
  ```

- [ ] **Check Project Git Status**
  ```bash
  cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration
  git status
  # Should show: On branch main
  ```

---

## ?? **PHASE 7: DOCUMENTATION REVIEW (10 minutes)**

### **Read Supporting Documents**

- [ ] **Read START_HERE.md** (5 min)
  - Understand document structure
  - Know what to read next

- [ ] **Read RCM_PORTAL_DEVELOPMENT_GUIDE.md** (15 min)
  - Understand architecture
  - Review Quick Start section
  - Familiarize with project structure

- [ ] **Read NVM_SETUP_GUIDE.md** (if needed)
  - Understand NVM workflow
  - Learn how to switch versions

- [ ] **Read ENVIRONMENT_VERIFICATION.md** (if needed)
  - Know how to verify setup
  - Learn troubleshooting steps

---

## ? **PHASE 8: PRE-CREATION VERIFICATION (5 minutes)**

### **Final Checks Before Creating Project**

**Verify All Prerequisites Met:**

```bash
# 1. Check NVM
nvm --version
# ? Shows version (e.g., 1.1.11)

# 2. Check Node versions
nvm list
# ? Shows both 16 and 18 installed
# ? Arrow points to 18

# 3. Check current Node is 18
node --version
# ? Shows v18.x.x

# 4. Check npm
npm --version
# ? Shows 9.x.x

# 5. Check Angular CLI
ng version
# ? Shows Angular CLI: 18.x.x

# 6. Check Git
git status
# ? Shows: On branch main

# 7. Check disk space
# ? At least 20GB free

# 8. Check internet
# ? Can access npm registry
```

### **Checkoff Template**

```
Pre-Creation Verification:
- [ ] NVM version: ___________
- [ ] Node versions: 16, 18
- [ ] Current Node: v18.x.x
- [ ] npm version: 9.x.x
- [ ] Angular CLI: 18.x.x
- [ ] Git status: main branch
- [ ] Disk space: 20GB+ free
- [ ] Internet: Working
- [ ] All checks passed: YES ?
```

---

## ?? **PHASE 9: PROJECT CREATION (5 minutes)**

### **Ready to Create RCM Portal?**

Once all Phase 1-8 checks pass:

```bash
# 1. Make sure you're in correct directory
cd C:\Users\sibtain.alimadad\source\repos

# 2. Verify Node 18 is active
nvm use 18
node --version  # Should be v18.x.x

# 3. Create project
ng new rcm-portal-antd --routing --style=less --package-manager=npm

# 4. This will take 3-5 minutes
# It will:
# - Create project folder
# - Install npm packages
# - Setup Angular structure

# 5. When done, navigate to project
cd rcm-portal-antd

# 6. Verify project structure
ls -la
# Should show: src/, node_modules/, angular.json, package.json, etc.

# 7. Create .nvmrc for automatic version switching
echo "18" > .nvmrc

# 8. Verify project
ng version
# Should still show Angular 18
npm list @angular/core
# Should show @angular/core: 18.x.x
```

---

## ?? **TROUBLESHOOTING CHECKLIST**

If something fails during setup:

### **Problem: NVM not recognized**
```bash
# Solution:
# 1. Close PowerShell
# 2. Restart computer
# 3. Open PowerShell as Administrator
# 4. Try: nvm --version
```

### **Problem: Wrong Node version active**
```bash
# Solution:
nvm use 18
node --version  # Verify
```

### **Problem: Angular CLI not found**
```bash
# Solution:
npm install -g @angular/cli@18
ng version  # Verify
```

### **Problem: npm install fails**
```bash
# Solution 1: Clear npm cache
npm cache clean --force

# Solution 2: Reinstall npm
npm install -g npm@latest

# Solution 3: Check internet connection
ping www.npmjs.com
```

### **Problem: ng new takes too long**
```bash
# This is normal! ng new can take 5-10 minutes
# It's downloading and installing 1000+ npm packages
# Don't interrupt it!
```

### **Problem: Project creation fails**
```bash
# Try with simpler command:
ng new rcm-portal-antd --skip-git --package-manager=npm

# Then add routing/styling manually
```

---

## ?? **COMPLETE SETUP CHECKLIST**

### **Before Starting Development**

```
SYSTEM REQUIREMENTS:
  [ ] Windows 10/11 64-bit
  [ ] 8GB+ RAM available
  [ ] 20GB+ disk space free
  [ ] Stable internet connection
  [ ] Administrator access

INSTALL PREREQUISITES:
  [ ] Git for Windows installed
  [ ] NVM for Windows installed
  [ ] Visual Studio Code (optional)
  [ ] Computer restarted after NVM install

NODE SETUP:
  [ ] NVM version: 1.1.x+
  [ ] Node 16 installed
  [ ] Node 18 installed
  [ ] Node 18 active (node --version = v18.x.x)
  [ ] npm 9.x installed
  [ ] npm cache clean done

ANGULAR SETUP:
  [ ] Angular CLI 18 installed globally
  [ ] ng version shows Angular 18.x.x
  [ ] Global packages verified

VERIFICATION:
  [ ] All verification commands pass
  [ ] Verification script runs successfully
  [ ] Git repository status: main branch
  [ ] Internet connection working
  [ ] No errors in console

DOCUMENTATION:
  [ ] Started here (PRE-DEVELOPMENT)
  [ ] Read START_HERE.md
  [ ] Read RCM_PORTAL_DEVELOPMENT_GUIDE.md
  [ ] Reviewed NVM_SETUP_GUIDE.md
  [ ] Reviewed ENVIRONMENT_VERIFICATION.md

READY FOR DEVELOPMENT:
  [ ] All above checked
  [ ] All prerequisites installed
  [ ] All verifications passing
  [ ] Ready to create project
  [ ] Ready to start coding

? ALL CHECKS PASSED - READY TO BUILD!
```

---

## ?? **YOUR NEXT IMMEDIATE STEPS**

1. **Print this checklist** or keep it open
2. **Go through each phase systematically**
3. **Check off items as you complete them**
4. **Fix any issues using troubleshooting section**
5. **When all checked ?, start Phase 9: Project Creation**
6. **Follow RCM_PORTAL_DEVELOPMENT_GUIDE.md for development**

---

## ?? **TIME ESTIMATE**

```
Phase 1: System Requirements ........... 5 min
Phase 2: Install Prerequisites ......... 30 min
Phase 3: Node Setup ................... 20 min
Phase 4: Angular CLI Setup ............ 10 min
Phase 5: Project Directory Setup ....... 5 min
Phase 6: Environment Verification ...... 5 min
Phase 7: Documentation Review ......... 10 min
Phase 8: Pre-Creation Verification .... 5 min
Phase 9: Project Creation ............. 5-10 min
????????????????????????????????????????????????
TOTAL TIME ........................... 90-100 min

That's about 1.5 hours for complete setup!
```

---

## ?? **YOU'RE FULLY PREPARED WHEN:**

? All phases 1-8 complete  
? All checks pass  
? No errors in verification  
? Documentation reviewed  
? Node/npm/Angular versions correct  
? Git repository accessible
? Disk space available  
? Internet working  

**Then you're ready to:**
- Create your RCM Portal project
- Add Ant Design
- Start development
- Build features

---

## ?? **LET'S GO!**

**Start with Phase 1 above and work through each phase.**

**When complete, you'll have:**
- ? Proper development environment
- ? Multiple Node versions (no conflicts)
- ? Angular 18 ready to use
- ? Verified working setup
- ? Ready to create RCM Portal!

**Estimated Completion Time: 1.5-2 hours**

---

**Status**: ? PRE-DEVELOPMENT CHECKLIST COMPLETE  
**Last Updated**: January 2024  

?? **Follow this checklist and you'll have everything set up perfectly!** ??

