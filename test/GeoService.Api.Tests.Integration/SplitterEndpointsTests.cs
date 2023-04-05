namespace GeoService.Api.Tests.Integration;

public sealed partial class SplitterEndpointsTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    #region Fields

    private readonly WebApplicationFactory<IApiMarker> _factory;
    private readonly List<Guid> _createdSplitters = new();

    #endregion Fields

    public SplitterEndpointsTests(WebApplicationFactory<IApiMarker> factory)
    {
        _factory = factory;
    }

    #region Add

    [Fact]
    public async Task CreateSplitter_CreatesSplitter_WhenDataIsCorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createSplitterRequest = GenerateCreateSplitterRequest();

        //Act
        var result = await httpClient.PostAsJsonAsync(ApiRoutes.Splitters.CreateSplitter, createSplitterRequest);
        var response = await result.Content.ReadFromJsonAsync<CreateSplitterResponse>() ?? throw new Exception("");

        _createdSplitters.Add(response.Id);

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Should()
            .NotBeNull();

        response!.Id.Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task CreateSplitter_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createSplitterRequest = GenerateCreateSplitterRequest();
        createSplitterRequest.Name = "b";
        createSplitterRequest.Latitude = -200;
        createSplitterRequest.Longitude = 200;

        //Act
        var result = await httpClient.PostAsJsonAsync(ApiRoutes.Splitters.CreateSplitter, createSplitterRequest);
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
    public async Task GetSplitterById_ReturnSplitter_WhenExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createSplitterRequest = GenerateCreateSplitterRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Splitters.CreateSplitter, createSplitterRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateSplitterResponse>() ?? throw new Exception("");
        _createdSplitters.Add(createResponse.Id);

        //Act
        var result = await httpClient.GetAsync(GetSplitterByIdRoute(createResponse.Id));
        var response = await result.Content.ReadFromJsonAsync<GetSplitterByIdResponse>();

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(createSplitterRequest);
    }

    [Fact]
    public async Task GetSplitterById_Return404_WhenNotFound()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.GetAsync(GetSplitterByIdRoute(Guid.NewGuid()));

        //Assert

        result.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSplitterById_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.GetAsync(GetSplitterByIdRoute(Guid.Empty));
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
    public async Task UpdateSplitter_UpdatesSplitter_WhenDataIsCorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createSplitterRequest = GenerateCreateSplitterRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Splitters.CreateSplitter, createSplitterRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateSplitterResponse>() ?? throw new Exception("");
        _createdSplitters.Add(createResponse.Id);

        //Act
        var updateSplitterRequest = new UpdateSplitterRequest
        {
            Id = createResponse.Id,
            Latitude = 20,
            Longitude = 30,
            Name = "XYZ"
        };

        var updateResult = await httpClient.PutAsJsonAsync(ApiRoutes.Splitters.UpdateSplitter, updateSplitterRequest);
        var updateResponse = await updateResult.Content.ReadFromJsonAsync<UpdateSplitterResponse>();

        //Assert
        updateResult.StatusCode.Should()
            .Be(HttpStatusCode.OK);
        updateResponse.Should()
            .BeEquivalentTo(updateSplitterRequest);
    }

    [Fact]
    public async Task UpdateSplitter_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createSplitterRequest = GenerateCreateSplitterRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Splitters.CreateSplitter, createSplitterRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateSplitterResponse>() ?? throw new Exception("");
        _createdSplitters.Add(createResponse.Id);

        //Act
        var updateSplitterRequest = new UpdateSplitterRequest
        {
            Id = createResponse.Id,
            Latitude = 200,
            Longitude = 200,
            Name = "AB"
        };

        var updateResult = await httpClient.PutAsJsonAsync(ApiRoutes.Splitters.UpdateSplitter, updateSplitterRequest);
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
    public async Task UpdateSplitter_Retur404_WhenNotFound()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var updateSplitterRequest = new UpdateSplitterResponse
        {
            Id = Guid.NewGuid(),
            Name = "ABC",
            Latitude = 40,
            Longitude = 50
        };

        var updateResult = await httpClient.PutAsJsonAsync(ApiRoutes.Splitters.UpdateSplitter, updateSplitterRequest);

        //Assert
        updateResult.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    #endregion Update

    #region Helpers

    private static string GetSplitterByIdRoute(Guid id)
    {
        return ReplaceIdRegex().Replace(ApiRoutes.Splitters.GetSplitterById, id.ToString());
    }

    private static string DeleteSplitterRoute(Guid id)
    {
        return ReplaceIdRegex().Replace(ApiRoutes.Splitters.DeleteSplitter, id.ToString());
    }

    [GeneratedRegex("{id}")]
    private static partial Regex ReplaceIdRegex();

    #endregion Helpers

    #region Setup & Teardown

    private static CreateSplitterRequest GenerateCreateSplitterRequest()
    {
        return new CreateSplitterRequest
        {
            Name = "ABC",
            Latitude = 1,
            Longitude = 2
        };
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        if (!_createdSplitters.Any())
            return;

        var httpClient = _factory.CreateClient();

        foreach (var splitterId in _createdSplitters)
        {
            await httpClient.DeleteAsync(DeleteSplitterRoute(splitterId));
        }
    }

    #endregion Setup & Teardown
}