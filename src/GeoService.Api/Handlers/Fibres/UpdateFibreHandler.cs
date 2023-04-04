namespace GeoService.Api.Handlers.Fibres;

internal sealed class UpdateFibreHandler : IRequestHandler<UpdateFibreRequest, IResult>
{
    private readonly IValidator<UpdateFibreRequest> _validator;
    private readonly IFibreRepo _repo;

    public UpdateFibreHandler(IValidator<UpdateFibreRequest> validator, IFibreRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(UpdateFibreRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var fibre = request.Adapt<Fibre>();
        if (fibre == null)
            return Results.BadRequest();

        if (!await _repo.UpdateFibre(fibre))
            return Results.NotFound();

        return Results.Ok(fibre.Adapt<UpdateFibreResponse>());
    }
}