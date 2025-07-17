using PaymentService.Data;
using Common.Middleware;
using Microsoft.EntityFrameworkCore;
using Common.Repositories;
using Common.Interfaces;
using PaymentService.Models;
using PaymentServiceApi.Interfaces;
using BookingServiceApi.Mappers;
;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddControllers();  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
 
builder.Services.AddScoped<IGetRepository<PaymentRecord>, GetRepository<ApplicationDbContext, PaymentRecord>>();
builder.Services.AddScoped<IGetAllRepository<PaymentRecord>, GetAllRepository<ApplicationDbContext, PaymentRecord>>();
builder.Services.AddScoped<IDeleteRepository<PaymentRecord>, DeleteRepository<ApplicationDbContext, PaymentRecord>>();
builder.Services.AddScoped<ICreateRepository<PaymentRecord>, CreateRepository<ApplicationDbContext, PaymentRecord>>();
builder.Services.AddScoped<IUpdateRepository<PaymentRecord>, UpdateRepository<ApplicationDbContext, PaymentRecord>>();
builder.Services.AddScoped<IPaymentService, PaymentServiceApi.Services.PaymentService>();

builder.Services.AddAutoMapper(typeof(PaymentRecordProfile));

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
