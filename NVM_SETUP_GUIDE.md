# ?? **NVM SETUP GUIDE - MANAGING MULTIPLE ANGULAR VERSIONS**

## ? **COMPLETE NVM INSTALLATION & CONFIGURATION GUIDE**

Since you have legacy .NET 9 backend projects with older Angular versions, using NVM (Node Version Manager) is **ESSENTIAL** to avoid conflicts.

---

## ?? **WHY NVM FOR YOUR PROJECT?**

```
PROBLEM:
?? You have multiple Angular projects
   ?? Existing RCM Portal (possibly Angular 12-14)
   ?? New RCM Portal (Angular 18)
   ?? Each needs different Node/npm versions

SOLUTION:
?? Use NVM to switch between Node versions instantly
   ?? Switch for old project
   ?? Switch for new project
   ?? No version conflicts!
```

---

## ?? **STEP 1: CHECK YOUR CURRENT SETUP**

### **Check Current Node Version**

```bash
node --version
# Example output: v16.14.0 or v18.0.0 or similar
```

### **Check Current npm Version**

```bash
npm --version
# Example output: 8.3.1 or 9.5.0 or similar
```

### **Check Angular CLI Version**

```bash
ng version
# Shows Angular version if installed globally
# Or: npm list -g @angular/cli
```

### **Check All Installed Global Packages**

```bash
npm list -g --depth=0
# Shows all globally installed packages
```

---

## ?? **STEP 2: UNDERSTAND NODE/ANGULAR VERSIONS**

### **Node Version Requirements**

```
Angular 12: Node 12.13+ (LTS 12.x, 14.x)
Angular 13: Node 12.13+ (LTS 14.x, 16.x)
Angular 14: Node 14.x, 16.x
Angular 15: Node 16.x, 18.x
Angular 16: Node 16.x, 18.x
Angular 17: Node 18.x, 20.x
Angular 18: Node 18.x, 20.x ? YOUR NEW PROJECT
```

### **Your Likely Scenario**

```
Existing Projects: Angular 12-14 ? Node 14.x or 16.x
New RCM Portal: Angular 18 ? Node 18.x or 20.x
```

---

## ?? **STEP 3: INSTALL NVM ON WINDOWS**

### **Option A: Using nvm-windows (Recommended for Windows)**

**Step 1: Download NVM for Windows**

Go to: https://github.com/coreybutler/nvm-windows/releases

Download the latest **nvm-setup.exe** file

**Step 2: Run the Installer**

1. Double-click `nvm-setup.exe`
2. Accept license agreement
3. Choose installation directory (default is fine)
4. Choose symlink directory (default is fine)
5. Click "Install"
6. Wait for installation to complete
7. **Restart your computer or PowerShell**

**Step 3: Verify Installation**

Open PowerShell and run:

```bash
nvm version
# Output: 1.1.x or similar
```

**Step 4: Check Available Node Versions**

```bash
nvm list available
# Shows all available Node versions
```

### **Option B: Using Chocolatey (If You Have It)**

```bash
choco install nvm
```

### **Option C: Using Git Bash or WSL**

If using Git Bash or Windows Subsystem for Linux:

```bash
# Install NVM (original version for Linux/Mac/WSL)
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.0/install.sh | bash

# Reload shell config
exec bash

# Verify
nvm --version
```

---

## ? **STEP 4: INSTALL REQUIRED NODE VERSIONS**

### **For Your Existing Projects (Node 16)**

```bash
# Install Node 16 (LTS)
nvm install 16

# Verify installation
nvm list
# Should show: 16.20.x

# Use Node 16
nvm use 16

# Verify switch
node --version
npm --version
```

### **For Your New RCM Portal (Node 18 or 20)**

```bash
# Install Node 18 (LTS)
nvm install 18

# OR install Node 20 (Latest LTS)
nvm install 20

# Verify installation
nvm list
# Should show both 16 and 18/20

# Use Node 18
nvm use 18

# Verify switch
node --version
npm --version
```

### **Set Default Node Version**

```bash
# Use Node 18 as default
nvm alias default 18

# Verify default
nvm alias
# Shows all aliases, default should be 18
```

---

## ?? **STEP 5: SWITCHING BETWEEN VERSIONS**

### **Quick Switch Commands**

```bash
# Switch to Node 16 (for old projects)
nvm use 16
node --version  # v16.20.x

# Switch to Node 18 (for new RCM portal)
nvm use 18
node --version  # v18.x.x

# Switch to Node 20 (for latest projects)
nvm use 20
node --version  # v20.x.x
```

### **Using .nvmrc File (Automatic Version Switching)**

Create `.nvmrc` file in your project root:

**For existing Angular 12-14 project:**
```bash
# In old project directory
echo "16" > .nvmrc

# When you cd into this folder:
nvm use
# Automatically switches to Node 16
```

**For new RCM portal project:**
```bash
# In new project directory
echo "18" > .nvmrc

# When you cd into this folder:
nvm use
# Automatically switches to Node 18
```

---

## ?? **STEP 6: INSTALL ANGULAR CLI VERSIONS**

### **For Node 16 (Existing Projects)**

```bash
# Switch to Node 16
nvm use 16

# Install Angular CLI 12 or 14
npm install -g @angular/cli@14

# Verify
ng version
```

### **For Node 18 (New RCM Portal)**

```bash
# Switch to Node 18
nvm use 18

# Install Angular CLI 18
npm install -g @angular/cli@18

# Verify
ng version
```

### **View Installed Global Packages**

```bash
npm list -g --depth=0
# Shows @angular/cli and other global packages
```

---

## ?? **COMPLETE WORKFLOW EXAMPLE**

### **Working with Old Project**

```bash
# Navigate to old project
cd C:\path\to\old-angular-project

# Switch to Node 16 and Angular CLI 14
nvm use 16
npm install
ng serve

# Visit http://localhost:4200
```

### **Switching to New RCM Portal**

```bash
# Close old project terminal

# Navigate to new RCM project
cd C:\path\to\rcm-portal-antd

# Switch to Node 18 and Angular CLI 18
nvm use 18
npm install
ng add ng-zorro-antd
ng serve

# Visit http://localhost:4200
```

### **Back to Old Project**

```bash
# Switch back to Node 16
nvm use 16

# Old project works with its dependencies
ng serve
```

---

## ??? **TROUBLESHOOTING NVM ON WINDOWS**

### **Problem: NVM command not recognized**

**Solution:**
```bash
# Close PowerShell and open as Administrator
# Run: nvm version
# If still not working, restart computer after NVM install
```

### **Problem: Can't switch Node versions**

**Solution:**
```bash
# Check if Node is in use
node --version
nvm list
# Try again: nvm use 16
```

### **Problem: npm packages not found after switching**

**Solution:**
```bash
# Reinstall npm packages after switching
npm install
# npm looks in node_modules and installs missing packages
```

### **Problem: Wrong Angular CLI version**

**Solution:**
```bash
# Check global Angular CLI version
ng version

# Uninstall old version
npm uninstall -g @angular/cli

# Install correct version
npm install -g @angular/cli@18
```

---

## ?? **COMPLETE SETUP CHECKLIST**

### **Before Creating New RCM Portal**

- [ ] NVM installed (`nvm version`)
- [ ] Node 16 installed (`nvm install 16`)
- [ ] Node 18 installed (`nvm install 18`)
- [ ] Node 18 is default (`nvm alias default 18`)
- [ ] Angular CLI 18 installed (`ng version`)
- [ ] .nvmrc file in new project (`echo "18" > .nvmrc`)

### **Project Structure**

```
C:\Users\sibtain.alimadad\source\repos\
??? NPhies_FHIR_Integration/
?   ??? .nvmrc (contains: 16)
?   ??? [Old Angular 12-14 project]
?
??? rcm-portal-antd/
    ??? .nvmrc (contains: 18)
    ??? [New Angular 18 + Ant Design project]
```

---

## ? **VERIFY EVERYTHING WORKS**

### **Step 1: Check Node Versions Installed**

```bash
nvm list
```

**Expected output:**
```
    16.20.2
    18.18.2
->  18.18.2 (Currently using 64-bit executable)
    default -> 18 (-> 18.18.2)
```

### **Step 2: Check npm Version**

```bash
npm --version
# Should be 9.x for Node 18, 8.x for Node 16
```

### **Step 3: Check Angular CLI**

```bash
ng version
# Should show Angular 18 if on Node 18
```

### **Step 4: Check Global Packages**

```bash
npm list -g --depth=0
```

**Expected output (for Node 18):**
```
??? @angular/cli@18.0.x
??? npm@9.x.x
??? [other packages]
```

---

## ?? **READY TO CREATE RCM PORTAL PROJECT**

Once NVM is set up correctly:

```bash
# 1. Make sure Node 18 is active
nvm use 18

# 2. Create new project
ng new rcm-portal-antd --routing --style=less --package-manager=npm

# 3. Navigate to project
cd rcm-portal-antd

# 4. Create .nvmrc file
echo "18" > .nvmrc

# 5. Add Ant Design
ng add ng-zorro-antd

# 6. Install dependencies
npm install chart.js ng2-charts moment ngx-moment lodash-es

# 7. Start development
ng serve --open
```

---

## ?? **USEFUL NVM COMMANDS REFERENCE**

```bash
# Version management
nvm install <version>          # Install specific Node version
nvm uninstall <version>        # Remove Node version
nvm use <version>    # Switch to version
nvm alias <name> <version>     # Create alias
nvm unalias <name>  # Remove alias

# Information
nvm list  # List installed versions
nvm list available     # List available versions online
nvm current     # Show current version
nvm version     # Show NVM version

# Other
nvm reinstall-packages <version>  # Reinstall packages from another version
nvm which <version>          # Show path to executable
nvm default <version>       # Set default version
```

---

## ?? **YOUR NEXT STEPS**

1. ? Install NVM (if not already done)
2. ? Install Node 16 for existing projects
3. ? Install Node 18 for new RCM portal
4. ? Set Node 18 as default
5. ? Install Angular CLI 18 globally
6. ? Create `.nvmrc` file in each project
7. ? Create new RCM portal project
8. ? Start development!

---

## ?? **PRO TIPS**

### **Tip 1: Use Project-Specific .nvmrc**

Every project should have `.nvmrc`:
```bash
# Old project
echo "16" > .nvmrc

# New project
echo "18" > .nvmrc
```

### **Tip 2: Automate Version Switching**

Add to your shell profile (if using bash/zsh):
```bash
# Auto switch Node version when entering directory with .nvmrc
cd() { builtin cd "$@"; nvm use &>/dev/null || :; }
```

### **Tip 3: Keep npm Updated**

After switching Node versions, update npm:
```bash
nvm use 18
npm install -g npm@latest

nvm use 16
npm install -g npm@latest
```

### **Tip 4: Separate Global Packages**

NVM keeps global packages separate per Node version:
```bash
nvm use 16
npm list -g           # Global packages for Node 16

nvm use 18
npm list -g           # Global packages for Node 18 (different!)
```

---

## ? **COMMON COMMANDS FOR YOUR WORKFLOW**

### **Starting Work on Old Project**

```bash
cd C:\path\to\old-project
nvm use 16
npm install
ng serve
```

### **Switching to RCM Portal Work**

```bash
cd C:\path\to\rcm-portal-antd
nvm use 18
npm install
ng add ng-zorro-antd
ng serve
```

### **Quick Node Version Check**

```bash
node --version
npm --version
ng version
```

---

## ?? **FINAL CHECKLIST BEFORE DEVELOPMENT**

```bash
# 1. Verify NVM installed
nvm --version

# 2. List all Node versions
nvm list

# 3. Verify Node 18 is current
node --version  # Should be v18.x.x

# 4. Verify npm version
npm --version   # Should be 9.x.x

# 5. Verify Angular CLI 18
ng version      # Should show Angular 18

# 6. Ready to create project?
echo "? YES - Let's build the RCM Portal!"
```

---

## ?? **YOU'RE READY!**

With NVM properly configured:
- ? No version conflicts between projects
- ? Easy switching between Angular versions
- ? Clean, isolated environments
- ? Ready to start RCM portal development

**Next: Follow `RCM_PORTAL_DEVELOPMENT_GUIDE.md` to create your project!**

---

**Status**: ? NVM SETUP GUIDE COMPLETE  
**Ready for Development**: YES ?  
**Multi-Version Support**: YES ?  

?? **Let's build your RCM Portal!** ??

