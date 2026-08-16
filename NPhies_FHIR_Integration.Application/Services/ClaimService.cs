using AutoMapper;
using NPhies_FHIR_Integration.Application.DTOs;
using NPhies_FHIR_Integration.Domain.Entities;
using NPhies_FHIR_Integration.Infrastructure.Repositories;

namespace NPhies_FHIR_Integration.Application.Services;

/// <summary>
/// Service interface for Claim operations
/// </summary>
public interface IClaimService
{
    /// <summary>
    /// Create a new claim
    /// </summary>
    Task<ClaimDto> CreateClaimAsync(CreateClaimDto dto);

    /// <summary>
    /// Get claim by ID
    /// </summary>
    Task<ClaimDto?> GetClaimAsync(string claimId);

    /// <summary>
    /// Get claim by ID (alias method)
    /// </summary>
    Task<ClaimDto?> GetClaimByIdAsync(string claimId);

    /// <summary>
    /// Get claim by claim number
    /// </summary>
    Task<ClaimDto?> GetClaimByNumberAsync(string claimNumber);

    /// <summary>
    /// Get all claims for a patient
    /// </summary>
    Task<IEnumerable<ClaimDto>> GetPatientClaimsAsync(string patientId);

    /// <summary>
    /// Get all claims by patient ID
    /// </summary>
    Task<IEnumerable<ClaimDto>> GetClaimsByPatientIdAsync(string patientId);

    /// <summary>
    /// Get all claims
    /// </summary>
    Task<IEnumerable<ClaimDto>> GetAllClaimsAsync();

    /// <summary>
    /// Get claims by status
    /// </summary>
    Task<IEnumerable<ClaimDto>> GetClaimsByStatusAsync(string status);

    /// <summary>
    /// Update claim
    /// </summary>
    Task<ClaimDto> UpdateClaimAsync(string claimId, UpdateClaimDto dto);

    /// <summary>
    /// Get claim with all details
    /// </summary>
    Task<ClaimDto?> GetClaimWithDetailsAsync(string claimId);
}

/// <summary>
/// Implementation of IClaimService
/// </summary>
public class ClaimService : IClaimService
{
    private readonly IClaimRepository _claimRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor with dependencies
    /// </summary>
    public ClaimService(IClaimRepository claimRepository, IMapper mapper)
    {
        _claimRepository = claimRepository ?? throw new ArgumentNullException(nameof(claimRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Create a new claim
    /// </summary>
    public async Task<ClaimDto> CreateClaimAsync(CreateClaimDto dto)
    {
        try
        {
            var claim = _mapper.Map<Claim>(dto);
            claim.Status = "active";
            claim.CreatedAt = DateTime.UtcNow;

            await _claimRepository.AddAsync(claim);
            await _claimRepository.SaveChangesAsync();

            return _mapper.Map<ClaimDto>(claim);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to create claim", ex);
        }
    }

    /// <summary>
    /// Get claim by ID
    /// </summary>
    public async Task<ClaimDto?> GetClaimAsync(string claimId)
    {
        try
        {
            var claim = await _claimRepository.GetByIdAsync(claimId);
            return claim != null ? _mapper.Map<ClaimDto>(claim) : null;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve claim", ex);
        }
    }

    /// <summary>
    /// Get claim by ID (alias method)
    /// </summary>
    public async Task<ClaimDto?> GetClaimByIdAsync(string claimId)
    {
        return await GetClaimAsync(claimId);
    }

    /// <summary>
    /// Get claim by claim number
    /// </summary>
    public async Task<ClaimDto?> GetClaimByNumberAsync(string claimNumber)
    {
        try
        {
            var claim = await _claimRepository.GetByClaimNumberAsync(claimNumber);
            return claim != null ? _mapper.Map<ClaimDto>(claim) : null;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve claim by number", ex);
        }
    }

    /// <summary>
    /// Get all claims for a patient
    /// </summary>
    public async Task<IEnumerable<ClaimDto>> GetPatientClaimsAsync(string patientId)
    {
        try
        {
            var claims = await _claimRepository.GetByPatientIdAsync(patientId);
            return _mapper.Map<IEnumerable<ClaimDto>>(claims);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve patient claims", ex);
        }
    }

    /// <summary>
    /// Get all claims by patient ID (alias method)
    /// </summary>
    public async Task<IEnumerable<ClaimDto>> GetClaimsByPatientIdAsync(string patientId)
    {
        return await GetPatientClaimsAsync(patientId);
    }

    /// <summary>
    /// Get all claims
    /// </summary>
    public async Task<IEnumerable<ClaimDto>> GetAllClaimsAsync()
    {
        try
        {
            var claims = await _claimRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClaimDto>>(claims);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve all claims", ex);
        }
    }

    /// <summary>
    /// Get claims by status
    /// </summary>
    public async Task<IEnumerable<ClaimDto>> GetClaimsByStatusAsync(string status)
    {
        try
        {
            var claims = await _claimRepository.GetByStatusAsync(status);
            return _mapper.Map<IEnumerable<ClaimDto>>(claims);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve claims by status", ex);
        }
    }

    /// <summary>
    /// Update claim
    /// </summary>
    public async Task<ClaimDto> UpdateClaimAsync(string claimId, UpdateClaimDto dto)
    {
        try
        {
            var claim = await _claimRepository.GetByIdAsync(claimId);
            if (claim == null)
                throw new KeyNotFoundException($"Claim with ID {claimId} not found");

            _mapper.Map(dto, claim);
            claim.UpdatedAt = DateTime.UtcNow;

            _claimRepository.Update(claim);
            await _claimRepository.SaveChangesAsync();

            return _mapper.Map<ClaimDto>(claim);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to update claim", ex);
        }
    }

    /// <summary>
    /// Get claim with all details
    /// </summary>
    public async Task<ClaimDto?> GetClaimWithDetailsAsync(string claimId)
    {
        try
        {
            var claim = await _claimRepository.GetWithDetailsAsync(claimId);
            return claim != null ? _mapper.Map<ClaimDto>(claim) : null;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve claim with details", ex);
        }
    }
}
