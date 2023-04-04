namespace GeoService.Api.Handlers.Cabinets;

internal sealed class UpdateCabinetHandler : IRequestHandler<UpdateCabinetRequest, IResult>
{
    private readonly IValidator<UpdateCabinetRequest> _validator;
    private readonly ICabinetRepo _repo;

    public UpdateCabinetHandler(IValidator<UpdateCabinetRequest> validator, ICabinetRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(UpdateCabinetRequest request, CancellationToken cancellationToken)
    {
        var validationResults = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResults.IsValid)
            return Results.BadRequest(validationResults.Errors);

        var cabinet = request.Adapt<Cabinet>();
        if (cabinet == null)
            return Results.BadRequest();

        if (!await _repo.UpdateCabinet(cabinet))
            return Results.NotFound();

        return Results.Ok(cabinet.Adapt<UpdateCabinetResponse>());
    }
}