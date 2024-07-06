using Bogus;
using Bogus.Extensions;
using SimpleChat.DAL.Context;
using SimpleChat.DAL.Entities;

namespace SimpleChat.DAL.Helpers;

public static class Seed
{
    public static async Task SeedDatabaseAsync(ApplicationContext context)
    {
        if (context.Users.Any())
            return;

        var users = new Faker<User>()
            .RuleFor(u => u.Name, f => f.Name.FullName().ClampLength(max: 45))
            .RuleFor(u => u.CreatedAt, new DateTime())
            .Generate(10);
        
        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }
}