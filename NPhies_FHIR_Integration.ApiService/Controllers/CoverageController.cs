using Microsoft.AspNetCore.Mvc;
using NPhies_FHIR_Integration.Domain.DTOs;
using NPhies_FHIR_Integration.Infrastructure.Repositories;
using AutoMapper;
using NPhies_FHIR_Integration.Common.Models;

namespace NPhies_FHIR_Integration.ApiService.Controllers;

/// <summary>
/// Controller for managing insurance coverage
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class CoverageController : BaseController
{
    private readonly ICoverageRepository _coverageRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CoverageController> _logger;

    /// <summary>
    /// Constructor with dependency injection
    /// </summary>
    public CoverageController(ICoverageRepository coverageRepository, IMapper mapper, ILogger<CoverageController> logger)
    {
        _coverageRepository = coverageRepository ?? throw new ArgumentNullException(nameof(coverageRepository));
    _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all active coverages
    /// </summary>
    /// <returns>List of active coverages</returns>
  /// <response code="200">Coverages retrieved successfully</response>
    /// <response code="500">Internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CoverageDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCoverages()
    {
        try
 {
     _logger.LogInformation("Retrieving all coverages");

       var coverages = await _coverageRepository.GetAllAsync(c => c.Patient, c => c.Insurer);
            var coverageDtos = _mapper.Map<IEnumerable<CoverageDto>>(coverages);

      return Ok(coverageDtos, "Coverages retrieved successfully");
      }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving coverages");
  return InternalServerError("Failed to retrieve coverages");
        }
    }

    /// <summary>
    /// Get coverage by ID
  /// </summary>
    /// <param name="id">Coverage ID</param>
    /// <returns>Coverage details</returns>
    /// <response code="200">Coverage found</response>
    /// <response code="404">Coverage not found</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CoverageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCoverageById(string id)
    {
        try
        {
      if (string.IsNullOrEmpty(id))
        return BadRequest("Coverage ID is required");

    _logger.LogInformation("Retrieving coverage {CoverageId}", id);

     var coverage = await _coverageRepository.GetWithDetailsAsync(id);

   if (coverage == null)
   return NotFound($"Coverage with ID {id} not found");

   var coverageDto = _mapper.Map<CoverageDto>(coverage);
return Ok(coverageDto, "Coverage retrieved successfully");
    }
   catch (Exception ex)
   {
            _logger.LogError(ex, "Error retrieving coverage {CoverageId}", id);
     return InternalServerError("Failed to retrieve coverage");
        }
    }

  /// <summary>
    /// Get coverage by policy number
    /// </summary>
    /// <param name="policyNumber">Insurance policy number</param>
    /// <returns>Coverage details</returns>
    /// <response code="200">Coverage found</response>
    /// <response code="404">Coverage not found</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("policy/{policyNumber}")]
    [ProducesResponseType(typeof(ApiResponse<CoverageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCoverageByPolicyNumber(string policyNumber)
    {
   try
        {
    if (string.IsNullOrEmpty(policyNumber))
 return BadRequest("Policy number is required");

    _logger.LogInformation("Retrieving coverage by policy number {PolicyNumber}", policyNumber);

            var coverage = await _coverageRepository.GetByPolicyNumberAsync(policyNumber);

    if (coverage == null)
          return NotFound($"Coverage with policy number {policyNumber} not found");

            var coverageDto = _mapper.Map<CoverageDto>(coverage);
  return Ok(coverageDto, "Coverage retrieved successfully");
  }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving coverage by policy number {PolicyNumber}", policyNumber);
            return InternalServerError("Failed to retrieve coverage");
        }
    }

    /// <summary>
    /// Get active coverages for a patient
    /// </summary>
    /// <param name="patientId">Patient ID</param>
    /// <returns>List of active coverages for patient</returns>
    /// <response code="200">Coverages retrieved successfully</response>
/// <response code="404">Patient not found</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("patient/{patientId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CoverageDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetActiveCoveragesForPatient(string patientId)
 {
        try
        {
     if (string.IsNullOrEmpty(patientId))
         return BadRequest("Patient ID is required");

      _logger.LogInformation("Retrieving active coverages for patient {PatientId}", patientId);

            var coverages = await _coverageRepository.GetActiveByPatientIdAsync(patientId);

      if (!coverages.Any())
    return NotFound($"No active coverages found for patient {patientId}");

    var coverageDtos = _mapper.Map<IEnumerable<CoverageDto>>(coverages);
      return Ok(coverageDtos, "Active coverages retrieved successfully");
        }
        catch (Exception ex)
 {
       _logger.LogError(ex, "Error retrieving coverages for patient {PatientId}", patientId);
  return InternalServerError("Failed to retrieve coverages");
        }
    }

    /// <summary>
    /// Create a new coverage
    /// </summary>
    /// <param name="coverageDto">Coverage data</param>
    /// <returns>Created coverage with ID</returns>
    /// <response code="201">Coverage created successfully</response>
    /// <response code="400">Invalid coverage data</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CoverageDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
 public async Task<IActionResult> CreateCoverage([FromBody] CoverageDto coverageDto)
    {
try
        {
            if (coverageDto == null)
      return BadRequest("Coverage data is required");

 if (string.IsNullOrEmpty(coverageDto.PolicyNumber))
                return BadRequest("Policy number is required");

        if (string.IsNullOrEmpty(coverageDto.MemberID))
                return BadRequest("Member ID is required");

            if (string.IsNullOrEmpty(coverageDto.PatientId))
          return BadRequest("Patient ID is required");

       _logger.LogInformation("Creating new coverage with policy number {PolicyNumber}", coverageDto.PolicyNumber);

         var coverage = _mapper.Map<Domain.Entities.Coverage>(coverageDto);
            var createdCoverage = await _coverageRepository.AddAsync(coverage);
            await _coverageRepository.SaveChangesAsync();

   var createdCoverageDto = _mapper.Map<CoverageDto>(createdCoverage);
        return Created($"/api/v1/coverage/{createdCoverage.Id}", createdCoverageDto);
        }
        catch (Exception ex)
        {
  _logger.LogError(ex, "Error creating coverage");
          return InternalServerError("Failed to create coverage");
        }
    }

    /// <summary>
    /// Update an existing coverage
    /// </summary>
    /// <param name="id">Coverage ID</param>
    /// <param name="coverageDto">Updated coverage data</param>
    /// <returns>Updated coverage</returns>
    /// <response code="200">Coverage updated successfully</response>
    /// <response code="400">Invalid coverage data</response>
    /// <response code="404">Coverage not found</response>
    /// <response code="500">Internal server error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CoverageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCoverage(string id, [FromBody] CoverageDto coverageDto)
 {
     try
    {
    if (string.IsNullOrEmpty(id))
        return BadRequest("Coverage ID is required");

            if (coverageDto == null)
        return BadRequest("Coverage data is required");

      _logger.LogInformation("Updating coverage {CoverageId}", id);

            var existingCoverage = await _coverageRepository.GetByIdAsync(id);
   if (existingCoverage == null)
    return NotFound($"Coverage with ID {id} not found");

            // Map only non-null values
         if (!string.IsNullOrEmpty(coverageDto.Status))
        existingCoverage.Status = coverageDto.Status;
     if (coverageDto.AnnualDeductible > 0)
    existingCoverage.AnnualDeductible = coverageDto.AnnualDeductible;
     if (coverageDto.Copay >= 0)
             existingCoverage.Copay = coverageDto.Copay;
       if (coverageDto.CoinsurancePercent >= 0)
  existingCoverage.CoinsurancePercent = coverageDto.CoinsurancePercent;

    var updatedCoverage = _coverageRepository.Update(existingCoverage);
      await _coverageRepository.SaveChangesAsync();

        var updatedCoverageDto = _mapper.Map<CoverageDto>(updatedCoverage);
            return Ok(updatedCoverageDto, "Coverage updated successfully");
        }
        catch (Exception ex)
        {
      _logger.LogError(ex, "Error updating coverage {CoverageId}", id);
            return InternalServerError("Failed to update coverage");
        }
    }

 /// <summary>
    /// Check if coverage is active on a specific date
    /// </summary>
    /// <param name="id">Coverage ID</param>
    /// <param name="date">Date to check (format: yyyy-MM-dd)</param>
    /// <returns>Activation status</returns>
    /// <response code="200">Coverage activation status returned</response>
    /// <response code="404">Coverage not found</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("{id}/active/{date}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CheckCoverageActive(string id, string date)
    {
  try
        {
     if (string.IsNullOrEmpty(id))
       return BadRequest("Coverage ID is required");

            if (!DateTime.TryParse(date, out var checkDate))
      return BadRequest("Invalid date format. Use yyyy-MM-dd");

   _logger.LogInformation("Checking if coverage {CoverageId} is active on {Date}", id, checkDate);

    var isActive = await _coverageRepository.IsCoverageActiveAsync(id, checkDate);

    return Ok(new { id, date = checkDate.Date, isActive }, "Coverage activation status retrieved");
        }
        catch (Exception ex)
   {
  _logger.LogError(ex, "Error checking coverage activation for {CoverageId}", id);
    return InternalServerError("Failed to check coverage activation");
     }
    }

    /// <summary>
    /// Delete a coverage
    /// </summary>
    /// <param name="id">Coverage ID</param>
    /// <returns>No content</returns>
    /// <response code="204">Coverage deleted successfully</response>
 /// <response code="404">Coverage not found</response>
    /// <response code="500">Internal server error</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCoverage(string id)
    {
        try
     {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Coverage ID is required");

      _logger.LogInformation("Deleting coverage {CoverageId}", id);

     var coverage = await _coverageRepository.GetByIdAsync(id);
            if (coverage == null)
      return NotFound($"Coverage with ID {id} not found");

     _coverageRepository.Delete(coverage);
            await _coverageRepository.SaveChangesAsync();

   return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting coverage {CoverageId}", id);
     return InternalServerError("Failed to delete coverage");
        }
    }
}
