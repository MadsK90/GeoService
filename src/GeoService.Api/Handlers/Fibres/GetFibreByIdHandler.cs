namespace GeoService.Api.Handlers.Fibres;

internal sealed class GetFibreByIdHandler : IRequestHandler<GetFibreByIdRequest, IResult>
{
    private readonly IValidator<GetFibreByIdRequest> _validator;
    private readonly IFibreRepo _repo;

    public GetFibreByIdHandler(IValidator<GetFibreByIdRequest> validator, IFibreRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(GetFibreByIdRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var fibre = await _repo.GetFibreById(request.Id);
        if (fibre == null)
            return Results.NotFound();

        return Results.Ok(fibre.Adapt<GetFibreByIdResponse>());
    }
}