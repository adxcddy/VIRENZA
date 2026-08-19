using Virenza.Api.Models.Commerce;

namespace Virenza.Api.Services.Payments;

public class PaymentProviderResolver
{
    private readonly IEnumerable<IPaymentProvider> _providers;

    public PaymentProviderResolver(IEnumerable<IPaymentProvider> providers)
    {
        _providers = providers;
    }

    public IPaymentProvider Resolve(PaymentProvider provider)
    {
        return _providers.FirstOrDefault(x => x.Provider == provider)
            ?? throw new InvalidOperationException(
                $"Payment provider '{provider}' is not configured.");
    }
}
