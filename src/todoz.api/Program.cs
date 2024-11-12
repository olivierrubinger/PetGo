using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using todoz.api.Controllers;
using todoz.api.Data;
using todoz.api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configura o Application Insights usando uma variável de ambiente
//var applicationInsightsConnectionString = Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING");
//if (!string.IsNullOrEmpty(applicationInsightsConnectionString))
//{
//    builder.Services.AddApplicationInsightsTelemetry(options =>
//    {
//        options.ConnectionString = applicationInsightsConnectionString;
//    });
//}

// Configura o logging
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();
//builder.Logging.AddEventSourceLogger();


// Configuração de CORS para permitir o front-end se comunicar com a API.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
// ----------------------------------------------------------------------------------------- CORS

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todoz API", Version = "v1" });
    });

// Configura o banco de dados com base no ambiente
//string? connectionString;

//if (builder.Environment.IsDevelopment())
//{
//    connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
//    builder.Services.AddDbContext<TodoContext>(options => { options.UseSqlite(connectionString); });
//}
//else
//{
var connectionString = builder.Configuration.GetConnectionString("SQLCONNSTR_TODOZDB");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A variável de ambiente 'SQLCONNSTR_TODOZDB' não está configurada.");
}
builder.Services.AddDbContext<TodoContext>(options => { options.UseSqlServer(connectionString); });

//}
// Adicionando injeção de dependência para os repositórios de banco de dados.
builder.Services.AddScoped<ITodosRepository, TodosInDatabase>();
builder.Services.AddControllers();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Adiciona suporte a HSTS em produção
}
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todoz API V1");
});

// ----------------------------------------------------------------------------------------- CORS
app.UseCors("AllowFrontend");
// ----------------------------------------------------------------------------------------- CORS

app.MapControllers();
app.Run();
