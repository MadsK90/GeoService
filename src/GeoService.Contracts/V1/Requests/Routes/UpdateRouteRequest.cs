namespace GeoService.Contracts.V1.Requests.Routes;

public class UpdateRouteRequest : IHttpRequest
{
    public Guid Id { get; set; }

    public RouteType Type { get; set; }

    public IEnumerable<PointDoubleDto> Points { get; set; } = default!;
}