namespace GeoService.Database.Models;

public sealed class Fibre
{
    [Key]
    public Guid Id { get; set; }

    public bool Air { get; set; }

    public int Size { get; set; }

    public ICollection<PointDouble> Points { get; set; } = default!;
}