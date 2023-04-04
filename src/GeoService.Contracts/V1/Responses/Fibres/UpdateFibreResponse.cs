namespace GeoService.Contracts.V1.Responses.Fibres;

public sealed class UpdateFibreResponse
{
    public bool Air { get; set; }

    public int Size { get; set; }

    public List<PointDoubleDto> Points { get; set; } = default!;
}