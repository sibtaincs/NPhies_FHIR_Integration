namespace NPhies_FHIR_Integration.Application.DTOs;

using NPhies_FHIR_Integration.Domain.DTOs;

/// <summary>
/// DTO for Task entity
/// Used for API communication and data transfer
/// </summary>
public class TaskDto : BaseDto
{
    /// <summary>
    /// Task ID (FHIR resource ID)
    /// </summary>
    public string TaskId { get; set; } = string.Empty;

    /// <summary>
    /// Task identifier system
    /// </summary>
    public string? TaskIdentifierSystem { get; set; }

    /// <summary>
    /// Task identifier value
    /// </summary>
  public string? TaskIdentifierValue { get; set; }

    /// <summary>
    /// Task status
    /// </summary>
    public string Status { get; set; } = "requested";

    /// <summary>
    /// Task intent
    /// </summary>
    public string Intent { get; set; } = "order";

    /// <summary>
    /// Task priority
    /// </summary>
    public string Priority { get; set; } = "normal";

    /// <summary>
    /// Task code
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Task code system
    /// </summary>
    public string? CodeSystem { get; set; }

    /// <summary>
    /// Task code display text
    /// </summary>
    public string? CodeDisplay { get; set; }

    /// <summary>
    /// When the task was authored
    /// </summary>
    public DateTime AuthoredOn { get; set; }

    /// <summary>
    /// When the task was last modified
    /// </summary>
    public DateTime LastModified { get; set; }

    /// <summary>
    /// Requester organization ID
    /// </summary>
    public string RequesterId { get; set; } = string.Empty;

    /// <summary>
    /// Owner/Responsible organization ID
    /// </summary>
    public string OwnerId { get; set; } = string.Empty;

/// <summary>
    /// Poll input type
    /// </summary>
    public string? PollInputType { get; set; }

    /// <summary>
    /// Poll input value (message types to retrieve)
    /// </summary>
  public string? PollInputValue { get; set; }

    /// <summary>
    /// Focus resource type (for cancellation)
    /// </summary>
    public string? FocusResourceType { get; set; }

    /// <summary>
    /// Focus identifier system
    /// </summary>
    public string? FocusIdentifierSystem { get; set; }

    /// <summary>
    /// Focus identifier value
    /// </summary>
    public string? FocusIdentifierValue { get; set; }

    /// <summary>
    /// Reason code (cancellation reason)
    /// </summary>
    public string? ReasonCode { get; set; }

    /// <summary>
    /// Reason code system
    /// </summary>
    public string? ReasonCodeSystem { get; set; }

    /// <summary>
    /// Output type for poll response
    /// </summary>
    public string? OutputType { get; set; }

    /// <summary>
    /// Output type system
    /// </summary>
    public string? OutputTypeSystem { get; set; }

  /// <summary>
    /// Output bundle ID (reference to queued messages bundle)
    /// </summary>
    public string? OutputBundleId { get; set; }

    /// <summary>
    /// Output bundle reference URL
    /// </summary>
    public string? OutputBundleReference { get; set; }

    /// <summary>
    /// Response code from poll completion
    /// </summary>
    public string? ResponseCode { get; set; }

    /// <summary>
    /// Response identifier (acknowledgment ID)
  /// </summary>
    public string? ResponseIdentifier { get; set; }

    /// <summary>
 /// Meta tag indicating queue status
    /// </summary>
    public string? MetaTag { get; set; }

    /// <summary>
    /// Task notes
    /// </summary>
    public string? Notes { get; set; }

  /// <summary>
    /// Related MessageHeader ID
    /// </summary>
    public string? MessageHeaderId { get; set; }
}

/// <summary>
/// DTO for creating a Task
/// </summary>
public class CreateTaskDto
{
    /// <summary>
    /// Task ID
    /// </summary>
    public string TaskId { get; set; } = string.Empty;

    /// <summary>
    /// Task identifier system
    /// </summary>
    public string? TaskIdentifierSystem { get; set; }

    /// <summary>
    /// Task identifier value
    /// </summary>
    public string? TaskIdentifierValue { get; set; }

    /// <summary>
    /// Task status
    /// </summary>
    public string Status { get; set; } = "requested";

    /// <summary>
    /// Task intent
    /// </summary>
    public string Intent { get; set; } = "order";

    /// <summary>
    /// Task priority
    /// </summary>
    public string Priority { get; set; } = "normal";

    /// <summary>
    /// Task code
 /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Task code system
    /// </summary>
    public string? CodeSystem { get; set; }

    /// <summary>
    /// Task code display text
 /// </summary>
    public string? CodeDisplay { get; set; }

  /// <summary>
/// Requester organization ID
    /// </summary>
    public string RequesterId { get; set; } = string.Empty;

    /// <summary>
    /// Owner/Responsible organization ID
    /// </summary>
    public string OwnerId { get; set; } = string.Empty;

    /// <summary>
    /// Poll input type
    /// </summary>
    public string? PollInputType { get; set; }

    /// <summary>
    /// Poll input value
    /// </summary>
 public string? PollInputValue { get; set; }

    /// <summary>
    /// Focus resource type
    /// </summary>
    public string? FocusResourceType { get; set; }

    /// <summary>
    /// Focus identifier system
    /// </summary>
    public string? FocusIdentifierSystem { get; set; }

/// <summary>
    /// Focus identifier value
    /// </summary>
    public string? FocusIdentifierValue { get; set; }

  /// <summary>
    /// Reason code
    /// </summary>
    public string? ReasonCode { get; set; }

    /// <summary>
    /// Reason code system
    /// </summary>
  public string? ReasonCodeSystem { get; set; }

    /// <summary>
    /// Output type
    /// </summary>
    public string? OutputType { get; set; }

    /// <summary>
    /// Output type system
    /// </summary>
    public string? OutputTypeSystem { get; set; }

  /// <summary>
    /// Output bundle ID
  /// </summary>
  public string? OutputBundleId { get; set; }

    /// <summary>
    /// Output bundle reference URL
    /// </summary>
    public string? OutputBundleReference { get; set; }

    /// <summary>
    /// Response code
 /// </summary>
    public string? ResponseCode { get; set; }

    /// <summary>
    /// Response identifier
    /// </summary>
    public string? ResponseIdentifier { get; set; }

    /// <summary>
    /// Meta tag
    /// </summary>
public string? MetaTag { get; set; }

    /// <summary>
    /// Task notes
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Related MessageHeader ID
    /// </summary>
    public string? MessageHeaderId { get; set; }
}

/// <summary>
/// DTO for updating a Task
/// </summary>
public class UpdateTaskDto
{
    /// <summary>
    /// Task status
    /// </summary>
  public string? Status { get; set; }

    /// <summary>
  /// Task priority
    /// </summary>
  public string? Priority { get; set; }

    /// <summary>
    /// Poll input type
    /// </summary>
    public string? PollInputType { get; set; }

    /// <summary>
    /// Poll input value
    /// </summary>
    public string? PollInputValue { get; set; }

    /// <summary>
    /// Task notes
    /// </summary>
    public string? Notes { get; set; }
}

/// <summary>
/// DTO for poll requests
/// </summary>
public class PollRequestDto
{
    /// <summary>
    /// Requester organization ID
    /// </summary>
    public string RequesterId { get; set; } = string.Empty;

    /// <summary>
    /// Message types to poll for (comma-separated)
    /// Example: "claim-response", "eligibility-response"
    /// </summary>
    public string? MessageTypes { get; set; }

    /// <summary>
    /// Task priority
    /// </summary>
    public string Priority { get; set; } = "normal";

    /// <summary>
    /// Task notes
    /// </summary>
    public string? Notes { get; set; }
}
