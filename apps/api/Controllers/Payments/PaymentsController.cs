using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Virenza.Api.DTOs.Payments;
using Virenza.Api.Models.Commerce;
using Virenza.Api.Services.Payments;

namespace Virenza.Api.Controllers.Payments;

[ApiController]
[Route("api/payments")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;

    public PaymentsController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePaymentRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        if (!TryParseSupportedProvider(request.Provider, out var provider))
        {
            return BadRequest(new
            {
                success = false,
                message = "Unsupported payment provider.",
                supportedProviders = new[]
                {
                    "MTNMobileMoney",
                    "AirtelMoney",
                    "Visa",
                    "Mastercard",
                    "BankTransfer"
                }
            });
        }

        if (!TryParsePaymentPurpose(request.Purpose, out var purpose))
        {
            return BadRequest(new
            {
                success = false,
                message = "Unsupported payment purpose.",
                supportedPurposes = new[]
                {
                    "Subscription",
                    "Course",
                    "Donation",
                    "Sponsorship",
                    "Scholarship",
                    "Other"
                }
            });
        }

        try
        {
            var payment = await _paymentService.CreatePaymentAsync(
                userId.Value,
                request.Amount,
                request.Currency,
                provider,
                purpose,
                request.PhoneNumber,
                request.Description,
                cancellationToken);

            return Ok(new
            {
                success = true,
                payment = new
                {
                    payment.Id,
                    payment.Amount,
                    payment.Currency,
                    payment.Provider,
                    Purpose = payment.Purpose.ToString(),
                    Status = payment.Status.ToString(),
                    payment.ExternalReference,
                    payment.ProviderReference,
                    payment.CreatedAt
                }
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }

    [HttpGet("{id:guid}/status")]
    public async Task<IActionResult> Status(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        var payment = await _paymentService.CheckStatusAsync(
            id,
            userId.Value,
            cancellationToken);

        if (payment == null)
            return NotFound();

        return Ok(new
        {
            success = true,
            payment = new
            {
                payment.Id,
                payment.Amount,
                payment.Currency,
                payment.Provider,
                Purpose = payment.Purpose.ToString(),
                Status = payment.Status.ToString(),
                payment.ExternalReference,
                payment.ProviderReference,
                payment.CreatedAt,
                payment.CompletedAt,
                payment.FailedAt,
                payment.FailureReason
            }
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        var payment = await _paymentService.GetAsync(
            id,
            userId.Value,
            cancellationToken);

        if (payment == null)
            return NotFound(new
            {
                success = false,
                message = "Payment not found."
            });

        return Ok(new
        {
            success = true,
            payment = new
            {
                payment.Id,
                payment.Amount,
                payment.Currency,
                payment.Provider,
                Purpose = payment.Purpose.ToString(),
                Status = payment.Status.ToString(),
                payment.ExternalReference,
                payment.ProviderReference,
                payment.CreatedAt,
                payment.FailedAt,
                payment.FailureReason
            }
        });
    }

    private static bool TryParseSupportedProvider(
        string? value,
        out PaymentProvider provider)
    {
        provider = default;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        if (int.TryParse(value, out _))
            return false;

        if (!Enum.TryParse<PaymentProvider>(
                value.Trim(),
                ignoreCase: true,
                out provider))
        {
            return false;
        }

        return provider is
            PaymentProvider.MTNMobileMoney or
            PaymentProvider.AirtelMoney or
            PaymentProvider.Visa or
            PaymentProvider.Mastercard or
            PaymentProvider.BankTransfer;
    }

    private static bool TryParsePaymentPurpose(
        string? value,
        out PaymentPurpose purpose)
    {
        purpose = default;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        if (int.TryParse(value, out _))
            return false;

        return Enum.TryParse(
            value.Trim(),
            ignoreCase: true,
            out purpose);
    }

    private Guid? GetUserId()
    {
        var value =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        return Guid.TryParse(value, out var id)
            ? id
            : null;
    }
}
