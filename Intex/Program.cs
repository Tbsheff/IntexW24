using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Intex.Data;
using Intex.Services;
using Intex.Models;
using Intex.Middleware;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration["DefaultConnection"] ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var clientId = builder.Configuration["Authentication:Google:ClientId"];
var clientSecret = builder.Configuration["Authentication:Google:ClientSecret"];


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    // Existing configuration: require confirmed account
    options.SignIn.RequireConfirmedAccount = true;

    // Additional configuration: stronger password policy
    options.Password.RequireDigit = true; // Require at least one digit
    options.Password.RequireLowercase = true; // Require at least one lowercase letter
    options.Password.RequireUppercase = true; // Require at least one uppercase letter
    options.Password.RequireNonAlphanumeric = true; // Require at least one non-alphanumeric character
    options.Password.RequiredLength = 12; // Require at least 12 characters
    options.Password.RequiredUniqueChars = 6; // Require at least 6 unique characters
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();




//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ILegoRepository, EFLegoRepository>();

builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365); // Set the max age to 365 days, adjust as needed
});

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = clientId;
    googleOptions.ClientSecret = clientSecret;
});

// Enable runtime compilation for Razor pages
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);





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
app.UseCartItemCount();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; " +
                                                            "style-src 'self' intex-gtgzecb5avarh7gg.z01.azurefd.net fonts.cdnfonts.com fonts.googleapis.com 'unsafe-inline';" +
                                                            "font-src 'self' intex-gtgzecb5avarh7gg.z01.azurefd.net fonts.cdnfonts.com fonts.googleapis.com fonts.gstatic.com cdn.linearicons.com; " +
                                                            "script-src 'self' intex-gtgzecb5avarh7gg.z01.azurefd.net ajax.googleapis.com https://www.chatbase.co code.jquery.com www.google.com www.gstatic.com https://js.monitor.azure.com/ 'unsafe-inline';" +
                                                            "frame-src 'self' intex-gtgzecb5avarh7gg.z01.azurefd.net www.google.com https://www.chatbase.co; " + // Added www.google.com
                                                            "img-src 'self' m.media-amazon.com https://www.lego.com intex-gtgzecb5avarh7gg.z01.azurefd.net brickset.com https://www.brickeconomy.com images.brickset.com i.pinimg.com data:; " +
                                                            "connect-src *;");
    await next.Invoke();
});

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{pageSize?}/{pageNum?}/{category?}/{primaryColor?}/{secondaryColor?}");

    endpoints.MapControllerRoute(
        name: "editUser",
        pattern: "Admin/{action=EditUser}/{id?}");

    endpoints.MapRazorPages(); // Include this line to enable Razor Pages

  
});
app.Run();