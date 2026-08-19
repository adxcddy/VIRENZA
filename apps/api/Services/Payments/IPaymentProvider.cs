using Virenza.Api.Models.Commerce;

namespace Virenza.Api.Services.Payments;

public interface IPaymentProvider
{
    PaymentProvider Provider { get; }

    Task<PaymentInitiationResult> InitiateAsync(
        decimal amount,
        string currency,
        string phoneNumber,
        string reference,
        CancellationToken cancellationToken = default);

    Task<PaymentStatusResult> CheckStatusAsync(
        string providerReference,
        CancellationToken cancellationToken = default);
}

public record PaymentInitiationResult(
    bool Success,
    string? ProviderReference,
    string? Message);

public record PaymentStatusResult(
    bool Success,
    PaymentStatus Status,
    string? Message);
