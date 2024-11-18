using BlogManagement.Infrastructure.Configuration;
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
}

builder.Services.AddTransient<IFileUploader, FileUploader>();
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
