using Route = GeoService.Database.Models.Route;

namespace GeoService.Api.Repos;

public sealed class RouteRepo : IRouteRepo
{
    private readonly DataContext _context;

    public RouteRepo(DataContext context)
    {
        _context = context;
    }

    public async Task<Guid?> CreateRoute(Route route)
    {
        if (route == null)
            return null;

        route.Id = Guid.NewGuid();
        await _context.Routes.AddAsync(route);

        if (await SaveChanges())
            return route.Id;

        return null;
    }

    public async Task<IEnumerable<Guid>> CreateRoutes(IEnumerable<Route> routes)
    {
        if (routes == null || !routes.Any())
            return Array.Empty<Guid>();

        var guids = new List<Guid>();

        foreach (var route in routes)
        {
            var guid = Guid.NewGuid();
            route.Id = guid;

            guids.Add(guid);
        }

        await _context.Routes.AddRangeAsync(routes);

        if (!await SaveChanges())
            return Array.Empty<Guid>();

        return guids;
    }

    public async Task<bool> DeleteRoute(Guid routeId)
    {
        var route = await GetRouteById(routeId);
        if (route == null)
            return false;

        _context.Routes.Remove(route);

        return await SaveChanges();
    }

    public async Task<Route?> GetRouteById(Guid routeId)
    {
        return await _context.Routes
            .Include(x => x.Points)
            .FirstOrDefaultAsync(x => x.Id == routeId);
    }

    public async Task<bool> UpdateRoute(Route route)
    {
        if (!await _context.Routes.AnyAsync(x => x.Id == route.Id))
            return false;

        _context.Routes.Update(route);

        return await SaveChanges();
    }

    public async Task<bool> SaveChanges()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}