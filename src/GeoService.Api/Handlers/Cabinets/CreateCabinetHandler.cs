namespace GeoService.Api.Handlers.Cabinets;

internal sealed class CreateCabinetHandler : IRequestHandler<CreateCabinetRequest, IResult>
{
    private readonly IValidator<CreateCabinetRequest> _validator;
    private readonly ICabinetRepo _repo;

    public CreateCabinetHandler(IValidator<CreateCabinetRequest> validator, ICabinetRepo cabinetRepo)
    {
        _validator = validator;
        _repo = cabinetRepo;
    }

    public async Task<IResult> Handle(CreateCabinetRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var cabinet = request.Adapt<Cabinet>();
        if (cabinet == null)
            return Results.BadRequest();

        var id = await _repo.CreateCabinet(cabinet);
        if (id == null)
            return Results.BadRequest();

        return Results.Ok(new CreateCabinetResponse { Id = id.Value });
    }
}