using GeoService.Contracts.V1.Requests.Cabinets;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GeoService.Sdk.Tests.Integration;

public sealed partial class SplitterEndpointsTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    #region Fields

    private readonly WebApplicationFactory<IApiMarker> _factory;
    private readonly List<Guid> _createdSplitters = new();
    private readonly IGeoServiceApi _sut;

    #endregion Fields

    public SplitterEndpointsTests(WebApplicationFactory<IApiMarker> factory)
    {
        _factory = factory;
        _sut = RestService.For<IGeoServiceApi>(_factory.CreateClient());
    }

    #region Add

    [Fact]
    public async Task CreateSplitter_CreatesSplitter_WhenDataIsCorrect()
    {
        //Arrange
        var createSplitterRequest = GenerateCreateSplitterRequest();

        var response = await _sut.CreateSplitterAsync(createSplitterRequest) ?? throw new Exception("");
        if (response.Content != null)
            _createdSplitters.Add(response.Content.Id);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
            .NotBeNull();

        response.Content!.Id.Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task CreateSplitter_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var createSplitterRequest = GenerateCreateSplitterRequest();
        createSplitterRequest.Name = "b";
        createSplitterRequest.Latitude = -200;
        createSplitterRequest.Longitude = 200;

        //Act
        var response = await _sut.CreateSplitterAsync(createSplitterRequest) ?? throw new Exception("");
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
    public async Task GetSplitterById_ReturnSplitter_WhenExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createSplitterRequest = GenerateCreateSplitterRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Splitters.CreateSplitter, createSplitterRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateSplitterResponse>() ?? throw new Exception("");
        _createdSplitters.Add(createResponse.Id);

        //Act
        var response = await _sut.GetSplitterByIdAsync(new GetSplitterByIdRequest { Id = createResponse.Id });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(createSplitterRequest);
    }

    [Fact]
    public async Task GetSplitterById_Return404_WhenNotFound()
    {
        //Act
        var response = await _sut.GetSplitterByIdAsync(new GetSplitterByIdRequest { Id = Guid.NewGuid() });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSplitterById_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Act
        var response = await _sut.GetSplitterByIdAsync(new GetSplitterByIdRequest { Id = Guid.Empty });
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
        var response = await _sut.UpdateSplitterAsync(updateSplitterRequest);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Content.Should()
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
        var response = await _sut.UpdateSplitterAsync(updateSplitterRequest);
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
    public async Task UpdateSplitter_Retur404_WhenNotFound()
    {
        //Act
        var updateSplitterRequest = new UpdateSplitterRequest
        {
            Id = Guid.NewGuid(),
            Name = "ABC",
            Latitude = 40,
            Longitude = 50
        };
        var response = await _sut.UpdateSplitterAsync(updateSplitterRequest);

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    #endregion Update

    #region Delete

    [Fact]
    public async Task DeleteSplitter_Return200_WhenExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createSplitterRequest = GenerateCreateSplitterRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Splitters.CreateSplitter, createSplitterRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateSplitterResponse>() ?? throw new Exception("");

        //Act
        var response = await _sut.DeleteSplitterAsync(new DeleteSplitterRequest { Id = createResponse.Id });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteSplitter_Return404_WhenNotFound()
    {
        //Act
        var response = await _sut.DeleteSplitterAsync(new DeleteSplitterRequest { Id = Guid.NewGuid() });

        //Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteSplitter_Return400_WhenIdEmpty()
    {
        //Act
        var response = await _sut.DeleteSplitterAsync(new DeleteSplitterRequest { Id = Guid.Empty });
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