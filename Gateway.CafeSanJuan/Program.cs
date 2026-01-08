using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ? Configurar puerto dinámico para Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// ? Configurar HttpClient para aceptar certificados SSL
builder.Services.AddHttpClient();

// Configurar Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

// ? Configurar CORS para permitir cualquier origen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
  {
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("*"); // Exponer todos los headers en las respuestas
    });
});

var app = builder.Build();

// ? Habilitar CORS ANTES de Ocelot
app.UseCors("AllowAll");

// ? Usar Ocelot como API Gateway
await app.UseOcelot();

app.Run();
