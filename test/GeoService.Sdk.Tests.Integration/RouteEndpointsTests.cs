namespace GeoService.Sdk.Tests.Integration;

public sealed partial class RouteEndpointsTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    #region Fields

    private readonly WebApplicationFactory<IApiMarker> _factory;
    private readonly List<Guid> _createdRoutes = new();
    private readonly IGeoServiceApi _sut;

    #endregion Fields

    public RouteEndpointsTests(WebApplicationFactory<IApiMarker> factory)
    {
        _factory = factory;
        _sut = RestService.For<IGeoServiceApi>(_factory.CreateClient());
    }

    #region Add

    [Fact]
    public async Task CreateRoute_CreatesRoute_WhenDataIsCorrect()
    {
        //Arrange
        var createRouteRequest = GenerateCreateRouteRequest();

        //Act
        var response = await _sut.CreateRouteAsync(createRouteRequest) ?? throw new Exception("");
        if (response.Content != null)
            _createdRoutes.Add(response.Content.Id);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
            .NotBeNull();

        response.Content!.Id.Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task CreateRoute_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var createRouteRequest = new CreateRouteRequest
        {
            Points = new PointDoubleDto[]
            {
                new PointDoubleDto
                {
                    Latitude = 200,
                    Longitude = 200
                }
            }
        };

        //Act
        var response = await _sut.CreateRouteAsync(createRouteRequest) ?? throw new Exception("");
        var error = await response.Error!.GetContentAsAsync<List<ValidationFailure>>();

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        error.Should()
            .NotBeNullOrEmpty()
            .And
            .HaveCount(3);
    }

    #endregion Add

    #region Get

    [Fact]
    public async Task GetRouteById_ReturnRoute_WhenExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createRouteRequest = GenerateCreateRouteRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Routes.CreateRoute, createRouteRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateRouteResponse>() ?? throw new Exception("");
        _createdRoutes.Add(createResponse.Id);

        //Act
        var response = await _sut.GetRouteByIdAsync(new GetRouteByIdRequest { Id = createResponse.Id });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(createRouteRequest);
    }

    [Fact]
    public async Task GetRouteById_Return404_WhenNotFound()
    {
        //Act
        var response = await _sut.GetRouteByIdAsync(new GetRouteByIdRequest { Id = Guid.NewGuid() });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetRouteById_Return400_And_Validation_Errors_WhenDataIsIncorrect()
    {
        //Act
        var response = await _sut.GetRouteByIdAsync(new GetRouteByIdRequest { Id = Guid.Empty });
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
    public async Task UpdateRoute_UpdatesRoute_WhenDataIsCorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createRouteRequest = GenerateCreateRouteRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Routes.CreateRoute, createRouteRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateRouteResponse>() ?? throw new Exception("");
        _createdRoutes.Add(createResponse.Id);

        //Act
        var updateRuteRequest = new UpdateRouteRequest
        {
            Id = createResponse.Id,
            Type = RouteType.Klamring,
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
        var response = await _sut.UpdateRouteAsync(updateRuteRequest);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
            .BeEquivalentTo(updateRuteRequest);
    }

    [Fact]
    public async Task UpdateRoute_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createRouteRequest = GenerateCreateRouteRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Routes.CreateRoute, createRouteRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateRouteResponse>() ?? throw new Exception("");
        _createdRoutes.Add(createResponse.Id);

        //Act
        var updateRuteRequest = new UpdateRouteRequest
        {
            Id = createResponse.Id,
            Points = new PointDoubleDto[]
            {
                new PointDoubleDto
                {
                    Latitude = 200,
                    Longitude = 200
                },
            }
        };
        var response = await _sut.UpdateRouteAsync(updateRuteRequest);
        var error = await response.Error!.GetContentAsAsync<List<ValidationFailure>>();

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        error.Should()
            .NotBeNullOrEmpty()
            .And
            .HaveCount(3);
    }

    [Fact]
    public async Task UpdateRoute_Return404_WhenNotFound()
    {
        //Act
        var updateRouteRequest = new UpdateRouteRequest
        {
            Id = Guid.NewGuid(),
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
                }
            }
        };
        var response = await _sut.UpdateRouteAsync(updateRouteRequest);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    #endregion Update

    #region Delete

    [Fact]
    public async Task DeleteRoute_Return200_WhenExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createRouteRequest = GenerateCreateRouteRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Routes.CreateRoute, createRouteRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateRouteResponse>() ?? throw new Exception("");

        //Act
        var response = await _sut.DeleteRouteAsync(new DeleteRouteRequest { Id = createResponse.Id });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteRoute_Retur404_WhenNotFound()
    {
        //Act
        var response = await _sut.DeleteRouteAsync(new DeleteRouteRequest { Id = Guid.NewGuid() });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteRoute_Return400_WhenIdEmpty()
    {
        //Act
        var response = await _sut.DeleteRouteAsync(new DeleteRouteRequest { Id = Guid.Empty });
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

    private static string GetRouteByIdRoute(Guid id)
    {
        return ReplaceIdRegex().Replace(ApiRoutes.Routes.GetRouteById, id.ToString());
    }

    private static string DeleteRouteRoute(Guid id)
    {
        return ReplaceIdRegex().Replace(ApiRoutes.Routes.DeleteRoute, id.ToString());
    }

    [GeneratedRegex("{id}")]
    private static partial Regex ReplaceIdRegex();

    #endregion Helpers

    #region Setup & Teardown

    private static CreateRouteRequest GenerateCreateRouteRequest()
    {
        return new CreateRouteRequest
        {
            Type = RouteType.Kundegraving,
            Points = new PointDoubleDto[]
            {
                new PointDoubleDto
                {
                    Latitude = 4,
                    Longitude = 5
                },
                new PointDoubleDto
                {
                    Latitude = 6,
                    Longitude = 7
                }
            }
        };
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        if (!_createdRoutes.Any())
            return;

        var httpClient = _factory.CreateClient();

        foreach (var routeId in _createdRoutes)
        {
            await httpClient.DeleteAsync(DeleteRouteRoute(routeId));
        }
    }

    #endregion Setup & Teardown
}