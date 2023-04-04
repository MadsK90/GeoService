namespace GeoService.Api.Validations.Cabinets;

public sealed class GetCabinetByIdValidator : AbstractValidator<GetCabinetByIdRequest>
{
    public GetCabinetByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}