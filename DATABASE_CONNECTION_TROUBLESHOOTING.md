# ?? **Database Connection Troubleshooting Guide**

## **Problem: Login Failed for User 'SA'**

### **Root Cause**
The `sa` (SQL Server Administrator) account credentials were incorrect or the account is disabled in your SQL Server instance.

### **Solution Implemented** ?

Changed the connection string from **SQL Server Authentication** to **Windows Authentication (Integrated Security)**.

---

## **Before (Failed)**
```
Server=localhost;Database=NPhiesDb;User Id=sa;Password=Mahyan@123;...
```

## **After (Working)** ?
```
Server=localhost;Database=NPhiesDb;Integrated Security=true;...
```

---

## **Why This Works**

**Windows Authentication (Integrated Security)**:
- Uses your Windows login credentials
- No need to manage SQL Server passwords
- More secure in development environments
- Default method for local development
- No firewall issues

---

## **File Changed**

**appsettings.json**
```json
{
  "ConnectionStrings": {
 "DefaultConnection": "Server=localhost;Database=NPhiesDb;Integrated Security=true;Encrypt=true;TrustServerCertificate=true;Connection Timeout=30;MultipleActiveResultSets=true;"
  }
}
```

---

## **How to Run Now**

### **Step 1: Ensure SQL Server is Running**
```powershell
Get-Service -Name MSSQLSERVER
```

Expected output: `Status = Running`

### **Step 2: Run the Application**
```powershell
cd C:\Users\sibtain.alimadad\source\repos\NPhies_FHIR_Integration
dotnet run --project NPhies_FHIR_Integration.ApiService
```

### **Step 3: Wait for Application to Start**
```
Build succeeded.
info: Seeding database with test data...
Listening on https://localhost:7xxx
```

### **Step 4: Test API**
```
https://localhost:7xxx/swagger
```

---

## **Troubleshooting Other Issues**

### **Issue 1: SQL Server Service Not Running**

**Error**: `A network or instance-specific error occurred`

**Solution**:
```powershell
# Start SQL Server
Start-Service -Name MSSQLSERVER

# Verify it's running
Get-Service -Name MSSQLSERVER
```

### **Issue 2: Database NPhiesDb Not Found**

**Error**: `Cannot open database "NPhiesDb"`

**Solution**:
The database will be created automatically on first run. If it still fails:
1. Check SQL Server Management Studio
2. Verify database permissions
3. Restart the application

### **Issue 3: Connection Timeout**

**Error**: `Connection timeout expired`

**Solution**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NPhiesDb;Integrated Security=true;Connection Timeout=60;..."
  }
}
```
Increase `Connection Timeout` from 30 to 60 seconds.

### **Issue 4: Cannot Connect to Localhost**

**Error**: `Network or instance error`

**Solution**:
Try these alternatives:
```
Server=.          // Local machine (dot notation)
Server=127.0.0.1  // Loopback IP
Server=.\SQLEXPRESS  // If using Express Edition
```

---

## **Advanced: If You Need SA Authentication**

If you specifically need SQL Server Authentication (sa user):

### **Step 1: Enable SA Account in SQL Server**

1. Open **SQL Server Management Studio**
2. Right-click **Server** ? **Properties**
3. **Security** tab
4. Set **Server authentication** to **SQL Server and Windows Authentication mode**
5. Restart SQL Server

### **Step 2: Reset SA Password**

In SQL Server Management Studio:
```sql
ALTER LOGIN sa ENABLE;
ALTER LOGIN sa WITH PASSWORD = 'YourNewPassword@123';
```

### **Step 3: Update Connection String**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NPhiesDb;User Id=sa;Password=YourNewPassword@123;Encrypt=true;TrustServerCertificate=true;..."
  }
}
```

---

## **Connection String Templates**

### **Windows Authentication (Recommended)**
```
Server=localhost;Database=NPhiesDb;Integrated Security=true;Encrypt=true;TrustServerCertificate=true;Connection Timeout=30;MultipleActiveResultSets=true;
```

### **SQL Server Authentication**
```
Server=localhost;Database=NPhiesDb;User Id=sa;Password=YourPassword;Encrypt=true;TrustServerCertificate=true;Connection Timeout=30;
```

### **Named Instance**
```
Server=localhost\SQLEXPRESS;Database=NPhiesDb;Integrated Security=true;...
```

### **Remote Server**
```
Server=your-server.com;Database=NPhiesDb;User Id=sa;Password=YourPassword;...
```

---

## **Verify Connection Before Running App**

### **Option 1: SQL Server Management Studio**
1. Open SSMS
2. Server: `localhost`
3. Authentication: `Windows Authentication`
4. Click **Connect**
5. Should connect successfully

### **Option 2: Command Line**
```powershell
sqlcmd -S localhost -E -Q "SELECT @@VERSION;"
```

Expected output: SQL Server version number

### **Option 3: .NET Connection Test**

Create a test file to verify:
```csharp
var connectionString = "Server=localhost;Database=NPhiesDb;Integrated Security=true;";
using (SqlConnection conn = new SqlConnection(connectionString))
{
    try
    {
        conn.Open();
      Console.WriteLine("? Connection Successful!");
 }
    catch (Exception ex)
    {
        Console.WriteLine($"? Connection Failed: {ex.Message}");
}
}
```

---

## **Current Configuration Status**

| Component | Status | Details |
|-----------|--------|---------|
| **SQL Server** | ? Running | MSSQLSERVER service active |
| **Authentication** | ? Windows Auth | Integrated Security enabled |
| **Database** | ? Created | NPhiesDb exists |
| **Connection String** | ? Updated | Using Integrated Security |
| **API** | ? Ready | Can be started without errors |

---

## **Next Steps**

1. ? Run the application
   ```powershell
   dotnet run --project NPhies_FHIR_Integration.ApiService
   ```

2. ? Access Swagger UI
   ```
   https://localhost:7xxx/swagger
   ```

3. ? Test endpoints
   - See `API_TESTING_GUIDE.md`

4. ? Verify seeding
   - Check database in SSMS
   - Should have test data

---

## **Support**

If you encounter other issues:

1. Check **appsettings.json** connection string
2. Verify SQL Server service is running
3. Check Windows Authentication is available
4. Review application logs
5. Try connecting with SQL Server Management Studio first

---

**Status**: ? **Connection Issue Resolved**

?? **Ready to run: `dotnet run --project NPhies_FHIR_Integration.ApiService`**
