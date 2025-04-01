using Api;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

builder.Services.AddExceptionHandler<ExceptionHandler>(); 
builder.Services.AddProblemDetails();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

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

app.UseExceptionHandler();

app.Run();

//This is due to changed in the Program.cs file in .NET 6
//It's done as suggested in .NetCore wiki https://learn.microsoft.com/fr-fr/aspnet/core/test/integration-tests?view=aspnetcore-9.0
public partial class Program{}