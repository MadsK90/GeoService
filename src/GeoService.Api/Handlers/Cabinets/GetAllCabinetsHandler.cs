namespace GeoService.Api.Handlers.Cabinets;

internal sealed class GetAllCabinetsHandler : IRequestHandler<GetAllCabinetsRequest, IResult>
{
    private readonly IValidator<GetAllCabinetsRequest> _validator;
    private readonly ICabinetRepo _repo;

    public GetAllCabinetsHandler(IValidator<GetAllCabinetsRequest> validator, ICabinetRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(GetAllCabinetsRequest request, CancellationToken cancellationToken)
    {
        var validationResults = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResults.IsValid)
            return Results.BadRequest(validationResults.Errors);

        var cabinets = await _repo.GetAllCabinets();
        if (!cabinets.Any())
            return Results.NotFound();

        return Results.Ok(new GetAllCabinetsResponse { Cabinets = cabinets.Adapt<IEnumerable<CabinetDto>>() });
    }
}