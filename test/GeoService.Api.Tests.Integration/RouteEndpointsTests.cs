﻿namespace GeoService.Api.Tests.Integration;

public sealed partial class RouteEndpointsTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    #region Fields

    private readonly WebApplicationFactory<IApiMarker> _factory;
    private readonly List<Guid> _createdRoutes = new();

    #endregion Fields

    public RouteEndpointsTests(WebApplicationFactory<IApiMarker> factory)
    {
        _factory = factory;
    }

    #region Add

    [Fact]
    public async Task CreateRoute_CreatesRoute_WhenDataIsCorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createRouteRequest = GenerateCreateRouteRequest();

        //Act
        var result = await httpClient.PostAsJsonAsync(ApiRoutes.Routes.CreateRoute, createRouteRequest);
        var response = await result.Content.ReadFromJsonAsync<CreateRouteResponse>() ?? throw new Exception("");

        _createdRoutes.Add(response.Id);

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Should()
            .NotBeNull();

        response!.Id.Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task CreateRoute_Return400_And_ValidationErrors_WhenDataIsIncorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
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
        var result = await httpClient.PostAsJsonAsync(ApiRoutes.Routes.CreateRoute, createRouteRequest);
        var response = await result.Content.ReadFromJsonAsync<List<ValidationFailure>>() ?? throw new Exception("");

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
    public async Task GetRouteById_ReturnRoute_WhenExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var createRouteRequest = GenerateCreateRouteRequest();
        var createResult = await httpClient.PostAsJsonAsync(ApiRoutes.Routes.CreateRoute, createRouteRequest);
        var createResponse = await createResult.Content.ReadFromJsonAsync<CreateRouteResponse>() ?? throw new Exception("");

        _createdRoutes.Add(createResponse.Id);

        //Act
        var result = await httpClient.GetAsync(GetRouteByIdRoute(createResponse.Id));
        var response = await result.Content.ReadFromJsonAsync<GetRouteByIdResponse>();

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        response.Should()
            .NotBeNull()
            .And.BeEquivalentTo(createRouteRequest);
    }

    [Fact]
    public async Task GetRouteById_Return404_WhenNotFound()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.GetAsync(GetRouteByIdRoute(Guid.NewGuid()));

        //Assert

        result.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetRouteById_Return400_And_Validation_Errors_WhenDataIsIncorrect()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.GetAsync(GetRouteByIdRoute(Guid.Empty));
        var response = await result.Content.ReadFromJsonAsync<List<ValidationFailure>>();

        //Assert
        result.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);

        response.Should()
            .NotBeNullOrEmpty()
            .And.HaveCount(1);
    }

    #endregion Get

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