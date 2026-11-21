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
}
