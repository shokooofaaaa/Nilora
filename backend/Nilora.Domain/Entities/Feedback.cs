using Nilora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Domain.Entities
{
    public class Feedback
    {
        public int Id { get; private set; }

        public int TechnicianId { get; private set; }

        public int? AdminId { get; private set; }

        public string Comment { get; private set; } = null!;

        public int Rating { get; private set; }

        public ReviewStatus Status { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime? ReviewedAt { get; private set; }

        public Technician Technician { get; private set; } = null!;

        public User? Admin { get; private set; }

        private readonly List<FeedbackMedia> _media = [];
        public IReadOnlyCollection<FeedbackMedia> Media => _media;

        private Feedback() { }

        public Feedback(
            int technicianId,
            string firstName,
            string lastName,
            string comment,
            int rating)
        {
            if (technicianId <= 0)
                throw new ArgumentException("Invalid technician ID.");

            TechnicianId = technicianId;
            Update(firstName, lastName, comment, rating);

            Status = ReviewStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(
            string firstName,
            string lastName,
            string comment,
            int rating)
        {
            if (Status != ReviewStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending reviews can be edited.");

            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.");

            if (string.IsNullOrWhiteSpace(comment))
                throw new ArgumentException("Comment is required.");

            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException(nameof(rating));
            Comment = comment.Trim();
            Rating = rating;
        }

        public void AddMedia(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path is required.");

            _media.Add(new FeedbackMedia(filePath));
        }

        public void Approve(int adminId)
        {
            if (Status != ReviewStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending reviews can be approved.");

            if (adminId <= 0)
                throw new ArgumentException("Invalid admin ID.");

            if (_media.Count == 0)
                throw new InvalidOperationException(
                    "Review must have at least one image.");

            Status = ReviewStatus.Approved;
            AdminId = adminId;
            ReviewedAt = DateTime.UtcNow;
        }

        public void Reject(int adminId)
        {
            if (Status != ReviewStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending reviews can be rejected.");

            if (adminId <= 0)
                throw new ArgumentException("Invalid admin ID.");

            Status = ReviewStatus.Rejected;
            AdminId = adminId;
            ReviewedAt = DateTime.UtcNow;
        }
    }
}
