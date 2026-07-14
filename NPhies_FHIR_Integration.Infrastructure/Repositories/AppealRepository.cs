using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NPhies_FHIR_Integration.Domain.Entities;
using NPhies_FHIR_Integration.Infrastructure.Data;

namespace NPhies_FHIR_Integration.Infrastructure.Repositories;

/// <summary>
/// Appeal Repository Implementation
/// Manages all data access for appeals
/// </summary>
public class AppealRepository : IAppealRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AppealRepository> _logger;

    public AppealRepository(
  ApplicationDbContext context,
    ILogger<AppealRepository> logger)
    {
  _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

    // ========== CREATE OPERATIONS ==========

    public async Task<AppealRequest> AddAsync(AppealRequest appeal, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding appeal {AppealNumber}", appeal.AppealNumber);
        _context.AppealRequests.Add(appeal);
     await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Appeal {AppealNumber} added successfully", appeal.AppealNumber);
        return appeal;
    }

  public async Task AddRangeAsync(IEnumerable<AppealRequest> appeals, CancellationToken cancellationToken = default)
    {
 _logger.LogInformation("Adding {Count} appeals in batch", appeals.Count());
        _context.AppealRequests.AddRange(appeals);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Batch of appeals added successfully");
    }

    // ========== READ OPERATIONS ==========

    public async Task<AppealRequest?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
return await _context.AppealRequests
      .Include(a => a.StatusHistory)
        .Include(a => a.AttachedDocuments)
  .AsNoTracking()
   .FirstOrDefaultAsync(a => a.Id == id && a.IsActive, cancellationToken);
    }

    public async Task<AppealRequest?> GetByAppealNumberAsync(string appealNumber, CancellationToken cancellationToken = default)
    {
 return await _context.AppealRequests
  .AsNoTracking()
          .FirstOrDefaultAsync(a => a.AppealNumber == appealNumber && a.IsActive, cancellationToken);
    }

    public async Task<List<AppealRequest>> GetAllAsync(CancellationToken cancellationToken = default)
{
return await _context.AppealRequests
         .AsNoTracking()
            .Where(a => a.IsActive)
            .ToListAsync(cancellationToken);
    }

    // ========== APPEALS BY RELATIONSHIP ==========

    public async Task<List<AppealRequest>> GetByClaimIdAsync(string claimId, CancellationToken cancellationToken = default)
    {
      return await _context.AppealRequests
.AsNoTracking()
    .Where(a => a.ClaimId == claimId && a.IsActive)
      .OrderByDescending(a => a.CreatedAt)
.ToListAsync(cancellationToken);
    }

    public async Task<List<AppealRequest>> GetByPatientIdAsync(string patientId, CancellationToken cancellationToken = default)
    {
        return await _context.AppealRequests
        .AsNoTracking()
    .Where(a => a.PatientId == patientId && a.IsActive)
            .OrderByDescending(a => a.CreatedAt)
      .ToListAsync(cancellationToken);
    }

    public async Task<List<AppealRequest>> GetByInsurerIdAsync(string insurerId, CancellationToken cancellationToken = default)
    {
        return await _context.AppealRequests
        .AsNoTracking()
            .Where(a => a.InsurerId == insurerId && a.IsActive)
   .OrderByDescending(a => a.CreatedAt)
      .ToListAsync(cancellationToken);
    }

    public async Task<List<AppealRequest>> GetByProviderIdAsync(string providerId, CancellationToken cancellationToken = default)
    {
        return await _context.AppealRequests
            .AsNoTracking()
   .Where(a => a.ProviderId == providerId && a.IsActive)
.OrderByDescending(a => a.CreatedAt)
    .ToListAsync(cancellationToken);
    }

    // ========== APPEAL STATUS QUERIES ==========

    public async Task<List<AppealRequest>> GetActiveAppealsAsync(CancellationToken cancellationToken = default)
    {
return await _context.AppealRequests
          .AsNoTracking()
         .Where(a => a.IsActive && !a.IsWithdrawn)
     .OrderByDescending(a => a.CreatedAt)
 .ToListAsync(cancellationToken);
    }

 public async Task<List<AppealRequest>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.AppealRequests
        .AsNoTracking()
   .Where(a => a.AppealStatus == status && a.IsActive)
      .OrderByDescending(a => a.CreatedAt)
    .ToListAsync(cancellationToken);
    }

    public async Task<List<AppealRequest>> GetByLevelAsync(int level, CancellationToken cancellationToken = default)
    {
        return await _context.AppealRequests
            .AsNoTracking()
   .Where(a => a.AppealLevel == level && a.IsActive)
.OrderByDescending(a => a.CreatedAt)
     .ToListAsync(cancellationToken);
    }

    // ========== TIMELINE QUERIES ==========

    public async Task<List<AppealRequest>> GetAppealsNearingDeadlineAsync(int daysThreshold = 5, CancellationToken cancellationToken = default)
    {
    var thresholdDate = DateTime.UtcNow.AddDays(daysThreshold);
        return await _context.AppealRequests
   .AsNoTracking()
        .Where(a => a.IsActive && !a.IsWithdrawn &&
        a.AppealDeadlineDate <= thresholdDate &&
      a.AppealDeadlineDate > DateTime.UtcNow &&
  string.IsNullOrEmpty(a.AppealOutcome))
  .OrderBy(a => a.AppealDeadlineDate)
   .ToListAsync(cancellationToken);
    }

    public async Task<List<AppealRequest>> GetAppealsPastDeadlineAsync(CancellationToken cancellationToken = default)
 {
        return await _context.AppealRequests
      .AsNoTracking()
 .Where(a => a.IsActive &&
      a.AppealDeadlineDate < DateTime.UtcNow &&
             string.IsNullOrEmpty(a.AppealOutcome))
     .OrderBy(a => a.AppealDeadlineDate)
          .ToListAsync(cancellationToken);
    }

    public async Task<List<AppealRequest>> GetPendingDecisionAppealsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AppealRequests
  .AsNoTracking()
    .Where(a => a.IsActive &&
    (a.AppealStatus == "submitted" || a.AppealStatus == "acknowledged") &&
   string.IsNullOrEmpty(a.AppealOutcome))
   .OrderByDescending(a => a.AppealSubmittedDate)
  .ToListAsync(cancellationToken);
    }

    public async Task<List<AppealRequest>> GetDecidedAppealsAsync(CancellationToken cancellationToken = default)
    {
  return await _context.AppealRequests
   .AsNoTracking()
       .Where(a => a.IsActive && !string.IsNullOrEmpty(a.AppealOutcome))
   .OrderByDescending(a => a.ReviewCompletedDate)
  .ToListAsync(cancellationToken);
    }

    // ========== UPDATE OPERATIONS ==========

    public async Task<AppealRequest> UpdateAsync(AppealRequest appeal, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating appeal {AppealNumber}", appeal.AppealNumber);
     _context.AppealRequests.Update(appeal);
        await _context.SaveChangesAsync(cancellationToken);
   _logger.LogInformation("Appeal {AppealNumber} updated successfully", appeal.AppealNumber);
   return appeal;
 }

    public async Task UpdateStatusAsync(string appealId, string newStatus, CancellationToken cancellationToken = default)
    {
  _logger.LogInformation("Updating appeal {AppealId} status to {Status}", appealId, newStatus);
        var appeal = await _context.AppealRequests.FindAsync(new object[] { appealId }, cancellationToken: cancellationToken);
   if (appeal != null)
    {
   appeal.AppealStatus = newStatus;
 appeal.LastStatusUpdateDate = DateTime.UtcNow;
  await _context.SaveChangesAsync(cancellationToken);
  _logger.LogInformation("Appeal {AppealId} status updated to {Status}", appealId, newStatus);
       }
 }

    public async Task MarkAsSubmittedAsync(string appealId, DateTime submittedDate, CancellationToken cancellationToken = default)
    {
  var appeal = await _context.AppealRequests.FindAsync(new object[] { appealId }, cancellationToken: cancellationToken);
  if (appeal != null)
         {
   appeal.AppealSubmittedDate = submittedDate;
      appeal.AppealStatus = "submitted";
   appeal.LastStatusUpdateDate = DateTime.UtcNow;
    await _context.SaveChangesAsync(cancellationToken);
   _logger.LogInformation("Appeal {AppealId} marked as submitted", appealId);
  }
}

    public async Task MarkAsWithdrawnAsync(string appealId, string reason, CancellationToken cancellationToken = default)
    {
var appeal = await _context.AppealRequests.FindAsync(new object[] { appealId }, cancellationToken: cancellationToken);
       if (appeal != null)
      {
   appeal.IsWithdrawn = true;
      appeal.WithdrawnDate = DateTime.UtcNow;
   appeal.WithdrawalReason = reason;
            appeal.AppealStatus = "withdrawn";
   appeal.IsActive = false;
       await _context.SaveChangesAsync(cancellationToken);
  _logger.LogInformation("Appeal {AppealId} marked as withdrawn", appealId);
         }
    }

    public async Task RecordDecisionAsync(string appealId, string outcome, decimal? approvedAmount, string explanation, CancellationToken cancellationToken = default)
    {
     var appeal = await _context.AppealRequests.FindAsync(new object[] { appealId }, cancellationToken: cancellationToken);
    if (appeal != null)
         {
     appeal.AppealOutcome = outcome;
       appeal.ApprovedAmount = approvedAmount;
         appeal.DecisionExplanation = explanation;
 appeal.ReviewCompletedDate = DateTime.UtcNow;
      appeal.AppealStatus = "decided";
            await _context.SaveChangesAsync(cancellationToken);
   _logger.LogInformation("Appeal {AppealId} decision recorded: {Outcome}", appealId, outcome);
       }
    }

    // ========== DELETE OPERATIONS ==========

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var appeal = await _context.AppealRequests.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
       if (appeal != null)
{
        appeal.IsActive = false;
    await _context.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("Appeal {AppealId} deleted", id);
      }
    }

    // ========== STATUS HISTORY ==========

    public async Task AddStatusHistoryAsync(string appealId, AppealStatusHistory history, CancellationToken cancellationToken = default)
    {
        history.AppealId = appealId;
        _context.AppealStatusHistories.Add(history);
        await _context.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("Status history added for appeal {AppealId}", appealId);
    }

    public async Task<List<AppealStatusHistory>> GetStatusHistoryAsync(string appealId, CancellationToken cancellationToken = default)
    {
  return await _context.AppealStatusHistories
         .AsNoTracking()
 .Where(h => h.AppealId == appealId)
    .OrderByDescending(h => h.StatusChangeDate)
            .ToListAsync(cancellationToken);
 }

    // ========== DOCUMENTS ==========

    public async Task AddDocumentAsync(string appealId, AppealDocument document, CancellationToken cancellationToken = default)
    {
 document.AppealId = appealId;
        _context.AppealDocuments.Add(document);
     await _context.SaveChangesAsync(cancellationToken);
   _logger.LogInformation("Document added to appeal {AppealId}", appealId);
    }

    public async Task<List<AppealDocument>> GetDocumentsAsync(string appealId, CancellationToken cancellationToken = default)
    {
        return await _context.AppealDocuments
       .AsNoTracking()
    .Where(d => d.AppealId == appealId && d.IsActive)
            .OrderByDescending(d => d.AttachedDate)
    .ToListAsync(cancellationToken);
    }

    public async Task RemoveDocumentAsync(string appealId, string documentId, CancellationToken cancellationToken = default)
    {
   var document = await _context.AppealDocuments.FindAsync(new object[] { documentId }, cancellationToken: cancellationToken);
      if (document != null)
      {
 document.IsActive = false;
   await _context.SaveChangesAsync(cancellationToken);
   _logger.LogInformation("Document {DocumentId} removed from appeal {AppealId}", documentId, appealId);
}
    }

    // ========== STATISTICS ==========

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
   return await _context.AppealRequests
  .AsNoTracking()
   .Where(a => a.IsActive)
    .CountAsync(cancellationToken);
    }

    public async Task<int> GetCountByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.AppealRequests
  .AsNoTracking()
 .Where(a => a.AppealStatus == status && a.IsActive)
       .CountAsync(cancellationToken);
  }

    public async Task<decimal> GetApprovalRateAsync(CancellationToken cancellationToken = default)
    {
        var totalDecided = await _context.AppealRequests
            .AsNoTracking()
          .Where(a => !string.IsNullOrEmpty(a.AppealOutcome) && a.IsActive)
    .CountAsync(cancellationToken);

        if (totalDecided == 0) return 0m;

    var approved = await _context.AppealRequests
   .AsNoTracking()
            .Where(a => a.AppealOutcome == "approved" && a.IsActive)
 .CountAsync(cancellationToken);

        return (decimal)approved / totalDecided * 100;
    }

    public async Task<decimal> GetTotalApprovedAmountAsync(CancellationToken cancellationToken = default)
 {
 return await _context.AppealRequests
         .AsNoTracking()
   .Where(a => a.AppealOutcome == "approved" && a.ApprovedAmount.HasValue && a.IsActive)
        .SumAsync(a => a.ApprovedAmount.Value, cancellationToken);
    }

    // ========== SAVE ==========

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
