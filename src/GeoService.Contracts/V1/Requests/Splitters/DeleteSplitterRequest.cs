﻿namespace GeoService.Contracts.V1.Requests.Splitters;

public class DeleteSplitterRequest : IHttpRequest
{
    public Guid Id { get; set; }
}