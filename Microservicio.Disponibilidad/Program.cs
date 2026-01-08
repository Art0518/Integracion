var builder = WebApplication.CreateBuilder(args);

// ? Configurar puerto dinámico para Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Microservicio de Disponibilidad - Café San Juan",
        Version = "v1",
        Description = "API para consultar disponibilidad de mesas"
 });
});

var app = builder.Build();

// Configure the HTTP request pipeline - Swagger SIEMPRE habilitado
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Disponibilidad API v1");
    c.RoutePrefix = string.Empty; // Swagger en la raíz (http://localhost:5004/)
});

app.UseAuthorization();
app.MapControllers();

app.Run();
