using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Intex.Data;
using Intex.Areas.Identity.Data;
using Intex.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
string ClientId = Environment.GetEnvironmentVariable("Authentication:Google:ClientId");
string ClientSecret = Environment.GetEnvironmentVariable("Authentication:Google:ClientSecret");



builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ILegoRepository, EFLegoRepository>();


builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = ClientId;
    googleOptions.ClientSecret = ClientSecret;
});

// Enable runtime compilation for Razor pages
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{pageNum?}/{category?}");

    endpoints.MapRazorPages(); // Include this line to enable Razor Pages

  
});
app.Run();