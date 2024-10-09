using Microsoft.EntityFrameworkCore;
using todoz.api.Controllers;
using todoz.api.Data;
using todoz.api.Repositories;

var builder = WebApplication.CreateBuilder(args);
// ----------------------------------------------------------------------------------------- CORS
// Obtém a URL do front-end da variável de ambiente.
var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:5193";

// Configuração de CORS para permitir o front-end se comunicar com a API.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(frontendUrl)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
// ----------------------------------------------------------------------------------------- CORS

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura o banco de dados com base no ambiente
string? connectionString;
if (builder.Environment.IsDevelopment())
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<TodoContext>(options => { options.UseSqlite(connectionString); });
}
else
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<TodoContext>(options => { options.UseSqlServer(connectionString); });
}

// Adicionando injeção de dependência para os repositórios de banco de dados.
builder.Services.AddScoped<ITodoRepository, TodoInDatabase>();
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
// ----------------------------------------------------------------------------------------- CORS
app.UseCors("AllowFrontend");
// ----------------------------------------------------------------------------------------- CORS
app.UseAuthorization();
app.MapControllers();
app.Run();
