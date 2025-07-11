using LeadsManagement.Domain.Entities;
using LeadsManagement.Domain.Enums;
using LeadsManagement.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LeadsManagement.Infra.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(DataContext context)
    {
        if (await context.Leads.AnyAsync())
        {
            return;
        }

        var leads = new List<Lead>
        {
            new Lead(
                "João", 
                "Silva", 
                "joao.silva@email.com", 
                "(11) 99999-1111", 
                "Interessado em apartamento de 2 quartos na Vila Olímpia",
                "Vila Olímpia", 
                550.00m
            ),
            new Lead(
                "Maria", 
                "Santos", 
                "maria.santos@email.com", 
                "(11) 99999-2222", 
                "Procura casa com quintal para família grande",
                "Brooklin", 
                680.00m
            ),
            new Lead(
                "Pedro", 
                "Costa", 
                "pedro.costa@email.com", 
                "(11) 99999-3333", 
                "Investidor interessado em imóveis comerciais",
                "Faria Lima", 
                520.00m
            ),
            new Lead(
                "Ana", 
                "Oliveira", 
                "ana.oliveira@email.com", 
                "(11) 99999-4444", 
                "Preciso de um chaveiro",
                "Santo André", 
                28.00m
            ),
            new Lead(
                "Carlos", 
                "Ferreira", 
                "carlos.ferreira@email.com", 
                "(11) 99999-5555", 
                "Preciso de um pintor",
                "Santos", 
                12.00m
            )
        };
        await context.Leads.AddRangeAsync(leads);
        await context.SaveChangesAsync();

        Console.WriteLine($"Database seeded successfully with {leads.Count} leads!");
    }
} 