using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace GastroApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        [UIHint("LocalDate")]
        public DateTime CreatedDateTime { get; set; }
        [UIHint("LocalDate")]
        public DateTime UpdatedDateTime { get; set; }
        [UIHint("LocalDate")]
        public DateTime? PaidDateTime { get; set; }
        public bool IsPaid { get; set; }
        public int? PaymentMethodId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public float TotalPrice { get; set; }
        public List<Meal> Meals { get; set; } = new List<Meal>();

        public void SetAsPaid(PaymentMethod paymentMethod)
        {
            PaidDateTime = DateTime.UtcNow;
            PaymentMethodId = paymentMethod.Id;
            PaymentMethod = paymentMethod;
            IsPaid = true;
        }
        public void SetUserANdTableForNew(User user, Table table)
        {
            UserId = user.Id;
            User = user;
            Table = table;
        }
    }
}