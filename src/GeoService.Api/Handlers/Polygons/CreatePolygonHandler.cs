namespace GeoService.Api.Handlers.Polygons;

internal sealed class CreatePolygonHandler : IRequestHandler<CreatePolygonRequest, IResult>
{
    private readonly IValidator<CreatePolygonRequest> _validator;
    private readonly IPolygonRepo _repo;

    public CreatePolygonHandler(IValidator<CreatePolygonRequest> validator, IPolygonRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(CreatePolygonRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        throw new NotImplementedException("");
    }
}