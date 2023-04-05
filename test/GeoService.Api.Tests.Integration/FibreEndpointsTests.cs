namespace GeoService.Api.Tests.Integration;

public sealed partial class FibreEndpointsTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    #region Fields

    private readonly WebApplicationFactory<IApiMarker> _factory;
    private readonly List<Guid> _createdFibres = new();

    #endregion Fields

    public FibreEndpointsTests(WebApplicationFactory<IApiMarker> factory)
    {
        _factory = factory;
    }

    #region Add

    [Fact]
    public async Task CreateFibre_CreatesFibre_WhenDataIsCorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createFibreRequest = GenereateCreateFibreRequest();

        //Act
        var result = await httpClient.PostAsJsonAsync(ApiRoutes.Fibres.CreateFibre, createFibreRequest);
        var response = await result.Content.ReadFromJsonAsync<CreateFibreResponse>() ?? throw new Exception("");

        _createdFibres.Add(response.Id);

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Should()
            .NotBeNull();

        response!.Id.Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task CreateFibre_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createFibreRequest = GenereateCreateFibreRequest();
        createFibreRequest.Size = -1;
        createFibreRequest.Points = Array.Empty<PointDoubleDto>();

        //Act
        var result = await httpClient.PostAsJsonAsync(ApiRoutes.Fibres.CreateFibre, createFibreRequest);
        var response = await result.Content.ReadFromJsonAsync<List<ValidationFailure>>() ?? throw new Exception("");

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        response.Should()
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
        var result = await httpClient.GetAsync(GetFibreByIdRoute(createResponse.Id));
        var response = await result.Content.ReadFromJsonAsync<GetFibreByIdResponse>();

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(createFibreRequest);
    }

    [Fact]
    public async Task GetFibreById_Return404_WhenNotFound()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.GetAsync(GetFibreByIdRoute(Guid.NewGuid()));

        //Assert

        result.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetFibreById_Return400_AndValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.GetAsync(GetFibreByIdRoute(Guid.Empty));
        var response = await result.Content.ReadFromJsonAsync<List<ValidationFailure>>();

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        response.Should()
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

        var updateResult = await httpClient.PutAsJsonAsync(ApiRoutes.Fibres.UpdateFibre, updateFibreRequest);
        var updateResponse = await updateResult.Content.ReadFromJsonAsync<UpdateFibreResponse>();

        //Assert
        updateResult.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        updateResponse.Should()
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

        var updateResult = await httpClient.PutAsJsonAsync(ApiRoutes.Fibres.UpdateFibre, updateFibreRequest);
        var updateResponse = await updateResult.Content.ReadFromJsonAsync<List<ValidationFailure>>();

        //Assert
        updateResult.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        updateResponse.Should()
            .NotBeNullOrEmpty()
            .And
            .HaveCount(2);
    }

    [Fact]
    public async Task UpdateFibre_Return404_WhenNotFound()

    {
        //Arrange
        var httpClient = _factory.CreateClient();

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

        var updateResult = await httpClient.PutAsJsonAsync(ApiRoutes.Fibres.UpdateFibre, updateFibreRequest);

        //Assert
        updateResult.StatusCode.Should()
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

        _createdFibres.Add(createResponse.Id);

        //Act
        var result = await httpClient.DeleteAsync(DeleteFibreRoute(createResponse.Id));

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteFibre_Return404_WhenNotFound()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.DeleteAsync(DeleteFibreRoute(Guid.NewGuid()));

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteFibre_Return400_WhenIdEmpty()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.DeleteAsync(DeleteFibreRoute(Guid.Empty));
        var response = await result.Content.ReadFromJsonAsync<List<ValidationFailure>>();

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        response.Should()
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