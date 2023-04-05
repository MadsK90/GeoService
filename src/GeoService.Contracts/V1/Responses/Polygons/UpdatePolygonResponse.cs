namespace GeoService.Contracts.V1.Responses.Polygons;

public sealed class UpdatePolygonResponse
{
    public Guid Id { get; set; }

    public IEnumerable<PointDoubleDto> Points { get; set; } = default!;
}