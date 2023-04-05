namespace GeoService.Api.Validations.Polygons;

public sealed class GetPolygonByIdValidator : AbstractValidator<GetPolygonByIdRequest>
{
    public GetPolygonByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}