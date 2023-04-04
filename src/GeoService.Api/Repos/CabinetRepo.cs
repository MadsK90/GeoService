namespace GeoService.Api.Repos;

public sealed class CabinetRepo : ICabinetRepo
{
    private readonly DataContext _context;

    public CabinetRepo(DataContext context)
    {
        _context = context;
    }

    public async Task<Guid?> CreateCabinet(Cabinet cabinet)
    {
        if (cabinet == null)
            return null;

        cabinet.Id = Guid.NewGuid();
        await _context.Cabinets.AddAsync(cabinet);

        if (!await SaveChanges())
            return null;

        return cabinet.Id;
    }

    public async Task<IEnumerable<Guid>> CreateCabinets(IEnumerable<Cabinet> cabinets)
    {
        if (cabinets == null || !cabinets.Any())
            return Array.Empty<Guid>();

        var guids = new List<Guid>();

        foreach (var cabinet in cabinets)
        {
            var guid = Guid.NewGuid();
            cabinet.Id = guid;

            guids.Add(guid);
        }

        await _context.Cabinets.AddRangeAsync(cabinets);

        if (!await SaveChanges())
            return Array.Empty<Guid>();

        return guids;
    }

    public async Task<bool> DeleteCabinet(Guid cabinetId)
    {
        var cabinet = await GetCabinetById(cabinetId);
        if (cabinet == null)
            return false;

        _context.Cabinets.Remove(cabinet);

        return await SaveChanges();
    }

    public async Task<Cabinet?> GetCabinetById(Guid cabinetId)
    {
        return await _context.Cabinets.FirstOrDefaultAsync(x => x.Id == cabinetId);
    }

    public async Task<bool> UpdateCabinet(Cabinet cabinet)
    {
        if (!await _context.Cabinets.AnyAsync(x => x.Id == cabinet.Id))
            return false;

        _context.Cabinets.Update(cabinet);

        return await SaveChanges();
    }

    public async Task<bool> SaveChanges()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}