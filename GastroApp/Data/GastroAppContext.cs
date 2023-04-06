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

                eb.HasOne(r => r.Room)
                .WithMany(t => t.Tables)
                .HasForeignKey(t => t.RoomId);
            });
            builder.Entity<Category>(eb =>
            {
                eb.HasKey(c => c.Id);
                eb.Property(c => c.Name).IsRequired();

                eb.HasMany(c => c.Meals)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId);
            });
            builder.Entity<Meal>(eb =>
            {
                eb.HasKey(m => m.Id);
                eb.Property(m => m.Name).IsRequired();
                eb.Property(m => m.Price).IsRequired().HasPrecision(7, 2);
                eb.Property(m => m.VATRate).IsRequired().HasPrecision(5, 2);
                eb.Property(m => m.CategoryId).IsRequired();

                //eb.HasOne(m => m.Category)
                //.WithMany(m => m.Meals)
                //.HasForeignKey(m => m.CategoryId);
            });
            builder.Entity<Order>(eb =>
            {
                eb.HasKey(o => o.Id);
                eb.Property(o => o.TableId).IsRequired();
                eb.Property(o => o.UserId).IsRequired();
                eb.Property(o => o.CreatedDateTime).IsRequired().HasDefaultValue(DateTime.UtcNow);
                eb.Property(o => o.IsPaid).HasDefaultValue(false);
                eb.Property(o => o.TotalPrice).HasDefaultValue(0).HasPrecision(7, 2);

                eb.HasOne(o => o.Table)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.TableId);

                eb.HasOne(o => o.User)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.UserId);

                eb.HasMany(o => o.Meals)
                .WithMany(m => m.Orders)
                .UsingEntity<OrderedMeal>(
                    o => o.HasOne(om => om.Meal)
                    .WithMany()
                    .HasForeignKey(om => om.MealId),

                    o => o.HasOne(om => om.Order)
                    .WithMany()
                    .HasForeignKey(om => om.OrderId),

                    om =>
                    {
                        om.HasKey(x => new { x.OrderId, x.MealId });
                        om.Property(x => x.Annotation).HasMaxLength(255);
                        om.Property(x => x.CreatedDateTime).IsRequired().HasDefaultValue(DateTime.UtcNow);
                    }
                    );
            });
        }
    }
}
