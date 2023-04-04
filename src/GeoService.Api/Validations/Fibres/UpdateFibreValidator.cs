namespace GeoService.Api.Validations.Fibres;

public sealed class UpdateFibreValidator : AbstractValidator<UpdateFibreRequest>
{
    public UpdateFibreValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Air)
            .NotNull();

        RuleFor(x => x.Size)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(1024)
            .WithMessage("Fibre cable cant be smaller than 1 and not bigger than 1024");

        RuleFor(x => x.Points)
            .Must(y => y.Count() > 1)
            .WithMessage("Fibre needs to have atleast 2 points");
    }
}