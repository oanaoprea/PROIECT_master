using System.Security.Policy;

namespace PROIECT.Models
{
    public class AvailablePlant
    {
        public int ShopID { get; set; }
        public int PlantID { get; set; }
        public Shop? Shop { get; set; }
        public Plant? Plant { get; set; }
    }
}
