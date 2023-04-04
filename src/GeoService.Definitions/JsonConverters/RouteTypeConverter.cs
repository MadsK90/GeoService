namespace GeoService.Definitions.JsonConverters;

public sealed class RouteTypeConverter : JsonConverter<RouteType>
{
    public override RouteType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();

        if (!RouteTypeExtensions.TryParse(str, out var value, true))
            throw new Exception($"[RouteTypeConverter.Read] Failed to parse as type: {str}");

        return value;

        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, RouteType value, JsonSerializerOptions options)
    {
        if (!RouteTypeExtensions.IsDefined(value))
            throw new Exception($"[RouteTypeConverter.Write] Type isn't defined: {value}");

        writer.WriteStringValue(value.ToStringFast());
    }
}