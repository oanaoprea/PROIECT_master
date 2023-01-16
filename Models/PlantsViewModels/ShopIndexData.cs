using System.Security.Policy;

namespace PROIECT.Models.PlantsViewModels
{
    public class ShopIndexData
    {
        public IEnumerable<Shop> Shops { get; set; }
        public IEnumerable<Plant> Plants { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
