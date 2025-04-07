﻿using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using WebCRM.Domain;
using WebCRM.WebApi.Extensions;
using WebCRM.WebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpLogging(opt =>
{
    opt.LoggingFields = HttpLoggingFields.RequestBody | HttpLoggingFields.RequestHeaders |
                        HttpLoggingFields.Duration | HttpLoggingFields.RequestPath |
                        HttpLoggingFields.ResponseBody | HttpLoggingFields.ResponseHeaders;
});

builder
    .AddBearerAuthentication()
    .AddOptions()
    .AddSwagger()
    .AddData()
    .AddApplicationServices()
    .AddIntegrationServices()
    .AddBackgroundService();

var app = builder.Build();

app.UseHttpLogging();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();