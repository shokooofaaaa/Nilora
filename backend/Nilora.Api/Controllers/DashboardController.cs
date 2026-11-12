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
    public async Task<IActionResult> GetOverview(
        [FromQuery] int technicianId,
        CancellationToken cancellationToken)
    {
        if (technicianId <= 0)
            return BadRequest("Invalid technician ID.");

        var result = await _dashboardService.GetOverviewAsync(
            technicianId,
            cancellationToken);

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
