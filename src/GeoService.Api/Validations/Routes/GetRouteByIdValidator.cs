namespace GeoService.Api.Validations.Routes;

public sealed class GetRouteByIdValidator : AbstractValidator<GetRouteByIdRequest>
{
    public GetRouteByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}