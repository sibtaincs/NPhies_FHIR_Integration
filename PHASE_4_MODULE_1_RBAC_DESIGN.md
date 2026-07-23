# ?? **PHASE 4 - MODULE 1: AUTHENTICATION & AUTHORIZATION (RBAC)**

**Status**: Ready for Implementation  
**Duration**: 4-5 hours  
**Complexity**: High  
**Priority**: Critical  

---

## ?? **MODULE 1 OVERVIEW**

### **What is RBAC?**

Role-Based Access Control (RBAC) is a security model where:
- Users are assigned **Roles**
- Roles have specific **Permissions**
- Permissions control what users can **do** in the system

### **Why RBAC?**

```
? Security: Enforce principle of least privilege
? Flexibility: Easy role management
? Scalability: Handles any user base
? Compliance: Required for enterprise systems
? Auditability: Track who has access to what
```

---

## ?? **PHASE 4, MODULE 1 ARCHITECTURE**

### **RBAC Components**

```
RBAC System
??? Roles
?   ??? Admin
?   ??? Manager
?   ??? Operator
?   ??? Viewer
?
??? Permissions
?   ??? Create
?   ??? Read
?   ??? Update
???? Delete
?   ??? Approve
?   ??? Export
?
??? Resources
?   ??? Claims
?   ??? Eligibility
? ??? Pre-Auth
?   ??? Reports
?   ??? Users
?
??? Access Control
    ??? Route Guards
  ??? Component Guards
   ??? Service Guards
    ??? Directives
```

### **Role Hierarchy**

```
ADMIN
??? Full system access
??? Can manage all users
??? Can view all data
??? Can perform all actions
??? Can view audit logs

MANAGER
??? Can view most modules
??? Can approve pre-auth
??? Can generate reports
??? Cannot manage users
??? Limited system settings

OPERATOR
??? Can create claims
??? Can submit pre-auth
??? Can view own records
??? Cannot approve
??? Limited data access

VIEWER
??? Read-only access
??? Can view reports
??? Can export data
??? Cannot edit data
??? Very limited access
```

---

## ?? **IMPLEMENTATION PLAN**

### **Step 1: Define Models & Interfaces (30 min)**

```typescript
// Role model
export interface Role {
  id: number;
  name: 'ADMIN' | 'MANAGER' | 'OPERATOR' | 'VIEWER';
  description: string;
  permissions: Permission[];
  userCount: number;
  createdAt: Date;
  updatedAt: Date;
}

// Permission model
export interface Permission {
  id: number;
  resource: 'CLAIMS' | 'ELIGIBILITY' | 'PREAUTH' | 'REPORTS' | 'USERS';
  action: 'CREATE' | 'READ' | 'UPDATE' | 'DELETE' | 'APPROVE' | 'EXPORT';
  description: string;
}

// User with roles
export interface User {
  id: number;
  username: string;
  email: string;
  roles: Role[];
  permissions: Permission[];
  status: 'ACTIVE' | 'INACTIVE' | 'SUSPENDED';
  lastLogin: Date;
}

// Access token payload
export interface TokenPayload {
  userId: number;
  username: string;
  roles: string[];
  permissions: string[];
  email: string;
  iat: number;
  exp: number;
}
```

### **Step 2: Create RBAC Service (1 hour)**

```typescript
// auth.service.ts
@Injectable({ providedIn: 'root' })
export class AuthService {
  private currentUser$ = new BehaviorSubject<User | null>(null);
  private currentRoles$ = new BehaviorSubject<Role[]>([]);
  private currentPermissions$ = new BehaviorSubject<Permission[]>([]);

  // Check if user has role
  hasRole(roleName: string): boolean { }

  // Check if user has permission
  hasPermission(resource: string, action: string): boolean { }

  // Check if user has any of the roles
  hasAnyRole(roles: string[]): boolean { }

  // Check if user has all permissions
  hasAllPermissions(permissions: string[]): boolean { }

  // Get current user
  getCurrentUser(): Observable<User | null> { }

  // Get current roles
  getCurrentRoles(): Observable<Role[]> { }

  // Get current permissions
  getCurrentPermissions(): Observable<Permission[]> { }
}
```

### **Step 3: Create Route Guard (30 min)**

```typescript
// auth.guard.ts
@Injectable({providedIn: 'root'})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
 route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    // Check if user authenticated
    // Check required roles
    // Check required permissions
    // Return true/false
  }
}

// Usage in routing:
{
  path: 'admin',
  component: AdminComponent,
  canActivate: [AuthGuard],
  data: { roles: ['ADMIN'] }
}
```

### **Step 4: Create Permission Directive (30 min)**

```typescript
// app-has-permission.directive.ts
@Directive({
  selector: '[appHasPermission]'
})
export class HasPermissionDirective implements OnInit {
  @Input() appHasPermission: string;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.authService.hasPermission(...).subscribe(hasPermission => {
 if (hasPermission) {
        this.viewContainer.createEmbeddedView(this.templateRef);
      } else {
   this.viewContainer.clear();
  }
  });
  }
}

// Usage:
<button *appHasPermission="'CLAIMS:CREATE'">Create Claim</button>
```

### **Step 5: Create Role Management Component (1.5 hours)**

```typescript
// role-management.component.ts
@Component({
  selector: 'app-role-management',
  templateUrl: './role-management.component.html',
  styleUrls: ['./role-management.component.less']
})
export class RoleManagementComponent implements OnInit {
  roles: Role[] = [];
  selectedRole: Role | null = null;
  loading = false;
  isEditMode = false;

  ngOnInit() {
    this.loadRoles();
  }

  loadRoles(): void {
    // Load all roles
  }

  selectRole(role: Role): void {
    // Select role for editing
  }

  createRole(): void {
    // Create new role
  }

  updateRole(role: Role): void {
    // Update selected role
  }

  deleteRole(roleId: number): void {
    // Delete role with confirmation
  }

  assignPermissions(role: Role, permissions: Permission[]): void {
    // Assign permissions to role
  }
}
```

### **Step 6: Create Permission Matrix Component (1 hour)**

```typescript
// permission-matrix.component.ts
@Component({
  selector: 'app-permission-matrix',
  templateUrl: './permission-matrix.component.html',
  styleUrls: ['./permission-matrix.component.less']
})
export class PermissionMatrixComponent implements OnInit {
  roles: Role[] = [];
  resources: string[] = ['CLAIMS', 'ELIGIBILITY', 'PREAUTH', 'REPORTS'];
  actions: string[] = ['CREATE', 'READ', 'UPDATE', 'DELETE', 'APPROVE', 'EXPORT'];
  permissionMatrix: Map<string, boolean[][]> = new Map();

  ngOnInit() {
    this.loadPermissionMatrix();
  }

  loadPermissionMatrix(): void {
    // Load current permission matrix
  }

  togglePermission(role: Role, resource: string, action: string): void {
    // Toggle permission
  }

  savePermissions(): void {
    // Save all permission changes
  }

  getMatrixForRole(role: Role): Permission[] {
    // Get permissions for specific role
  }
}
```

---

## ?? **COMPONENTS TO BUILD**

### **1. Role List Component** (400 lines)
- Display all roles
- List with actions
- Create/Edit/Delete
- Role statistics

### **2. Role Form Component** (300 lines)
- Role creation form
- Role editing form
- Permission selection
- Validation

### **3. Permission Matrix Component** (350 lines)
- Visual permission matrix
- Role-permission mapping
- Bulk permission assignment
- Permission preview

### **4. Access Control Service** (250 lines)
- Check permissions
- Check roles
- Permission caching
- Role management

### **5. Route & Component Guards** (200 lines)
- Route protection
- Component access control
- Unauthorized handling
- Guard logic

---

## ?? **UI/UX DESIGN**

### **Role Management Page**

```
????????????????????????????????????????????
? Role Management         ?
????????????????????????????????????????????
? [Create New Role]  ?
????????????????????????????????????????????
? Role List: ?
? ??????????????????????????????   ?
? ? Name      ? Permissions ? Users ? ?
? ??????????????????????????????   ?
? ? ADMIN     ? 20      ? 2   ? ? ? ?
? ? MANAGER   ? 15      ? 5   ? ? ? ?
? ? OPERATOR  ? 10      ? 20  ? ? ? ?
? ? VIEWER    ? 5       ? 50  ? ? ? ?
? ??????????????????????????????   ?
????????????????????????????????????????????
```

### **Permission Matrix**

```
???????????????????????????????????????????????
? Permission Matrix (Role: MANAGER)    ?
???????????????????????????????????????????????
? Resource  ? Create ? Read ? Update ? Delete ?
???????????????????????????????????????????????
? CLAIMS    ?   ?    ?  ?   ?   ?    ?   ?    ?
? ELIGI...  ?   ?    ?  ?   ?   ?    ?   ?    ?
? PREAUTH   ?   ?    ?  ?   ?   ?    ?   ?    ?
? REPORTS   ?   ? ?  ?   ?   ?    ??    ?
? USERS     ?   ?    ?  ?   ?   ?    ?   ?    ?
???????????????????????????????????????????????
```

---

## ?? **EXPECTED CODE METRICS**

```
Files to Create:
??? auth.service.ts           (250 lines)
??? auth.guard.ts         (150 lines)
??? permission.directive.ts    (100 lines)
??? role-management.component.ts    (200 lines)
??? role-form.component.ts    (200 lines)
??? permission-matrix.component.ts  (250 lines)
??? role-management.component.html  (150 lines)
??? Models & Interfaces   (100 lines)
??? Styling (.less files)      (200 lines)

Total: ~1,600 lines of code
```

---

## ? **IMPLEMENTATION CHECKLIST**

### **Phase 1: Core Infrastructure**
- [ ] Define RBAC models
- [ ] Create auth service
- [ ] Implement role checking
- [ ] Implement permission checking
- [ ] Create route guards
- [ ] Create permission directives

### **Phase 2: Management UI**
- [ ] Create role list component
- [ ] Create role form component
- [ ] Create permission matrix
- [ ] Add role management features
- [ ] Add permission assignment

### **Phase 3: Integration**
- [ ] Apply guards to routes
- [ ] Apply guards to components
- [ ] Apply directives to UI elements
- [ ] Test all access controls
- [ ] Test all permissions

### **Phase 4: Documentation**
- [ ] Document role definitions
- [ ] Document permission matrix
- [ ] Create usage guide
- [ ] Create admin guide
- [ ] Document best practices

---

## ?? **SUCCESS CRITERIA**

? All roles working correctly  
? All permissions enforced  
? Route guards preventing unauthorized access  
? Component guards hiding unauthorized features  
? Permission directives working  
? Role management UI functional  
? Permission matrix operational  
? Documentation complete  
? No security vulnerabilities  
? Performance optimized  

---

## ?? **NEXT STEPS**

### **Immediate (Hour 1)**
1. Create RBAC models/interfaces
2. Implement auth service
3. Create route guards

### **Next (Hour 2)**
1. Create permission directives
2. Create role management component
3. Create permission matrix

### **Final (Hour 3-5)**
1. Integration & testing
2. Documentation
3. Commit & push to Git

---

## ?? **CRITICAL IMPLEMENTATION NOTES**

### **Security Best Practices**
```
? Never trust client-side checks alone
? Always validate on backend
? Use JWT tokens properly
? Implement token refresh
? Hash and salt passwords
? Log all permission checks
? Audit role changes
? Implement session timeout
```

### **Performance Optimization**
```
? Cache permission checks
? Minimize API calls
? Use guards efficiently
? Lazy load permission data
? Optimize matrix queries
? Use memoization where possible
```

---

# **MODULE 1 DESIGN COMPLETE!**

**Ready to implement RBAC for enterprise security!** ??

---

**Status**: Design Complete  
**Duration**: 4-5 hours  
**Lines of Code**: ~1,600  
**Complexity**: High  
**Next**: Begin Implementation  
