using BookingServiceApi.Data;
using BookingServiceApi.Interfaces;
using BookingServiceApi.Mappers; 
using BookingServiceApi.Repositories;
using BookingServiceApi.Services; 
using Common.Middleware; 
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//Repositories
builder.Services.AddScoped<IBookingUnitOfWork, BookingUnitOfWork>();
//Services
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddAutoMapper(typeof(BookingProfile));

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

//TODO
//connect to rabbit mq 
