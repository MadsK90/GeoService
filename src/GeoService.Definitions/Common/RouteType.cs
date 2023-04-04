namespace GeoService.Definitions.Common;

[JsonConverter(typeof(RouteTypeConverter))]
[EnumExtensions]
public enum RouteType
{
    GravingJord,
    GravingAsfalt,
    Subkanalisering,
    Luft,
    LuftDrop,
    Kundegraving,
    Veikryssing,
    Borring,
    Fellesgrøft,
    Klamring,
    Brostein,
    Hybridkabel
}