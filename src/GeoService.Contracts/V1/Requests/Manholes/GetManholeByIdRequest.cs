namespace GeoService.Contracts.V1.Requests.Manholes;

public class GetManholeByIdRequest : IHttpRequest
{
    public Guid Id { get; set; }
}