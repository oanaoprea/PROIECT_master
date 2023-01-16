using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace PROIECT.Models
{
    public class Plant
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public string Description { get; set; }
        public string? Light { get; set; }
        public string? Water { get; set; }
        public string? Temperature { get; set; }
        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }
        public string Image { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public Category? Category { get; set; }
        public ICollection<AvailablePlant> AvailablePlants { get; set; }

    }
}
