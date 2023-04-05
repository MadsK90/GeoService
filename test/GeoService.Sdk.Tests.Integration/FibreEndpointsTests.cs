namespace GeoService.Sdk.Tests.Integration;

public sealed partial class FibreEndpointsTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    #region Fields

    private readonly WebApplicationFactory<IApiMarker> _factory;
    private readonly List<Guid> _createdFibres = new();
    private readonly IGeoServiceApi _sut;

    #endregion Fields

    public FibreEndpointsTests(WebApplicationFactory<IApiMarker> factory)
    {
        _factory = factory;
        _sut = RestService.For<IGeoServiceApi>(_factory.CreateClient());
    }

    #region Add

    [Fact]
    public async Task CreateFibre_CreatesFibre_WhenDataIsCorrect()
    {
        //Arrange
        var createFibreRequest = GenereateCreateFibreRequest();

        //Act
        var response = await _sut.CreateFibreAsync(createFibreRequest) ?? throw new Exception("");
        if (response.Content != null)
            _createdFibres.Add(response.Content.Id);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
            .NotBeNull();

        response.Content!.Id.Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task CreateFibre_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var createFibreRequest = GenereateCreateFibreRequest();
        createFibreRequest.Size = -1;
        createFibreRequest.Points = Array.Empty<PointDoubleDto>();

        //Act
        var response = await _sut.CreateFibreAsync(createFibreRequest) ?? throw new Exception("");
        var error = await response.Error!.GetContentAsAsync<List<ValidationFailure>>();

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        error.Should()
            .NotBeNullOrEmpty()
            .And
            .HaveCount(2);
    }

    #endregion Add

    #region Get

    [Fact]
    public async Task GetFibreById_ReturnFibre_WhenExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createFibreRequest = GenereateCreateFibreRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Fibres.CreateFibre, createFibreRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateFibreResponse>() ?? throw new Exception("");

        _createdFibres.Add(createResponse.Id);

        //Act
        var response = await _sut.GetFibreByIdAsync(new GetFibreByIdRequest { Id = createResponse.Id });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(createFibreRequest);
    }

    [Fact]
    public async Task GetFibreById_Return404_WhenNotFound()
    {
        //Act
        var response = await _sut.GetFibreByIdAsync(new GetFibreByIdRequest { Id = Guid.NewGuid() });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetFibreById_Return400_AndValidationErrors_WhenDataIsIncorrect()
    {
        //Act
        var response = await _sut.GetFibreByIdAsync(new GetFibreByIdRequest { Id = Guid.Empty });
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
    public async Task UpdateFibre_UpdatesFibre_WhenDataIsCorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createFibreRequest = GenereateCreateFibreRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Fibres.CreateFibre, createFibreRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateFibreResponse>() ?? throw new Exception("");
        _createdFibres.Add(createResponse.Id);

        //Act
        var updateFibreRequest = new UpdateFibreRequest
        {
            Id = createResponse.Id,
            Air = false,
            Size = 96,
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
                }
            }
        };
        var response = await _sut.UpdateFibreAsync(updateFibreRequest);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
            .BeEquivalentTo(updateFibreRequest);
    }

    [Fact]
    public async Task UpdateFibre_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createFibreRequest = GenereateCreateFibreRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Fibres.CreateFibre, createFibreRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateFibreResponse>() ?? throw new Exception("");
        _createdFibres.Add(createResponse.Id);

        //Act
        var updateFibreRequest = new UpdateFibreRequest
        {
            Id = createResponse.Id,
            Air = false,
            Size = -96,
            Points = new PointDoubleDto[]
            {
                new PointDoubleDto
                {
                    Latitude = 5,
                    Longitude = 6
                }
            }
        };

        var response = await _sut.UpdateFibreAsync(updateFibreRequest);
        var error = await response.Error!.GetContentAsAsync<List<ValidationFailure>>();

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        error.Should()
            .NotBeNullOrEmpty()
            .And
            .HaveCount(2);
    }

    [Fact]
    public async Task UpdateFibre_Return404_WhenNotFound()
    {
        //Act
        var updateFibreRequest = new UpdateFibreRequest
        {
            Id = Guid.NewGuid(),
            Air = true,
            Size = 48,
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
        var response = await _sut.UpdateFibreAsync(updateFibreRequest);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    #endregion Update

    #region Delete

    [Fact]
    public async Task DeleteFibre_Return200_WhenExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createFibreRequest = GenereateCreateFibreRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Fibres.CreateFibre, createFibreRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateFibreResponse>() ?? throw new Exception("");

        //Act
        var response = await _sut.DeleteFibreAsync(new DeleteFibreRequest { Id = createResponse.Id });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteFibre_Return404_WhenNotFound()
    {
        //Act
        var response = await _sut.DeleteFibreAsync(new DeleteFibreRequest { Id = Guid.NewGuid() });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteFibre_Return400_WhenIdEmpty()
    {
        //Act
        var response = await _sut.DeleteFibreAsync(new DeleteFibreRequest { Id = Guid.Empty });
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

    private static string GetFibreByIdRoute(Guid id)
    {
        return ReplaceIdRegex().Replace(ApiRoutes.Fibres.GetFibreById, id.ToString());
    }

    private static string DeleteFibreRoute(Guid id)
    {
        return ReplaceIdRegex().Replace(ApiRoutes.Fibres.DeleteFibre, id.ToString());
    }

    [GeneratedRegex("{id}")]
    private static partial Regex ReplaceIdRegex();

    #endregion Helpers

    #region Setup & Teardown

    private static CreateFibreRequest GenereateCreateFibreRequest()
    {
        return new CreateFibreRequest
        {
            Air = true,
            Size = 48,
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
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        if (!_createdFibres.Any())
            return;

        var httpClient = _factory.CreateClient();

        foreach (var fibreId in _createdFibres)
        {
            await httpClient.DeleteAsync(DeleteFibreRoute(fibreId));
        }
    }

    #endregion Setup & Teardown
}