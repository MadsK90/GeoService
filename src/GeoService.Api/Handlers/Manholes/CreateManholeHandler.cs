namespace GeoService.Api.Handlers.Manholes;

internal sealed class CreateManholeHandler : IRequestHandler<CreateManholeRequest, IResult>
{
    private readonly IValidator<CreateManholeRequest> _validator;
    private readonly IManholeRepo _repo;

    public CreateManholeHandler(IValidator<CreateManholeRequest> validator, IManholeRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(CreateManholeRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var manhole = request.Adapt<Manhole>();
        if (manhole == null)
            return Results.BadRequest();

        var id = await _repo.CreateManhole(manhole);
        if (id == null)
            return Results.BadRequest();

        return Results.Ok(new CreateManholeResponse { Id = id.Value });
    }
}