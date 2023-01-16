using Microsoft.EntityFrameworkCore;
using PROIECT.Models;

namespace PROIECT.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new PlantsContext(serviceProvider.GetRequiredService<DbContextOptions<PlantsContext>>()))
            {
                if (context.Plants.Any())
                {
                    return; // BD a fost creata anterior 
                    context.SaveChanges();
                }
            }
        }
    }
}
