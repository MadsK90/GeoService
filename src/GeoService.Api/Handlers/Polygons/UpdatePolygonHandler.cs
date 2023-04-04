namespace GeoService.Api.Handlers.Polygons;

internal sealed class UpdatePolygonHandler : IRequestHandler<UpdatePolygonRequest, IResult>
{
    private readonly IValidator<UpdatePolygonRequest> _validator;
    private readonly IPolygonRepo _repo;

    public UpdatePolygonHandler(IValidator<UpdatePolygonRequest> validator, IPolygonRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(UpdatePolygonRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        throw new NotImplementedException("");
    }
}