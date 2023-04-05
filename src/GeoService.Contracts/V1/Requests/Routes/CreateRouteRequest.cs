namespace GeoService.Contracts.V1.Requests.Routes;

public class CreateRouteRequest : IHttpRequest
{
    public RouteType Type { get; set; }

    public IEnumerable<PointDoubleDto> Points { get; set; } = default!;
}