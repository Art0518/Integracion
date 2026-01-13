var builder = WebApplication.CreateBuilder(args);

// Configurar logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Configurar puerto dinámico para Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar CORS - DEBE IR ANTES de los HttpClients
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
 {
  policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configurar HttpClient para cada microservicio con opciones mejoradas
builder.Services.AddHttpClient("FacturaService", client =>
{
    client.BaseAddress = new Uri("https://factura-production-7d28.up.railway.app/");
    client.Timeout = TimeSpan.FromSeconds(120);
    client.DefaultRequestHeaders.Add("User-Agent", "ApiGateway");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
    AllowAutoRedirect = true,
    MaxConnectionsPerServer = 10,
    UseProxy = false
})
.SetHandlerLifetime(TimeSpan.FromMinutes(5));

builder.Services.AddHttpClient("UsuarioService", client =>
{
    client.BaseAddress = new Uri("https://usuario-production1.up.railway.app/");
    client.Timeout = TimeSpan.FromSeconds(120);
    client.DefaultRequestHeaders.Add("User-Agent", "ApiGateway");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
    AllowAutoRedirect = true,
    MaxConnectionsPerServer = 10,
    UseProxy = false
})
.SetHandlerLifetime(TimeSpan.FromMinutes(5));

builder.Services.AddHttpClient("ReservaService", client =>
{
    client.BaseAddress = new Uri("https://reservas-production1.up.railway.app/");
    client.Timeout = TimeSpan.FromSeconds(120);
    client.DefaultRequestHeaders.Add("User-Agent", "ApiGateway");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
    AllowAutoRedirect = true,
    MaxConnectionsPerServer = 10,
    UseProxy = false
})
.SetHandlerLifetime(TimeSpan.FromMinutes(5));

builder.Services.AddHttpClient("BusquedaService", client =>
{
    client.BaseAddress = new Uri("https://busqueda-production-70a5.up.railway.app/");
    client.Timeout = TimeSpan.FromSeconds(120);
    client.DefaultRequestHeaders.Add("User-Agent", "ApiGateway");
  client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
    AllowAutoRedirect = true,
    MaxConnectionsPerServer = 10,
    UseProxy = false
})
.SetHandlerLifetime(TimeSpan.FromMinutes(5));

// Configurar Swagger con agrupación por servicios
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ApiGateway - Café San Juan",
        Version = "v1",
    Description = "Gateway unificado para todos los microservicios"
    });

    // Agrupar endpoints por tags
    c.TagActionsBy(api =>
  {
    var controllerName = api.ActionDescriptor.RouteValues["controller"];
        return new[] { controllerName ?? "Default" };
    });

    c.DocInclusionPredicate((docName, apiDesc) => true);
});

var app = builder.Build();

// IMPORTANTE: CORS debe ser lo primero en el pipeline
app.UseCors("AllowAll");

// Swagger SIEMPRE habilitado
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiGateway v1");
    c.RoutePrefix = string.Empty; // Swagger en la raíz
    c.DisplayRequestDuration();
    c.EnableTryItOutByDefault();
});

// Middleware de logging para diagnosticar requests
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");
    await next();
 logger.LogInformation($"Response: {context.Response.StatusCode}");
});

app.UseAuthorization();
app.MapControllers();

app.Run();
