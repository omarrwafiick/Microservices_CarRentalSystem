using AuthenticationApi.Data;
using AuthenticationApi.Models;
using AuthenticationApi.Repositories;
using Common.Interfaces;
using Common.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AuthSection:Key").Value!);
        var issuer = builder.Configuration.GetSection("AuthSection:Issuer").Value!;
        var audience = builder.Configuration.GetSection("AuthSection:Audience").Value!;
        options.RequireHttpsMetadata = false;
        options.SaveToken = true; 
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = audience,
            ValidIssuer = issuer,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IGetRepository<User>, GetRepository>();
//todos
//builder.Services.AddScoped<IGetAllRepository<User>, GetRepository>();
//builder.Services.AddScoped<IDeleteRepository<User>, GetRepository>();
//builder.Services.AddScoped<ICreateRepository<User>, GetRepository>();
//builder.Services.AddScoped<IUpdateRepository<User>, GetRepository>();

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
