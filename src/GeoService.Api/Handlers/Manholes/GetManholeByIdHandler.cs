namespace GeoService.Api.Handlers.Manholes;

internal sealed class GetManholeByIdHandler : IRequestHandler<GetManholeByIdRequest, IResult>
{
    private readonly IValidator<GetManholeByIdRequest> _validator;
    private readonly IManholeRepo _repo;

    public GetManholeByIdHandler(IValidator<GetManholeByIdRequest> validator, IManholeRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(GetManholeByIdRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        throw new NotImplementedException("");
    }
}