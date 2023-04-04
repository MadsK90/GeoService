namespace GeoService.Api.Handlers.Manholes;

internal sealed class DeleteManholeHandler : IRequestHandler<DeleteManholeRequest, IResult>
{
    private readonly IValidator<DeleteManholeRequest> _validator;
    private readonly IManholeRepo _repo;

    public DeleteManholeHandler(IValidator<DeleteManholeRequest> validator, IManholeRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(DeleteManholeRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        if (!await _repo.DeleteManhole(request.Id))
            return Results.NotFound();

        return Results.NoContent();
    }
}