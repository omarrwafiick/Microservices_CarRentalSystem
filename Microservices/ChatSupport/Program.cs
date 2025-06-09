using ChatSupportApi.Data;
using ChatSupportApi.Hubs;
using ChatSupportApi.Interfaces;
using ChatSupportApi.Models;
using ChatSupportApi.Services;
using Common.Interfaces;
using Common.Middleware;
using Common.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
 
builder.Services.AddScoped<IGetRepository<ChatMessage>, GetRepository<ApplicationDbContext, ChatMessage>>();
builder.Services.AddScoped<IGetAllRepository<ChatMessage>, GetAllRepository<ApplicationDbContext, ChatMessage>>();
builder.Services.AddScoped<ICreateRepository<ChatMessage>, CreateRepository<ApplicationDbContext, ChatMessage>>();
builder.Services.AddScoped<ICreateRepository<Chat>, CreateRepository<ApplicationDbContext, Chat>>();
builder.Services.AddScoped<IGetRepository<Chat>, GetRepository<ApplicationDbContext, Chat>>();
builder.Services.AddScoped<IGetAllRepository<Chat>, GetAllRepository<ApplicationDbContext, Chat>>();
builder.Services.AddScoped<IChatMessageService, ChatMessageService>();

builder.Services.AddSignalR();

builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

app.UseHttpsRedirection();

app.MapHub<ChatHub>("/chathub");

app.UseMiddleware<RestrictAccessMiddleware>();

app.MapControllers();

app.Run();
