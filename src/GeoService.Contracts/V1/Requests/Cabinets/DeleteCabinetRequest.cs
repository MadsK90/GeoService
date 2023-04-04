namespace GeoService.Contracts.V1.Requests.Cabinets;

public class DeleteCabinetRequest : IHttpRequest
{
    public Guid Id { get; set; }
}