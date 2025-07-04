using LeadsManagement.Domain.Handlers;
using LeadsManagement.Domain.Repositories;
using LeadsManagement.Domain.Service;
using LeadsManagement.Infra.Contexts;
using LeadsManagement.Infra.Respositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("Database"));

ConfigureServices(builder);

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevPolicy",
        x => x.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

await SeedDatabase(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DevPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();


async Task SeedDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    
    try
    {
        var context = services.GetRequiredService<DataContext>();
        await context.Database.EnsureCreatedAsync();
        await LeadsManagement.Infra.Seed.DatabaseSeeder.SeedAsync(context);
        Console.WriteLine("Database initialization completed successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
    }
}

void ConfigureServices(WebApplicationBuilder builder){
    builder.Services.AddTransient<ILeadRepository, LeadRepository>();
    builder.Services.AddTransient<IEmailService, EmailService>();
    builder.Services.AddTransient<LeadHandler>();
}