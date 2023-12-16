using MongoDB.Driver;
using TinyURL.Controllers;
using TinyURL.Repositories;
using TinyURL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mongo client
builder.Services.AddSingleton<IMongoClient, MongoClient>();

// Services
builder.Services.AddSingleton<TinyUrlService>();

// Repositories
builder.Services.AddSingleton<TinyUrlRepository>();

//
builder.Services.AddSingleton<TinyUrlController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }