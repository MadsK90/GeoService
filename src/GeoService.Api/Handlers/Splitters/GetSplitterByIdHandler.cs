namespace GeoService.Api.Handlers.Splitters;

internal sealed class GetSplitterByIdHandler : IRequestHandler<GetSplitterByIdRequest, IResult>
{
    private readonly IValidator<GetSplitterByIdRequest> _validator;
    private readonly ISplitterRepo _repo;

    public GetSplitterByIdHandler(IValidator<GetSplitterByIdRequest> validator, ISplitterRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(GetSplitterByIdRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        throw new NotImplementedException();
    }
}