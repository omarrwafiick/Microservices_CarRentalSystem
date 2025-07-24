using VehicleServiceApi.Mappers; 
using Common.Middleware; 
using Microsoft.EntityFrameworkCore;
using VehicleServiceApi.Data;
using VehicleServiceApi.Interfaces; 
using VehicleServiceApi.Services;
using VehicleServiceApi.Interfaces.UnitOfWork;
using VehicleServiceApi.Repositiories;

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
builder.Services.AddScoped<IVehicleUnitOfWork, VehicleUnitOfWork>(); 
//Vehicle images 
builder.Services.AddScoped<IVehicleImagesUnitOfWork, VehicleImagesUnitOfWork>(); 
//Location 
builder.Services.AddScoped<ILocationUnitOfWork, LocationUnitOfWork>();
//Centers
builder.Services.AddScoped<ICenterUnitOfWork, CenterUnitOfWork>();
//Records
builder.Services.AddScoped<IRecordUnitOfWork, RecordUnitOfWork>();

//Services
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleImagesService, VehicleImagesService>();
builder.Services.AddScoped<IMaintenanceCenterService, MaintenanceCenterService>();
builder.Services.AddScoped<IMaintenanceRecordsService, MaintenanceRecordsService>();
builder.Services.AddSingleton<ConsumeServicesViaBroker>();
builder.Services.AddHostedService<RabbitMqBackgroundService>();

//Mappers
builder.Services.AddAutoMapper(typeof(VehicleProfiles)); 
builder.Services.AddAutoMapper(typeof(VehicleImageProfile)); 
builder.Services.AddAutoMapper(typeof(LocationProfile));
builder.Services.AddAutoMapper(typeof(MaintenanceCenterProfile));
builder.Services.AddAutoMapper(typeof(MaintenanceRecordProfile));

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
