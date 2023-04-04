namespace GeoService.Contracts.V1.Requests.Fibres;

public class DeleteFibreRequest : IHttpRequest
{
    public Guid Id { get; set; }
}