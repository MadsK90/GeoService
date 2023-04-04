namespace GeoService.Api.Repos.Abstract;

public interface IManholeRepo
{
    Task<Guid?> CreateManhole(Manhole manhole);

    Task<IEnumerable<Guid>> CreateManholes(IEnumerable<Manhole> manholes);

    Task<bool> UpdateManhole(Manhole manhole);

    Task<bool> DeleteManhole(Guid manholeId);

    Task<IEnumerable<Manhole>> GetAllManholes();

    Task<Manhole?> GetManholeById(Guid manholeId);
}