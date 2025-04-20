using Common.Interfaces;
using Common.Middleware;
using Common.Repositories;
using Microsoft.EntityFrameworkCore;
using VehicleServiceApi.Data;
using VehicleServiceApi.Interfaces;
using VehicleServiceApi.Models;
using VehicleServiceApi.Services;

var builder = WebApplication.CreateBuilder(args);
  
builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IGetRepository<Vehicle>, GetRepository<Vehicle>>();
builder.Services.AddScoped<IGetAllRepository<Vehicle>, GetAllRepository<Vehicle>>();
builder.Services.AddScoped<IDeleteRepository<Vehicle>, DeleteRepository<Vehicle>>();
builder.Services.AddScoped<ICreateRepository<Vehicle>, CreateRepository<Vehicle>>();
builder.Services.AddScoped<IUpdateRepository<Vehicle>, UpdateRepository<Vehicle>>();

var app = builder.Build();
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<RestrictAccessMiddleware>();

app.MapControllers();

app.Run();
