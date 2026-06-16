using Microsoft.AspNetCore.Mvc;
using NPhies_FHIR_Integration.Common.Constants;
using NPhies_FHIR_Integration.Common.Models;

namespace NPhies_FHIR_Integration.ApiService.Controllers;

/// <summary>
/// Base controller for all API controllers
/// Provides common methods and response patterns
/// </summary>
[ApiController]
[Route(AppConstants.ApiRoutePrefix + "/[controller]")]
public abstract class BaseController : ControllerBase
{
    /// <summary>
 /// Returns 200 OK response
    /// </summary>
    protected new IActionResult Ok<T>(T data, string message = "Success") =>
        base.Ok(ApiResponse<T>.SuccessResponse(data, message));

    /// <summary>
    /// Returns 201 Created response
    /// </summary>
    protected IActionResult Created<T>(T data, string location = "") =>
        base.Created(location, ApiResponse<T>.SuccessResponse(data, AppConstants.Messages.RecordCreatedSuccessfully));

  /// <summary>
/// Returns 204 No Content response
    /// </summary>
    protected new IActionResult NoContent() =>
        base.NoContent();

    /// <summary>
    /// Returns 400 Bad Request response
    /// </summary>
    protected new IActionResult BadRequest(string message, List<string>? errors = null) =>
        base.BadRequest(ApiResponse<object>.ErrorResponse(message, errors));

    /// <summary>
    /// Returns 404 Not Found response
    /// </summary>
    protected new IActionResult NotFound(string message = AppConstants.Messages.RecordNotFound) =>
        base.NotFound(ApiResponse<object>.ErrorResponse(message));

    /// <summary>
 /// Returns 500 Internal Server Error response
/// </summary>
    protected IActionResult InternalServerError(string message = AppConstants.Messages.InternalServerError) =>
        base.StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<object>.ErrorResponse(message));
}
