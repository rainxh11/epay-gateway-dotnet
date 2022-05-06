using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Chargily.EpayGateway.NET.Validations
{
    public class PaymentRequestValidator : AbstractValidator<EpayPaymentRequest>
    {
        public PaymentRequestValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(75)
                .WithMessage("Payment Amount must be greater or equal to 75.0!");

            RuleFor(x => x.DiscountPercentage)
                .LessThan(100).WithMessage("Discount Percentage must be less than 100%!")
                .GreaterThanOrEqualTo(0).WithMessage("Discount Percentage must be a valid percentage value!");

            RuleFor(x => x.InvoiceNumber)
                .NotEmpty().WithMessage("Invoice Number cannot bet null or empty!");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Client Email cannot be empty!")
                .EmailAddress().WithMessage("Client Email must be a valid email address!");

            RuleFor(x => x.Name)
                .MinimumLength(3).WithMessage("Client Name minimum length is 3!")
                .NotEmpty().WithMessage("Client Name cannot be empty!");

            RuleFor(x => x.CameFrom)
                .NotEmpty().WithMessage("Source Url is required!")
                .Must(x => Uri.TryCreate(x, UriKind.Absolute, out var _)).WithMessage("Source Url must a valid URL!");

            RuleFor(x => x.RedirectBackTo)
                .NotEmpty().WithMessage("Rediction To Url is required!")
                .Must(x => Uri.TryCreate(x, UriKind.Absolute, out var _))
                .WithMessage("Redirection To Url must a valid URL!");

            RuleFor(x => x.PaymentMethod)
                .NotNull().WithMessage("Payment Method is required");
        }
    }
}