namespace GeoService.Sdk.Tests.Integration;

public sealed partial class PolygonEndpointsTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    #region Fields

    private readonly WebApplicationFactory<IApiMarker> _factory;
    private readonly List<Guid> _createdPolygons = new();
    private readonly IGeoServiceApi _sut;

    #endregion Fields

    public PolygonEndpointsTests(WebApplicationFactory<IApiMarker> factory)
    {
        _factory = factory;
        _sut = RestService.For<IGeoServiceApi>(_factory.CreateClient());
    }

    #region Add

    [Fact]
    public async Task CreatePolygon_CreatesPolygon_WhenDataIsCorrect()
    {
        //Arrange
        var createPolygonRequest = GenerateCreatePolygonRequest();

        //Act
        var response = await _sut.CreatePolygonAsync(createPolygonRequest) ?? throw new Exception("");
        if (response.Content != null)
            _createdPolygons.Add(response.Content.Id);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
            .NotBeNull();

        response.Content!.Id.Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task CreatePolygon_Return400_And_Validationerrors_WhenDataIsIncorrect()
    {
        //Arrange
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
        var response = await _sut.CreatePolygonAsync(createPolygonRequest) ?? throw new Exception("");
        var error = await response.Error!.GetContentAsAsync<List<ValidationFailure>>();

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        error.Should()
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
        var response = await _sut.GetPolygonByIdAsync(new GetPolygonByIdRequest { Id = createResponse.Id });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(createPolygonRequest);
    }

    [Fact]
    public async Task GetPolygonById_Return404_WhenNotFound()
    {
        //Act
        var response = await _sut.GetPolygonByIdAsync(new GetPolygonByIdRequest { Id = Guid.NewGuid() });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPolygonById_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Act
        var response = await _sut.GetPolygonByIdAsync(new GetPolygonByIdRequest { Id = Guid.Empty });
        var error = await response.Error!.GetContentAsAsync<List<ValidationFailure>>();

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        error.Should()
            .NotBeNullOrEmpty()
            .And
            .HaveCount(1);
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
        var response = await _sut.UpdatePolygonAsync(updatePolygonRequest);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
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
        var response = await _sut.UpdatePolygonAsync(updatePolygonRequest);
        var error = await response.Error!.GetContentAsAsync<List<ValidationFailure>>();

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        error.Should()
            .NotBeNullOrEmpty()
            .And
            .HaveCount(5);
    }

    [Fact]
    public async Task UpdatePolygon_Return404_WhenNotFound()
    {
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
        var response = await _sut.UpdatePolygonAsync(updatePolygonRequest);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    #endregion Update

    #region Delete

    [Fact]
    public async Task DeletePolygon_Return200_WhenExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createPolygonRequest = GenerateCreatePolygonRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Polygons.CreatePolygon, createPolygonRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreatePolygonResponse>() ?? throw new Exception("");
        _createdPolygons.Add(createResponse.Id);

        //Act
        var response = await _sut.DeletePolygonAsync(new DeletePolygonRequest { Id = createResponse.Id });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeletePolygon_Return404_WhenNotFound()
    {
        //Act
        var response = await _sut.DeletePolygonAsync(new DeletePolygonRequest { Id = Guid.NewGuid() });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeletePolygon_Return400_WhenIdEmpty()
    {
        //Act
        var response = await _sut.DeletePolygonAsync(new DeletePolygonRequest { Id = Guid.Empty });
        var error = await response.Error!.GetContentAsAsync<List<ValidationFailure>>();

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        error.Should()
            .NotBeNullOrEmpty()
            .And
            .HaveCount(1);
    }

    #endregion Delete

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