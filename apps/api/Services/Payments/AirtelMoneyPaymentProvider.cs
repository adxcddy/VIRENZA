using Virenza.Api.Models.Commerce;

namespace Virenza.Api.Services.Payments;

public class AirtelMoneyPaymentProvider : IPaymentProvider
{
    private readonly IConfiguration _configuration;

    public AirtelMoneyPaymentProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public PaymentProvider Provider => PaymentProvider.AirtelMoney;

    public Task<PaymentInitiationResult> InitiateAsync(
        decimal amount,
        string currency,
        string phoneNumber,
        string reference,
        CancellationToken cancellationToken = default)
    {
        // Production Airtel Money integration goes here.
        // Credentials must come from configuration/user-secrets.

        return Task.FromResult(
            new PaymentInitiationResult(
                false,
                null,
                "Airtel Money provider is not configured yet."));
    }

    public Task<PaymentStatusResult> CheckStatusAsync(
        string providerReference,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            new PaymentStatusResult(
                false,
                PaymentStatus.Pending,
                "Airtel Money provider is not configured yet."));
    }
}
