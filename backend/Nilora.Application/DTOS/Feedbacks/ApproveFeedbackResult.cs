using System;

namespace Nilora.Application.DTOS.Feedbacks
{
    /// <summary>
    /// نتیجه سرویس تأیید بازخورد؛ وضعیت را طوری نگه می‌دارد که کنترلر
    /// بتواند آن را به 200، 404 یا 409 نگاشت کند.
    /// </summary>
    public class ApproveFeedbackResult
    {
        public ApproveFeedbackStatus Status { get; init; }

        public string? Message { get; init; }

        public ApproveFeedbackResponseDto? Data { get; init; }

        public static ApproveFeedbackResult Success(ApproveFeedbackResponseDto data)
        {
            ArgumentNullException.ThrowIfNull(data);

            return new ApproveFeedbackResult
            {
                Status = ApproveFeedbackStatus.Success,
                Data = data
            };
        }

        public static ApproveFeedbackResult NotFound(string message)
        {
            return new ApproveFeedbackResult
            {
                Status = ApproveFeedbackStatus.NotFound,
                Message = message
            };
        }

        public static ApproveFeedbackResult Conflict(string message)
        {
            return new ApproveFeedbackResult
            {
                Status = ApproveFeedbackStatus.Conflict,
                Message = message
            };
        }
    }
}
