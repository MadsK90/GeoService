namespace GeoService.Contracts.V1.Requests.Polygons;

public class GetPolygonByIdRequest : IHttpRequest
{
    public Guid Id { get; set; }
}