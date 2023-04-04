namespace GeoService.Contracts.V1.Requests.Manholes;

public class CreateManholeRequest : IHttpRequest
{
    public string Name { get; set; } = default!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}