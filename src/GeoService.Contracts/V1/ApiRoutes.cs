namespace GeoService.Contracts.V1;

public static class ApiRoutes
{
    public const string Root = "api";
    public const string Version = "v1";
    public const string Base = $"/{Root}/{Version}";

    public static class Cabinets
    {
        public const string GetCabinetById = $"{Base}/cabinets/{{id}}";
        public const string CreateCabinet = $"{Base}/cabinets";
        public const string DeleteCabinet = $"{Base}/cabinets/{{id}}";
        public const string UpdateCabinet = $"{Base}/cabinets";
    }

    public static class Fibres
    {
        public const string GetFibreById = $"{Base}/fibres/{{id}}";
        public const string CreateFibre = $"{Base}/fibres";
        public const string DeleteFibre = $"{Base}/fibres/{{id}}";
        public const string UpdateFibre = $"{Base}/fibres";
    }

    public static class Manholes
    {
        public const string GetManholeById = $"{Base}/manholes/{{id}}";
        public const string CreateManhole = $"{Base}/manholes";
        public const string DeleteManhole = $"{Base}/manholes/{{id}}";
        public const string UpdateManhole = $"{Base}/manholes";
    }

    public static class Polygons
    {
        public const string GetPolygonById = $"{Base}/polygons/{{id}}";
        public const string CreatePolygon = $"{Base}/polygons";
        public const string DeletePolygon = $"{Base}/polygons/{{id}}";
        public const string UpdatePolygon = $"{Base}/polygons";
    }

    public static class Routes
    {
        public const string GetRouteById = $"{Base}/routes/{{id}}";
        public const string CreateRoute = $"{Base}/routes";
        public const string DeleteRoute = $"{Base}/routes/{{id}}";
        public const string UpdateRoute = $"{Base}/routes";
    }
}