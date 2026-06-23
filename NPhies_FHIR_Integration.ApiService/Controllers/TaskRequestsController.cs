using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NPhies_FHIR_Integration.Application.DTOs;
using NPhies_FHIR_Integration.Domain.Entities;
using NPhies_FHIR_Integration.Domain.Interfaces;

namespace NPhies_FHIR_Integration.ApiService.Controllers;

/// <summary>
/// Task Request API Controller
/// Manages task requests from providers to insurers (e.g., claim cancellation requests)
/// FHIR Resource: Task
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class TaskRequestsController : ControllerBase
{
    private readonly IRepository<TaskRequest> _taskRequestRepository;
 private readonly IMapper _mapper;
    private readonly ILogger<TaskRequestsController> _logger;

    /// <summary>
    /// Constructor with dependency injection
    /// </summary>
    public TaskRequestsController(
     IRepository<TaskRequest> taskRequestRepository,
        IMapper mapper,
        ILogger<TaskRequestsController> logger)
    {
        _taskRequestRepository = taskRequestRepository ?? throw new ArgumentNullException(nameof(taskRequestRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all task requests
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
  /// <returns>Paginated list of task requests</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
  try
        {
    if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
   {
             _logger.LogWarning("Invalid pagination parameters");
           return BadRequest(new { message = "Invalid pagination parameters" });
            }

      var allTaskRequests = await _taskRequestRepository.GetAllAsync();
            var totalCount = allTaskRequests.Count();
  var taskRequests = allTaskRequests
      .Skip((pageNumber - 1) * pageSize)
 .Take(pageSize)
  .ToList();

  var taskRequestDtos = _mapper.Map<List<TaskRequestDto>>(taskRequests);

    _logger.LogInformation($"Retrieved {taskRequestDtos.Count} task requests (Page {pageNumber})");
  return Ok(new
            {
 pageNumber = pageNumber,
     pageSize = pageSize,
        totalCount = totalCount,
           totalPages = (totalCount + pageSize - 1) / pageSize,
     items = taskRequestDtos
            });
        }
        catch (Exception ex)
        {
      _logger.LogError(ex, "Error retrieving task requests");
      return StatusCode(500, new { message = "An error occurred while retrieving task requests" });
        }
    }

    /// <summary>
    /// Get task request by ID
    /// </summary>
    /// <param name="id">Task Request ID</param>
    /// <returns>Task request details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
    {
      try
        {
            if (string.IsNullOrWhiteSpace(id))
          {
   return BadRequest(new { message = "Task Request ID is required" });
  }

      var taskRequest = await _taskRequestRepository.GetByIdAsync(id);
         if (taskRequest == null)
      {
   _logger.LogWarning($"Task request not found: {id}");
  return NotFound(new { message = "Task request not found" });
         }

   var taskRequestDto = _mapper.Map<TaskRequestDto>(taskRequest);
         _logger.LogInformation($"Retrieved task request: {id}");
            return Ok(taskRequestDto);
        }
     catch (Exception ex)
{
  _logger.LogError(ex, $"Error retrieving task request: {id}");
         return StatusCode(500, new { message = "An error occurred while retrieving the task request" });
     }
    }

  /// <summary>
    /// Get task requests by status
    /// </summary>
    /// <param name="status">Status filter (requested, in-progress, completed, failed, cancelled)</param>
    /// <returns>List of task requests with specified status</returns>
 [HttpGet("status/{status}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByStatus(string status)
    {
        try
        {
        if (string.IsNullOrWhiteSpace(status))
 {
         return BadRequest(new { message = "Status is required" });
 }

    var validStatuses = new[] { "requested", "in-progress", "completed", "failed", "cancelled" };
     if (!validStatuses.Contains(status.ToLower()))
     {
            return BadRequest(new { message = $"Invalid status. Valid values: {string.Join(", ", validStatuses)}" });
 }

        var taskRequests = await _taskRequestRepository.FindAsync(t => t.Status == status);
      var taskRequestDtos = _mapper.Map<List<TaskRequestDto>>(taskRequests);

        _logger.LogInformation($"Retrieved {taskRequestDtos.Count} task requests with status: {status}");
      return Ok(new { status = status, count = taskRequestDtos.Count, items = taskRequestDtos });
        }
    catch (Exception ex)
        {
      _logger.LogError(ex, $"Error retrieving task requests by status: {status}");
return StatusCode(500, new { message = "An error occurred while retrieving task requests" });
     }
    }

    /// <summary>
    /// Get task requests by code (e.g., "cancel")
    /// </summary>
    /// <param name="code">Code filter (cancel, review, approve, reject)</param>
    /// <returns>List of task requests with specified code</returns>
    [HttpGet("code/{code}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByCode(string code)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(code))
  {
   return BadRequest(new { message = "Code is required" });
            }

  var taskRequests = await _taskRequestRepository.FindAsync(t => t.Code == code);
        var taskRequestDtos = _mapper.Map<List<TaskRequestDto>>(taskRequests);

            _logger.LogInformation($"Retrieved {taskRequestDtos.Count} task requests with code: {code}");
            return Ok(new { code = code, count = taskRequestDtos.Count, items = taskRequestDtos });
     }
        catch (Exception ex)
        {
 _logger.LogError(ex, $"Error retrieving task requests by code: {code}");
            return StatusCode(500, new { message = "An error occurred while retrieving task requests" });
        }
    }

    /// <summary>
    /// Get task requests by owner (insurer)
    /// </summary>
    /// <param name="ownerId">Owner Organization ID</param>
/// <returns>List of task requests assigned to the owner</returns>
    [HttpGet("owner/{ownerId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByOwner(string ownerId)
    {
        try
        {
   if (string.IsNullOrWhiteSpace(ownerId))
    {
 return BadRequest(new { message = "Owner ID is required" });
      }

      var taskRequests = await _taskRequestRepository.FindAsync(t => t.OwnerId == ownerId);
            var taskRequestDtos = _mapper.Map<List<TaskRequestDto>>(taskRequests);

            _logger.LogInformation($"Retrieved {taskRequestDtos.Count} task requests for owner: {ownerId}");
            return Ok(new { ownerId = ownerId, count = taskRequestDtos.Count, items = taskRequestDtos });
      }
  catch (Exception ex)
     {
   _logger.LogError(ex, $"Error retrieving task requests by owner: {ownerId}");
            return StatusCode(500, new { message = "An error occurred while retrieving task requests" });
      }
    }

    /// <summary>
    /// Create a new task request
    /// </summary>
    /// <param name="createDto">Task request creation data</param>
    /// <returns>Created task request</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequestDto createDto)
    {
    try
        {
            if (createDto == null)
          {
                return BadRequest(new { message = "Task request data is required" });
            }

            if (string.IsNullOrWhiteSpace(createDto.TaskId))
    {
        return BadRequest(new { message = "Task ID is required" });
            }

var taskRequest = _mapper.Map<TaskRequest>(createDto);
         await _taskRequestRepository.AddAsync(taskRequest);
     await _taskRequestRepository.SaveChangesAsync();

      var taskRequestDto = _mapper.Map<TaskRequestDto>(taskRequest);
    _logger.LogInformation($"Created new task request: {taskRequest.Id}");

      return CreatedAtAction(nameof(GetById), new { id = taskRequest.Id }, taskRequestDto);
        }
        catch (Exception ex)
  {
        _logger.LogError(ex, "Error creating task request");
            return StatusCode(500, new { message = "An error occurred while creating the task request" });
        }
    }

    /// <summary>
    /// Update an existing task request
 /// </summary>
    /// <param name="id">Task Request ID</param>
    /// <param name="updateDto">Updated task request data</param>
    /// <returns>Updated task request</returns>
    [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateTaskRequestDto updateDto)
    {
        try
        {
       if (string.IsNullOrWhiteSpace(id))
            {
             return BadRequest(new { message = "Task Request ID is required" });
            }

 var taskRequest = await _taskRequestRepository.GetByIdAsync(id);
  if (taskRequest == null)
  {
            _logger.LogWarning($"Task request not found for update: {id}");
         return NotFound(new { message = "Task request not found" });
            }

   _mapper.Map(updateDto, taskRequest);
  _taskRequestRepository.Update(taskRequest);
          await _taskRequestRepository.SaveChangesAsync();

            var taskRequestDto = _mapper.Map<TaskRequestDto>(taskRequest);
            _logger.LogInformation($"Updated task request: {id}");

     return Ok(taskRequestDto);
      }
      catch (Exception ex)
        {
   _logger.LogError(ex, $"Error updating task request: {id}");
     return StatusCode(500, new { message = "An error occurred while updating the task request" });
        }
    }

    /// <summary>
    /// Delete a task request
    /// </summary>
    /// <param name="id">Task Request ID</param>
    /// <returns>Deletion status</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
try
   {
      if (string.IsNullOrWhiteSpace(id))
     {
    return BadRequest(new { message = "Task Request ID is required" });
   }

            var taskRequest = await _taskRequestRepository.GetByIdAsync(id);
            if (taskRequest == null)
     {
   _logger.LogWarning($"Task request not found for deletion: {id}");
       return NotFound(new { message = "Task request not found" });
      }

       _taskRequestRepository.Delete(taskRequest);
    await _taskRequestRepository.SaveChangesAsync();

_logger.LogInformation($"Deleted task request: {id}");
            return Ok(new { message = "Task request deleted successfully" });
      }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting task request: {id}");
    return StatusCode(500, new { message = "An error occurred while deleting the task request" });
     }
    }
}
