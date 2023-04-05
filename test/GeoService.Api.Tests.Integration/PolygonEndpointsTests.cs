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

    #region Get

    [Fact]
    public async Task GetPolygonById_ReturnPolygon_WhenExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createPolygonRequest = GenerateCreatePolygonRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Polygons.CreatePolygon, createPolygonRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreatePolygonResponse>() ?? throw new Exception("");

        _createdPolygons.Add(createResponse.Id);

        //Act
        var result = await httpClient.GetAsync(GetPolygonByIdRoute(createResponse.Id));
        var response = await result.Content.ReadFromJsonAsync<GetPolygonByIdResponse>();

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Should()
            .NotBeNull()
            .And.BeEquivalentTo(createPolygonRequest);
    }

    [Fact]
    public async Task GetPolygonById_Return404_WhenNotFound()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.GetAsync(GetPolygonByIdRoute(Guid.NewGuid()));

        //Assert

        result.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPolygonById_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.GetAsync(GetPolygonByIdRoute(Guid.Empty));
        var response = await result.Content.ReadFromJsonAsync<List<ValidationFailure>>();

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        response.Should()
            .NotBeNullOrEmpty()
            .And.HaveCount(1);
    }

    #endregion Get

    #region Update

    [Fact]
    public async Task UpdatePolygon_UpdatesPolygon_WhenDataIsCorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createPolygonRequest = GenerateCreatePolygonRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Polygons.CreatePolygon, createPolygonRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreatePolygonResponse>() ?? throw new Exception("");
        _createdPolygons.Add(createResponse.Id);

        //Act
        var updatePolygonRequest = new UpdatePolygonRequest
        {
            Id = createResponse.Id,
            Points = new PointDoubleDto[]
            {
                new PointDoubleDto
                {
                    Latitude = 5,
                    Longitude = 6
                },
                new PointDoubleDto
                {
                    Latitude = 7,
                    Longitude = 8
                },
                new PointDoubleDto
                {
                    Latitude = 9,
                    Longitude = 10
                },
                new PointDoubleDto
                {
                    Latitude = 11,
                    Longitude = 12
                }
            }
        };

        var updateResult = await httpClient.PutAsJsonAsync(ApiRoutes.Polygons.UpdatePolygon, updatePolygonRequest);
        var updateResponse = await updateResult.Content.ReadFromJsonAsync<UpdatePolygonResponse>();

        //Assert
        updateResult.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        updateResponse.Should()
            .BeEquivalentTo(updatePolygonRequest);
    }

    [Fact]
    public async Task UpdatePolygon_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createPolygonRequest = GenerateCreatePolygonRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Polygons.CreatePolygon, createPolygonRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreatePolygonResponse>() ?? throw new Exception("");
        _createdPolygons.Add(createResponse.Id);

        //Act
        var updatePolygonRequest = new UpdatePolygonRequest
        {
            Id = createResponse.Id,
            Points = new PointDoubleDto[]
            {
                new PointDoubleDto
                {
                    Latitude = 200,
                    Longitude = 200
                },
                new PointDoubleDto
                {
                    Latitude = 200,
                    Longitude = 200
                },
            }
        };

        var updateResult = await httpClient.PutAsJsonAsync(ApiRoutes.Polygons.UpdatePolygon, updatePolygonRequest);
        var updateResponse = await updateResult.Content.ReadFromJsonAsync<List<ValidationFailure>>();

        //Assert
        updateResult.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        updateResponse.Should()
            .NotBeNullOrEmpty()
            .And
            .HaveCount(5);
    }

    [Fact]
    public async Task UpdatePolygon_Return404_WhenNotFound()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createPolygonRequest = GenerateCreatePolygonRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Polygons.CreatePolygon, createPolygonRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreatePolygonResponse>() ?? throw new Exception("");
        _createdPolygons.Add(createResponse.Id);

        //Act
        var updatePolygonRequest = new UpdatePolygonRequest
        {
            Id = Guid.NewGuid(),
            Points = new PointDoubleDto[]
            {
                new PointDoubleDto
                {
                    Latitude = 5,
                    Longitude = 6
                },
                new PointDoubleDto
                {
                    Latitude = 7,
                    Longitude = 8
                },
                new PointDoubleDto
                {
                    Latitude = 9,
                    Longitude = 10
                },
                new PointDoubleDto
                {
                    Latitude = 13,
                    Longitude = 12
                }
            }
        };

        var updateResult = await httpClient.PutAsJsonAsync(ApiRoutes.Polygons.UpdatePolygon, updatePolygonRequest);

        //Assert
        updateResult.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    #endregion Update

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