# ?? **NPM INSTALLATION TROUBLESHOOTING - SOLUTIONS**

## ? **ERROR YOU'RE SEEING**

```
Cannot find module @rollup/rollup-win32-x64-msvc
```

This is a Windows-specific issue with npm's handling of optional native dependencies.

---

## ? **SOLUTION 1: CLEAN REINSTALL (RECOMMENDED)**

### **Step 1: Close all terminals and file explorers**

Make sure no files are locked by:
- ? Close all PowerShell/Terminal windows
- ? Close Visual Studio Code
- ? Close file explorer windows showing the rcm-portal-antd folder

### **Step 2: Delete node_modules and package-lock.json**

```powershell
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd

# Remove node_modules folder (this may take a minute)
rmdir node_modules -r -Force

# Remove package-lock.json
Remove-Item package-lock.json -Force

# Verify they're gone
Get-ChildItem | Select-Object Name
# Should NOT show node_modules or package-lock.json
```

### **Step 3: Clean npm cache**

```powershell
npm cache clean --force
```

### **Step 4: Reinstall with legacy-peer-deps**

```powershell
npm install --legacy-peer-deps
```

**Expected output:**
```
added XXX packages
```

### **Step 5: Test the installation**

```powershell
npm list | head -20
# Should show package tree
```

---

## ? **SOLUTION 2: IF STEP 1 FAILS (Permission Issues)**

If you get permission errors (EPERM), run PowerShell as Administrator:

1. **Right-click PowerShell ? Run as administrator**
2. **Run the commands again:**

```powershell
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd
rmdir node_modules -r -Force
Remove-Item package-lock.json -Force
npm cache clean --force
npm install --legacy-peer-deps
```

---

## ? **SOLUTION 3: NUCLEAR OPTION (If solutions 1 & 2 fail)**

Delete everything and start fresh:

```powershell
# Run as Administrator

# Navigate to repos folder
cd C:\Users\sibtain.alimadad\source\repos

# Delete entire rcm-portal-antd folder
rmdir rcm-portal-antd -r -Force

# Recreate the project
ng new rcm-portal-antd --routing --style=less --package-manager=npm --skip-git

# Navigate into it
cd rcm-portal-antd

# Install with legacy-peer-deps
npm install --legacy-peer-deps

# Add Ant Design
ng add ng-zorro-antd

# Install chart.js, moment, etc.
npm install chart.js ng2-charts@4.1.1 moment ngx-moment lodash-es --legacy-peer-deps
```

---

## ?? **AFTER DEPENDENCIES ARE INSTALLED**

Once npm install completes successfully:

```powershell
# Verify installation
npm list chart.js ng2-charts moment ngx-moment lodash-es

# Should show all packages with versions
```

Then start the dev server:

```powershell
ng serve --open
```

---

## ?? **IF YOU STILL GET ERRORS**

### **Error: "Cannot find module @rollup..."**

```powershell
# Try this
npm install @rollup/rollup-win32-x64-msvc --save-dev --legacy-peer-deps
```

### **Error: "EPERM: operation not permitted"**

This means files are locked. Try:

1. Close all applications
2. Restart your computer
3. Run PowerShell as Administrator
4. Try npm install again

### **Error: "ECONNRESET / network aborted"**

Your internet connection was interrupted. Try:

```powershell
# Wait a minute, then try again
npm install --legacy-peer-deps --verbose

# Or use a different registry
npm install --legacy-peer-deps --registry https://registry.npmjs.org/
```

---

## ? **CORRECT PROCEDURE (COMPLETE)**

If you're starting completely fresh, here's the complete procedure:

### **Step 1: Navigate to repos folder**

```powershell
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd
```

### **Step 2: Clean everything**

```powershell
# Close all terminals first!
# Then run as Administrator:

rmdir node_modules -r -Force
Remove-Item package-lock.json -Force
npm cache clean --force
```

### **Step 3: Reinstall packages**

```powershell
npm install --legacy-peer-deps
```

### **Step 4: Verify**

```powershell
npm list chart.js ng2-charts moment ngx-moment lodash-es
```

### **Step 5: Start server**

```powershell
ng serve --open
```

---

## ?? **HOW TO VERIFY INSTALLATION IS CORRECT**

After npm install, check these files exist:

```
node_modules/
??? @angular/
??? @angular/cdk/
??? chart.js/
??? ng2-charts/
??? moment/
??? ngx-moment/
??? lodash-es/
??? ng-zorro-antd/
```

Check with:

```powershell
Get-ChildItem node_modules | Select-Object Name | head -20
```

---

## ?? **PREVENTING FUTURE ISSUES**

Add this to your `package.json` to make `--legacy-peer-deps` automatic:

```json
{
  "name": "rcm-portal-antd",
  "version": "0.0.0",
  "description": "RCM Portal with Angular and Ant Design",
  "npmrc": {
    "legacy-peer-deps": true
  },
  // ... rest of package.json
}
```

Or create `.npmrc` file in project root:

```
legacy-peer-deps=true
```

Then npm install will use this flag automatically!

---

## ?? **QUICK COMMAND (TL;DR)**

```powershell
# Close all windows first!
# Then:

cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd

# Run as Administrator:
rmdir node_modules -r -Force; Remove-Item package-lock.json; npm cache clean --force; npm install --legacy-peer-deps

# Then:
ng serve --open
```

---

## ? **EXPECTED SUCCESS**

When everything works, you'll see:

```
? Compiled successfully.
? Application bundle generated successfully. [2.1 MB]
Local:     http://localhost:4200/
Use Ctrl+C to stop the server.
```

And your browser opens http://localhost:4200

---

## ?? **SUMMARY OF SOLUTIONS**

| Issue | Solution |
|-------|----------|
| **@rollup module not found** | Clean node_modules + npm install --legacy-peer-deps |
| **EPERM permission errors** | Run PowerShell as Administrator |
| **Network errors** | Wait and retry, check internet |
| **Still failing** | Delete entire folder and recreate (nuclear option) |

---

## ?? **WHAT TO TRY FIRST**

1. **Try Solution 1** (Clean install with legacy-peer-deps) - 95% of cases work
2. **Try Solution 2** (Run as Administrator) - Most permission issues fixed
3. **Try Solution 3** (Nuclear option) - Only if others fail

---

**Status**: Ready to fix! Follow solutions above.  
**Most likely to work**: Solution 1  
**Time to fix**: 5-10 minutes  

Pick a solution and let me know the result! ??

