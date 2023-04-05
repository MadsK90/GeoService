namespace GeoService.Api.Handlers.Routes;

internal sealed class DeleteRouteHandler : IRequestHandler<DeleteRouteRequest, IResult>
{
    private readonly IValidator<DeleteRouteRequest> _validator;
    private readonly IRouteRepo _repo;

    public DeleteRouteHandler(IValidator<DeleteRouteRequest> validator, IRouteRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(DeleteRouteRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        if (!await _repo.DeleteRoute(request.Id))
            return Results.NotFound();

        return Results.NoContent();
    }
}