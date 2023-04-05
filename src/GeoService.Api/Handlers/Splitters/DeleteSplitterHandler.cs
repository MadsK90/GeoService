namespace GeoService.Api.Handlers.Splitters;

internal sealed class DeleteSplitterHandler : IRequestHandler<DeleteSplitterRequest, IResult>
{
    private readonly IValidator<DeleteSplitterRequest> _validator;
    private readonly ISplitterRepo _repo;

    public DeleteSplitterHandler(IValidator<DeleteSplitterRequest> validator, ISplitterRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(DeleteSplitterRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        throw new NotImplementedException();
    }
}