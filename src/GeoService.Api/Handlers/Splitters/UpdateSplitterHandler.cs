namespace GeoService.Api.Handlers.Splitters;

internal sealed class UpdateSplitterHandler : IRequestHandler<UpdateSplitterRequest, IResult>
{
    private readonly IValidator<UpdateSplitterRequest> _validator;
    private readonly ISplitterRepo _repo;

    public UpdateSplitterHandler(IValidator<UpdateSplitterRequest> validator, ISplitterRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(UpdateSplitterRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        throw new NotImplementedException();
    }
}