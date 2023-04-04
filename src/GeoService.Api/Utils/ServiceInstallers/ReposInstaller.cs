namespace GeoService.Api.Utils.ServiceInstallers;

public sealed class ReposInstaller : IServiceInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICabinetRepo, CabinetRepo>();
        services.AddScoped<IFibreRepo, FibreRepo>();
        services.AddScoped<IRouteRepo, RouteRepo>();
        services.AddScoped<ISplitterRepo, SplitterRepo>();
        services.AddScoped<IManholeRepo, ManholeRepo>();
    }
}