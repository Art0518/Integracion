using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// ? Configurar puerto din�mico para Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// ? Agregar Controllers
builder.Services.AddControllers();

// ? Registrar un esquema de autenticación por defecto "NoAuth" ligero
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "NoAuth";
})
// Handler simple que autentica siempre como "Anonymous" (evita excepciones cuando
// AuthorizationMiddleware se ejecuta sin un esquema configurado). Si se desea
// autenticación real, reemplazar por el esquema correspondiente.
.AddScheme<AuthenticationSchemeOptions, Gateway.CafeSanJuan.NoAuth.NoAuthHandler>(
    "NoAuth", options => { });

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
        Title = "API Gateway - Caf� San Juan",
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
    c.RoutePrefix = string.Empty; // Swagger en la ra�z
});

// ? Mapear Controllers (ANTES de Ocelot)
app.MapControllers();

// Diagnostic: logear rutas cargadas desde ocelot.json para depuración
try
{
    var routeSection = builder.Configuration.GetSection("Routes");
    var routeChildren = routeSection.GetChildren().ToList();
    Console.WriteLine($"[DIAG] Ocelot routes count: {routeChildren.Count}");
    foreach (var r in routeChildren)
    {
        var up = r["UpstreamPathTemplate"] ?? "(no-upstream)";
        var downHosts = r.GetSection("DownstreamHostAndPorts").GetChildren().Select(h => h["Host"]).ToArray();
        Console.WriteLine($"[DIAG] Route: {up} -> {string.Join(',', downHosts)}");
    }
}
catch (Exception ex)
{
    Console.WriteLine("[DIAG] Error leyendo Routes desde configuración: " + ex.Message);
}

// ? Usar Ocelot SOLO para rutas no manejadas por Controllers/Swagger
// ? Asegurar que el middleware de autenticación/autorization esté presente
app.UseAuthentication();
app.UseAuthorization();

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
