﻿namespace GeoService.Contracts.V1.Requests.Manholes;

public class DeleteManholeRequest : IHttpRequest
{
    public Guid Id { get; set; }
}