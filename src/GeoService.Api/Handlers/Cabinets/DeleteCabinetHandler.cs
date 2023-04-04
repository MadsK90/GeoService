namespace GeoService.Api.Handlers.Cabinets;

internal sealed class DeleteCabinetHandler : IRequestHandler<DeleteCabinetRequest, IResult>
{
    private readonly IValidator<DeleteCabinetRequest> _validator;
    private readonly ICabinetRepo _repo;

    public DeleteCabinetHandler(IValidator<DeleteCabinetRequest> validator, ICabinetRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(DeleteCabinetRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        if (!await _repo.DeleteCabinet(request.Id))
            return Results.NotFound();

        return Results.NoContent();
    }
}