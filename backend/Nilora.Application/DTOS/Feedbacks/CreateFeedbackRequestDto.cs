using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nilora.Application.DTOS.Feedbacks
{
    public class CreateFeedbackRequestDto
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Quality rating must be between 1 and 5.")]
        public int QualityRating { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Hygiene rating must be between 1 and 5.")]
        public int HygieneRating { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Durability rating must be between 1 and 5.")]
        public int DurabilityRating { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Behavior rating must be between 1 and 5.")]
        public int BehaviorRating { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Overall rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [MaxLength(2000, ErrorMessage = "Comment cannot exceed 2000 characters.")]
        public string? Comment { get; set; }
    }
}
