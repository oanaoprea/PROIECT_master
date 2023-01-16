using System.ComponentModel.DataAnnotations;

namespace PROIECT.Models
{
    public class Shop
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Shop Name")]
        [StringLength(50)]
        public string ShopName { get; set; }
        [StringLength(70)]
        public string Adress { get; set; }
        public ICollection<AvailablePlant> AvailablePlants { get; set; }
    }
}
