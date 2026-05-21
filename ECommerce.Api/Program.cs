using ECommerce.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar servicios de controladores y Swagger nativo de .NET 8
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Registrar la infraestructura y la base de datos SQLite
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// 3. Configurar Swagger en el pipeline de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();