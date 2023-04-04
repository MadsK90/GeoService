namespace GeoService.Contracts.V1.Responses.Cabinets;

public sealed class UpdateCabinetResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string? Address { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}