namespace GeoService.Contracts.V1.Requests.Splitters;

public class UpdateSplitterRequest : IHttpRequest
{
    public Guid Id { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string Name { get; set; } = null!;
}