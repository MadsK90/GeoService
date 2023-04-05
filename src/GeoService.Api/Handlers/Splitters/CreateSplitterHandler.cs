namespace GeoService.Api.Handlers.Splitters;

internal sealed class CreateSplitterHandler : IRequestHandler<CreateSplitterRequest, IResult>
{
    private readonly IValidator<CreateSplitterRequest> _validator;
    private readonly ISplitterRepo _repo;

    public CreateSplitterHandler(IValidator<CreateSplitterRequest> validator, ISplitterRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(CreateSplitterRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var splitter = request.Adapt<Splitter>();
        if (splitter == null)
            return Results.BadRequest();

        var id = await _repo.CreateSplitter(splitter);
        if (id == null)
            return Results.BadRequest();

        return Results.Ok(new CreateSplitterResponse { Id = id.Value });
    }
}