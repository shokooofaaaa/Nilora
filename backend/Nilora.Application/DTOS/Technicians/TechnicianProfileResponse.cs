namespace Nilora.Application.DTOS.Technicians
{
    public record TechnicianProfileResponse
    {
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string? Bio { get; init; }
        public string? Address { get; init; }
        public string? ProfileImagePath { get; init; }
        public string? CoverImagePath { get; init; }

        public IReadOnlyCollection<TechnicianSkillResponse> Skills { get; init; }
            = [];
    }
}