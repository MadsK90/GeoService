namespace GeoService.Api.Validations.Splitters;

public sealed class DeleteSplitterValidator : AbstractValidator<DeleteSplitterRequest>
{
    public DeleteSplitterValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}