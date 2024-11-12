using Microsoft.EntityFrameworkCore;

using todoz.api.Controllers;
using todoz.api.Data;
using todoz.api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add database services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A variável de ambiente 'DefaultConnection' não está configurada.");
}
builder.Services.AddDbContext<TodoContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Add swagger services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Add dependency injection to the container.
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
    //app.UseHsts(); // Adiciona suporte a HSTS em produção
}

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// ----------------------------------------------------------------------------------------- CORS
app.UseCors("AllowAll");
//---------------------------------------------------------------------------------------- CORS

app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.Run();
