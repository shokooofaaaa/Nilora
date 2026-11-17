using System;

namespace Nilora.Application.DTOs.Technicians;

/// <summary>
/// نتیجه سرویس فرم عمومی بازخورد؛ وضعیت را طوری نگه می‌دارد که کنترلر
/// بتواند آن را به 200، 400، 403 یا 404 نگاشت کند.
/// </summary>
public class PublicFeedbackFormResult
{
    public PublicFeedbackFormStatus Status { get; init; }

    public string? Message { get; init; }

    public PublicFeedbackFormDto? Data { get; init; }

    public static PublicFeedbackFormResult Success(PublicFeedbackFormDto data)
    {
        ArgumentNullException.ThrowIfNull(data);

        return new PublicFeedbackFormResult
        {
            Status = PublicFeedbackFormStatus.Success,
            Data = data
        };
    }

    public static PublicFeedbackFormResult InvalidToken(string message)
    {
        return new PublicFeedbackFormResult
        {
            Status = PublicFeedbackFormStatus.InvalidToken,
            Message = message
        };
    }

    public static PublicFeedbackFormResult NotFound(string message)
    {
        return new PublicFeedbackFormResult
        {
            Status = PublicFeedbackFormStatus.NotFound,
            Message = message
        };
    }

    public static PublicFeedbackFormResult Forbidden(string message)
    {
        return new PublicFeedbackFormResult
        {
            Status = PublicFeedbackFormStatus.FeedbackDisabled,
            Message = message
        };
    }
}
