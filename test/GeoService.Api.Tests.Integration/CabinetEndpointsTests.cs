namespace GeoService.Api.Tests.Integration;

public sealed partial class CabinetEndpointsTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    private readonly WebApplicationFactory<IApiMarker> _factory;
    private readonly List<Guid> _createdCabinets = new();

    public CabinetEndpointsTests(WebApplicationFactory<IApiMarker> factory)
    {
        _factory = factory;
    }

    #region Add

    [Fact]
    public async Task CreateCabinet_CreatesCabinet_WhenDataIsCorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createCabinetRequest = GenerateCreateCabinetRequest();

        //Act
        var result = await httpClient.PostAsJsonAsync(ApiRoutes.Cabinets.CreateCabinet, createCabinetRequest);
        var response = await result.Content.ReadFromJsonAsync<CreateCabinetResponse>() ?? throw new Exception("");

        _createdCabinets.Add(response.Id);

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Should()
            .NotBeNull();

        response!.Id.Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task CreateCabinet_Return400_And_ValidationErrors_WhenValidationFailes()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createCabinetRequest = GenerateCreateCabinetRequest();
        createCabinetRequest.Name = "b";
        createCabinetRequest.Latitude = -200;
        createCabinetRequest.Longitude = 200;

        //Act
        var result = await httpClient.PostAsJsonAsync(ApiRoutes.Cabinets.CreateCabinet, createCabinetRequest);
        var response = await result.Content.ReadFromJsonAsync<List<ValidationFailure>>();

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        response.Should()
            .NotBeNullOrEmpty()
            .And.HaveCount(3);
    }

    #endregion Add

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
        var result = await httpClient.GetAsync(GetCabinetByIdRoute(createResponse.Id));
        var response = await result.Content.ReadFromJsonAsync<GetCabinetByIdResponse>();

        //Assert

        result.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Should()
            .NotBeNull()
            .And.BeEquivalentTo(createCabinetRequest);
    }

    [Fact]
    public async Task GetCabinetById_Return404_WhenNotFound()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.GetAsync(GetCabinetByIdRoute(Guid.NewGuid()));

        //Assert

        result.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetCabinetById_Return400_And_ValidationErrors_WhenValidationFails()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.GetAsync(GetCabinetByIdRoute(Guid.Empty));
        var response = await result.Content.ReadFromJsonAsync<List<ValidationFailure>>();

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        response.Should()
            .NotBeNullOrEmpty()
            .And.HaveCount(1);
    }

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