using Microsoft.EntityFrameworkCore;
using petgo.api.Data;
using petgo.api.Services;

var builder = WebApplication.CreateBuilder(args);

// APENAS PostgreSQL (Supabase) - Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' não foi encontrada.");
}

Console.WriteLine("🐘 Usando PostgreSQL (Supabase)");

// FORÇAR IPv4 - APENAS no DbContext (não globalmente)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null
        );
        npgsqlOptions.CommandTimeout(60);
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:3000",
                    "https://localhost:3000",
                    "http://localhost:5173",
                    "https://petgo-frontend.vercel.app",
                    "https://*.vercel.app"
                  )
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.MaxDepth = 64;
    });

var app = builder.Build();

// Aplicar migrations e seed automaticamente
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    try
    {
        Console.WriteLine("📦 Verificando migrations...");
        
        // Aplicar migrations pendentes
        if (context.Database.GetPendingMigrations().Any())
        {
            Console.WriteLine("📦 Aplicando migrations...");
            await context.Database.MigrateAsync();
        }
        else
        {
            Console.WriteLine("✅ Banco de dados atualizado!");
        }
        
        // Seed apenas se banco estiver vazio
        if (!await context.Produtos.AnyAsync())
        {
            Console.WriteLine("🌱 Executando seed inicial...");
            await DatabaseSeeder.SeedAsync(context);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Erro ao inicializar banco: {ex.Message}");
        Console.WriteLine($"Stack: {ex.StackTrace}");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine($"🚀 PetGo API iniciada!");
Console.WriteLine($"📍 Ambiente: {app.Environment.EnvironmentName}");
Console.WriteLine($"🗄️ Database: PostgreSQL (Supabase)");

app.Run();