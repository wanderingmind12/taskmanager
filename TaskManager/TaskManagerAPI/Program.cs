using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// --- CORS policy ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

var app = builder.Build();

// --- middleware order ---
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

// --- explicit handler for pre-flight requests ---
app.MapMethods("{*path}", new[] { "OPTIONS" }, (HttpContext ctx) =>
{
    ctx.Response.Headers.Append("Access-Control-Allow-Origin", "*");
    ctx.Response.Headers.Append("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE,OPTIONS");
    ctx.Response.Headers.Append("Access-Control-Allow-Headers", "*");
    return Results.Ok();
});

app.Run();
