using BookingServiceApi.Data;
using BookingServiceApi.Models;
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

builder.Services.AddScoped<IGetRepository<Booking>, GetRepository<Booking>>();
builder.Services.AddScoped<IGetAllRepository<Booking>, GetAllRepository<Booking>>();
builder.Services.AddScoped<IDeleteRepository<Booking>, DeleteRepository<Booking>>();
builder.Services.AddScoped<ICreateRepository<Booking>, CreateRepository<Booking>>();
builder.Services.AddScoped<IUpdateRepository<Booking>, UpdateRepository<Booking>>();

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
