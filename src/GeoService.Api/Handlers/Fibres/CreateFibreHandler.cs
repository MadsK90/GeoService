namespace GeoService.Api.Handlers.Fibres;

internal sealed class CreateFibreHandler : IRequestHandler<CreateFibreRequest, IResult>
{
    private readonly IValidator<CreateFibreRequest> _validator;
    private readonly IFibreRepo _repo;

    public CreateFibreHandler(IValidator<CreateFibreRequest> validator, IFibreRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(CreateFibreRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var fibre = request.Adapt<Fibre>();
        if (fibre == null)
            return Results.BadRequest();

        var id = await _repo.CreateFibre(fibre);
        if (id == null)
            return Results.BadRequest();

        return Results.Ok(new CreateFibreResponse { Id = id.Value });
    }
}