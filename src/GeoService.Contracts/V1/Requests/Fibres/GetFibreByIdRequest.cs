namespace GeoService.Contracts.V1.Requests.Fibres;

public class GetFibreByIdRequest : IHttpRequest
{
    public Guid Id { get; set; }
}