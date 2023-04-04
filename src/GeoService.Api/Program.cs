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
    AddCabinetEndPoints(app);

    #region Local Functions

    static void AddCabinetEndPoints(WebApplication app)
    {
        app.MediatePost<CreateCabinetRequest, CreateCabinetResponse>(ApiRoutes.Cabinets.CreateCabinet)
            .CacheOutput()
            .WithTags("cabinet")
            .Produces(400);

        app.MediateGet<GetCabinetByIdRequest, GetCabinetByIdResponse>(ApiRoutes.Cabinets.GetCabinetById)
            .CacheOutput()
            .WithTags("cabinet")
            .Produces(400);

        app.MediateGet<GetAllCabinetsRequest, GetAllCabinetsResponse>(ApiRoutes.Cabinets.GetAllCabinets)
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

    #endregion Local Functions
}