namespace GeoService.Database.Models;

public sealed class Polygon
{
    [Key]
    public Guid Id { get; set; }

    public ICollection<PointDouble> Points { get; set; } = default!;
}