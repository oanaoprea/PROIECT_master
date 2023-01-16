namespace PROIECT.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public ICollection<Plant>? Plants { get; set; }
    }
}
