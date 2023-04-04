namespace GeoService.Api.Repos;

public sealed class SplitterRepo : ISplitterRepo
{
    private readonly DataContext _context;

    public SplitterRepo(DataContext context)
    {
        _context = context;
    }

    public async Task<Guid?> CreateSplitter(Splitter splitter)
    {
        if (splitter == null)
            return null;

        splitter.Id = Guid.NewGuid();
        await _context.Splitters.AddAsync(splitter);

        if (await SaveChanges())
            return splitter.Id;

        return null;
    }

    public async Task<IEnumerable<Guid>> CreateSplitters(IEnumerable<Splitter> splitters)
    {
        if (splitters == null || !splitters.Any())
            return Array.Empty<Guid>();

        var guids = new List<Guid>();

        foreach (var splitter in splitters)
        {
            var guid = Guid.NewGuid();
            splitter.Id = guid;

            guids.Add(guid);
        }

        await _context.Splitters.AddRangeAsync(splitters);

        if (!await SaveChanges())
            return Array.Empty<Guid>();

        return guids;
    }

    public async Task<bool> DeleteSplitter(Guid splitterId)
    {
        var splitter = await GetSplitterById(splitterId);
        if (splitter == null)
            return false;

        _context.Splitters.Remove(splitter);

        return await SaveChanges();
    }

    public async Task<IEnumerable<Splitter>> GetAllSplitters()
    {
        return await _context.Splitters.ToArrayAsync();
    }

    public async Task<Splitter?> GetSplitterById(Guid splitterId)
    {
        return await _context.Splitters.FirstOrDefaultAsync(x => x.Id == splitterId);
    }

    public async Task<bool> UpdateSplitter(Splitter splitter)
    {
        _context.Splitters.Update(splitter);

        return await SaveChanges();
    }

    public async Task<bool> SaveChanges()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}