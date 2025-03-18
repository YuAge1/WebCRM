using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebCRM.Application.Abstractions;
using WebCRM.Application.Services;
using WebCRM.Domain;

namespace WebCRM.WebApi.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Orders API",
                    Version = "v1"
                });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
             {
                 {
                     new OpenApiSecurityScheme
                     {
                         Reference = new OpenApiReference
                         {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                         }
                     },
                     Array.Empty<string>()
                 }
             });
            });

            return builder;
        }

        public static WebApplicationBuilder AddData(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<OrdersDbContext>(opt =>
                opt.UseNpgsql(builder.Configuration.GetConnectionString("Orders")));

            return builder;
        }

        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICartsService, CartsService>();
            builder.Services.AddScoped<IOrderService, OrdersService>();

            return builder;
        }

        public static WebApplicationBuilder AddIntegrationServices(this WebApplicationBuilder builder)
        {
            return builder;
        }
    }
}
