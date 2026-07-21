# ?? **HOW TO START YOUR ANGULAR DEV SERVER**

## ?? **SERVER NOT RUNNING**

Your Angular dev server is currently **stopped**. Here's how to restart it.

---

## ?? **OPTION 1: START VIA BATCH FILE (EASIEST)**

A batch file has been created for you:

**File**: `C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd\start-server.bat`

### **Steps:**

1. **Navigate to your project folder**:
   ```
   C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd\
   ```

2. **Double-click**: `start-server.bat`

3. **A new window opens** with the Angular dev server starting

4. **Wait for**: `Application bundle generation complete`

5. **Visit**: http://localhost:4300 in your browser

---

## ?? **OPTION 2: START VIA POWERSHELL (RECOMMENDED)**

### **Steps:**

1. **Open PowerShell** and navigate to your project:
   ```powershell
   cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd
   ```

2. **Start the dev server**:
   ```powershell
   npx ng serve --port 4300
   ```

3. **Wait for this message**:
   ```
   ? Building...
   Application bundle generation complete
   ? Local:   http://localhost:4300/
   ```

4. **Keep the terminal open** - don't close it!

5. **Visit**: http://localhost:4300 in your browser

---

## ?? **OPTION 3: USE NPM SCRIPT**

Add this script to your `package.json` (already added):

```bash
npm start
```

This is a shortcut for:
```bash
npm run start
```

Which runs:
```bash
ng serve --port 4300
```

---

## ?? **EXPECTED OUTPUT**

When the server starts successfully, you'll see:

```
Warning: This is a simple server for use in testing or debugging...

? Building...
- Building...
\ Building...
| Building...
/ Building...

? Building...
Initial chunk files | Names  | Raw size
styles.css   | styles        | 667.90 kB |
polyfills.js        | polyfills     | 90.20 kB  |
main.js  | main      | 23.20 kB  |

      | Initial total | 781.29 kB |

Application bundle generation complete. [3.223 seconds]

Watch mode enabled. Watching for file changes...

?  Local:   http://localhost:4300/
?  Network:   http://10.21.200.25:4300/
```

---

## ? **THEN OPEN YOUR BROWSER**

Once you see the above message:

1. **Open your browser**
2. **Visit**: http://localhost:4300/
3. **See**: Your Angular app with Ant Design

---

## ?? **TO STOP THE SERVER**

### **Option A: In PowerShell**
Press: `Ctrl + C`

### **Option B: Use Shortcuts**
Press: `q` + Enter

### **Option C: Close the terminal**
Just close the PowerShell window

---

## ? **QUICK REFERENCE**

| Command | What it does |
|---------|------------|
| `npx ng serve` | Starts on port 4200 |
| `npx ng serve --port 4300` | Starts on port 4300 |
| `npx ng serve --open` | Starts and opens browser |
| `npx ng serve --poll 1000` | Starts with file polling |
| `npm start` | Alias for ng serve |
| `npm run build` | Build for production |

---

## ?? **TROUBLESHOOTING**

### **Port 4300 is already in use**

Use a different port:
```powershell
npx ng serve --port 4301
```

Then visit: http://localhost:4301

### **"ng: The term 'ng' is not recognized"**

Use npx instead:
```powershell
npx ng serve --port 4300
```

### **"Cannot find module" errors**

Run:
```powershell
npm install
npx ng serve --port 4300
```

### **App won't load in browser**

1. Check terminal shows: "Application bundle generation complete"
2. Wait 5 seconds
3. Hard refresh browser: `Ctrl + Shift + R`
4. Check console (F12) for errors

---

## ?? **CHECKLIST**

- [ ] Open PowerShell
- [ ] Navigate to: `C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd`
- [ ] Run: `npx ng serve --port 4300`
- [ ] Wait for: "Application bundle generation complete"
- [ ] Open browser: http://localhost:4300
- [ ] See: Angular app with Ant Design
- [ ] **? Success!**

---

## ?? **YOU'RE READY!**

Your Angular dev server is ready to start!

Choose one of the 3 options above and get your app running!

---

**Status**: Dev server ready to start  
**Next**: Choose an option above  
**Expected time**: 30 seconds to 2 minutes  

?? **Start your server now!** ??

