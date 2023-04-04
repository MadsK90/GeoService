namespace GeoService.Api.Handlers.Cabinets;

internal sealed class GetCabinetByIdHandler : IRequestHandler<GetCabinetByIdRequest, IResult>
{
    private readonly IValidator<GetCabinetByIdRequest> _validator;
    private readonly ICabinetRepo _repo;

    public GetCabinetByIdHandler(IValidator<GetCabinetByIdRequest> validator, ICabinetRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(GetCabinetByIdRequest request, CancellationToken cancellationToken)
    {
        var validationResults = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResults.IsValid)
            return Results.BadRequest(validationResults.Errors);

        var cabinet = await _repo.GetCabinetById(request.Id);
        if (cabinet == null)
            return Results.NotFound();

        return Results.Ok(cabinet.Adapt<GetCabinetByIdResponse>());
    }
}