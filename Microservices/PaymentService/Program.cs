using PaymentService.Data;
using Common.Middleware;
using Microsoft.EntityFrameworkCore;
using Common.Repositories;
using Common.Interfaces;
using PaymentService.Models;
using PaymentServiceApi.Interfaces;
using PaymentServiceApi.Enums;
;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddControllers();  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
 
builder.Services.AddScoped<IGetRepository<PaymentsRecord>, GetRepository<ApplicationDbContext, PaymentsRecord>>();
builder.Services.AddScoped<IGetAllRepository<PaymentsRecord>, GetAllRepository<ApplicationDbContext, PaymentsRecord>>();
builder.Services.AddScoped<IDeleteRepository<PaymentsRecord>, DeleteRepository<ApplicationDbContext, PaymentsRecord>>();
builder.Services.AddScoped<ICreateRepository<PaymentsRecord>, CreateRepository<ApplicationDbContext, PaymentsRecord>>();
builder.Services.AddScoped<IUpdateRepository<PaymentsRecord>, UpdateRepository<ApplicationDbContext, PaymentsRecord>>();
builder.Services.AddScoped<IPaymentService, PaymentServiceApi.Services.PaymentService>();
 
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
