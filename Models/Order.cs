namespace PROIECT.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int PlantID { get; set; }
        public int? Number { get; set; }
        public DateTime? OrderDate { get; set; }
        public Customer? Customer { get; set; }
        public Plant? Plant { get; set; }

    }
}
