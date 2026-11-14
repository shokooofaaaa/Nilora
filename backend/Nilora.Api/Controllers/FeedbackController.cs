using Microsoft.AspNetCore.Mvc;
using Nilora.Application.Interfaces;

namespace Nilora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    [HttpGet("recent")]
    public async Task<IActionResult> GetRecentFeedbacks(
        [FromQuery] int technicianId,
        CancellationToken cancellationToken)
    {
        if (technicianId <= 0)
            return BadRequest("Invalid technician ID.");

        var result = await _feedbackService.GetRecentFeedbacksAsync(
            technicianId,
            cancellationToken);

        return Ok(result);
    }
}
