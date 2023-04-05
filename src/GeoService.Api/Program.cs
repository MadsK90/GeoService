var app = CreateApplication(args);
//app.UseOutputCache();
AddEndpoints(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();

static WebApplication CreateApplication(params string[] args)
{
    var assembly = typeof(Program).Assembly;

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddServiceInstallers(builder.Configuration, AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(assembly));
    builder.Services.AddValidatorsFromAssembly(assembly);
    builder.Services.AddDbContext<DataContext>();
    //builder.Services.AddOutputCache();

    var config = new TypeAdapterConfig();
    builder.Services.AddSingleton(config);
    builder.Services.AddScoped<IMapper, ServiceMapper>();

    builder.Services.AddSwaggerGen(x =>
    {
        x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Geo Service API",
            Version = "v1"
        });
    });

    return builder.Build();
}

static void AddEndpoints(WebApplication app)
{
    AddCabinetEndpoints(app);
    AddFibreEndpoints(app);
    AddManholeEndpoints(app);
    AddPolygonEndpoints(app);
    AddRouteEndpoints(app);
    AddSplitterEndpoints(app);

    #region Local Functions

    static void AddCabinetEndpoints(WebApplication app)
    {
        app.MediatePost<CreateCabinetRequest, CreateCabinetResponse>(ApiRoutes.Cabinets.CreateCabinet)
            .CacheOutput()
            .WithTags("cabinet")
            .Produces(400);

        app.MediateGet<GetCabinetByIdRequest, GetCabinetByIdResponse>(ApiRoutes.Cabinets.GetCabinetById)
            .CacheOutput()
            .WithTags("cabinet")
            .Produces(400);

        app.MediateDelete<DeleteCabinetRequest>(ApiRoutes.Cabinets.DeleteCabinet)
            .CacheOutput()
            .WithTags("cabinet");

        app.MediatePut<UpdateCabinetRequest, UpdateCabinetResponse>(ApiRoutes.Cabinets.UpdateCabinet)
            .CacheOutput()
            .WithTags("cabinet");
    }

    static void AddFibreEndpoints(WebApplication app)
    {
        app.MediatePost<CreateFibreRequest, CreateFibreResponse>(ApiRoutes.Fibres.CreateFibre)
            .CacheOutput()
            .WithTags("fibre")
            .Produces(400);

        app.MediateGet<GetFibreByIdRequest, GetCabinetByIdResponse>(ApiRoutes.Fibres.GetFibreById)
            .CacheOutput()
            .WithTags("fibre")
            .Produces(400);

        app.MediateDelete<DeleteFibreRequest>(ApiRoutes.Fibres.DeleteFibre)
            .CacheOutput()
            .WithTags("fibre");

        app.MediatePut<UpdateFibreRequest, UpdateFibreResponse>(ApiRoutes.Fibres.UpdateFibre)
            .CacheOutput()
            .WithTags("fibre");
    }

    static void AddManholeEndpoints(WebApplication app)
    {
        app.MediatePost<CreateManholeRequest, CreateManholeResponse>(ApiRoutes.Manholes.CreateManhole)
            .CacheOutput()
            .WithTags("manhole")
            .Produces(400);

        app.MediateGet<GetManholeByIdRequest, GetManholeByIdResponse>(ApiRoutes.Manholes.GetManholeById)
            .CacheOutput()
            .WithTags("manhole")
            .Produces(400);

        app.MediateDelete<DeleteManholeRequest>(ApiRoutes.Manholes.DeleteManhole)
            .CacheOutput()
            .WithTags("manhole");

        app.MediatePut<UpdateManholeRequest, UpdateManholeResponse>(ApiRoutes.Manholes.UpdateManhole)
            .CacheOutput()
            .WithTags("manhole");
    }

    static void AddPolygonEndpoints(WebApplication app)
    {
        app.MediatePost<CreatePolygonRequest, CreatePolygonResponse>(ApiRoutes.Polygons.CreatePolygon)
            .CacheOutput()
            .WithTags("polygon")
            .Produces(400);

        app.MediateGet<GetPolygonByIdRequest, GetPolygonByIdResponse>(ApiRoutes.Polygons.GetPolygonById)
            .CacheOutput()
            .WithTags("polygon")
            .Produces(400);

        app.MediateDelete<DeletePolygonRequest>(ApiRoutes.Polygons.DeletePolygon)
            .CacheOutput()
            .WithTags("polygon");

        app.MediatePut<UpdatePolygonRequest, UpdatePolygonResponse>(ApiRoutes.Polygons.UpdatePolygon)
            .CacheOutput()
            .WithTags("polygon");
    }

    static void AddRouteEndpoints(WebApplication app)
    {
        app.MediatePost<CreateRouteRequest, CreateRouteResponse>(ApiRoutes.Routes.CreateRoute)
            .CacheOutput()
            .WithTags("route")
            .Produces(400);

        app.MediateGet<GetRouteByIdRequest, GetRouteByIdResponse>(ApiRoutes.Routes.GetRouteById)
            .CacheOutput()
            .WithTags("route")
            .Produces(400);

        app.MediateDelete<DeleteRouteRequest>(ApiRoutes.Routes.DeleteRoute)
            .CacheOutput()
            .WithTags("route");

        app.MediatePut<UpdateRouteRequest, UpdateRouteResponse>(ApiRoutes.Routes.UpdateRoute)
            .CacheOutput()
            .WithTags("route");
    }

    static void AddSplitterEndpoints(WebApplication app)
    {
        app.MediatePost<CreateSplitterRequest, CreateSplitterResponse>(ApiRoutes.Splitters.CreateSplitter)
            .CacheOutput()
            .WithTags("splitter")
            .Produces(400);

        app.MediateGet<GetSplitterByIdRequest, GetSplitterByIdResponse>(ApiRoutes.Splitters.GetSplitterById)
            .CacheOutput()
            .WithTags("splitter")
            .Produces(400);

        app.MediateDelete<DeleteSplitterRequest>(ApiRoutes.Splitters.DeleteSplitter)
            .CacheOutput()
            .WithTags("splitter");

        app.MediatePut<UpdateSplitterRequest, UpdateSplitterResponse>(ApiRoutes.Splitters.UpdateSplitter)
            .CacheOutput()
            .WithTags("splitter");
    }

    #endregion Local Functions
}