using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcLibrary.Data;
using System;
using System.Linq;

namespace MvcLibrary.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new MvcLibraryContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<MvcLibraryContext>>()))
        {
            if (context.Library.Any())
            {
                return;   // DB has been seeded
            }
            context.SaveChanges();
        }
    }
}