# ?? **PHASE 1 - CORRECTED COMMANDS**

## ? **CORRECT ng generate SYNTAX**

The `--skip-tests` flag only works with **components**, NOT modules.

---

## ?? **STEP 6: GENERATE MODULES (CORRECTED)**

```powershell
# Generate modules WITHOUT --skip-tests
ng generate module core --routing
ng generate module shared
ng generate module features/dashboard --routing
ng generate module features/claims --routing
ng generate module features/eligibility --routing
ng generate module features/pre-auth --routing
ng generate module features/reports --routing
ng generate module auth --routing
```

**Expected output:**
```
? CREATE src/app/core/core.module.ts
? CREATE src/app/core/core-routing.module.ts
```

---

## ?? **STEP 7: GENERATE COMPONENTS (CORRECTED)**

For components, `--skip-tests` IS valid:

```powershell
# Generate components WITH --skip-tests (this works)
ng generate component shared/components/navbar --skip-tests
ng generate component shared/components/sidebar --skip-tests
ng generate component shared/components/footer --skip-tests
ng generate component auth/components/login --skip-tests
ng generate component auth/components/logout --skip-tests
ng generate component features/dashboard/dashboard --skip-tests
```

**Expected output:**
```
? CREATE src/app/shared/components/navbar/navbar.component.ts
? CREATE src/app/shared/components/navbar/navbar.component.html
? CREATE src/app/shared/components/navbar/navbar.component.less
```

---

## ?? **QUICK COPY-PASTE COMMAND BLOCK**

Copy and run all at once:

```powershell
# MODULES
ng generate module core --routing
ng generate module shared
ng generate module features/dashboard --routing
ng generate module features/claims --routing
ng generate module features/eligibility --routing
ng generate module features/pre-auth --routing
ng generate module features/reports --routing
ng generate module auth --routing

# COMPONENTS
ng generate component shared/components/navbar --skip-tests
ng generate component shared/components/sidebar --skip-tests
ng generate component shared/components/footer --skip-tests
ng generate component auth/components/login --skip-tests
ng generate component auth/components/logout --skip-tests
ng generate component features/dashboard/dashboard --skip-tests

Write-Host "? All modules and components generated successfully!" -ForegroundColor Green
```

---

## ?? **WHAT EACH FLAG DOES**

| Flag | Used With | Purpose |
|------|-----------|---------|
| `--routing` | modules & components | Add routing module |
| `--skip-tests` | **components only** | Skip `.spec.ts` test file |
| `--skip-tests` | modules | ? **NOT VALID** |

---

## ?? **STEP-BY-STEP EXECUTION**

### **Step 1: Make sure you're in the right folder**

```powershell
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd
Get-Location  # Verify location
```

### **Step 2: Generate all modules first**

```powershell
ng generate module core --routing
ng generate module shared
ng generate module features/dashboard --routing
ng generate module features/claims --routing
ng generate module features/eligibility --routing
ng generate module features/pre-auth --routing
ng generate module features/reports --routing
ng generate module auth --routing
```

Wait for each to complete (you'll see ? CREATE messages)

### **Step 3: Generate all components**

```powershell
ng generate component shared/components/navbar --skip-tests
ng generate component shared/components/sidebar --skip-tests
ng generate component shared/components/footer --skip-tests
ng generate component auth/components/login --skip-tests
ng generate component auth/components/logout --skip-tests
ng generate component features/dashboard/dashboard --skip-tests
```

Wait for each to complete

---

## ? **VERIFY GENERATION**

Check if files were created:

```powershell
# Verify modules
Get-ChildItem -Path src/app -Include "*.module.ts" -Recurse

# Verify components
Get-ChildItem -Path src/app -Include "*.component.ts" -Recurse
```

You should see:
- 8 `.module.ts` files
- 6 `.component.ts` files

---

## ?? **WHAT YOU'LL HAVE AFTER THIS STEP**

```
src/app/
??? core/
?   ??? core.module.ts
?   ??? core-routing.module.ts
??? shared/
?   ??? shared.module.ts
?   ??? components/
?       ??? navbar/
?       ?   ??? navbar.component.ts
? ?   ??? navbar.component.html
?       ?   ??? navbar.component.less
?   ??? sidebar/
?       ?   ??? sidebar.component.ts
?       ?   ??? sidebar.component.html
?       ?   ??? sidebar.component.less
?       ??? footer/
?           ??? footer.component.ts
?  ??? footer.component.html
?           ??? footer.component.less
??? features/
?   ??? dashboard/
?   ?   ??? dashboard.module.ts
?   ?   ??? dashboard-routing.module.ts
?   ?   ??? dashboard/
?   ?       ??? dashboard.component.ts
?   ?       ??? dashboard.component.html
?   ?       ??? dashboard.component.less
?   ??? claims/
?   ?   ??? claims.module.ts
?   ?   ??? claims-routing.module.ts
?   ??? eligibility/
?   ?   ??? eligibility.module.ts
?   ?   ??? eligibility-routing.module.ts
?   ??? pre-auth/
?   ?   ??? pre-auth.module.ts
?   ? ??? pre-auth-routing.module.ts
?   ??? reports/
? ??? reports.module.ts
?    ??? reports-routing.module.ts
??? auth/
    ??? auth.module.ts
    ??? auth-routing.module.ts
    ??? components/
        ??? login/
        ???? login.component.ts
        ?   ??? login.component.html
        ?   ??? login.component.less
    ??? logout/
   ??? logout.component.ts
   ??? logout.component.html
         ??? logout.component.less
```

---

## ? **NEXT STEPS AFTER THIS**

Once modules and components are generated:

1. ? Update environment files (Step 8)
2. ? Setup theme files (Step 9)
3. ? Create .nvmrc (Step 10)
4. ? Git commit (Step 11)
5. ? Start dev server (Step 12)

---

## ?? **TROUBLESHOOTING**

### **Error: "Cannot find module"**
```powershell
# Make sure you're in the rcm-portal-antd directory
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd
```

### **Error: "ng command not found"**
```powershell
# Angular CLI not found, install it
npm install -g @angular/cli@18
```

### **Error: Still getting --skip-tests error**
```powershell
# For modules, just don't use --skip-tests
ng generate module features/claims --routing
# This works!
```

---

**Status**: ? Commands corrected  
**Next**: Run the module & component generation commands  
**Time**: ~5-10 minutes  

?? **Continue with corrected commands!** ??

