namespace GeoService.Contracts.V1.Requests.Polygons;

public class DeletePolygonRequest : IHttpRequest
{
    public Guid Id { get; set; }
}