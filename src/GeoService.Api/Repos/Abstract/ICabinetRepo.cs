namespace GeoService.Api.Repos.Abstract;

public interface ICabinetRepo
{
    Task<Guid?> CreateCabinet(Cabinet cabinet);

    Task<IEnumerable<Guid>> CreateCabinets(IEnumerable<Cabinet> cabinets);

    Task<bool> UpdateCabinet(Cabinet cabinet);

    Task<bool> DeleteCabinet(Guid cabinetId);

    Task<Cabinet?> GetCabinetById(Guid cabinetId);
}