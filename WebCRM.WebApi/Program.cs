using Microsoft.EntityFrameworkCore;
using WebCRM.Domain;
using WebCRM.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder
    .AddSwagger()
    .AddData()
    .AddApplicationServices()
    .AddIntegrationServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();