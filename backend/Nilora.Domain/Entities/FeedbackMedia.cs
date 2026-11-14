
namespace Nilora.Domain.Entities
{
    public class FeedbackMedia
    {
        public int Id { get; private set; }

        public int FeedbackId { get; private set; }

        public string FilePath { get; private set; } = null!;

        public Feedback Feedback { get; private set; } = null!;

        private FeedbackMedia() { }

        internal FeedbackMedia(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path is required.");

            string normalizedPath = filePath.Trim();

            if (normalizedPath.Length > 500)
                throw new ArgumentException(
                    "File path cannot exceed 500 characters.",
                    nameof(filePath));

            FilePath = normalizedPath;
        }
    }
}
