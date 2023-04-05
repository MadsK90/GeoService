namespace GeoService.Sdk.Attributes;

internal sealed class DeleteWithReplaceIdAttribute : HttpMethodWithReplaceIdAttribute
{
    public DeleteWithReplaceIdAttribute(string path, string replaceWith = "{request.id}") : base(path, replaceWith)
    {
    }

    public override HttpMethod Method => HttpMethod.Delete;
}