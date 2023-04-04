namespace GeoService.Api.Repos.Abstract;

public interface IFibreRepo
{
    Task<Guid?> CreateFibre(Fibre fibre);

    Task<IEnumerable<Guid>> CreateFibres(IEnumerable<Fibre> fibres);

    Task<bool> UpdateFibre(Fibre fibre);

    Task<bool> DeleteFibre(Guid fibreId);

    Task<IEnumerable<Fibre>> GetAllFibres();

    Task<Fibre?> GetFibreById(Guid fibreId);
}