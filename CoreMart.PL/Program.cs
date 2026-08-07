
using CoreMart.BLL.Repository.Implementation;
using CoreMart.BLL.Repository.Interface;
using CoreMart.DAL.Context;
using CoreMart.DAL.Models;
using CoreMart.PL.DbSeeder;
using CoreMart.PL.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<CoreMartDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            );

            sqlOptions.CommandTimeout(60); // ??? ??? timeout

        }
    )
);



builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Value;
// Add Identity services


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
{
    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);

})
    .AddEntityFrameworkStores<CoreMartDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();



//builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
//{
//    options.SignIn.RequireConfirmedAccount = false;
//    // Configure other options as needed
//})
//    .AddRoles<IdentityRole>() // If you need roles
//    .AddEntityFrameworkStores<CoreMartDbContext>();


//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<CoreMartDbContext>();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IProductRepository,  ProductRepository>();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


var app = builder.Build();

try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<CoreMartDbContext>();
        db.Database.CanConnect(); // ????? ???????
        Console.WriteLine("Database connection successful!");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Database connection failed: {ex.Message}");
    Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
}


// Seed Database with Admin User
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DbSeeder.SeedRolesAndAdminAsync(services);
        Console.WriteLine("Admin user seeded successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
    }
}
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

//StripeConfiguration.ApiKey= builder.Configuration.GetSection("Stripe : SecretKey").Get<string>();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();


app.MapControllerRoute(
    name: "customer",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{area: exits}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "admin",
    pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");




app.Run();
