using Nilora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Domain.Entities
{

    public class Technician
    {
        public int Id { get; private set; }

        public int UserId { get; private set; }

        public string FirstName { get; private set; } = null!;

        public string LastName { get; private set; } = null!;

        public string? Bio { get; private set; }

        public string? Address { get; private set; }

        public string? ProfileImagePath { get; private set; }

        public string? CoverImagePath { get; private set; }

        public string QRCodeToken { get; private set; } = null!;

        public bool IsActive { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public User User { get; private set; } = null!;

        private readonly List<TechnicianSkill> _skills = [];
        public IReadOnlyCollection<TechnicianSkill> Skills => _skills;

        private readonly List<PortfolioItem> _portfolioItems = [];
        public IReadOnlyCollection<PortfolioItem> PortfolioItems => _portfolioItems;

      private readonly List<Feedback> _feedbacks = [];
     public IReadOnlyCollection<Feedback> Feedbacks => _feedbacks;

        private Technician() { }

        public Technician(
            int userId,
            string firstName,
            string lastName,
            string qrCodeToken)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID.");

            UserId = userId;
            ChangeName(firstName, lastName);

            if (string.IsNullOrWhiteSpace(qrCodeToken))
                throw new ArgumentException("QR token is required.");

            QRCodeToken = qrCodeToken;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        public void ChangeName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.");

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }

        public void UpdateProfile(
            string? bio,
            string? address,
            string? profileImagePath,
            string? coverImagePath)
        {
            Bio = bio?.Trim();
            Address = address?.Trim();
            ProfileImagePath = profileImagePath;
            CoverImagePath = coverImagePath;
        }

        public void Activate() => IsActive = true;

        public void Deactivate() => IsActive = false;

        public void AddSkill(int skillId)
        {
            if (skillId <= 0)
                throw new ArgumentException("Invalid skill ID.");

            if (_skills.Any(x => x.SkillId == skillId))
                throw new InvalidOperationException("Skill already exists.");

            _skills.Add(new TechnicianSkill(Id, skillId));
        }

        public void RemoveSkill(int skillId)
        {
            var skill = _skills.FirstOrDefault(x => x.SkillId == skillId);

            if (skill is not null)
                _skills.Remove(skill);
        }
    }
}
