namespace GeoService.Api.Validations.Splitters;

public sealed class UpdateSplitterValidator : AbstractValidator<UpdateSplitterRequest>
{
    public UpdateSplitterValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(15)
            .WithMessage("Name must be between 3 and 15 characters long");

        RuleFor(x => x.Latitude)
            .GreaterThanOrEqualTo(-90)
            .LessThanOrEqualTo(90)
            .WithMessage("Latitude is invalid, value must be between 90 and -90");

        RuleFor(x => x.Longitude)
            .GreaterThanOrEqualTo(-180)
            .LessThanOrEqualTo(180)
            .WithMessage("Longitude is invalid, value must be between 180 and -180");
    }
}