namespace GeoService.Api.Tests.Integration;

public sealed partial class PolygonEndpointsTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    #region Fields

    private readonly WebApplicationFactory<IApiMarker> _factory;
    private readonly List<Guid> _createdPolygons = new();

    #endregion Fields

    public PolygonEndpointsTests(WebApplicationFactory<IApiMarker> factory)
    {
        _factory = factory;
    }

    #region Add

    [Fact]
    public async Task CreatePolygon_CreatesPolygon_WhenDataIsCorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createPolygonRequest = GenerateCreatePolygonRequest();

        //Act
        var result = await httpClient.PostAsJsonAsync(ApiRoutes.Polygons.CreatePolygon, createPolygonRequest);
        var response = await result.Content.ReadFromJsonAsync<CreatePolygonResponse>() ?? throw new Exception("");

        _createdPolygons.Add(response.Id);

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Should()
            .NotBeNull();

        response!.Id.Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task CreatePolygon_Return400_AndValidationerrors_WhenDataIsIncorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createPolygonRequest = new CreatePolygonRequest
        {
            Points = new PointDoubleDto[]
            {
                new PointDoubleDto
                {
                    Longitude = 200,
                    Latitude = 200,
                },
                new PointDoubleDto
                {
                    Longitude = 200,
                    Latitude = 200
                }
            }
        };

        //Act
        var result = await httpClient.PostAsJsonAsync(ApiRoutes.Polygons.CreatePolygon, createPolygonRequest);
        var response = await result.Content.ReadFromJsonAsync<List<ValidationFailure>>() ?? throw new Exception("");

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        response.Should()
            .NotBeNullOrEmpty()
            .And
            .HaveCount(5);
    }

    #endregion Add

    #region Helpers

    private static string GetPolygonByIdRoute(Guid id)
    {
        return ReplaceIdRegex().Replace(ApiRoutes.Polygons.GetPolygonById, id.ToString());
    }

    private static string DeletePolygonRoute(Guid id)
    {
        return ReplaceIdRegex().Replace(ApiRoutes.Polygons.DeletePolygon, id.ToString());
    }

    [GeneratedRegex("{id}")]
    private static partial Regex ReplaceIdRegex();

    #endregion Helpers

    #region Setup & Teardown

    private static CreatePolygonRequest GenerateCreatePolygonRequest()
    {
        return new CreatePolygonRequest
        {
            Points = new PointDoubleDto[]
            {
                new PointDoubleDto
                {
                    Latitude = 1,
                    Longitude = 2
                },
                new PointDoubleDto
                {
                    Latitude = 3,
                    Longitude = 4
                },
                new PointDoubleDto
                {
                    Latitude = 5,
                    Longitude = 6
                }
            }
        };
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        if (!_createdPolygons.Any())
            return;

        var httpClient = _factory.CreateClient();

        foreach (var polygonId in _createdPolygons)
        {
            await httpClient.DeleteAsync(DeletePolygonRoute(polygonId));
        }
    }

    #endregion Setup & Teardown
}