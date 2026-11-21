using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nilora.Application.DTOS.Feedbacks;
using Nilora.Application.Interfaces;

namespace Nilora.Api.Controllers;

[ApiController]
[Route("api/admin/feedbacks")]
[Authorize(Roles = "Admin")]
public class AdminFeedbacksController : ControllerBase
{
    private readonly IAdminFeedbackService _adminFeedbackService;

    public AdminFeedbacksController(IAdminFeedbackService adminFeedbackService)
    {
        _adminFeedbackService = adminFeedbackService;
    }

    /// <summary>
    /// دریافت لیست بازخوردهای در حال بررسی (Pending) برای مدیران، با صفحه‌بندی.
    /// </summary>
    /// <param name="page">شماره صفحه (حداقل ۱).</param>
    /// <param name="pageSize">تعداد آیتم در هر صفحه (۱ تا ۵۰).</param>
    /// <param name="cancellationToken">توکن لغو درخواست.</param>
    /// <returns>200 با لیست بازخوردهای در حال بررسی یا 400 برای پارامترهای نامعتبر.</returns>
    [HttpGet("pending")]
    [ProducesResponseType(typeof(PendingFeedbackListResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPendingFeedbacks(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (page < 1)
            return BadRequest("Page must be greater than or equal to 1.");

        if (pageSize < 1 || pageSize > 50)
            return BadRequest("Page size must be between 1 and 50.");

        var result = await _adminFeedbackService.GetPendingFeedbacksAsync(
            page,
            pageSize,
            cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// تأیید یک بازخورد در حال بررسی (Pending) توسط مدیر.
    /// </summary>
    /// <param name="id">شناسه بازخورد.</param>
    /// <param name="cancellationToken">توکن لغو درخواست.</param>
    /// <returns>200 در صورت تأیید موفق، 400 برای شناسه نامعتبر، 404 برای بازخورد ناموجود و 409 برای بازخورد بررسی‌شده.</returns>
    [HttpPut("{id:int}/approve")]
    [ProducesResponseType(typeof(ApproveFeedbackResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ApproveFeedback(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return BadRequest("Feedback ID must be greater than zero.");

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null ||
            !int.TryParse(userIdClaim.Value, out var adminUserId))
        {
            return Unauthorized();
        }

        var result = await _adminFeedbackService.ApproveFeedbackAsync(
            id,
            adminUserId,
            cancellationToken);

        return result.Status switch
        {
            ApproveFeedbackStatus.Success => Ok(result.Data),

            ApproveFeedbackStatus.NotFound => NotFound(new
            {
                message = result.Message ?? "بازخورد موردنظر پیدا نشد."
            }),

            ApproveFeedbackStatus.Conflict => Conflict(new
            {
                message = result.Message
                    ?? "این بازخورد قبلاً بررسی شده است و امکان تأیید مجدد آن وجود ندارد."
            }),

            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
