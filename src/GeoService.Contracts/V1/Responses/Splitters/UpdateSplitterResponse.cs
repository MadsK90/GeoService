namespace GeoService.Contracts.V1.Responses.Splitters;

public sealed class UpdateSplitterResponse
{
    public Guid Id { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string Name { get; set; } = null!;
}