using LBHTenancyAPI.Extensions.Controller;
using LBHTenancyAPI.Infrastructure.UseCase.Execution;
using LBHTenancyAPI.UseCases.ArrearsAgreements;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult HandleResponse<T>( IExecuteWrapper<T> result) where T: class
        {
            return this.StandardResponse(result);
        }
    }
}
