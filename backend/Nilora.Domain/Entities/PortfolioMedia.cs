using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Domain.Entities
{
    public class PortfolioMedia
    {
        public int Id { get; private set; }

        public int PortfolioItemId { get; private set; }

        public string FilePath { get; private set; } = null!;

        public int DisplayOrder { get; private set; }

        public PortfolioItem PortfolioItem { get; private set; } = null!;

        private PortfolioMedia() { }

        internal PortfolioMedia(string filePath, int displayOrder)
        {
            ChangeFilePath(filePath);
            ChangeDisplayOrder(displayOrder);
        }

        public void ChangeFilePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path is required.");

            FilePath = filePath;
        }

        public void ChangeDisplayOrder(int displayOrder)
        {
            if (displayOrder < 0)
                throw new ArgumentOutOfRangeException(nameof(displayOrder));

            DisplayOrder = displayOrder;
        }
    }
}
