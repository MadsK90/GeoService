namespace GeoService.Sdk.Attributes;

internal abstract partial class HttpMethodWithReplaceIdAttribute : HttpMethodAttribute
{
    public HttpMethodWithReplaceIdAttribute(string path, string replaceWith = "{request.id}") : base(ReplaceRegex().Replace(path, replaceWith))
    {
    }

    [GeneratedRegex("{id}")]
    private static partial Regex ReplaceRegex();
}