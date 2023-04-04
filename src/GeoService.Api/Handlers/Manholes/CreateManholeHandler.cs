namespace GeoService.Api.Handlers.Manholes;

internal sealed class CreateManholeHandler : IRequestHandler<CreateManholeRequest, IResult>
{
    private readonly IValidator<CreateManholeRequest> _validator;
    private readonly IManholeRepo _repo;

    public CreateManholeHandler(IValidator<CreateManholeRequest> validator, IManholeRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(CreateManholeRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        throw new NotImplementedException("");
    }
}