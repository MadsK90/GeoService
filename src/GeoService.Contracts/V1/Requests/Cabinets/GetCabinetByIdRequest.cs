namespace GeoService.Contracts.V1.Requests.Cabinets;

public class GetCabinetByIdRequest : IHttpRequest
{
    public Guid Id { get; set; }
}