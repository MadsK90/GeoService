namespace GeoService.Api.Repos.Abstract;

public interface IPolygonRepo
{
    Task<Guid?> CreatePolygon(Polygon polygon);

    Task<IEnumerable<Guid>> CreatePolygons(IEnumerable<Polygon> polygons);

    Task<bool> UpdatePolygon(Polygon polygon);

    Task<bool> DeletePolygon(Guid polygonId);

    Task<Manhole?> GetPolygonById(Guid polygonId);
}