using LBHTenancyAPI.Extensions.Controller;
using LBHTenancyAPI.Infrastructure.V1.UseCase.Execution;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult HandleResponse<T>( IExecuteWrapper<T> result) where T: class
        {
            return this.ExecuteStandardResponse(result);
        }

        public IActionResult HandleResponse<T>(T result) where T : class
        {
            return this.StandardResponse(result);
        }
    }
}
