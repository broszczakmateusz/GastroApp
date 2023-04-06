namespace GastroApp.Models
{
    public class OrderedMeal
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int MealId { get; set; }
        public Meal Meal { get; set; }
        public string? Annotation { get; set;}
        public DateTime CreatedDateTime { get; set; }
    }
}
