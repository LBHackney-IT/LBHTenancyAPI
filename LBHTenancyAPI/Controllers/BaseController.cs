using LBHTenancyAPI.Extensions.Controller;
using LBHTenancyAPI.UseCases.ArrearsAgreements;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult HandleResponse<T>( IExecuteWrapper<T> result)
        {
            return this.StandardResponse(result);
        }
    }
}
