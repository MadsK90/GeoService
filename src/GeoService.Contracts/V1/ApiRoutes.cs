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
}