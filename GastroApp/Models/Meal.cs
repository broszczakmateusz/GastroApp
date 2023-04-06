namespace GastroApp.Models
{
    public class Meal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public double VATRate { get; set;}
        public int CategoryId { get; set; }
    }
}
