namespace GeoService.Api.Handlers.Routes;

internal sealed class UpdateRouteHandler : IRequestHandler<UpdateRouteRequest, IResult>
{
    private readonly IValidator<UpdateRouteRequest> _validator;

    public UpdateRouteHandler(IValidator<UpdateRouteRequest> validator)
    {
        _validator = validator;
    }

    public async Task<IResult> Handle(UpdateRouteRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        throw new NotImplementedException();
    }
}