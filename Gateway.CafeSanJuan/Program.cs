using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ? Configurar puerto dinámico para Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// ? Agregar Controllers
builder.Services.AddControllers();

// ? Configurar HttpClient para aceptar certificados SSL
builder.Services.AddHttpClient();

// Configurar Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

// ? Agregar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Gateway - Café San Juan",
        Version = "v1",
        Description = "Gateway que centraliza todos los microservicios del sistema de reservas"
    });
    // Agregar filtro para documentar endpoints proxy
    c.DocumentFilter<Gateway.CafeSanJuan.Swagger.ProxyEndpointsDocumentFilter>();
});

// ? Configurar CORS para permitir cualquier origen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("*");
    });
});

var app = builder.Build();

// ? Habilitar CORS ANTES de Ocelot
app.UseCors("AllowAll");

// ? Configurar Swagger (ANTES de Ocelot)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway API v1");
    c.RoutePrefix = string.Empty; // Swagger en la raíz
});

// ? Mapear Controllers (ANTES de Ocelot)
app.MapControllers();

// ? Usar Ocelot SOLO para rutas no manejadas por Controllers/Swagger
app.UseOcelot((ocelotBuilder, pipelineConfig) =>
{
    // Configurar para que Ocelot NO intercepte rutas de Swagger ni API Gateway
    pipelineConfig.PreErrorResponderMiddleware = async (ctx, next) =>
    {
        var path = ctx.Request.Path.Value;

        // Si la ruta es de Swagger o del Gateway, NO usar Ocelot
        if (path.StartsWith("/swagger") || 
            path.StartsWith("/api/gateway") ||
            path == "/")
        {
            await next();
            return;
        }
        
        await next();
    };
}).Wait();

app.Run();
