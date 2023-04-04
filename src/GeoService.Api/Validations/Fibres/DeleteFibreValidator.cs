namespace GeoService.Api.Validations.Fibres;

public sealed class DeleteFibreValidator : AbstractValidator<DeleteFibreRequest>
{
    public DeleteFibreValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}