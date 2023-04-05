namespace GeoService.Api.Handlers.Routes;

internal sealed class GetRouteByIdHandler : IRequestHandler<GetRouteByIdRequest, IResult>
{
    private readonly IValidator<GetRouteByIdRequest> _validator;

    public GetRouteByIdHandler(IValidator<GetRouteByIdRequest> validator)
    {
        _validator = validator;
    }

    public async Task<IResult> Handle(GetRouteByIdRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        throw new NotImplementedException();
    }
}