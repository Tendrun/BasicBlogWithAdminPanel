using BasicBlogWithAdminPanel.Data;
using BasicBlogWithAdminPanel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// ----------  DB + Identity ----------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("DefaultConnection not found");
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(opts =>
{
    opts.SignIn.RequireConfirmedAccount = false;
    opts.Password.RequireDigit = true;
    opts.Password.RequireLowercase = true;
    opts.Password.RequireUppercase = true;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequiredLength = 6;
}).AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>();

// ----------  Localisation ----------
builder.Services.AddLocalization(o => o.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();
builder.Services.AddRazorPages()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();

var supportedCultures = new[] { "en", "pl" };
builder.Services.Configure<RequestLocalizationOptions>(opts =>
{
    opts.SetDefaultCulture("en")
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
    // allow culture switching via ?culture=xx
    opts.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
});

// ----------  Misc ----------
builder.Services.AddSession();

var app = builder.Build();

// ----------  Pipeline ----------
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// MVC + Razor Pages
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
