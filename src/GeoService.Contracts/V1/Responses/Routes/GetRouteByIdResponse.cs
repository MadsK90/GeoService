namespace GeoService.Contracts.V1.Responses.Routes;

public sealed class GetRouteByIdResponse
{
    public Guid Id { get; set; }

    public RouteType Type { get; set; }

    public IEnumerable<PointDoubleDto> Points { get; set; } = default!;
}