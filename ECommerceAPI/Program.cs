using Microsoft.EntityFrameworkCore;
using FluentValidation;
using ECommerceAPI.Data;
using ECommerceAPI.Repositories;
using ECommerceAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Entity Framework configuration
builder.Services.AddDbContext<ECommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository pattern registration
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// FluentValidation registration
builder.Services.AddScoped<CreateCustomerValidator>();
builder.Services.AddScoped<CreateOrderValidator>();
builder.Services.AddScoped<UpdateOrderStatusValidator>();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ECommerceContext>();
    await DbSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
