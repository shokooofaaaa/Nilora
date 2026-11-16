namespace Nilora.Application.DTOS.Technicians
{
    public record TechnicianSkillResponse
    {
        public int SkillId { get; init; }
        public string Title { get; init; } = null!;
    }
}