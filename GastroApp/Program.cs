using GastroApp.Data;
using GastroApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GastroAppContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<GastroAppContext>();

builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<GastroAppContext>();

builder.Services.AddAuthorization(options =>
{   
    options.AddPolicy("RequireAdminRole",
         policy => policy.RequireRole("Admin"));

    options.AddPolicy("RequireRestaurantManagerRole",
    policy => policy.RequireRole("RestaurantManager"));

    options.AddPolicy("RequireWaiterRole",
        policy => policy.RequireRole("Waiter"));

    options.AddPolicy("RequireWaiterManagerRole",
        policy => policy.RequireRole("WaiterManager"));

    options.AddPolicy("RequireChiefRole",
        policy => policy.RequireRole("Chief"));


});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
