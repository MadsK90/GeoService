namespace GeoService.Api.Validations.Manholes;

public sealed class DeleteManholeValidator : AbstractValidator<DeleteManholeRequest>
{
    public DeleteManholeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}