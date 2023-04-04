namespace GeoService.Api.Tests.Integration;

public sealed partial class ManholeEndpointsTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    #region Fields

    private readonly WebApplicationFactory<IApiMarker> _factory;
    private readonly List<Guid> _createdManholes = new();

    #endregion Fields

    public ManholeEndpointsTests(WebApplicationFactory<IApiMarker> factory)
    {
        _factory = factory;
    }

    #region Add

    [Fact]
    public async Task CreateManhole_CreatesManhole_WhenDataIsCorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createManholeRequest = GenerateCreateManholeRequest();

        //Act
        var result = await httpClient.PostAsJsonAsync(ApiRoutes.Manholes.CreateManhole, createManholeRequest);
        var response = await result.Content.ReadFromJsonAsync<CreateManholeResponse>() ?? throw new Exception("");

        _createdManholes.Add(response.Id);

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Should()
            .NotBeNull();

        response!.Id.Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task CreateManhole_Return400_AndValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createManholeRequest = GenerateCreateManholeRequest();
        createManholeRequest.Name = "b";
        createManholeRequest.Latitude = -200;
        createManholeRequest.Longitude = -200;

        //Act
        var result = await httpClient.PostAsJsonAsync(ApiRoutes.Manholes.CreateManhole, createManholeRequest);
        var response = await result.Content.ReadFromJsonAsync<List<ValidationFailure>>();

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        response.Should()
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
        var result = await httpClient.GetAsync(GetManholeByIdRoute(createResponse.Id));
        var response = await result.Content.ReadFromJsonAsync<GetManholeByIdResponse>();

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Should()
            .NotBeNull()
            .And.BeEquivalentTo(createManholeRequest);
    }

    [Fact]
    public async Task GetManholeById_Return404_WheNotFound()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.GetAsync(GetManholeByIdRoute(Guid.NewGuid()));

        //Assert

        result.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetManholeById_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.GetAsync(GetManholeByIdRoute(Guid.Empty));
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
        var updateResult = await httpClient.PutAsJsonAsync(ApiRoutes.Manholes.UpdateManhole, updateManholeRequest);
        var updateResponse = await updateResult.Content.ReadFromJsonAsync<UpdateManholeResponse>();

        //Assert
        updateResult.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        updateResponse.Should()
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
        var updateResult = await httpClient.PutAsJsonAsync(ApiRoutes.Manholes.UpdateManhole, updateManholeRequest);
        var updateResponse = await updateResult.Content.ReadFromJsonAsync<List<ValidationFailure>>();

        //Assert
        updateResult.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        updateResponse.Should()
            .NotBeNullOrEmpty()
            .And
            .HaveCount(3);
    }

    [Fact]
    public async Task UpdateManhole_Return404_WhenNotFound()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var updateManholeRequest = new UpdateManholeRequest
        {
            Id = Guid.NewGuid(),
            Latitude = 40,
            Longitude = 50,
            Name = "ABC"
        };

        var updateResult = await httpClient.PutAsJsonAsync(ApiRoutes.Manholes.UpdateManhole, updateManholeRequest);

        //Assert
        updateResult.StatusCode.Should()
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
        var result = await httpClient.DeleteAsync(DeleteManholeRoute(createResponse.Id));

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteManhole_Return404_WhenNotFound()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.DeleteAsync(DeleteManholeRoute(Guid.NewGuid()));

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteManhole_Return400_WhenIdEmpty()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.DeleteAsync(DeleteManholeRoute(Guid.Empty));
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