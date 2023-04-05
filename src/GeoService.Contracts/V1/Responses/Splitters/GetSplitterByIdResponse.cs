namespace GeoService.Contracts.V1.Responses.Splitters;

public sealed class GetSplitterByIdResponse
{
    public Guid Id { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string Name { get; set; } = null!;
}