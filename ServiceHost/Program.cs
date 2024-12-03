using System.Text.Encodings.Web;
using System.Text.Unicode;
using AccountManagement.Infrastructure.Configuration;
using BlogManagement.Infrastructure.Configuration;
using CommentManagement.Infrastructure.Configuration;
using DiscountManagement.infrastructure.Configuration;
using Framework.Application;
using InventoryManagement.Infrastructure.Configuration;
using ServiceHost;
using ShopManagement.infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

builder.Services.AddTransient<IFileUploader, FileUploader>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configuration the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
