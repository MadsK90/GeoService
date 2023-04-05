namespace GeoService.Api.Validations.Splitters;

public sealed class GetSplitterByIdValidator : AbstractValidator<GetSplitterByIdRequest>
{
    public GetSplitterByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}