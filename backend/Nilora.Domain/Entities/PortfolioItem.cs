using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Domain.Entities
{
    public class PortfolioItem
    {
        public int Id { get; private set; }

        public int TechnicianId { get; private set; }

        public string Title { get; private set; } = null!;

        public string? Description { get; private set; }

        public string? CoverImagePath { get; private set; }

        public bool IsPublished { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public Technician Technician { get; private set; } = null!;

        private readonly List<PortfolioMedia> _media = [];
        public IReadOnlyCollection<PortfolioMedia> Media => _media;

        private PortfolioItem() { }

        public PortfolioItem(
            int technicianId,
            string title,
            string? description,
            string? coverImagePath)
        {
            if (technicianId <= 0)
                throw new ArgumentException("Invalid technician ID.");

            TechnicianId = technicianId;
            Update(title, description, coverImagePath);

            IsPublished = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(
            string title,
            string? description,
            string? coverImagePath)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.");

            Title = title.Trim();
            Description = description?.Trim();
            CoverImagePath = coverImagePath;
        }

        public void AddMedia(string filePath, int displayOrder = 0)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path is required.");

            if (displayOrder < 0)
                throw new ArgumentOutOfRangeException(nameof(displayOrder));

            _media.Add(new PortfolioMedia(filePath, displayOrder));
        }

        public void RemoveMedia(int mediaId)
        {
            var media = _media.FirstOrDefault(x => x.Id == mediaId);

            if (media is not null)
                _media.Remove(media);
        }

        public void Publish() => IsPublished = true;

        public void Unpublish() => IsPublished = false;
    }
}
