namespace GeoService.Api.Handlers.Routes;

internal sealed class DeleteRouteHandler : IRequestHandler<DeleteRouteRequest, IResult>
{
    private readonly IValidator<DeleteRouteRequest> _validator;

    public DeleteRouteHandler(IValidator<DeleteRouteRequest> validator)
    {
        _validator = validator;
    }

    public async Task<IResult> Handle(DeleteRouteRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        throw new NotImplementedException();
    }
}