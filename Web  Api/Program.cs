using Application;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Web__Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Generic Repositories
builder.Services.AddScoped<IRepository<Domain.User>, GenericRepository<Domain.User>>();
builder.Services.AddScoped<IRepository<Domain.ScrapItem>, GenericRepository<Domain.ScrapItem>>();
builder.Services.AddScoped<IRepository<Domain.Address>, GenericRepository<Domain.Address>>();
builder.Services.AddScoped<IRepository<Admin>, GenericRepository<Admin>>();
builder.Services.AddScoped<IRepository<Notification>, GenericRepository<Notification>>();
builder.Services.AddScoped<IRepository<Payment>, GenericRepository<Payment>>();
builder.Services.AddScoped<IRepository<Pickup>, GenericRepository<Pickup>>();
builder.Services.AddScoped<IRepository<Vehicle>, GenericRepository<Vehicle>>();

// Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ScrapItemService>();
builder.Services.AddScoped<PickupService>();

// SignalR
builder.Services.AddSignalR(); // Register SignalR

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Map the SignalR Hub
app.MapHub<PickupHub>("/pickupHub"); // Map SignalR hub endpoint

app.Run();

