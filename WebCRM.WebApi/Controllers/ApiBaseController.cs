using Microsoft.AspNetCore.Mvc;
using WebCRM.WebApi.Filters;

namespace WebCRM.WebApi.Controllers
{
    [ApiController]
    // [TypeFilter(typeof(ApiExceptionFilter))]
    [TypeFilter<ApiExceptionFilter>]
    public class ApiBaseController : ControllerBase
    {

    }
}
