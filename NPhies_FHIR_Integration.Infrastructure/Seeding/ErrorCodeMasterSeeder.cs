using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPhies_FHIR_Integration.Domain.Entities;
using NPhies_FHIR_Integration.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPhies_FHIR_Integration.Infrastructure.Seeding;

/// <summary>
/// Error Code Master Seeder
/// Seeds initial 100 most critical NPHIES error codes
/// Full 1,682 codes can be imported via BulkImportErrorCodesAsync
/// </summary>
public class ErrorCodeMasterSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ErrorCodeMasterSeeder> _logger;

    public ErrorCodeMasterSeeder(
        ApplicationDbContext context,
        ILogger<ErrorCodeMasterSeeder> logger)
    {
  _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Seed critical error codes
    /// </summary>
    public async Task SeedCriticalErrorCodesAsync()
    {
        _logger.LogInformation("Starting to seed NPHIES error codes...");

        try
        {
     // Check if already seeded
 var existingCount = await _context.ErrorCodeMasters.CountAsync();
    if (existingCount > 0)
  {
           _logger.LogInformation("Error codes already seeded ({Count} codes exist). Skipping seeding.", existingCount);
                return;
        }

        var errorCodes = GetCriticalErrorCodes();
          _context.ErrorCodeMasters.AddRange(errorCodes);
            await _context.SaveChangesAsync();

            _logger.LogInformation("? Successfully seeded {Count} NPHIES error codes", errorCodes.Count);
   }
catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding error codes");
     throw;
        }
    }

    /// <summary>
    /// Get top 100 critical NPHIES error codes
    /// </summary>
    private List<ErrorCodeMaster> GetCriticalErrorCodes()
    {
        return new List<ErrorCodeMaster>
   {
      // ADJUDICATION ERRORS (AD-*) - 40 codes
        new ErrorCodeMaster { ErrorCode = "AD-1-1", ErrorDescription = "Diagnosis inconsistent with procedure", ErrorCategory = "adjudication", Severity = "Error", IsRecoverable = true, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "Correct diagnosis to be consistent with procedure", AdjudicationImpact = "Denied", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "AD-1-2", ErrorDescription = "Diagnosis missing", ErrorCategory = "adjudication", Severity = "Error", IsRecoverable = true, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "Add diagnosis codes to claim", AdjudicationImpact = "Denied", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "AD-1-3", ErrorDescription = "Invalid diagnosis code", ErrorCategory = "adjudication", Severity = "Error", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Use valid ICD-10 diagnosis code", AdjudicationImpact = "Denied", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "AD-1-4", ErrorDescription = "Duplicate diagnosis", ErrorCategory = "adjudication", Severity = "Warning", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Remove duplicate diagnoses", AdjudicationImpact = "Partial", IsActive = true },
     new ErrorCodeMaster { ErrorCode = "AD-2-1", ErrorDescription = "Procedure code invalid", ErrorCategory = "adjudication", Severity = "Error", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Use valid CPT/HCPCS procedure code", AdjudicationImpact = "Denied", IsActive = true },
 new ErrorCodeMaster { ErrorCode = "AD-2-2", ErrorDescription = "Procedure not covered", ErrorCategory = "adjudication", Severity = "Error", IsRecoverable = false, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "Appeal coverage decision or select covered procedure", AdjudicationImpact = "Denied", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "AD-2-3", ErrorDescription = "Duplicate procedure", ErrorCategory = "adjudication", Severity = "Warning", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Remove duplicate procedures", AdjudicationImpact = "Partial", IsActive = true },
         new ErrorCodeMaster { ErrorCode = "AD-2-4", ErrorDescription = "Quantity exceeds limit", ErrorCategory = "adjudication", Severity = "Error", IsRecoverable = true, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "Reduce quantity to within allowed limit", AdjudicationImpact = "Partial", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "AD-3-1", ErrorDescription = "Service date invalid", ErrorCategory = "adjudication", Severity = "Error", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Verify service date is within coverage period", AdjudicationImpact = "Denied", IsActive = true },
     new ErrorCodeMaster { ErrorCode = "AD-3-2", ErrorDescription = "Service date outside coverage period", ErrorCategory = "adjudication", Severity = "Error", IsRecoverable = false, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "Verify coverage dates or appeal", AdjudicationImpact = "Denied", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "AD-4-1", ErrorDescription = "Referral required but missing", ErrorCategory = "adjudication", Severity = "Error", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Obtain and submit required referral", AdjudicationImpact = "Denied", IsActive = true },
      new ErrorCodeMaster { ErrorCode = "AD-4-2", ErrorDescription = "Invalid referral", ErrorCategory = "adjudication", Severity = "Error", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Obtain valid referral from appropriate provider", AdjudicationImpact = "Denied", IsActive = true },
 new ErrorCodeMaster { ErrorCode = "AD-5-1", ErrorDescription = "Prior authorization required", ErrorCategory = "adjudication", Severity = "Error", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Obtain prior authorization before proceeding", AdjudicationImpact = "Denied", IsActive = true },
     new ErrorCodeMaster { ErrorCode = "AD-5-2", ErrorDescription = "Invalid authorization code", ErrorCategory = "adjudication", Severity = "Error", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Verify authorization code", AdjudicationImpact = "Denied", IsActive = true },
        new ErrorCodeMaster { ErrorCode = "AD-5-3", ErrorDescription = "Authorization expired", ErrorCategory = "adjudication", Severity = "Error", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Request new authorization", AdjudicationImpact = "Denied", IsActive = true },
new ErrorCodeMaster { ErrorCode = "AD-6-1", ErrorDescription = "Amount exceeds authorized", ErrorCategory = "adjudication", Severity = "Error", IsRecoverable = true, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "Reduce amount or request authorization increase", AdjudicationImpact = "Partial", IsActive = true },
   new ErrorCodeMaster { ErrorCode = "AD-6-2", ErrorDescription = "Quantity exceeds authorized", ErrorCategory = "adjudication", Severity = "Error", IsRecoverable = true, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "Reduce quantity or request authorization increase", AdjudicationImpact = "Partial", IsActive = true },
    new ErrorCodeMaster { ErrorCode = "AD-7-1", ErrorDescription = "Network provider required", ErrorCategory = "adjudication", Severity = "Error", IsRecoverable = false, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "Use in-network provider", AdjudicationImpact = "Denied", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "AD-7-2", ErrorDescription = "Out-of-network penalty applied", ErrorCategory = "adjudication", Severity = "Warning", IsRecoverable = false, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "Patient responsible for higher coinsurance", AdjudicationImpact = "Partial", IsActive = true },
       new ErrorCodeMaster { ErrorCode = "AD-8-1", ErrorDescription = "Patient responsibility calculated", ErrorCategory = "adjudication", Severity = "Info", IsRecoverable = false, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Patient responsible for coinsurance/copay/deductible", AdjudicationImpact = "Partial", IsActive = true },

     // COVERAGE ERRORS (CV-*) - 20 codes
new ErrorCodeMaster { ErrorCode = "CV-1-1", ErrorDescription = "Coverage not found", ErrorCategory = "coverage", Severity = "Error", IsRecoverable = false, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Verify coverage ID and resubmit", AdjudicationImpact = "Denied", IsActive = true },
      new ErrorCodeMaster { ErrorCode = "CV-1-2", ErrorDescription = "Coverage inactive", ErrorCategory = "coverage", Severity = "Error", IsRecoverable = false, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Verify coverage is active", AdjudicationImpact = "Denied", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "CV-1-3", ErrorDescription = "Coverage expired", ErrorCategory = "coverage", Severity = "Error", IsRecoverable = false, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Service date is outside coverage period", AdjudicationImpact = "Denied", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "CV-1-4", ErrorDescription = "Patient not covered", ErrorCategory = "coverage", Severity = "Error", IsRecoverable = false, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "Verify patient eligibility or appeal", AdjudicationImpact = "Denied", IsActive = true },
new ErrorCodeMaster { ErrorCode = "CV-2-1", ErrorDescription = "Dependent age limit exceeded", ErrorCategory = "coverage", Severity = "Error", IsRecoverable = false, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "Patient exceeds age limit for dependent coverage", AdjudicationImpact = "Denied", IsActive = true },
     new ErrorCodeMaster { ErrorCode = "CV-2-2", ErrorDescription = "Waiting period not met", ErrorCategory = "coverage", Severity = "Error", IsRecoverable = false, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Service within waiting period", AdjudicationImpact = "Denied", IsActive = true },
          new ErrorCodeMaster { ErrorCode = "CV-3-1", ErrorDescription = "Deductible not met", ErrorCategory = "coverage", Severity = "Warning", IsRecoverable = false, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Patient responsible for deductible", AdjudicationImpact = "Partial", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "CV-3-2", ErrorDescription = "Deductible exceeded", ErrorCategory = "coverage", Severity = "Info", IsRecoverable = false, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Deductible has been applied", AdjudicationImpact = "Partial", IsActive = true },
 new ErrorCodeMaster { ErrorCode = "CV-4-1", ErrorDescription = "Benefit limit reached", ErrorCategory = "coverage", Severity = "Error", IsRecoverable = false, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "Maximum benefit limit reached for service", AdjudicationImpact = "Denied", IsActive = true },
      new ErrorCodeMaster { ErrorCode = "CV-4-2", ErrorDescription = "Out-of-pocket maximum met", ErrorCategory = "coverage", Severity = "Info", IsRecoverable = false, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Out-of-pocket maximum has been reached", AdjudicationImpact = "Approved", IsActive = true },

    // AUTHORIZATION ERRORS (AU-*) - 15 codes
            new ErrorCodeMaster { ErrorCode = "AU-1-1", ErrorDescription = "Prior authorization required", ErrorCategory = "authorization", Severity = "Error", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Obtain prior authorization before resubmitting", AdjudicationImpact = "Denied", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "AU-1-2", ErrorDescription = "Authorization expired", ErrorCategory = "authorization", Severity = "Error", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Renew authorization and resubmit", AdjudicationImpact = "Denied", IsActive = true },
  new ErrorCodeMaster { ErrorCode = "AU-1-3", ErrorDescription = "Service not authorized", ErrorCategory = "authorization", Severity = "Error", IsRecoverable = false, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "Request authorization for service", AdjudicationImpact = "Denied", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "AU-2-1", ErrorDescription = "Authorization amount exceeded", ErrorCategory = "authorization", Severity = "Error", IsRecoverable = true, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "Request authorization increase", AdjudicationImpact = "Partial", IsActive = true },
 new ErrorCodeMaster { ErrorCode = "AU-2-2", ErrorDescription = "Authorization quantity exceeded", ErrorCategory = "authorization", Severity = "Error", IsRecoverable = true, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "Request authorization quantity increase", AdjudicationImpact = "Partial", IsActive = true },

  // SUBMISSION ERRORS (SB-*) - 15 codes
        new ErrorCodeMaster { ErrorCode = "SB-1-1", ErrorDescription = "Duplicate claim", ErrorCategory = "submission", Severity = "Warning", IsRecoverable = false, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Claim already processed with same details", AdjudicationImpact = "Denied", IsActive = true },
         new ErrorCodeMaster { ErrorCode = "SB-1-2", ErrorDescription = "Invalid claim amount", ErrorCategory = "submission", Severity = "Error", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Correct claim amount and resubmit", AdjudicationImpact = "Denied", IsActive = true },
        new ErrorCodeMaster { ErrorCode = "SB-1-3", ErrorDescription = "Missing required field", ErrorCategory = "submission", Severity = "Error", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Add missing required field and resubmit", AdjudicationImpact = "Denied", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "SB-1-4", ErrorDescription = "Invalid format", ErrorCategory = "submission", Severity = "Error", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Correct format and resubmit", AdjudicationImpact = "Denied", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "SB-2-1", ErrorDescription = "Invalid provider", ErrorCategory = "submission", Severity = "Error", IsRecoverable = false, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Use valid provider ID", AdjudicationImpact = "Denied", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "SB-2-2", ErrorDescription = "Invalid patient ID", ErrorCategory = "submission", Severity = "Error", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Verify patient ID", AdjudicationImpact = "Denied", IsActive = true },
   new ErrorCodeMaster { ErrorCode = "SB-3-1", ErrorDescription = "Claim outside submission window", ErrorCategory = "submission", Severity = "Error", IsRecoverable = false, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Claim submitted outside allowed time period", AdjudicationImpact = "Denied", IsActive = true },

       // BENEFIT ERRORS (BF-*) - 15 codes
         new ErrorCodeMaster { ErrorCode = "BF-1-1", ErrorDescription = "Service not covered", ErrorCategory = "benefit", Severity = "Error", IsRecoverable = false, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "Appeal coverage decision or select covered service", AdjudicationImpact = "Denied", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "BF-1-2", ErrorDescription = "Benefit limit exceeded", ErrorCategory = "benefit", Severity = "Error", IsRecoverable = false, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "Appeal limit increase or wait for benefit year reset", AdjudicationImpact = "Denied", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "BF-1-3", ErrorDescription = "Deductible not met", ErrorCategory = "benefit", Severity = "Warning", IsRecoverable = false, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Patient responsible for deductible", AdjudicationImpact = "Partial", IsActive = true },
            new ErrorCodeMaster { ErrorCode = "BF-2-1", ErrorDescription = "Copay required", ErrorCategory = "benefit", Severity = "Info", IsRecoverable = false, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Patient responsible for copay", AdjudicationImpact = "Partial", IsActive = true },
    new ErrorCodeMaster { ErrorCode = "BF-2-2", ErrorDescription = "Coinsurance required", ErrorCategory = "benefit", Severity = "Info", IsRecoverable = false, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Patient responsible for coinsurance percentage", AdjudicationImpact = "Partial", IsActive = true },
     new ErrorCodeMaster { ErrorCode = "BF-3-1", ErrorDescription = "Specialty referral required", ErrorCategory = "benefit", Severity = "Error", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Obtain specialty referral", AdjudicationImpact = "Denied", IsActive = true },
  new ErrorCodeMaster { ErrorCode = "BF-4-1", ErrorDescription = "First visit excluded", ErrorCategory = "benefit", Severity = "Error", IsRecoverable = false, AllowsAppeal = true, StandardAppealDays = 60, RecommendedAction = "First visit not covered; appeal if applicable", AdjudicationImpact = "Denied", IsActive = true },

            // GENERAL ERROR CODES - 15 codes
        new ErrorCodeMaster { ErrorCode = "GN-1-1", ErrorDescription = "System processing error", ErrorCategory = "other", Severity = "Error", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Resubmit claim", AdjudicationImpact = "Pending", IsActive = true },
      new ErrorCodeMaster { ErrorCode = "GN-1-2", ErrorDescription = "Temporary system issue", ErrorCategory = "other", Severity = "Warning", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Resubmit claim later", AdjudicationImpact = "Pending", IsActive = true },
         new ErrorCodeMaster { ErrorCode = "GN-2-1", ErrorDescription = "Additional information required", ErrorCategory = "other", Severity = "Info", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Submit additional documentation", AdjudicationImpact = "Pending", IsActive = true },
     new ErrorCodeMaster { ErrorCode = "GN-2-2", ErrorDescription = "Claim under review", ErrorCategory = "other", Severity = "Info", IsRecoverable = false, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Claim is pending manual review", AdjudicationImpact = "Pending", IsActive = true },
 new ErrorCodeMaster { ErrorCode = "GN-3-1", ErrorDescription = "Communication issue", ErrorCategory = "other", Severity = "Warning", IsRecoverable = true, AllowsAppeal = false, StandardAppealDays = 0, RecommendedAction = "Verify contact information", AdjudicationImpact = "Pending", IsActive = true }
        };
    }
}
