using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Domain.Entities
{
    public class ReviewMedia
    {
        public int Id { get; private set; }

        public int ReviewId { get; private set; }

        public string FilePath { get; private set; } = null!;

        public Review Review { get; private set; } = null!;

        private ReviewMedia() { }

        internal ReviewMedia(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path is required.");

            FilePath = filePath;
        }
    }
}
