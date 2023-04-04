namespace GeoService.Api.Handlers.Manholes;

internal sealed class UpdateManholeHandler : IRequestHandler<UpdateManholeRequest, IResult>
{
    private readonly IValidator<UpdateManholeRequest> _validator;
    private readonly IManholeRepo _repo;

    public UpdateManholeHandler(IValidator<UpdateManholeRequest> validator, IManholeRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(UpdateManholeRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var manhole = request.Adapt<Manhole>();
        if (manhole == null)
            return Results.BadRequest();

        if (!await _repo.UpdateManhole(manhole))
            return Results.NotFound();

        return Results.Ok(manhole.Adapt<UpdateManholeResponse>());
    }
}