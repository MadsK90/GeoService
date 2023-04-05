namespace GeoService.Contracts.V1.Requests.Polygons;

public class UpdatePolygonRequest : IHttpRequest
{
    public Guid Id { get; set; }

    public IEnumerable<PointDoubleDto> Points { get; set; } = default!;
}