using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nilora.Application.DTOS.Feedbacks;
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
    [AllowAnonymous]
    [HttpPost("/api/public/technicians/{qrCodeToken}/feedback")]
    public async Task<IActionResult> CreateFeedback(
        [FromRoute] string qrCodeToken,
        [FromHeader(Name = "X-Client-Identifier")] string? clientIdentifier,
        [FromBody] CreateFeedbackRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var feedbackId = await _feedbackService.CreateFeedbackAsync(
                qrCodeToken,
                clientIdentifier ?? string.Empty,
                request,
                cancellationToken);

            return StatusCode(StatusCodes.Status201Created, new
            {
                success = true,
                data = new
                {
                    feedbackId,
                    status = "Pending"
                },
                message = "بازخورد شما با موفقیت ثبت شد."
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                success = false,
                error = new
                {
                    code = "TECHNICIAN_NOT_FOUND",
                    message = ex.Message
                }
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                success = false,
                error = new
                {
                    code = "INVALID_CLIENT_IDENTIFIER",
                    message = ex.Message
                }
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new
            {
                success = false,
                error = new
                {
                    code = "TECHNICIAN_INACTIVE",
                    message = ex.Message
                }
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                success = false,
                error = new
                {
                    code = "FEEDBACK_COOLDOWN_ACTIVE",
                    message = ex.Message
                }
            });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                success = false,
                error = new
                {
                    code = "INTERNAL_SERVER_ERROR",
                    message = "خطای داخلی سرور رخ داده است."
                }
            });
        }
    }
}
