namespace GeoService.Api.Handlers.Polygons;

internal sealed class DeletePolygonHandler : IRequestHandler<DeletePolygonRequest, IResult>
{
    private readonly IValidator<DeletePolygonRequest> _validator;
    private readonly IPolygonRepo _repo;

    public DeletePolygonHandler(IValidator<DeletePolygonRequest> validator, IPolygonRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(DeletePolygonRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        throw new NotImplementedException("");
    }
}