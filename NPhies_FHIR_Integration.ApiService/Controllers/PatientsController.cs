using Microsoft.AspNetCore.Mvc;
using NPhies_FHIR_Integration.Domain.DTOs;
using NPhies_FHIR_Integration.Infrastructure.Repositories;
using AutoMapper;
using NPhies_FHIR_Integration.Common.Models;

namespace NPhies_FHIR_Integration.ApiService.Controllers;

/// <summary>
/// Controller for managing patient data
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class PatientsController : BaseController
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<PatientsController> _logger;

    /// <summary>
    /// Constructor with dependency injection
    /// </summary>
    public PatientsController(IPatientRepository patientRepository, IMapper mapper, ILogger<PatientsController> logger)
    {
        _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all patients
    /// </summary>
    /// <returns>List of all patients</returns>
    /// <response code="200">Patients retrieved successfully</response>
    /// <response code="500">Internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PatientDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPatients()
    {
        try
        {
            _logger.LogInformation("Retrieving all patients");

            var patients = await _patientRepository.GetAllAsync(p => p.Coverages, p => p.EligibilityRequests);
            var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(patients);

            return Ok(patientDtos, "Patients retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patients");
            return InternalServerError("Failed to retrieve patients");
        }
    }

    /// <summary>
    /// Get patient by ID
    /// </summary>
    /// <param name="id">Patient ID</param>
    /// <returns>Patient details</returns>
    /// <response code="200">Patient found</response>
    /// <response code="404">Patient not found</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<PatientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPatientById(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Patient ID is required");

            _logger.LogInformation("Retrieving patient {PatientId}", id);

            var patient = await _patientRepository.GetWithCoverageAndEligibilityAsync(id);

            if (patient == null)
                return NotFound($"Patient with ID {id} not found");

            var patientDto = _mapper.Map<PatientDto>(patient);
            return Ok(patientDto, "Patient retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient {PatientId}", id);
            return InternalServerError("Failed to retrieve patient");
        }
    }

    /// <summary>
    /// Get patient by MRN
    /// </summary>
    /// <param name="mrn">Member Registration Number</param>
    /// <returns>Patient details</returns>
    /// <response code="200">Patient found</response>
    /// <response code="404">Patient not found</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("mrn/{mrn}")]
    [ProducesResponseType(typeof(ApiResponse<PatientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPatientByMRN(string mrn)
    {
        try
        {
            if (string.IsNullOrEmpty(mrn))
                return BadRequest("MRN is required");

            _logger.LogInformation("Retrieving patient by MRN {MRN}", mrn);

            var patient = await _patientRepository.GetByMRNAsync(mrn);

            if (patient == null)
                return NotFound($"Patient with MRN {mrn} not found");

            var patientDto = _mapper.Map<PatientDto>(patient);
            return Ok(patientDto, "Patient retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient by MRN {MRN}", mrn);
            return InternalServerError("Failed to retrieve patient");
        }
    }

    /// <summary>
    /// Create a new patient
    /// </summary>
    /// <param name="patientDto">Patient data</param>
    /// <returns>Created patient with ID</returns>
    /// <response code="201">Patient created successfully</response>
    /// <response code="400">Invalid patient data</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PatientDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePatient([FromBody] PatientDto patientDto)
    {
        try
        {
            if (patientDto == null)
                return BadRequest("Patient data is required");

            if (string.IsNullOrEmpty(patientDto.MRN))
                return BadRequest("MRN is required");

            if (string.IsNullOrEmpty(patientDto.FirstName) || string.IsNullOrEmpty(patientDto.LastName))
                return BadRequest("Patient first and last names are required");

            // Check if patient with MRN already exists
            var existingPatient = await _patientRepository.ExistsByMRNAsync(patientDto.MRN);
            if (existingPatient)
                return BadRequest($"Patient with MRN {patientDto.MRN} already exists");

            _logger.LogInformation("Creating new patient with MRN {MRN}", patientDto.MRN);

            var patient = _mapper.Map<Domain.Entities.Patient>(patientDto);
            var createdPatient = await _patientRepository.AddAsync(patient);
            await _patientRepository.SaveChangesAsync();

            var createdPatientDto = _mapper.Map<PatientDto>(createdPatient);
            return Created($"/api/v1/patients/{createdPatient.Id}", createdPatientDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating patient");
            return InternalServerError("Failed to create patient");
        }
    }

    /// <summary>
    /// Update an existing patient
    /// </summary>
    /// <param name="id">Patient ID</param>
    /// <param name="patientDto">Updated patient data</param>
    /// <returns>Updated patient</returns>
    /// <response code="200">Patient updated successfully</response>
    /// <response code="400">Invalid patient data</response>
    /// <response code="404">Patient not found</response>
    /// <response code="500">Internal server error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<PatientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePatient(string id, [FromBody] PatientDto patientDto)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Patient ID is required");

            if (patientDto == null)
                return BadRequest("Patient data is required");

            _logger.LogInformation("Updating patient {PatientId}", id);

            var existingPatient = await _patientRepository.GetByIdAsync(id);
            if (existingPatient == null)
                return NotFound($"Patient with ID {id} not found");

            // Map only non-null values
            if (!string.IsNullOrEmpty(patientDto.FirstName))
                existingPatient.FirstName = patientDto.FirstName;
            if (!string.IsNullOrEmpty(patientDto.LastName))
                existingPatient.LastName = patientDto.LastName;
            if (!string.IsNullOrEmpty(patientDto.Email))
                existingPatient.Email = patientDto.Email;
            if (!string.IsNullOrEmpty(patientDto.Phone))
                existingPatient.Phone = patientDto.Phone;
            if (!string.IsNullOrEmpty(patientDto.Status))
                existingPatient.Status = patientDto.Status;

            var updatedPatient = _patientRepository.Update(existingPatient);
            await _patientRepository.SaveChangesAsync();

            var updatedPatientDto = _mapper.Map<PatientDto>(updatedPatient);
            return Ok(updatedPatientDto, "Patient updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient {PatientId}", id);
            return InternalServerError("Failed to update patient");
        }
    }

    /// <summary>
    /// Delete a patient
    /// </summary>
    /// <param name="id">Patient ID</param>
    /// <returns>No content</returns>
    /// <response code="204">Patient deleted successfully</response>
    /// <response code="404">Patient not found</response>
    /// <response code="500">Internal server error</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePatient(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Patient ID is required");

            _logger.LogInformation("Deleting patient {PatientId}", id);

            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
                return NotFound($"Patient with ID {id} not found");

            _patientRepository.Delete(patient);
            await _patientRepository.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting patient {PatientId}", id);
            return InternalServerError("Failed to delete patient");
        }
    }
}
