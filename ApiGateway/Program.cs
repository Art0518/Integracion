var builder = WebApplication.CreateBuilder(args);

// Configurar puerto dinámico para Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar HttpClient para cada microservicio
builder.Services.AddHttpClient("FacturaService", client =>
{
    client.BaseAddress = new Uri("https://factura-production-7d28.up.railway.app/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient("UsuarioService", client =>
{
    client.BaseAddress = new Uri("https://usuario-production1.up.railway.app/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient("ReservaService", client =>
{
    client.BaseAddress = new Uri("https://reservas-production1.up.railway.app/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient("BusquedaService", client =>
{
    client.BaseAddress = new Uri("https://busqueda-production-70a5.up.railway.app/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

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

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
          .AllowAnyHeader();
    });
});

var app = builder.Build();

// Habilitar CORS
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

app.UseAuthorization();
app.MapControllers();

app.Run();
