using DAL.EF;
using Microsoft.EntityFrameworkCore;
using DAL.Repos;
using BLL.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache(); // Required for session 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session expiration 
    options.Cookie.HttpOnly = true;                // Prevent JavaScript access 
    options.Cookie.IsEssential = true;             // GDPR compliance 
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<UserTypeRepo>();
builder.Services.AddScoped<UserTypeService>();
builder.Services.AddScoped<UserRepo>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MenuItemRepo>();
builder.Services.AddScoped<MenuItemService>();
builder.Services.AddScoped<MealBookingRepo>();
builder.Services.AddScoped<MealBookingService>();
builder.Services.AddScoped<WalletTransactionRepo>();
builder.Services.AddScoped<WalletTransactionService>();
builder.Services.AddScoped<SystemLogRepo>();
builder.Services.AddScoped<SystemLogService>();

builder.Services.AddDbContext<CafeteriaDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CafeteriaDbContext")
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
