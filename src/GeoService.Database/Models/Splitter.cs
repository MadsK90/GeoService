namespace GeoService.Database.Models;

public sealed class Splitter
{
    [Key]
    public Guid Id { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string Name { get; set; } = null!;
}