namespace GastroApp.Models
{
    public class OrderedMeal
    {
        public int OrderId { get; set; }
        public int MealId { get; set; }
        public string? Annotation { get; set;}
        public DateTime CreatedDateTime { get; set; }
    }
}
