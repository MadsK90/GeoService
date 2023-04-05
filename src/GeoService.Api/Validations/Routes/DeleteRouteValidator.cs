namespace GeoService.Api.Validations.Routes;

public sealed class DeleteRouteValidator : AbstractValidator<DeleteRouteRequest>
{
    public DeleteRouteValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}