using System.ComponentModel.DataAnnotations;

namespace GastroApp.Models
{
    public class Meal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        //[RegularExpression(@"^[0-9]+([\,][0-9]?[0-9])?$")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public float Price { get; set; }
        //[RegularExpression(@"^[0]?([\,][0-9]?[0-9])|[1]?([\,][0]?[0])?$")]
        public float VATRate { get; set;}
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Order> Orders { get; set; } = new();
        public List<OrderedMeal> OrderedMeals { get; set; } = new();

    }
}
