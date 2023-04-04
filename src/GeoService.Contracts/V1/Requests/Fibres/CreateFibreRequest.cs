namespace GeoService.Contracts.V1.Requests.Fibres;

public class CreateFibreRequest : IHttpRequest
{
    public bool Air { get; set; }

    public int Size { get; set; }

    public IEnumerable<PointDoubleDto> Points { get; set; } = default!;
}