using VehicleServiceApi.Mappers;
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
//Repositories

//Vehicle
builder.Services.AddScoped<IGetRepository<Vehicle>, GetRepository<ApplicationDbContext, Vehicle>>();
builder.Services.AddScoped<IGetAllRepository<Vehicle>, GetAllRepository<ApplicationDbContext, Vehicle>>();
builder.Services.AddScoped<IDeleteRepository<Vehicle>, DeleteRepository<ApplicationDbContext, Vehicle>>();
builder.Services.AddScoped<ICreateRepository<Vehicle>, CreateRepository<ApplicationDbContext, Vehicle>>();
builder.Services.AddScoped<IUpdateRepository<Vehicle>, UpdateRepository<ApplicationDbContext, Vehicle>>();

//VehicleImages
builder.Services.AddScoped<IGetAllRepository<VehicleImages>, GetAllRepository<ApplicationDbContext, VehicleImages>>();
builder.Services.AddScoped<IDeleteRepository<VehicleImages>, DeleteRepository<ApplicationDbContext, VehicleImages>>();
builder.Services.AddScoped<ICreateRepository<VehicleImages>, CreateRepository<ApplicationDbContext, VehicleImages>>();

//Location
builder.Services.AddScoped<IGetRepository<Location>, GetRepository<ApplicationDbContext, Location>>();
builder.Services.AddScoped<ICreateRepository<Location>, CreateRepository<ApplicationDbContext, Location>>();
builder.Services.AddScoped<IDeleteRepository<Location>, DeleteRepository<ApplicationDbContext, Location>>();

//Services
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleImagesService, VehicleImagesService>();
builder.Services.AddScoped<IMaintenanceCenterService, MaintenanceCenterService>();
builder.Services.AddScoped<IMaintenanceRecordsService, MaintenanceRecordsService>();

//Mappers
builder.Services.AddAutoMapper(typeof(VehicleProfiles)); 
builder.Services.AddAutoMapper(typeof(VehicleImageProfile));

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
