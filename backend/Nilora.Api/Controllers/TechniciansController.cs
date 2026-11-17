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
}
