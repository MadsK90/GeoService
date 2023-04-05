namespace GeoService.Api.Repos;

public sealed class PolygonRepo : IPolygonRepo
{
    private readonly DataContext _context;

    public PolygonRepo(DataContext context)
    {
        _context = context;
    }

    public async Task<Guid?> CreatePolygon(Polygon polygon)
    {
        if (polygon == null)
            return null;

        polygon.Id = Guid.NewGuid();
        await _context.Polygons.AddAsync(polygon);

        if (!await SaveChanges())
            return null;

        return polygon.Id;
    }

    public async Task<IEnumerable<Guid>> CreatePolygons(IEnumerable<Polygon> polygons)
    {
        if (polygons == null || !polygons.Any())
            return Array.Empty<Guid>();

        var guids = new List<Guid>();

        foreach (var polygon in polygons)
        {
            var guid = Guid.NewGuid();
            polygon.Id = guid;

            guids.Add(guid);
        }

        await _context.Polygons.AddRangeAsync(polygons);

        if (!await SaveChanges())
            return Array.Empty<Guid>();

        return guids;
    }

    public async Task<bool> DeletePolygon(Guid polygonId)
    {
        var polygon = await GetPolygonById(polygonId);
        if (polygon == null)
            return false;

        _context.Polygons.Remove(polygon);

        return await SaveChanges();
    }

    public async Task<Polygon?> GetPolygonById(Guid polygonId)
    {
        return await _context.Polygons.FirstOrDefaultAsync(x => x.Id == polygonId);
    }

    public async Task<bool> UpdatePolygon(Polygon polygon)
    {
        if (!await _context.Polygons.AnyAsync(x => x.Id == polygon.Id))
            return false;

        _context.Polygons.Update(polygon);

        return await SaveChanges();
    }

    public async Task<bool> SaveChanges()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}