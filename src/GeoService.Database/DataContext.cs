namespace GeoService.Database;

public sealed class DataContext : DbContext
{
    public DbSet<Cabinet> Cabinets { get; set; } = default!;
    public DbSet<Fibre> Fibres { get; set; } = default!;
    public DbSet<Manhole> Manholes { get; set; } = default!;
    public DbSet<Route> Routes { get; set; } = default!;
    public DbSet<Splitter> Splitters { get; set; } = default!;
    public DbSet<Polygon> Polygons { get; set; } = default!;

    public string DbPath { get; }

    public DataContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);

        DbPath = Path.Join(path, "geoservice.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}