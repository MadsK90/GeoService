namespace GeoService.Api.Validations.Manholes;

public sealed class GetManholeByIdValidator : AbstractValidator<GetManholeByIdRequest>
{
    public GetManholeByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}