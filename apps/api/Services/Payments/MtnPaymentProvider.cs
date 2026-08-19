using Virenza.Api.Models.Commerce;

namespace Virenza.Api.Services.Payments;

public class MtnPaymentProvider : IPaymentProvider
{
    private readonly IConfiguration _configuration;

    public MtnPaymentProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public PaymentProvider Provider => PaymentProvider.MTNMobileMoney;

    public Task<PaymentInitiationResult> InitiateAsync(
        decimal amount,
        string currency,
        string phoneNumber,
        string reference,
        CancellationToken cancellationToken = default)
    {
        // Production MTN MoMo API integration goes here.
        // Credentials must come from configuration/user-secrets,
        // never from source code.

        return Task.FromResult(
            new PaymentInitiationResult(
                false,
                null,
                "MTN Mobile Money provider is not configured yet."));
    }

    public Task<PaymentStatusResult> CheckStatusAsync(
        string providerReference,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            new PaymentStatusResult(
                false,
                PaymentStatus.Pending,
                "MTN Mobile Money provider is not configured yet."));
    }
}
