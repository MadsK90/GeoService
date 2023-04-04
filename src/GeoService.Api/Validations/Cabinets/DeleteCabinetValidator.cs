namespace GeoService.Api.Validations.Cabinets;

public sealed class DeleteCabinetValidator : AbstractValidator<DeleteCabinetRequest>
{
    public DeleteCabinetValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}