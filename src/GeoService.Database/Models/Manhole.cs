namespace GeoService.Database.Models;

public sealed class Manhole
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}