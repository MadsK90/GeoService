namespace GeoService.Contracts.V1.Responses.Manholes;

public sealed class UpdateManholeResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}