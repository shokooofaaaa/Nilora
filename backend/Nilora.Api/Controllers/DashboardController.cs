using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nilora.Application.Interfaces;

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
