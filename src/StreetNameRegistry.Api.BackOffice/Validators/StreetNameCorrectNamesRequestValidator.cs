namespace StreetNameRegistry.Api.BackOffice.Validators
{
    using Abstractions.Requests;
    using Consumer;
    using FluentValidation;

    public class StreetNameCorrectNamesRequestValidator : AbstractValidator<StreetNameCorrectNamesRequest>
    {
        public StreetNameCorrectNamesRequestValidator(ConsumerContext consumerContext)
        {
            RuleForEach(x => x.Straatnamen)
                .Must(StreetNameNotEmptyValidator.IsValid)
                .WithMessage((_, streetName) => $"Straatnaam in '{streetName.Key.ToString().ToLowerInvariant()}' kan niet leeg zijn.")
                .WithErrorCode(StreetNameNotEmptyValidator.Code);

            RuleForEach(x => x.Straatnamen)
                .Must(StreetNameMaxLengthValidator.IsValid)
                .WithMessage((_, streetName) => $"Maximum lengte van een straatnaam in '{streetName.Key.ToString().ToLowerInvariant()}' is 60 tekens. U heeft momenteel {streetName.Value.Length} tekens.")
                .WithErrorCode(StreetNameMaxLengthValidator.Code);
        }
    }
}
