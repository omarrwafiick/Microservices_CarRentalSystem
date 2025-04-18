using GatewayPoint.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddEndpointsApiExplorer(); 

builder.Configuration.AddJsonFile("ocelot.json", false, true);

builder.Services.AddOcelot();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build(); 

app.UseCors();

app.UseHttpsRedirection();

app.UseMiddleware<InterceptMiddleware>();

app.UseMiddleware<TokenCheckerMiddleware>();

app.UseAuthorization();

app.UseOcelot().Wait();
  
app.Run();
