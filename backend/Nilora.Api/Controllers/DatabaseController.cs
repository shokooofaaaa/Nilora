
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nilora.Infrastructure.Persistence;

namespace Nilora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly NiloraDbContext _context;

    public DatabaseController(NiloraDbContext context)
    {
        _context = context;
    }

    [HttpGet("connection-test")]
    public async Task<IActionResult> ConnectionTest()
    {
        var canConnect = await _context.Database.CanConnectAsync();

        if (!canConnect)
            return StatusCode(500, "Database connection failed.");

        return Ok(new
        {
            Message = "Database connection successful!",
            Database = _context.Database.GetDbConnection().Database
        });
    }
}

