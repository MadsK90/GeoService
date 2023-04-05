namespace GeoService.Contracts.V1.Requests.Splitters;

public class GetSplitterByIdRequest : IHttpRequest
{
    public Guid Id { get; set; }
}