using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nilora.Application.Interfaces;
using System.Security.Claims;

namespace Nilora.Api.Controllers
{

    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class TechniciansController : ControllerBase
    {
        private readonly ITechnicianService _technicianService;

        public TechniciansController(ITechnicianService technicianService)
        {
            this._technicianService = technicianService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile(
     CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                return Unauthorized();
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            var technician = await _technicianService.GetCurrentTechnicianAsync(
                userId,
                cancellationToken);

            if (technician is null)
            {
                return NotFound();
            }

            return Ok(technician);
        }
    }
}
    
