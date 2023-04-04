namespace GeoService.Contracts.V1.Requests.Fibres;

public class UpdateFibreRequest : IHttpRequest
{
    public Guid Id { get; set; }

    public bool Air { get; set; }

    public int Size { get; set; }

    public IEnumerable<PointDoubleDto> Points { get; set; } = default!;
}