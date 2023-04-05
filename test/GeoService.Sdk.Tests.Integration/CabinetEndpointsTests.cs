namespace GeoService.Sdk.Tests.Integration;

public sealed partial class CabinetEndpointsTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    #region Fields

    private readonly WebApplicationFactory<IApiMarker> _factory;
    private readonly List<Guid> _createdCabinets = new();
    private readonly IGeoServiceApi _sut;

    #endregion Fields

    public CabinetEndpointsTests(WebApplicationFactory<IApiMarker> factory)
    {
        _factory = factory;
        _sut = RestService.For<IGeoServiceApi>(_factory.CreateClient());
    }

    #region Add

    [Fact]
    public async Task CreateCabinet_CreatesCabinet_WhenDataIsCorrect()
    {
        //Arrange
        var createCabinetRequest = GenerateCreateCabinetRequest();

        //Act
        var response = await _sut.CreateCabinetAsync(createCabinetRequest) ?? throw new Exception("");
        if (response.Content != null)
            _createdCabinets.Add(response.Content.Id);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
            .NotBeNull();

        response.Content!.Id.Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task CreateCabinet_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var createCabinetRequest = GenerateCreateCabinetRequest();
        createCabinetRequest.Name = "b";
        createCabinetRequest.Latitude = -200;
        createCabinetRequest.Longitude = 200;

        //Act
        var response = await _sut.CreateCabinetAsync(createCabinetRequest) ?? throw new Exception("");
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
    public async Task GetCabinetById_ReturnCabinet_WhenExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createCabinetRequest = GenerateCreateCabinetRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Cabinets.CreateCabinet, createCabinetRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateCabinetResponse>() ?? throw new Exception("");

        _createdCabinets.Add(createResponse.Id);

        //Act
        var response = await _sut.GetCabinetByIdAsync(new GetCabinetByIdRequest { Id = createResponse.Id });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(createCabinetRequest);
    }

    [Fact]
    public async Task GetCabinetById_Return404_WhenNotFound()
    {
        //Act
        var response = await _sut.GetCabinetByIdAsync(new GetCabinetByIdRequest { Id = Guid.NewGuid() });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetCabinetById_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Act
        var response = await _sut.GetCabinetByIdAsync(new GetCabinetByIdRequest { Id = Guid.Empty });
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
    public async Task UpdateCabinet_UpdatesCabinet_WhenDataIsCorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createCabinetRequest = GenerateCreateCabinetRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Cabinets.CreateCabinet, createCabinetRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateCabinetResponse>() ?? throw new Exception("");
        _createdCabinets.Add(createResponse.Id);

        //Act
        var updateCabinetRequest = new UpdateCabinetRequest
        {
            Id = createResponse.Id,
            Address = createCabinetRequest.Address,
            Latitude = createCabinetRequest.Latitude,
            Longitude = createCabinetRequest.Longitude,
            Name = "ABC"
        };

        var response = await _sut.UpdateCabinetAsync(updateCabinetRequest);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
            .BeEquivalentTo(updateCabinetRequest);
    }

    [Fact]
    public async Task UpdateCabinet_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createCabinetRequest = GenerateCreateCabinetRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Cabinets.CreateCabinet, createCabinetRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateCabinetResponse>() ?? throw new Exception("");
        _createdCabinets.Add(createResponse.Id);

        //Act
        var updateCabinetRequest = new UpdateCabinetRequest
        {
            Id = createResponse.Id,
            Address = createCabinetRequest.Address,
            Latitude = 200,
            Longitude = 200,
            Name = "AB"
        };

        var response = await _sut.UpdateCabinetAsync(updateCabinetRequest);
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
    public async Task UpdateCabinet_Return404_WhenNotFound()
    {
        //Act
        var updateCabinetRequest = new UpdateCabinetRequest
        {
            Id = Guid.NewGuid(),
            Name = "ABC",
            Latitude = 40,
            Longitude = 50
        };
        var response = await _sut.UpdateCabinetAsync(updateCabinetRequest);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    #endregion Update

    #region Delete

    [Fact]
    public async Task DeleteCabinet_Return200_WhenExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createCabinetRequest = GenerateCreateCabinetRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Cabinets.CreateCabinet, createCabinetRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateCabinetResponse>() ?? throw new Exception("");

        //Act
        var response = await _sut.DeleteCabinetAsync(new DeleteCabinetRequest { Id = createResponse.Id });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteCabinet_Return404_WhenNotFound()
    {
        //Act
        var response = await _sut.DeleteCabinetAsync(new DeleteCabinetRequest { Id = Guid.NewGuid() });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteCabinet_Return400_WhenIdEmpty()
    {
        //Act
        var response = await _sut.DeleteCabinetAsync(new DeleteCabinetRequest { Id = Guid.Empty });
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

    private static string GetCabinetByIdRoute(Guid id)
    {
        return ReplaceIdRegex().Replace(ApiRoutes.Cabinets.GetCabinetById, id.ToString());
    }

    private static string DeleteCabinetRoute(Guid id)
    {
        return ReplaceIdRegex().Replace(ApiRoutes.Cabinets.DeleteCabinet, id.ToString());
    }

    [GeneratedRegex("{id}")]
    private static partial Regex ReplaceIdRegex();

    #endregion Helpers

    #region Setup & Teardown

    private static CreateCabinetRequest GenerateCreateCabinetRequest()
    {
        return new CreateCabinetRequest
        {
            Name = "XYZ",
            Address = "Stordalen 31",
            Latitude = 30,
            Longitude = 40
        };
    }

    private static UpdateCabinetRequest GenerateUpdateCabinetRequest(Guid id)
    {
        return new UpdateCabinetRequest
        {
            Id = id,
            Name = "XYZ",
            Address = "Stordalen 31",
            Latitude = 30,
            Longitude = 40
        };
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        if (!_createdCabinets.Any())
            return;

        var httpClient = _factory.CreateClient();

        foreach (var cabinetId in _createdCabinets)
        {
            await httpClient.DeleteAsync(DeleteCabinetRoute(cabinetId));
        }
    }

    #endregion Setup & Teardown
}