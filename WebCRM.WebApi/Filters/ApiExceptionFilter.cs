using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebCRM.Domain.Exceptions;
using WebCRM.Domain.Extensions;
using WebCRM.WebApi.Models;

namespace WebCRM.WebApi.Filters;

public class ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        int statusCode = 400;
        ApiErrorResponse? response;
        
        switch (true)
        {
            case { } when exception is DuplicateEntityException:
            {
                response = new ApiErrorResponse
                {
                    Code = 10, 
                    Message = exception.Message,
                    Description = exception.ToText()
                };
                break;
            }
            case { } when exception is EntityNotFoundException:
            {
                statusCode = 404;
                response = new ApiErrorResponse
                {
                    Code = 20, 
                    Message = exception.Message,
                    Description = exception.ToText()
                };
                break;
            }
            case { } when exception is SoftEntityNotFoundException:
            {
                statusCode = 400;
                response = new ApiErrorResponse
                {
                    Code = 30, 
                    Message = exception.Message,
                    Description = exception.ToText()
                };
                break;
            }
            default:
            {
                response = new ApiErrorResponse
                {
                    Code = 666,
                    Message = exception.Message,
                    Description = exception.ToText()
                };
                break;
            }
        }
        
        logger.LogError($"Api method {context.HttpContext.Request.Path} finished with code {statusCode} and error {JsonSerializer.Serialize(response)}");
        context.Result = new JsonResult(new {response}){StatusCode = statusCode};
    }
}