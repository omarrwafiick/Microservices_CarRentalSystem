using PaymentService.Data;
using Common.Middleware;
using Microsoft.EntityFrameworkCore;
using Common.Repositories;
using Common.Interfaces;
using PaymentService.Models;
using PaymentServiceApi.Interfaces;
using BookingServiceApi.Mappers;
using PaymentServiceApi.Repositories;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddControllers();  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//Repositories
builder.Services.AddScoped<IPaymentUnitOfWork, PaymentUnitOfWork>();
//Services
builder.Services.AddScoped<IPaymentService, PaymentServiceApi.Services.PaymentService>();
builder.Services.AddAutoMapper(typeof(PaymentRecordProfile));
builder.Services.AddMemoryCache();

//logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

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
