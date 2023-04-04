namespace GeoService.Contracts.V1.Requests.Cabinets;

public class CreateCabinetRequest : IHttpRequest
{
    public string Name { get; set; } = default!;

    public string? Address { get; set; } = default!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}