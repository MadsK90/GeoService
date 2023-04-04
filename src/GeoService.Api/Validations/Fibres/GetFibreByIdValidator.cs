namespace GeoService.Api.Validations.Fibres;

public sealed class GetFibreByIdValidator : AbstractValidator<GetFibreByIdRequest>
{
    public GetFibreByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}