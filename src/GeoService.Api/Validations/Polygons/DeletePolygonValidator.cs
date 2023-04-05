namespace GeoService.Api.Validations.Polygons;

public sealed class DeletePolygonValidator : AbstractValidator<DeletePolygonRequest>
{
    public DeletePolygonValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}