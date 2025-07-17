using BookingServiceApi.Data;
using BookingServiceApi.Interfaces;
using BookingServiceApi.Mappers;
using BookingServiceApi.Models;
using BookingServiceApi.Services;
using Common.Interfaces;
using Common.Middleware;
using Common.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IGetRepository<Booking>, GetRepository<ApplicationDbContext, Booking>>(); 
builder.Services.AddScoped<IGetAllRepository<Booking>, GetAllRepository<ApplicationDbContext, Booking>>();
builder.Services.AddScoped<IDeleteRepository<Booking>, DeleteRepository<ApplicationDbContext, Booking>>();
builder.Services.AddScoped<ICreateRepository<Booking>, CreateRepository<ApplicationDbContext, Booking>>();
builder.Services.AddScoped<IUpdateRepository<Booking>, UpdateRepository<ApplicationDbContext, Booking>>();
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
