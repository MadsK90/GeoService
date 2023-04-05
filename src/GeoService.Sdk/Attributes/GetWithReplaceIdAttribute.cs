namespace GeoService.Sdk.Attributes;

internal sealed class GetWithReplaceIdAttribute : HttpMethodWithReplaceIdAttribute
{
    public GetWithReplaceIdAttribute(string path, string replaceWith = "{request.id}") : base(path, replaceWith)
    {
    }

    public override HttpMethod Method => HttpMethod.Get;
}