namespace GeoService.Contracts.V1.Requests.Manholes;

public class UpdateManholeRequest : IHttpRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}