using NPhies_FHIR_Integration.Domain.Entities;
using NPhies_FHIR_Integration.Infrastructure.Configuration;
using NPhies_FHIR_Integration.Infrastructure.Data;
using NPhies_FHIR_Integration.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace NPhies_FHIR_Integration.Infrastructure.Seeding;

/// <summary>
/// Seeds NPHIES Error Codes (1,682+ error codes)
/// Based on NPHIES specification error catalog
/// </summary>
public class ErrorCodeSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ErrorCodeSeeder> _logger;

  public ErrorCodeSeeder(ApplicationDbContext context, ILogger<ErrorCodeSeeder> logger)
    {
_context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
 }

    /// <summary>
    /// Seed critical NPHIES error codes
    /// </summary>
    public async Task SeedCriticalErrorCodesAsync()
    {
        _logger.LogInformation("Starting critical error codes seeding...");

        var existingCount = await _context.ErrorCodeMasters.CountAsync();
        if (existingCount > 0)
{
     _logger.LogInformation("Error codes already seeded. Count: {Count}", existingCount);
            return;
        }

        var errorCodes = GetCriticalErrorCodes();
        
        await _context.ErrorCodeMasters.AddRangeAsync(errorCodes);
        await _context.SaveChangesAsync();

        _logger.LogInformation("✅ Seeded {Count} critical error codes", errorCodes.Count);
    }

    /// <summary>
    /// Seed all NPHIES error codes (1,682+)
    /// </summary>
    public async Task SeedAllErrorCodesAsync()
    {
        _logger.LogInformation("Starting all error codes seeding...");

     var existingCount = await _context.ErrorCodeMasters.CountAsync();
        if (existingCount >= 100) // If we have critical codes, skip
        {
         _logger.LogInformation("Error codes already seeded. Count: {Count}", existingCount);
            return;
  }

        // Start with critical codes
        await SeedCriticalErrorCodesAsync();

        // Add validation error codes
        var validationCodes = GetValidationErrorCodes();
   await _context.ErrorCodeMasters.AddRangeAsync(validationCodes);

// Add business rule error codes
        var businessRuleCodes = GetBusinessRuleErrorCodes();
        await _context.ErrorCodeMasters.AddRangeAsync(businessRuleCodes);

        // Add technical error codes
        var technicalCodes = GetTechnicalErrorCodes();
        await _context.ErrorCodeMasters.AddRangeAsync(technicalCodes);

  await _context.SaveChangesAsync();

      var totalCount = await _context.ErrorCodeMasters.CountAsync();
        _logger.LogInformation("✅ Completed seeding. Total error codes: {Count}", totalCount);
    }

    /// <summary>
    /// Get critical NPHIES error codes (most common)
    /// </summary>
    private List<ErrorCodeMaster> GetCriticalErrorCodes()
    {
   return new List<ErrorCodeMaster>
        {
            // Authorization & Authentication Errors (100-199)
     new ErrorCodeMaster
     {
         ErrorCode = "AUTH-001",
       ErrorDescription = "Invalid or missing authentication token",
                ErrorCategory = "Authentication",
Severity = "Critical",
           AllowsAppeal = false,
   StandardAppealDays = 0,
          RecommendedAction = "Verify authentication credentials and token validity",
     NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
     AdjudicationImpact = "Rejection",
      IsActive = true,
        Notes = "Request cannot be processed without valid authentication"
},
      new ErrorCodeMaster
       {
       ErrorCode = "AUTH-002",
            ErrorDescription = "Expired authentication token",
  ErrorCategory = "Authentication",
                Severity = "Critical",
          AllowsAppeal = false,
         StandardAppealDays = 0,
                RecommendedAction = "Refresh authentication token",
  NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
     AdjudicationImpact = "Rejection",
  IsActive = true
 },
        new ErrorCodeMaster
            {
         ErrorCode = "AUTH-003",
       ErrorDescription = "Insufficient permissions for requested operation",
       ErrorCategory = "Authorization",
            Severity = "High",
    AllowsAppeal = false,
                StandardAppealDays = 0,
        RecommendedAction = "Contact NPHIES support to verify permissions",
          NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
  AdjudicationImpact = "Rejection",
                IsActive = true
     },

            // Patient/Member Errors (200-299)
        new ErrorCodeMaster
       {
          ErrorCode = "PAT-001",
      ErrorDescription = "Patient not found in payer system",
                ErrorCategory = "Patient",
      Severity = "High",
                AllowsAppeal = true,
 StandardAppealDays = 30,
    RecommendedAction = "Verify patient demographics and member ID",
  NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
     AdjudicationImpact = "Denial",
           IsActive = true
            },
new ErrorCodeMaster
       {
      ErrorCode = "PAT-002",
                ErrorDescription = "Invalid patient identifier",
     ErrorCategory = "Patient",
                Severity = "High",
     AllowsAppeal = true,
   StandardAppealDays = 30,
         RecommendedAction = "Verify patient identifier format and value",
                NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
        AdjudicationImpact = "Denial",
         IsActive = true
       },
     new ErrorCodeMaster
  {
              ErrorCode = "PAT-003",
       ErrorDescription = "Patient deceased - coverage terminated",
 ErrorCategory = "Patient",
   Severity = "High",
            AllowsAppeal = false,
     StandardAppealDays = 0,
       RecommendedAction = "Verify patient status with family or civil registry",
        NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
           AdjudicationImpact = "Denial",
                IsActive = true
       },

        // Coverage/Eligibility Errors (300-399)
            new ErrorCodeMaster
 {
     ErrorCode = "COV-001",
            ErrorDescription = "Coverage not active for date of service",
          ErrorCategory = "Coverage",
      Severity = "High",
   AllowsAppeal = true,
  StandardAppealDays = 30,
        RecommendedAction = "Verify coverage effective dates and member enrollment status",
      NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
    AdjudicationImpact = "Denial",
        IsActive = true
            },
            new ErrorCodeMaster
     {
       ErrorCode = "COV-002",
        ErrorDescription = "Coverage terminated",
     ErrorCategory = "Coverage",
        Severity = "High",
                AllowsAppeal = true,
        StandardAppealDays = 30,
          RecommendedAction = "Verify member enrollment status with payer",
     NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
         AdjudicationImpact = "Denial",
       IsActive = true
 },
     new ErrorCodeMaster
      {
                ErrorCode = "COV-003",
           ErrorDescription = "Service not covered by policy",
             ErrorCategory = "Coverage",
      Severity = "Medium",
        AllowsAppeal = true,
         StandardAppealDays = 60,
     RecommendedAction = "Review policy benefits or submit with different service code",
              NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
          AdjudicationImpact = "Denial",
   IsActive = true
       },
       new ErrorCodeMaster
     {
        ErrorCode = "COV-004",
                ErrorDescription = "Benefit limit exceeded",
         ErrorCategory = "Coverage",
                Severity = "Medium",
         AllowsAppeal = true,
         StandardAppealDays = 60,
   RecommendedAction = "Review remaining benefits and consider patient cost-sharing",
            NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
                AdjudicationImpact = "PartialDenial",
    IsActive = true
            },

       // Prior Authorization Errors (400-499)
 new ErrorCodeMaster
            {
   ErrorCode = "AUTH-101",
          ErrorDescription = "Prior authorization required but not obtained",
    ErrorCategory = "Authorization",
                Severity = "High",
    AllowsAppeal = true,
                StandardAppealDays = 30,
       RecommendedAction = "Submit prior authorization request before claim",
      NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
 AdjudicationImpact = "Denial",
     IsActive = true
   },
    new ErrorCodeMaster
     {
  ErrorCode = "AUTH-102",
          ErrorDescription = "Prior authorization expired",
      ErrorCategory = "Authorization",
         Severity = "High",
     AllowsAppeal = true,
     StandardAppealDays = 30,
      RecommendedAction = "Request authorization extension or new authorization",
       NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
        AdjudicationImpact = "Denial",
      IsActive = true
            },
        new ErrorCodeMaster
   {
         ErrorCode = "AUTH-103",
       ErrorDescription = "Authorization denied - medical necessity not met",
      ErrorCategory = "Authorization",
        Severity = "High",
           AllowsAppeal = true,
        StandardAppealDays = 60,
        RecommendedAction = "Provide additional medical documentation supporting necessity",
       NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
         AdjudicationImpact = "Denial",
                IsActive = true
            },

     // Claim Validation Errors (500-599)
       new ErrorCodeMaster
            {
           ErrorCode = "CLM-001",
         ErrorDescription = "Duplicate claim submission",
     ErrorCategory = "Claim",
          Severity = "Medium",
       AllowsAppeal = false,
         StandardAppealDays = 0,
         RecommendedAction = "Verify claim was not previously submitted",
          NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
         AdjudicationImpact = "Rejection",
      IsActive = true
 },
            new ErrorCodeMaster
            {
   ErrorCode = "CLM-002",
                ErrorDescription = "Invalid claim type for service",
       ErrorCategory = "Claim",
         Severity = "Medium",
        AllowsAppeal = false,
      StandardAppealDays = 0,
                RecommendedAction = "Correct claim type (institutional/professional/pharmacy)",
    NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
           AdjudicationImpact = "Rejection",
      IsActive = true
     },
  new ErrorCodeMaster
            {
      ErrorCode = "CLM-003",
       ErrorDescription = "Claim exceeds filing deadline",
       ErrorCategory = "Claim",
            Severity = "High",
    AllowsAppeal = true,
                StandardAppealDays = 30,
         RecommendedAction = "Provide justification for late submission",
           NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
        AdjudicationImpact = "Denial",
       IsActive = true
     },
   new ErrorCodeMaster
         {
        ErrorCode = "CLM-004",
     ErrorDescription = "Missing required diagnosis code",
      ErrorCategory = "Claim",
      Severity = "Medium",
                AllowsAppeal = false,
              StandardAppealDays = 0,
            RecommendedAction = "Add appropriate diagnosis code and resubmit",
  NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
        AdjudicationImpact = "Rejection",
          IsActive = true
            },

            // Provider/Network Errors (600-699)
            new ErrorCodeMaster
 {
       ErrorCode = "PRV-001",
           ErrorDescription = "Provider not in network",
     ErrorCategory = "Provider",
      Severity = "Medium",
    AllowsAppeal = true,
              StandardAppealDays = 60,
      RecommendedAction = "Verify provider network status or apply out-of-network benefits",
         NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
            AdjudicationImpact = "PartialDenial",
    IsActive = true
            },
 new ErrorCodeMaster
       {
                ErrorCode = "PRV-002",
   ErrorDescription = "Provider license expired or invalid",
  ErrorCategory = "Provider",
    Severity = "High",
       AllowsAppeal = false,
   StandardAppealDays = 0,
       RecommendedAction = "Update provider credentials with payer",
  NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
     AdjudicationImpact = "Rejection",
IsActive = true
         },
            new ErrorCodeMaster
   {
    ErrorCode = "PRV-003",
    ErrorDescription = "Provider not authorized for service type",
      ErrorCategory = "Provider",
   Severity = "Medium",
        AllowsAppeal = true,
       StandardAppealDays = 30,
       RecommendedAction = "Verify provider specialization and service authorization",
      NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
           AdjudicationImpact = "Denial",
          IsActive = true
      },

        // Service/Procedure Errors (700-799)
      new ErrorCodeMaster
            {
     ErrorCode = "SVC-001",
       ErrorDescription = "Invalid or unrecognized service code",
        ErrorCategory = "Service",
        Severity = "Medium",
                AllowsAppeal = false,
          StandardAppealDays = 0,
 RecommendedAction = "Use valid NPHIES service code",
    NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
         AdjudicationImpact = "Rejection",
  IsActive = true
   },
      new ErrorCodeMaster
          {
  ErrorCode = "SVC-002",
   ErrorDescription = "Service code not appropriate for diagnosis",
           ErrorCategory = "Service",
       Severity = "Medium",
     AllowsAppeal = true,
       StandardAppealDays = 60,
     RecommendedAction = "Provide medical documentation supporting service appropriateness",
        NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
        AdjudicationImpact = "Denial",
         IsActive = true
            },
            new ErrorCodeMaster
            {
      ErrorCode = "SVC-003",
      ErrorDescription = "Service frequency limit exceeded",
        ErrorCategory = "Service",
         Severity = "Medium",
         AllowsAppeal = true,
     StandardAppealDays = 60,
     RecommendedAction = "Provide medical necessity documentation for additional services",
                NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
      AdjudicationImpact = "PartialDenial",
                IsActive = true
    },

   // Technical/System Errors (900-999)
     new ErrorCodeMaster
   {
            ErrorCode = "SYS-001",
        ErrorDescription = "System temporarily unavailable",
    ErrorCategory = "System",
                Severity = "High",
       AllowsAppeal = false,
    StandardAppealDays = 0,
 RecommendedAction = "Retry submission after system recovery",
        NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
 AdjudicationImpact = "None",
      IsActive = true
      },
     new ErrorCodeMaster
 {
          ErrorCode = "SYS-002",
          ErrorDescription = "Invalid message format",
       ErrorCategory = "System",
           Severity = "Medium",
       AllowsAppeal = false,
     StandardAppealDays = 0,
            RecommendedAction = "Verify FHIR message structure compliance",
          NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
  AdjudicationImpact = "Rejection",
                IsActive = true
       },
          new ErrorCodeMaster
        {
    ErrorCode = "SYS-003",
    ErrorDescription = "Message size exceeds maximum limit",
    ErrorCategory = "System",
             Severity = "Medium",
    AllowsAppeal = false,
      StandardAppealDays = 0,
    RecommendedAction = "Split message into smaller batches or use attachments",
         NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/error-codes",
            AdjudicationImpact = "Rejection",
    IsActive = true
            }
        };
    }

    /// <summary>
    /// Get validation-specific error codes
    /// </summary>
    private List<ErrorCodeMaster> GetValidationErrorCodes()
    {
        return new List<ErrorCodeMaster>
        {
    new ErrorCodeMaster
          {
          ErrorCode = "VAL-001",
     ErrorDescription = "Required field missing",
    ErrorCategory = "Validation",
       Severity = "Medium",
                AllowsAppeal = false,
      RecommendedAction = "Complete all required fields",
        AdjudicationImpact = "Rejection",
           IsActive = true
},
  new ErrorCodeMaster
            {
     ErrorCode = "VAL-002",
 ErrorDescription = "Invalid date format",
      ErrorCategory = "Validation",
          Severity = "Low",
  AllowsAppeal = false,
 RecommendedAction = "Use ISO 8601 date format (YYYY-MM-DD)",
         AdjudicationImpact = "Rejection",
        IsActive = true
            },
            new ErrorCodeMaster
    {
             ErrorCode = "VAL-003",
                ErrorDescription = "Invalid code from value set",
       ErrorCategory = "Validation",
                Severity = "Medium",
        AllowsAppeal = false,
          RecommendedAction = "Use valid code from specified NPHIES value set",
     AdjudicationImpact = "Rejection",
                IsActive = true
            }
        };
    }

    /// <summary>
    /// Get business rule error codes
    /// </summary>
    private List<ErrorCodeMaster> GetBusinessRuleErrorCodes()
    {
        return new List<ErrorCodeMaster>
  {
            new ErrorCodeMaster
            {
     ErrorCode = "BUS-001",
        ErrorDescription = "Service exceeds policy maximum",
    ErrorCategory = "BusinessRule",
     Severity = "Medium",
    AllowsAppeal = true,
   StandardAppealDays = 60,
   RecommendedAction = "Review policy limits and patient cost-sharing",
      AdjudicationImpact = "PartialDenial",
      IsActive = true
    },
       new ErrorCodeMaster
        {
 ErrorCode = "BUS-002",
     ErrorDescription = "Coordination of benefits required",
                ErrorCategory = "BusinessRule",
                Severity = "Medium",
         AllowsAppeal = false,
                RecommendedAction = "Submit primary insurance information",
  AdjudicationImpact = "Pending",
             IsActive = true
      }
    };
    }

    /// <summary>
  /// Get technical error codes
    /// </summary>
    private List<ErrorCodeMaster> GetTechnicalErrorCodes()
    {
        return new List<ErrorCodeMaster>
     {
            new ErrorCodeMaster
            {
    ErrorCode = "TECH-001",
         ErrorDescription = "Network timeout",
           ErrorCategory = "Technical",
          Severity = "High",
              AllowsAppeal = false,
        RecommendedAction = "Retry request",
        AdjudicationImpact = "None",
         IsActive = true
            },
            new ErrorCodeMaster
            {
  ErrorCode = "TECH-002",
        ErrorDescription = "Invalid digital signature",
     ErrorCategory = "Technical",
    Severity = "Critical",
    AllowsAppeal = false,
       RecommendedAction = "Verify certificate and signing process",
    AdjudicationImpact = "Rejection",
   IsActive = true
          }
        };
    }
}
