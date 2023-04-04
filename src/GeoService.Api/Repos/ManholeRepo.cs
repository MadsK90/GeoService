namespace GeoService.Api.Repos;

public sealed class ManholeRepo : IManholeRepo
{
    private readonly DataContext _context;

    public ManholeRepo(DataContext context)
    {
        _context = context;
    }

    public async Task<Guid?> CreateManhole(Manhole manhole)
    {
        if (manhole == null)
            return null;

        manhole.Id = Guid.NewGuid();
        await _context.Manholes.AddAsync(manhole);

        if (await SaveChanges())
            return manhole.Id;

        return null;
    }

    public async Task<IEnumerable<Guid>> CreateManholes(IEnumerable<Manhole> manholes)
    {
        if (manholes == null || !manholes.Any())
            return Array.Empty<Guid>();

        var guids = new List<Guid>();

        foreach (var cabinet in manholes)
        {
            var guid = Guid.NewGuid();
            cabinet.Id = guid;

            guids.Add(guid);
        }

        await _context.Manholes.AddRangeAsync(manholes);

        if (!await SaveChanges())
            return Array.Empty<Guid>();

        return guids;
    }

    public async Task<bool> DeleteManhole(Guid manholeId)
    {
        var manhole = await GetManholeById(manholeId);
        if (manhole == null)
            return false;

        _context.Manholes.Remove(manhole);

        return await SaveChanges();
    }

    public async Task<Manhole?> GetManholeById(Guid manholeId)
    {
        return await _context.Manholes.FirstOrDefaultAsync(x => x.Id == manholeId);
    }

    public async Task<bool> UpdateManhole(Manhole manhole)
    {
        if (!await _context.Manholes.AnyAsync(x => x.Id == manhole.Id))
            return false;

        _context.Manholes.Update(manhole);

        return await SaveChanges();
    }

    public async Task<bool> SaveChanges()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}