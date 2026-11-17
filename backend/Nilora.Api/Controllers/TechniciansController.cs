using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nilora.Application.DTOs.Technicians;
using Nilora.Application.Interfaces;

namespace Nilora.Api.Controllers;

[ApiController]
[Route("api/public/technicians")]
[AllowAnonymous]
public class TechniciansController : ControllerBase
{
    private readonly ITechnicianService _technicianService;

    public TechniciansController(ITechnicianService technicianService)
    {
        _technicianService = technicianService;
    }

    /// <summary>
    /// دریافت پروفایل عمومی تکنسین با توکن QR.
    /// </summary>
    /// <param name="token">توکن QR تکنسین (QRCodeToken).</param>
    /// <param name="cancellationToken">توکن لغو درخواست.</param>
    /// <returns>پروفایل عمومی تکنسین یا 404 در صورت نامعتبر بودن توکن/غیرفعال بودن تکنسین یا کاربر.</returns>
    [HttpGet("{token}")]
    [ProducesResponseType(typeof(PublicTechnicianProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPublicProfile(
        [FromRoute] string token,
        CancellationToken cancellationToken)
    {
        var profile = await _technicianService.GetPublicProfileByTokenAsync(
            token,
            cancellationToken);

        if (profile is null)
            return NotFound();

        return Ok(profile);
    }

    /// <summary>
    /// دریافت اطلاعات فرم عمومی ثبت بازخورد برای تکنسین.
    /// </summary>
    /// <param name="qrCodeToken">توکن QR تکنسین (QRCodeToken).</param>
    /// <param name="cancellationToken">توکن لغو درخواست.</param>
    /// <returns>200 در صورت موفقیت، 400 برای توکن نامعتبر، 403 در صورت غیرفعال بودن ثبت بازخورد و 404 در صورت نبود/غیرفعال بودن تکنسین.</returns>
    [HttpGet("{qrCodeToken}/feedback-form")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PublicFeedbackFormDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPublicFeedbackForm(
        [FromRoute] string qrCodeToken,
        CancellationToken cancellationToken)
    {
        var result = await _technicianService.GetPublicFeedbackFormAsync(
            qrCodeToken,
            cancellationToken);

        return result.Status switch
        {
            PublicFeedbackFormStatus.Success => Ok(result.Data),

            PublicFeedbackFormStatus.InvalidToken => BadRequest(new
            {
                message = result.Message ?? "توکن ورودی نامعتبر است."
            }),

            PublicFeedbackFormStatus.NotFound => NotFound(new
            {
                message = result.Message ?? "تکنسین پیدا نشد یا غیرفعال است."
            }),

            PublicFeedbackFormStatus.FeedbackDisabled => StatusCode(
                StatusCodes.Status403Forbidden,
                new
                {
                    message = result.Message
                        ?? "امکان ثبت بازخورد برای این ناخن‌کار در حال حاضر غیرفعال است."
                }),

            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
