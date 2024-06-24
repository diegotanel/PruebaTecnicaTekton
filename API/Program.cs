using ApplicationServices;
using Data;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Shared.Middleware;
using System.Reflection;
using Shared.External;
using Microsoft.OpenApi.Models;
using Shared.Cache;
using Shared.Configs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var info = new OpenApiInfo()
{
    Title = "API Producto",
    Version = "v1",
    Description = "API Producto para prueba técnica",
    Contact = new OpenApiContact()
    {
        Name = "Diego Tanel",
        Email = "dtanel@gmail.com",
    }

};
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", info);
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProductContext>(o => o.UseSqlite(conn));

builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IApiExterna, MockApi>();
builder.Services.AddScoped<ICache, WrapperLazyCache>();
builder.Services.AddLazyCache();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(ProductService))));
MockApiUrl.Url = builder.Configuration.GetValue<string>("MockApiUrl:Url");
CacheConfig.Expiration = builder.Configuration.GetValue<int>("CacheConfig:Expiration");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    ProductContext context = scope.ServiceProvider.GetRequiredService<ProductContext>();
    context.Database.EnsureCreated();
}

app.UseMiddleware<RequestTimingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
