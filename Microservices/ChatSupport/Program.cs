using ChatSupportApi.Data;
using ChatSupportApi.Hubs;
using ChatSupportApi.Interfaces;
using ChatSupportApi.Repositiories;
using ChatSupportApi.Services; 
using Common.Middleware; 
using Microsoft.AspNetCore.SignalR;
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
builder.Services.AddScoped<IChatUnitOfWork, ChatUnitOfWork>();
//Servides
builder.Services.AddScoped<IChatMessageService, ChatMessageService>();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

var app = builder.Build();
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHub<ChatHub>("/chathub");

app.UseMiddleware<RestrictAccessMiddleware>();

app.MapControllers();

app.Run();
