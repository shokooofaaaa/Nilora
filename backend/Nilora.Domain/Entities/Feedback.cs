
using Nilora.Domain.Enums;
using System;
using System.Collections.Generic;

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

        public Feedback(int technicianId, string comment, int rating)
        {
            if (technicianId <= 0)
                throw new ArgumentException("Invalid technician ID.");

            TechnicianId = technicianId;
            Status = ReviewStatus.Pending;
            CreatedAt = DateTime.UtcNow;

            Update(comment, rating);
        }

        public void Update(string comment, int rating)
        {
            if (Status != ReviewStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending feedback can be edited.");

            if (string.IsNullOrWhiteSpace(comment))
                throw new ArgumentException(
                    "Comment is required.", nameof(comment));

            string normalizedComment = comment.Trim();

            if (normalizedComment.Length > 2000)
                throw new ArgumentException(
                    "Comment cannot exceed 2000 characters.",
                    nameof(comment));

            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException(nameof(rating));

            Comment = normalizedComment;
            Rating = rating;
        }

        public void AddMedia(string filePath)
        {
            if (Status != ReviewStatus.Pending)
                throw new InvalidOperationException(
                    "Media can only be added to pending feedback.");

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException(
                    "File path is required.", nameof(filePath));

            string normalizedPath = filePath.Trim();

            if (normalizedPath.Length > 500)
                throw new ArgumentException(
                    "File path cannot exceed 500 characters.",
                    nameof(filePath));

            _media.Add(new FeedbackMedia(normalizedPath));
        }

        public void Approve(int adminId)
        {
            if (Status != ReviewStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending feedback can be approved.");

            if (adminId <= 0)
                throw new ArgumentException("Invalid admin ID.");

            Status = ReviewStatus.Approved;
            AdminId = adminId;
            ReviewedAt = DateTime.UtcNow;
        }

        public void Reject(int adminId)
        {
            if (Status != ReviewStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending feedback can be rejected.");

            if (adminId <= 0)
                throw new ArgumentException("Invalid admin ID.");

            Status = ReviewStatus.Rejected;
            AdminId = adminId;
            ReviewedAt = DateTime.UtcNow;
        }
    }
}
