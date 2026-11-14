
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

        public string? Comment { get; private set; }

        public int Rating { get; private set; }

        public int QualityRating { get; private set; }

        public int HygieneRating { get; private set; }

        public int DurabilityRating { get; private set; }

        public int BehaviorRating { get; private set; }

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
            int qualityRating,
            int hygieneRating,
            int durabilityRating,
            int behaviorRating,
            int rating,
            string? comment = null)
        {
            if (technicianId <= 0)
                throw new ArgumentException("Invalid technician ID.");

            TechnicianId = technicianId;
            Status = ReviewStatus.Pending;
            CreatedAt = DateTime.UtcNow;

            Update(
                qualityRating,
                hygieneRating,
                durabilityRating,
                behaviorRating,
                rating,
                comment);
        }

        public void Update(
            int qualityRating,
            int hygieneRating,
            int durabilityRating,
            int behaviorRating,
            int rating,
            string? comment = null)
        {
            if (Status != ReviewStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending feedback can be edited.");

            ValidateRating(qualityRating, nameof(qualityRating));
            ValidateRating(hygieneRating, nameof(hygieneRating));
            ValidateRating(durabilityRating, nameof(durabilityRating));
            ValidateRating(behaviorRating, nameof(behaviorRating));
            ValidateRating(rating, nameof(rating));

            string? normalizedComment = string.IsNullOrWhiteSpace(comment)
                ? null
                : comment.Trim();

            if (normalizedComment?.Length > 2000)
                throw new ArgumentException(
                    "Comment cannot exceed 2000 characters.",
                    nameof(comment));

            QualityRating = qualityRating;
            HygieneRating = hygieneRating;
            DurabilityRating = durabilityRating;
            BehaviorRating = behaviorRating;
            Rating = rating;
            Comment = normalizedComment;
        }

        private static void ValidateRating(int rating, string parameterName)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    "Rating must be between 1 and 5.");
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
