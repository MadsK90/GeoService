namespace GeoService.Contracts.V1.Responses.Cabinets;

public sealed class GetAllCabinetsResponse
{
    public IEnumerable<CabinetDto> Cabinets { get; set; } = default!;
}