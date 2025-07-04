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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


void ConfigureServices(WebApplicationBuilder builder){
    builder.Services.AddTransient<ILeadRepository, LeadRepository>();
    builder.Services.AddTransient<IEmailService, EmailService>();
    builder.Services.AddTransient<LeadHandler>();
}