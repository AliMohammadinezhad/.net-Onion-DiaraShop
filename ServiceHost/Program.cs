using AccountManagement.Infrastructure.Configuration;
using BlogManagement.Infrastructure.Configuration;
using CommentManagement.Infrastructure.Configuration;
using DiscountManagement.infrastructure.Configuration;
using Framework.Application;
using Framework.Application.ZarinPal;
using Framework.Infrastructure;
using InventoryManagement.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using ServiceHost;
using ShopManagement.infrastructure.Configuration;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using InventoryManagement.Presentation.Api;
using ShopManagement.Presentation.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("BigShopDb");

if (connectionString != null)
{
    ShopManagementDependencyInjection.Configuration(builder.Services, connectionString);
    DiscountManagementDependencyInjection.Configuration(builder.Services, connectionString);
    InventoryManagementDependencyInjection.Configuration(builder.Services, connectionString);
    BlogManagementDependencyInjection.Configuration(builder.Services, connectionString);
    CommentManagementDependencyInjection.Configuration(builder.Services, connectionString);
    AccountManagementDependencyInjection.Configuration(builder.Services, connectionString);
}

builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic));

builder.Services.AddTransient<IAuthHelper, AuthHelper>();
builder.Services.AddTransient<IFileUploader, FileUploader>();
builder.Services.AddTransient<IZarinPalFactory, ZarinPalFactory>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddRazorPages();


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = new PathString("/Account");
        options.LogoutPath = new PathString("/Account");
        options.AccessDeniedPath = new PathString("/AccessDenied");
    });

builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminArea", policy => policy.RequireRole([Roles.Admin, Roles.ContentCreator]));
        options.AddPolicy("Shop", policy => policy.RequireRole([Roles.Admin]));
        options.AddPolicy("Discount", policy => policy.RequireRole([Roles.Admin]));
        options.AddPolicy("Account", policy => policy.RequireRole([Roles.Admin]));
    }
);

builder.Services.AddRazorPages()
    .AddMvcOptions(options => options.Filters.Add<SecurityPageFilter>())
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AuthorizeAreaFolder("Administration", "/", "AdminArea");
        options.Conventions.AuthorizeAreaFolder("Administration", "/Shop", "Shop");
        options.Conventions.AuthorizeAreaFolder("Administration", "/Discounts", "Discount");
        options.Conventions.AuthorizeAreaFolder("Administration", "/Accounts", "Account");
    })
    .AddApplicationPart(typeof(ProductController).Assembly)
    .AddApplicationPart(typeof(InventoryController).Assembly);


var app = builder.Build();

// Configuration the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
