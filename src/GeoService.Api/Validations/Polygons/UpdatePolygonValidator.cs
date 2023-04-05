namespace GeoService.Api.Validations.Polygons;

public sealed class UpdatePolygonValidator : AbstractValidator<UpdatePolygonRequest>
{
    public UpdatePolygonValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Points)
            .Must(y => y.Count() > 2)
            .WithMessage("Polygon needs to have atleast 3 points");

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