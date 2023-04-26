using System.ComponentModel.DataAnnotations;

namespace GastroApp.Models
{
    public class OrderedMeal
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int MealId { get; set; }
        public Meal Meal { get; set; }
        public string? Annotation { get; set; }
        [UIHint("LocalDate")]
        public DateTime CreatedDateTime { get; set; }
        public OrderedMeal() { }
        public OrderedMeal(int orderId, Order order, int mealId, Meal meal)
        {
            OrderId = orderId;
            Order = order;
            MealId = mealId;
            Meal = meal;
            CreatedDateTime = DateTime.UtcNow;
        }
}



}
