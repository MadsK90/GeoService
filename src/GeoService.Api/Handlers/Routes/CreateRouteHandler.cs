using Route = GeoService.Database.Models.Route;

namespace GeoService.Api.Handlers.Routes;

internal sealed class CreateRouteHandler : IRequestHandler<CreateRouteRequest, IResult>
{
    private readonly IValidator<CreateRouteRequest> _validator;
    private readonly IRouteRepo _repo;

    public CreateRouteHandler(IValidator<CreateRouteRequest> validator, IRouteRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(CreateRouteRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var route = request.Adapt<Route>();
        if (route == null)
            return Results.BadRequest();

        var id = await _repo.CreateRoute(route);
        if (id == null)
            return Results.BadRequest();

        return Results.Ok(new CreateRouteResponse { Id = id.Value });
    }
}