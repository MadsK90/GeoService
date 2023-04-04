namespace GeoService.Api.Validations.Fibres;

public sealed class CreateFibreValidator : AbstractValidator<CreateFibreRequest>
{
    public CreateFibreValidator()
    {
        RuleFor(x => x.Air)
            .NotNull();

        RuleFor(x => x.Size)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(1024)
            .WithMessage("Fibre cable cant be smaller than 1 and not bigger than 1024");

        RuleFor(x => x.Points)
            .Must(y => y.Count() > 1)
            .WithMessage("Fibre needs to have atleast 2 points");

        RuleForEach(x => x.Points)
            .ChildRules(point =>
            {
                point.RuleFor(p => p.Latitude)
                    .GreaterThanOrEqualTo(-90)
                    .LessThanOrEqualTo(90)
                    .WithMessage("Latitude is invalid, value must be between 90 and -90");

                point.RuleFor(p => p.Longitude)
                    .GreaterThanOrEqualTo(-180)
                    .LessThanOrEqualTo(180)
                    .WithMessage("Longitude is invalid, value must be between 180 and -180");
            });
    }
}