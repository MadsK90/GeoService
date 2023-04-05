namespace GeoService.Api.Handlers.Routes;

internal sealed class CreateRouteHandler : IRequestHandler<CreateRouteRequest, IResult>
{
    private readonly IValidator<CreateRouteRequest> _validator;

    public CreateRouteHandler(IValidator<CreateRouteRequest> validator)
    {
        _validator = validator;
    }

    public async Task<IResult> Handle(CreateRouteRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        throw new NotImplementedException();
    }
}