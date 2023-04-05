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