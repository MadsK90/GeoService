using Route = GeoService.Database.Models.Route;

namespace GeoService.Api.Repos.Abstract;

public interface IRouteRepo
{
    Task<Guid?> CreateRoute(Route route);

    Task<IEnumerable<Guid>> CreateRoutes(IEnumerable<Route> routes);

    Task<bool> UpdateRoute(Route route);

    Task<bool> DeleteRoute(Guid routeId);

    Task<Route?> GetRouteById(Guid routeId);
}