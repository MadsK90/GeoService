namespace GeoService.Database.Models;

public sealed class PointDouble
{
    [Key]
    public int Id { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}