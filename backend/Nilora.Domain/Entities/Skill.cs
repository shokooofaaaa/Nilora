using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Domain.Entities
{
    public class Skill
    {
        public int Id { get; private set; }

        public string Title { get; private set; } = null!;

        public bool IsActive { get; private set; }

        public int DisplayOrder { get; private set; }

        private Skill() { }

        public Skill(string title, int displayOrder = 0)
        {
            ChangeTitle(title);
            DisplayOrder = displayOrder;
            IsActive = true;
        }

        public void ChangeTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Skill title is required.");

            Title = title.Trim();
        }

        public void ChangeDisplayOrder(int displayOrder)
        {
            if (displayOrder < 0)
                throw new ArgumentOutOfRangeException(nameof(displayOrder));

            DisplayOrder = displayOrder;
        }

        public void Activate() => IsActive = true;

        public void Deactivate() => IsActive = false;
    }
}
