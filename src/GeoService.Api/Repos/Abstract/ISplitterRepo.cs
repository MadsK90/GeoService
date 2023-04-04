namespace GeoService.Api.Repos.Abstract;

public interface ISplitterRepo
{
    Task<Guid?> CreateSplitter(Splitter splitter);

    Task<IEnumerable<Guid>> CreateSplitters(IEnumerable<Splitter> splitters);

    Task<bool> UpdateSplitter(Splitter splitter);

    Task<bool> DeleteSplitter(Guid splitterId);

    Task<Splitter?> GetSplitterById(Guid splitterId);
}