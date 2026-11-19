using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nilora.Application.DTOs.Dashboard;
using Nilora.Application.Interfaces;
using Nilora.Domain.Enums;

namespace Nilora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }


    [HttpGet("overview")]
    [Authorize(Roles = "Technician")]
    public async Task<IActionResult> GetOverview(
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null ||
            !int.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var result = await _dashboardService.GetOverviewAsync(
            userId,
            cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("feedbacks/{id:int}")]
    [Authorize(Roles = "Technician")]
    [ProducesResponseType(typeof(DashboardFeedbackDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFeedbackDetail(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return BadRequest("Feedback ID must be greater than zero.");

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null ||
            !int.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var result = await _dashboardService.GetFeedbackDetailAsync(
            userId,
            id,
            cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("feedbacks")]
    [Authorize(Roles = "Technician")]
    [ProducesResponseType(typeof(DashboardFeedbackListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFeedbacks(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        CancellationToken cancellationToken = default)
    {
        if (page < 1)
            return BadRequest("Page must be greater than or equal to 1.");

        // TODO: Confirm max pageSize limit with product owner
        if (pageSize < 1 || pageSize > 50)
            return BadRequest("Page size must be between 1 and 50.");

        ReviewStatus? parsedStatus = null;

        if (!string.IsNullOrWhiteSpace(status))
        {
            var normalizedStatus = status.Trim();

            if (int.TryParse(normalizedStatus, out _)
                || !Enum.TryParse<ReviewStatus>(
                    normalizedStatus,
                    ignoreCase: true,
                    out var statusValue)
                || !Enum.IsDefined(statusValue))
            {
                return BadRequest(
                    "Status must be one of: Approved, Pending, or Rejected.");
            }

            parsedStatus = statusValue;
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null ||
            !int.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var result = await _dashboardService.GetFeedbacksAsync(
            userId,
            page,
            pageSize,
            parsedStatus,
            cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }


    [HttpGet("ratings")]
    public async Task<IActionResult> GetRatingDistribution(
        [FromQuery] int technicianId,
        CancellationToken cancellationToken)
    {
        if (technicianId <= 0)
            return BadRequest("Invalid technician ID.");

        var result = await _dashboardService.GetRatingDistributionAsync(
            technicianId,
            cancellationToken);

        return Ok(result);
    }


}
