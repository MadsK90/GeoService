namespace GeoService.Contracts.V1.Requests.Routes;

public class GetRouteByIdRequest : IHttpRequest
{
    public Guid Id { get; set; }
}