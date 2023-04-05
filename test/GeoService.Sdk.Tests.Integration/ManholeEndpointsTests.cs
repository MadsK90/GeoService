using GeoService.Contracts.V1.Requests.Cabinets;

namespace GeoService.Sdk.Tests.Integration;

public sealed partial class ManholeEndpointsTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    #region Fields

    private readonly WebApplicationFactory<IApiMarker> _factory;
    private readonly List<Guid> _createdManholes = new();
    private readonly IGeoServiceApi _sut;

    #endregion Fields

    public ManholeEndpointsTests(WebApplicationFactory<IApiMarker> factory)
    {
        _factory = factory;
        _sut = RestService.For<IGeoServiceApi>(_factory.CreateClient());
    }

    #region Add

    [Fact]
    public async Task CreateManhole_CreatesManhole_WhenDataIsCorrect()
    {
        //Arrange
        var createManholeRequest = GenerateCreateManholeRequest();

        //Act
        var response = await _sut.CreateManholeAsync(createManholeRequest) ?? throw new Exception("");
        if (response.Content != null)
            _createdManholes.Add(response.Content.Id);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
            .NotBeNull();

        response.Content!.Id.Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task CreateManhole_Return400_AndValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var createManholeRequest = GenerateCreateManholeRequest();
        createManholeRequest.Name = "b";
        createManholeRequest.Latitude = -200;
        createManholeRequest.Longitude = -200;

        //Act
        var response = await _sut.CreateManholeAsync(createManholeRequest) ?? throw new Exception("");
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
    public async Task GetManholeById_ReturnManhole_WhenExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createManholeRequest = GenerateCreateManholeRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Manholes.CreateManhole, createManholeRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateManholeResponse>() ?? throw new Exception("");

        _createdManholes.Add(createResponse.Id);

        //Act
        var response = await _sut.GetManholeByIdAsync(new GetManholeByIdRequest { Id = createResponse.Id });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(createManholeRequest);
    }

    [Fact]
    public async Task GetManholeById_Return404_WheNotFound()
    {
        //Act
        var response = await _sut.GetManholeByIdAsync(new GetManholeByIdRequest { Id = Guid.NewGuid() });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetManholeById_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Act
        var response = await _sut.GetManholeByIdAsync(new GetManholeByIdRequest { Id = Guid.Empty });
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
    public async Task UpdateManhole_UpdatesManhole_WhenDataIsCorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createManholeRequest = GenerateCreateManholeRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Manholes.UpdateManhole, createManholeRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateManholeResponse>() ?? throw new Exception("");
        _createdManholes.Add(createResponse.Id);

        //Act
        var updateManholeRequest = new UpdateManholeRequest
        {
            Id = createResponse.Id,
            Latitude = createManholeRequest.Latitude,
            Longitude = createManholeRequest.Longitude,
            Name = "ABC"
        };
        var response = await _sut.UpdateManholeAsync(updateManholeRequest);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
            .BeEquivalentTo(updateManholeRequest);
    }

    [Fact]
    public async Task UpdateManhole_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createManholeRequest = GenerateCreateManholeRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Manholes.UpdateManhole, createManholeRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateManholeResponse>() ?? throw new Exception("");
        _createdManholes.Add(createResponse.Id);

        //Act
        var updateManholeRequest = new UpdateManholeRequest
        {
            Id = createResponse.Id,
            Latitude = 200,
            Longitude = 200,
            Name = "AB"
        };
        var response = await _sut.UpdateManholeAsync(updateManholeRequest);
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
    public async Task UpdateManhole_Return404_WhenNotFound()
    {
        //Act
        var updateManholeRequest = new UpdateManholeRequest
        {
            Id = Guid.NewGuid(),
            Latitude = 40,
            Longitude = 50,
            Name = "ABC"
        };
        var response = await _sut.UpdateManholeAsync(updateManholeRequest);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    #endregion Update

    #region Delete

    [Fact]
    public async Task DeleteManhole_Return200_WhenExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createManholeRequest = GenerateCreateManholeRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Manholes.CreateManhole, createManholeRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateManholeResponse>() ?? throw new Exception("");

        //Act
        var response = await _sut.DeleteManholeAsync(new DeleteManholeRequest { Id = createResponse.Id });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteManhole_Return404_WhenNotFound()
    {
        //Act
        var response = await _sut.DeleteManholeAsync(new DeleteManholeRequest { Id = Guid.NewGuid() });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteManhole_Return400_WhenIdEmpty()
    {
        //Act
        var response = await _sut.DeleteManholeAsync(new DeleteManholeRequest { Id = Guid.Empty });
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

    private static string GetManholeByIdRoute(Guid id)
    {
        return ReplaceIdRegex().Replace(ApiRoutes.Manholes.GetManholeById, id.ToString());
    }

    private static string DeleteManholeRoute(Guid id)
    {
        return ReplaceIdRegex().Replace(ApiRoutes.Manholes.DeleteManhole, id.ToString());
    }

    [GeneratedRegex("{id}")]
    private static partial Regex ReplaceIdRegex();

    #endregion Helpers

    #region Setup & Teardown

    private static CreateManholeRequest GenerateCreateManholeRequest()
    {
        return new CreateManholeRequest
        {
            Name = "XYZ",
            Latitude = 40,
            Longitude = 50
        };
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        if (!_createdManholes.Any())
            return;

        var httpClient = _factory.CreateClient();

        foreach (var manholeId in _createdManholes)
        {
            await httpClient.DeleteAsync(DeleteManholeRoute(manholeId));
        }
    }

    #endregion Setup & Teardown
}