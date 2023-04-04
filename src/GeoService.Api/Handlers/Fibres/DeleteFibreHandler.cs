namespace GeoService.Api.Handlers.Fibres;

internal sealed class DeleteFibreHandler : IRequestHandler<DeleteFibreRequest, IResult>
{
    private readonly IValidator<DeleteFibreRequest> _validator;
    private readonly IFibreRepo _repo;

    public DeleteFibreHandler(IValidator<DeleteFibreRequest> validator, IFibreRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(DeleteFibreRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        if (!await _repo.DeleteFibre(request.Id))
            return Results.NotFound();

        return Results.NoContent();
    }
}