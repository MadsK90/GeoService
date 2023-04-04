namespace GeoService.Api.Repos;

public sealed class FibreRepo : IFibreRepo
{
    private readonly DataContext _context;

    public FibreRepo(DataContext context)
    {
        _context = context;
    }

    public async Task<Guid?> CreateFibre(Fibre fibre)
    {
        if (fibre == null)
            return null;

        fibre.Id = Guid.NewGuid();
        await _context.Fibres.AddAsync(fibre);

        if (await SaveChanges())
            return fibre.Id;

        return null;
    }

    public async Task<IEnumerable<Guid>> CreateFibres(IEnumerable<Fibre> fibres)
    {
        if (fibres == null || !fibres.Any())
            return Array.Empty<Guid>();

        var guids = new List<Guid>();

        foreach (var fibre in fibres)
        {
            var guid = Guid.NewGuid();
            fibre.Id = guid;

            guids.Add(guid);
        }

        await _context.Fibres.AddRangeAsync(fibres);

        if (!await SaveChanges())
            return Array.Empty<Guid>();

        return guids;
    }

    public async Task<bool> DeleteFibre(Guid fibreId)
    {
        var fibre = await GetFibreById(fibreId);
        if (fibre == null)
            return false;

        _context.Fibres.Remove(fibre);

        return await SaveChanges();
    }

    public async Task<Fibre?> GetFibreById(Guid fibreId)
    {
        return await _context.Fibres
            .Include(x => x.Points)
            .FirstOrDefaultAsync(x => x.Id == fibreId);
    }

    public async Task<bool> UpdateFibre(Fibre fibre)
    {
        if (!await _context.Fibres.AnyAsync(x => x.Id == fibre.Id))
            return false;

        _context.Fibres.Update(fibre);

        return await SaveChanges();
    }

    public async Task<bool> SaveChanges()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}