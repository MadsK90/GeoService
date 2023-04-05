﻿using Route = GeoService.Database.Models.Route;

namespace GeoService.Api.Handlers.Routes;

internal sealed class UpdateRouteHandler : IRequestHandler<UpdateRouteRequest, IResult>
{
    private readonly IValidator<UpdateRouteRequest> _validator;
    private readonly IRouteRepo _repo;

    public UpdateRouteHandler(IValidator<UpdateRouteRequest> validator, IRouteRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(UpdateRouteRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var route = request.Adapt<Route>();
        if (route == null)
            return Results.BadRequest();

        if (!await _repo.UpdateRoute(route))
            return Results.NotFound();

        return Results.Ok(route.Adapt<UpdatePolygonResponse>());
    }
}