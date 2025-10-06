using Microsoft.EntityFrameworkCore;
using petgo.api.Controllers;
using petgo.api.Data;
using petgo.api.Services;

var builder = WebApplication.CreateBuilder(args);

// Usar SQLite para desenvolvimento
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=petgo.db";

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(connectionString);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs",
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:3000",   
                    "https://localhost:3000",   
                    "http://localhost:5173",   
                    "https://petgo-frontend.vercel.app" 
                  )
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

builder.Services.AddControllers();
var app = builder.Build();

// Seed database em desenvolvimento
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await DatabaseSeeder.SeedAsync(context);
    }
    
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowNextJs"); 

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();