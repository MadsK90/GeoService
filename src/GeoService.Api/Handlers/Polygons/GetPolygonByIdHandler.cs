namespace GeoService.Api.Handlers.Polygons;

internal sealed class GetPolygonByIdHandler : IRequestHandler<GetPolygonByIdRequest, IResult>
{
    private readonly IValidator<GetPolygonByIdRequest> _validator;
    private readonly IPolygonRepo _repo;

    public GetPolygonByIdHandler(IValidator<GetPolygonByIdRequest> validator, IPolygonRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(GetPolygonByIdRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var polygon = await _repo.GetPolygonById(request.Id);
        if (polygon == null)
            return Results.NotFound();

        return Results.Ok(polygon.Adapt<GetPolygonByIdResponse>());
    }
}