namespace GeoService.Database.Models;

public sealed class Route
{
    [Key]
    public Guid Id { get; set; }

    public RouteType Type { get; set; }

    public ICollection<PointDouble> Points { get; set; } = default!;
}