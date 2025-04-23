using GatewayPoint.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Middleware; 
using Ocelot.Cache.CacheManager; 

var builder = WebApplication.CreateBuilder(args);
 
builder.Configuration.AddJsonFile("ocelot.json", false, true);

builder.Services.AddOcelot().AddCacheManager(options =>
{
    options.WithDictionaryHandle();
});

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
