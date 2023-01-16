using System.ComponentModel.DataAnnotations;

namespace PROIECT.Models.PlantsViewModels
{
    public class OrderGroup
    {
        [DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }
        public int PlantCount { get; set; }
    }
}
