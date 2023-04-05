namespace GeoService.Contracts.V1.Requests.Routes;

public class DeleteRouteRequest : IHttpRequest
{
    public Guid Id { get; set; }
}