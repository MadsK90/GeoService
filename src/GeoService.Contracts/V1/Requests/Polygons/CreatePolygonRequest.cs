namespace GeoService.Contracts.V1.Requests.Polygons;

public class CreatePolygonRequest : IHttpRequest
{
    public IEnumerable<PointDoubleDto> Points { get; set; } = default!;
}