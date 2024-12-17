using BlogManagement.Infrastructure.Configuration;
using CommentManagement.Infrastructure.Configuration;
using Framework.Application;
using ServiceHost;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("BigShopDb");
builder.Services.AddHttpContextAccessor();
BlogManagementDependencyInjection.Configuration(builder.Services, connectionString);
CommentManagementDependencyInjection.Configuration(builder.Services, connectionString);
builder.Services.AddTransient<IAuthHelper, AuthHelper>();
builder.Services.AddTransient<IFileUploader, FileUploader>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
