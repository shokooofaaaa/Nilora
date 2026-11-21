using System;

namespace Nilora.Application.DTOS.Feedbacks
{
    /// <summary>
    /// نتیجه سرویس رد بازخورد؛ وضعیت را طوری نگه می‌دارد که کنترلر
    /// بتواند آن را به 200، 404 یا 409 نگاشت کند.
    /// </summary>
    public class RejectFeedbackResult
    {
        public RejectFeedbackStatus Status { get; init; }

        public string? Message { get; init; }

        public RejectFeedbackResponseDto? Data { get; init; }

        public static RejectFeedbackResult Success(RejectFeedbackResponseDto data)
        {
            ArgumentNullException.ThrowIfNull(data);

            return new RejectFeedbackResult
            {
                Status = RejectFeedbackStatus.Success,
                Data = data
            };
        }

        public static RejectFeedbackResult NotFound(string message)
        {
            return new RejectFeedbackResult
            {
                Status = RejectFeedbackStatus.NotFound,
                Message = message
            };
        }

        public static RejectFeedbackResult Conflict(string message)
        {
            return new RejectFeedbackResult
            {
                Status = RejectFeedbackStatus.Conflict,
                Message = message
            };
        }
    }
}
