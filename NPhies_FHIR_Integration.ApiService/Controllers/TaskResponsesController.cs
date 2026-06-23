using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NPhies_FHIR_Integration.Application.DTOs;
using NPhies_FHIR_Integration.Domain.Entities;
using NPhies_FHIR_Integration.Domain.Interfaces;

namespace NPhies_FHIR_Integration.ApiService.Controllers;

/// <summary>
/// Task Response API Controller
/// Manages task responses from insurers to providers (e.g., responses to claim cancellation requests)
/// FHIR Resource: Task
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class TaskResponsesController : ControllerBase
{
    private readonly IRepository<TaskResponse> _taskResponseRepository;
    private readonly IRepository<TaskRequest> _taskRequestRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<TaskResponsesController> _logger;

    /// <summary>
    /// Constructor with dependency injection
    /// </summary>
    public TaskResponsesController(
   IRepository<TaskResponse> taskResponseRepository,
        IRepository<TaskRequest> taskRequestRepository,
        IMapper mapper,
        ILogger<TaskResponsesController> logger)
    {
        _taskResponseRepository = taskResponseRepository ?? throw new ArgumentNullException(nameof(taskResponseRepository));
    _taskRequestRepository = taskRequestRepository ?? throw new ArgumentNullException(nameof(taskRequestRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
  _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

    /// <summary>
    /// Get all task responses
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
  /// <param name="pageSize">Page size (default: 10)</param>
    /// <returns>Paginated list of task responses</returns>
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

         var allTaskResponses = await _taskResponseRepository.GetAllAsync();
  var totalCount = allTaskResponses.Count();
   var taskResponses = allTaskResponses
     .Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .ToList();

     var taskResponseDtos = _mapper.Map<List<TaskResponseDto>>(taskResponses);

            _logger.LogInformation($"Retrieved {taskResponseDtos.Count} task responses (Page {pageNumber})");
      return Ok(new
            {
                pageNumber = pageNumber,
             pageSize = pageSize,
  totalCount = totalCount,
                totalPages = (totalCount + pageSize - 1) / pageSize,
            items = taskResponseDtos
            });
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "Error retrieving task responses");
      return StatusCode(500, new { message = "An error occurred while retrieving task responses" });
        }
    }

    /// <summary>
    /// Get task response by ID
    /// </summary>
    /// <param name="id">Task Response ID</param>
    /// <returns>Task response details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
    if (string.IsNullOrWhiteSpace(id))
   {
                return BadRequest(new { message = "Task Response ID is required" });
  }

 var taskResponse = await _taskResponseRepository.GetByIdAsync(id);
            if (taskResponse == null)
            {
                _logger.LogWarning($"Task response not found: {id}");
   return NotFound(new { message = "Task response not found" });
       }

            var taskResponseDto = _mapper.Map<TaskResponseDto>(taskResponse);
        _logger.LogInformation($"Retrieved task response: {id}");
         return Ok(taskResponseDto);
        }
  catch (Exception ex)
        {
       _logger.LogError(ex, $"Error retrieving task response: {id}");
  return StatusCode(500, new { message = "An error occurred while retrieving the task response" });
        }
    }

    /// <summary>
    /// Get task responses by status
    /// </summary>
    /// <param name="status">Status filter (completed, failed, in-progress, rejected, cancelled)</param>
    /// <returns>List of task responses with specified status</returns>
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

        var validStatuses = new[] { "completed", "failed", "in-progress", "rejected", "cancelled" };
            if (!validStatuses.Contains(status.ToLower()))
            {
          return BadRequest(new { message = $"Invalid status. Valid values: {string.Join(", ", validStatuses)}" });
         }

          var taskResponses = await _taskResponseRepository.FindAsync(t => t.Status == status);
          var taskResponseDtos = _mapper.Map<List<TaskResponseDto>>(taskResponses);

       _logger.LogInformation($"Retrieved {taskResponseDtos.Count} task responses with status: {status}");
            return Ok(new { status = status, count = taskResponseDtos.Count, items = taskResponseDtos });
        }
catch (Exception ex)
    {
            _logger.LogError(ex, $"Error retrieving task responses by status: {status}");
       return StatusCode(500, new { message = "An error occurred while retrieving task responses" });
        }
 }

    /// <summary>
    /// Get task responses by response code
    /// </summary>
    /// <param name="responseCode">Response code filter (ok, error, partial-success, timeout)</param>
    /// <returns>List of task responses with specified response code</returns>
    [HttpGet("response-code/{responseCode}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByResponseCode(string responseCode)
    {
        try
     {
   if (string.IsNullOrWhiteSpace(responseCode))
            {
                return BadRequest(new { message = "Response code is required" });
 }

            var taskResponses = await _taskResponseRepository.FindAsync(t => t.ResponseCode == responseCode);
       var taskResponseDtos = _mapper.Map<List<TaskResponseDto>>(taskResponses);

            _logger.LogInformation($"Retrieved {taskResponseDtos.Count} task responses with response code: {responseCode}");
      return Ok(new { responseCode = responseCode, count = taskResponseDtos.Count, items = taskResponseDtos });
        }
        catch (Exception ex)
        {
   _logger.LogError(ex, $"Error retrieving task responses by response code: {responseCode}");
      return StatusCode(500, new { message = "An error occurred while retrieving task responses" });
   }
    }

    /// <summary>
    /// Get successful task responses
    /// </summary>
    /// <returns>List of successful task responses</returns>
    [HttpGet("filter/successful")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSuccessful()
    {
     try
      {
          var taskResponses = await _taskResponseRepository.FindAsync(t => t.ResponseCode == "ok" && t.ResponseStatusCode == 200);
            var taskResponseDtos = _mapper.Map<List<TaskResponseDto>>(taskResponses);

        _logger.LogInformation($"Retrieved {taskResponseDtos.Count} successful task responses");
       return Ok(new { count = taskResponseDtos.Count, items = taskResponseDtos });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving successful task responses");
        return StatusCode(500, new { message = "An error occurred while retrieving task responses" });
    }
    }

    /// <summary>
    /// Get failed task responses
    /// </summary>
    /// <returns>List of failed task responses</returns>
 [HttpGet("filter/failed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFailed()
    {
     try
 {
            var taskResponses = await _taskResponseRepository.FindAsync(t => t.ResponseCode == "error" || t.ResponseStatusCode >= 400);
 var taskResponseDtos = _mapper.Map<List<TaskResponseDto>>(taskResponses);

_logger.LogInformation($"Retrieved {taskResponseDtos.Count} failed task responses");
   return Ok(new { count = taskResponseDtos.Count, items = taskResponseDtos });
        }
        catch (Exception ex)
  {
            _logger.LogError(ex, "Error retrieving failed task responses");
return StatusCode(500, new { message = "An error occurred while retrieving task responses" });
}
    }

    /// <summary>
    /// Create a new task response
    /// </summary>
    /// <param name="createDto">Task response creation data</param>
    /// <returns>Created task response</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
 public async Task<IActionResult> Create([FromBody] CreateTaskResponseDto createDto)
    {
    try
        {
         if (createDto == null)
            {
        return BadRequest(new { message = "Task response data is required" });
          }

            if (string.IsNullOrWhiteSpace(createDto.TaskId))
            {
                return BadRequest(new { message = "Task ID is required" });
        }

            // If referenced task request ID provided, verify it exists
            if (!string.IsNullOrEmpty(createDto.TaskRequestId))
            {
                var taskRequest = await _taskRequestRepository.GetByIdAsync(createDto.TaskRequestId);
        if (taskRequest == null)
           {
       _logger.LogWarning($"Referenced task request not found: {createDto.TaskRequestId}");
         return BadRequest(new { message = "Referenced task request not found" });
}
   }

        var taskResponse = _mapper.Map<TaskResponse>(createDto);
        await _taskResponseRepository.AddAsync(taskResponse);
       await _taskResponseRepository.SaveChangesAsync();

            var taskResponseDto = _mapper.Map<TaskResponseDto>(taskResponse);
    _logger.LogInformation($"Created new task response: {taskResponse.Id}");

 return CreatedAtAction(nameof(GetById), new { id = taskResponse.Id }, taskResponseDto);
        }
        catch (Exception ex)
 {
      _logger.LogError(ex, "Error creating task response");
         return StatusCode(500, new { message = "An error occurred while creating the task response" });
        }
    }

    /// <summary>
    /// Update an existing task response
    /// </summary>
    /// <param name="id">Task Response ID</param>
    /// <param name="updateDto">Updated task response data</param>
    /// <returns>Updated task response</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateTaskResponseDto updateDto)
    {
 try
        {
            if (string.IsNullOrWhiteSpace(id))
     {
   return BadRequest(new { message = "Task Response ID is required" });
            }

 var taskResponse = await _taskResponseRepository.GetByIdAsync(id);
          if (taskResponse == null)
      {
    _logger.LogWarning($"Task response not found for update: {id}");
   return NotFound(new { message = "Task response not found" });
            }

        _mapper.Map(updateDto, taskResponse);
        _taskResponseRepository.Update(taskResponse);
   await _taskResponseRepository.SaveChangesAsync();

     var taskResponseDto = _mapper.Map<TaskResponseDto>(taskResponse);
            _logger.LogInformation($"Updated task response: {id}");

   return Ok(taskResponseDto);
        }
        catch (Exception ex)
        {
         _logger.LogError(ex, $"Error updating task response: {id}");
   return StatusCode(500, new { message = "An error occurred while updating the task response" });
   }
    }

    /// <summary>
    /// Delete a task response
    /// </summary>
    /// <param name="id">Task Response ID</param>
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
      return BadRequest(new { message = "Task Response ID is required" });
     }

       var taskResponse = await _taskResponseRepository.GetByIdAsync(id);
          if (taskResponse == null)
      {
      _logger.LogWarning($"Task response not found for deletion: {id}");
           return NotFound(new { message = "Task response not found" });
            }

            _taskResponseRepository.Delete(taskResponse);
     await _taskResponseRepository.SaveChangesAsync();

          _logger.LogInformation($"Deleted task response: {id}");
            return Ok(new { message = "Task response deleted successfully" });
        }
        catch (Exception ex)
        {
  _logger.LogError(ex, $"Error deleting task response: {id}");
   return StatusCode(500, new { message = "An error occurred while deleting the task response" });
        }
 }
}
