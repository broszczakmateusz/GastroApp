using GastroApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GastroApp.Data
{
    public class GastroAppContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderedMeal> OrderedMeals { get; set; }
        public GastroAppContext(DbContextOptions<GastroAppContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(eb =>
            {
                eb.HasKey(u => u.Id);
                eb.Property(u => u.FirstName).IsRequired();
                eb.Property(u => u.LastName).IsRequired();
            });
            builder.Entity<Room>(eb =>
            {
                eb.Property(r => r.Name).IsRequired();
            });
            builder.Entity<Table>(eb =>
            {
                eb.HasKey(t => t.Id);
                eb.Property(t => t.Name).IsRequired();
                eb.Property(t => t.RoomId).IsRequired();
                eb.Property(t => t.NumberOfSeats).IsRequired();
                eb.Property(t => t.IsTaken).HasDefaultValue(false);
            });
            builder.Entity<Category>(eb =>
            {
                eb.HasKey(c => c.Id);
                eb.Property(c => c.Name).IsRequired();
            });
            builder.Entity<Meal>(eb =>
            {
                eb.HasKey(m => m.Id);
                eb.Property(m => m.Name).IsRequired();
                eb.Property(m => m.Price).IsRequired().HasPrecision(7, 2);
                eb.Property(m => m.VATRate).IsRequired().HasPrecision(5, 2);
                eb.Property(m => m.CategoryId).IsRequired();
            });
            builder.Entity<Order>(eb =>
            {
                eb.HasKey(o => o.Id);
                eb.Property(o => o.TableId).IsRequired();
                eb.Property(o => o.UserId).IsRequired();
                eb.Property(o => o.CreatedDateTime).IsRequired().HasDefaultValue(DateTime.Now);
                eb.Property(o => o.IsPaid).HasDefaultValue(false);
                eb.Property(o => o.TotalPrice).HasDefaultValue(0).HasPrecision(7, 2);
            });
            builder.Entity<OrderedMeal>(eb => 
            {
                eb.HasKey(om => new { om.OrderId, om.MealId });
                eb.Property(om => om.Annotation).HasMaxLength(255);
                eb.Property(om => om.CreatedDateTime).IsRequired().HasDefaultValue(DateTime.Now);
            });
        }
    }
}
