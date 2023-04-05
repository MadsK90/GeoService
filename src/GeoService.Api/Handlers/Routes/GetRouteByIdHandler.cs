namespace GeoService.Api.Handlers.Routes;

internal sealed class GetRouteByIdHandler : IRequestHandler<GetRouteByIdRequest, IResult>
{
    private readonly IValidator<GetRouteByIdRequest> _validator;
    private readonly IRouteRepo _repo;

    public GetRouteByIdHandler(IValidator<GetRouteByIdRequest> validator, IRouteRepo repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<IResult> Handle(GetRouteByIdRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        throw new NotImplementedException();
    }
}