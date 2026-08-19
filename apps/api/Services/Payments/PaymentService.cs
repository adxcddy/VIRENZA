using Microsoft.EntityFrameworkCore;
using Virenza.Api.Data;
using Virenza.Api.Models.Commerce;

namespace Virenza.Api.Services.Payments;

public class PaymentService
{
    private readonly VirenzaDbContext _db;
    private readonly PaymentProviderResolver _resolver;

    public PaymentService(
        VirenzaDbContext db,
        PaymentProviderResolver resolver)
    {
        _db = db;
        _resolver = resolver;
    }

    public async Task<Payment> CreatePaymentAsync(
        Guid userId,
        decimal amount,
        string currency,
        PaymentProvider provider,
        PaymentPurpose purpose,
        string? phoneNumber,
        string? description,
        CancellationToken cancellationToken = default)
    {
        if (amount <= 0)
            throw new ArgumentException("Payment amount must be greater than zero.");

        if (purpose == PaymentPurpose.Subscription && amount < 10000)
            throw new ArgumentException(
                "Subscription payments must be at least 10,000 in the selected currency.");

        currency = currency.Trim().ToUpperInvariant();

        if (currency.Length != 3)
            throw new ArgumentException("Currency must be a valid 3-letter currency code.");

        if (provider is PaymentProvider.MTNMobileMoney or PaymentProvider.AirtelMoney)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException(
                    "A mobile-money phone number is required.");
        }

        var payment = new Payment
        {
            UserId = userId,
            Amount = amount,
            Currency = currency,
            Provider = provider.ToString(),
            Purpose = purpose,
            Status = PaymentStatus.Pending,
            PhoneNumber = phoneNumber,
            Description = description,
            ExternalReference = $"VIR-{Guid.NewGuid():N}",
            CreatedAt = DateTime.UtcNow
        };

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync(cancellationToken);

        if (provider is PaymentProvider.MTNMobileMoney or PaymentProvider.AirtelMoney)
        {
            var paymentProvider = _resolver.Resolve(provider);

            payment.Status = PaymentStatus.Processing;

            var result = await paymentProvider.InitiateAsync(
                amount,
                currency,
                phoneNumber!,
                payment.ExternalReference,
                cancellationToken);

            if (!result.Success)
            {
                payment.Status = PaymentStatus.Failed;
                payment.FailureReason = result.Message;
                payment.FailedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync(cancellationToken);

                return payment;
            }

            payment.ProviderReference = result.ProviderReference;

            await _db.SaveChangesAsync(cancellationToken);
        }

        return payment;
    }

    public async Task<Payment?> CheckStatusAsync(
        Guid paymentId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var payment = await _db.Payments
            .FirstOrDefaultAsync(
                x => x.Id == paymentId && x.UserId == userId,
                cancellationToken);

        if (payment == null)
            return null;

        if (string.IsNullOrWhiteSpace(payment.ProviderReference))
            return payment;

        if (!Enum.TryParse<PaymentProvider>(
                payment.Provider,
                true,
                out var provider))
        {
            payment.Status = PaymentStatus.Failed;
            payment.FailureReason =
                "Stored payment provider is invalid.";

            payment.FailedAt ??= DateTime.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);

            return payment;
        }

        if (provider is not (
            PaymentProvider.MTNMobileMoney
            or PaymentProvider.AirtelMoney))
        {
            return payment;
        }

        var paymentProvider = _resolver.Resolve(provider);

        var result = await paymentProvider.CheckStatusAsync(
            payment.ProviderReference,
            cancellationToken);

        if (result.Status == PaymentStatus.Successful)
        {
            payment.Status = PaymentStatus.Successful;
            payment.CompletedAt ??= DateTime.UtcNow;
            payment.FailureReason = null;
            payment.FailedAt = null;
        }
        else if (result.Status == PaymentStatus.Failed)
        {
            payment.Status = PaymentStatus.Failed;
            payment.FailureReason = result.Message;
            payment.FailedAt ??= DateTime.UtcNow;
        }
        else
        {
            payment.Status = result.Status;
        }

        await _db.SaveChangesAsync(cancellationToken);

        return payment;
    }

    public async Task<Payment?> GetAsync(
        Guid paymentId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _db.Payments
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == paymentId && x.UserId == userId,
                cancellationToken);
    }
}
