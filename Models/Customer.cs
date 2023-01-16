namespace PROIECT.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Nume { get; set; }    
        public string Address { get; set; } 
        public DateTime? BirthDate { get; set; } 
        public ICollection<Order>? Orders { get; set; }
    }
}
