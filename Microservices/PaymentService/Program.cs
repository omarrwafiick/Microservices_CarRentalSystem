using PaymentService.Data;
using Common.Middleware;
using Microsoft.EntityFrameworkCore;
using Common.Repositories;
using Common.Interfaces;
using PaymentService.Models;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddControllers();  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IGetRepository<Payment>, GetRepository<Payment>>();
builder.Services.AddScoped<IGetAllRepository<Payment>, GetAllRepository<Payment>>();
builder.Services.AddScoped<IDeleteRepository<Payment>, DeleteRepository<Payment>>();
builder.Services.AddScoped<ICreateRepository<Payment>, CreateRepository<Payment>>();
builder.Services.AddScoped<IUpdateRepository<Payment>, UpdateRepository<Payment>>();

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
