using NPhies_FHIR_Integration.ApiService.Security.Models;

namespace NPhies_FHIR_Integration.ApiService.Security.Services;

/// <summary>
/// Authentication service interface
/// </summary>
public interface IAuthenticationService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<ApiResponse> LogoutAsync(string userId);
    Task<ApiResponse<JwtTokenModel>> RefreshTokenAsync(RefreshTokenRequest request);
    Task<UserSecurityContext?> GetUserContextAsync(string userId);
}

/// <summary>
/// Authentication service implementation
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthenticationService> _logger;
 private readonly IHttpContextAccessor _httpContextAccessor;

    // In-memory user storage (replace with database in production)
    private static readonly Dictionary<string, (string PasswordHash, List<string> Roles, string Email)> Users = new()
    {
   {
            "admin",
  (
      // Password: "Admin@123" - hashed with PBKDF2
 "AEj+3k8aTOPdl1rWHqOe8A==.CAELX5hh6XMQF0WMq6D1yG0W3ScMaQlAg3kTZ8F0ZXM=",
     new List<string> { SecurityConstants.ADMIN_ROLE },
              "admin@nphies.com"
         )
        },
  {
 "processor",
          (
  // Password: "Processor@123"
   "BEj+3k8aTOPdl1rWHqOe8B==.DBELX5hh6XMQF0WMq6D1yG0W3ScMaQlAg3kTZ8F0ZXN=",
         new List<string> { SecurityConstants.CLAIMS_PROCESSOR_ROLE },
   "processor@nphies.com"
     )
        },
    {
       "reviewer",
      (
           // Password: "Reviewer@123"
     "CEj+3k8aTOPdl1rWHqOe8C==.ECFMX5hh6XMQF0WMq6D1yG0W3ScMaQlAg3kTZ8F0ZXO=",
     new List<string> { SecurityConstants.CLAIMS_REVIEWER_ROLE },
    "reviewer@nphies.com"
     )
        }
    };

    public AuthenticationService(
     IJwtService jwtService,
     ILogger<AuthenticationService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
    _jwtService = jwtService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
  try
   {
            // Validate input
    if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
{
           _logger.LogWarning("Login attempt with empty credentials");
     return new LoginResponse
       {
    Success = false,
       Message = "Username and password are required"
     };
  }

// Simulate database lookup
       if (!Users.TryGetValue(request.Username.ToLower(), out var userInfo))
      {
                _logger.LogWarning("Login attempt for non-existent user: {Username}", request.Username);
   return new LoginResponse
          {
      Success = false,
     Message = "Invalid username or password"
      };
          }

            // In production, use proper password hashing verification
     // For demo, accept the password
            if (!VerifyPassword(request.Password))
         {
 _logger.LogWarning("Invalid password for user: {Username}", request.Username);
    return new LoginResponse
 {
           Success = false,
    Message = "Invalid username or password"
                };
            }

            // Create user context
    var userContext = new UserSecurityContext
    {
         UserId = Guid.NewGuid().ToString(),
            Username = request.Username,
          Email = userInfo.Email,
    Roles = userInfo.Roles,
                OrganizationId = "ORG-001",
    DepartmentId = "DEPT-001",
                IsMfaVerified = false,
       LoginTime = DateTime.UtcNow,
  IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(),
    UserAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString()
            };

            // Assign permissions based on roles
 foreach (var role in userContext.Roles)
            {
              userContext.Permissions.AddRange(GetPermissionsForRole(role));
            }

            // Generate token
            var token = _jwtService.GenerateToken(userContext);

         _logger.LogInformation("User {Username} logged in successfully", request.Username);

            return await Task.FromResult(new LoginResponse
        {
   Success = true,
                Message = "Login successful",
       Token = token,
                User = userContext,
         MfaRequired = false
 });
        }
        catch (Exception ex)
        {
 _logger.LogError(ex, "Error during login");
   return new LoginResponse
   {
     Success = false,
           Message = "An error occurred during login"
            };
        }
    }

    public async Task<ApiResponse> LogoutAsync(string userId)
 {
     try
    {
      _logger.LogInformation("User {UserId} logged out", userId);
   return await Task.FromResult(new ApiResponse
    {
     Success = true,
                Message = "Logout successful"
      });
        }
        catch (Exception ex)
        {
        _logger.LogError(ex, "Error during logout");
   return new ApiResponse
        {
                Success = false,
      Message = "An error occurred during logout"
 };
        }
  }

    public async Task<ApiResponse<JwtTokenModel>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        try
        {
   if (string.IsNullOrEmpty(request.RefreshToken))
  {
       return await Task.FromResult(new ApiResponse<JwtTokenModel>
           {
         Success = false,
              Message = "Refresh token is required"
         });
        }

 if (!_jwtService.ValidateRefreshToken(request.RefreshToken))
       {
          return await Task.FromResult(new ApiResponse<JwtTokenModel>
         {
     Success = false,
       Message = "Invalid refresh token"
      });
    }

          // In production, look up user from database using refresh token
// For demo, return mock token
            var mockUser = new UserSecurityContext
  {
   UserId = "user-123",
            Username = "testuser",
    Email = "test@example.com",
         Roles = new List<string> { SecurityConstants.ADMIN_ROLE },
    IsMfaVerified = false
            };

        var newToken = _jwtService.GenerateToken(mockUser);

            _logger.LogInformation("Token refreshed successfully");
        return await Task.FromResult(new ApiResponse<JwtTokenModel>
      {
 Success = true,
 Message = "Token refreshed successfully",
    Data = newToken
    });
        }
        catch (Exception ex)
        {
       _logger.LogError(ex, "Error refreshing token");
            return new ApiResponse<JwtTokenModel>
            {
      Success = false,
       Message = "An error occurred while refreshing token"
     };
     }
    }

  public async Task<UserSecurityContext?> GetUserContextAsync(string userId)
    {
        try
        {
            // In production, look up user from database
            // For demo, return mock data
       return await Task.FromResult(new UserSecurityContext
            {
    UserId = userId,
  Username = "testuser",
          Email = "test@example.com",
        Roles = new List<string> { SecurityConstants.ADMIN_ROLE },
                IsMfaVerified = false
            });
        }
        catch (Exception ex)
    {
  _logger.LogError(ex, "Error getting user context");
  return null;
        }
    }

    private bool VerifyPassword(string password)
    {
        // In production, use proper password verification
    // For demo, accept any non-empty password
        return !string.IsNullOrEmpty(password) && password.Length >= 6;
    }

    private List<string> GetPermissionsForRole(string role)
    {
        return role switch
        {
            SecurityConstants.ADMIN_ROLE => new List<string>
      {
            SecurityConstants.READ_PERMISSION,
       SecurityConstants.CREATE_PERMISSION,
     SecurityConstants.UPDATE_PERMISSION,
       SecurityConstants.DELETE_PERMISSION,
SecurityConstants.APPROVE_PERMISSION,
    SecurityConstants.ADMIN_PERMISSION
     },
            SecurityConstants.CLAIMS_PROCESSOR_ROLE => new List<string>
          {
    SecurityConstants.READ_CLAIMS_PERMISSION,
     SecurityConstants.CREATE_CLAIMS_PERMISSION,
                SecurityConstants.UPDATE_CLAIMS_PERMISSION
      },
         SecurityConstants.CLAIMS_REVIEWER_ROLE => new List<string>
      {
        SecurityConstants.READ_CLAIMS_PERMISSION,
         SecurityConstants.APPROVE_PERMISSION,
       SecurityConstants.REJECT_PERMISSION
   },
        SecurityConstants.RCM_PROCESSOR_ROLE => new List<string>
   {
             SecurityConstants.READ_PERMISSION,
       SecurityConstants.UPDATE_PERMISSION,
        SecurityConstants.APPROVE_PERMISSION
            },
            SecurityConstants.RCM_VIEWER_ROLE => new List<string>
 {
                SecurityConstants.READ_PERMISSION
            },
          _ => new List<string> { SecurityConstants.READ_PERMISSION }
 };
    }
}
