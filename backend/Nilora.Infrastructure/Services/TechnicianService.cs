using Microsoft.EntityFrameworkCore;
using Nilora.Application.DTOS.Technicians;
using Nilora.Application.Interfaces;
using Nilora.Infrastructure.Persistence;

namespace Nilora.Infrastructure.Services
{
    public class TechnicianService : ITechnicianService
    {
        private readonly NiloraDbContext _context;

        public TechnicianService(NiloraDbContext context)
        {
            _context = context;
        }

        public async Task<TechnicianProfileResponse?> GetCurrentTechnicianAsync(int userId, CancellationToken cancellationToken = default)
        {
            var technician = await _context.Technicians
                              .Include(x => x.Skills)
                            .ThenInclude(x => x.Skill)
                           .FirstOrDefaultAsync(
                            x => x.UserId == userId,
                            cancellationToken);

            if (technician is null)
            {
                return null;
            }

            TechnicianProfileResponse technicianProfile =
                new TechnicianProfileResponse()
                {
                   FirstName = technician.FirstName,
                   LastName = technician.LastName,
                   Bio = technician.Bio,
                   Address = technician.Address,
                    ProfileImagePath = technician.ProfileImagePath,
                    CoverImagePath = technician.CoverImagePath,
                    Skills = technician.Skills
                   .Select(x => new TechnicianSkillResponse
                   {
                     SkillId = x.SkillId,
                     Title = x.Skill.Title
                    })
                   .ToList()


                };
            return technicianProfile;
        }
    }
}
