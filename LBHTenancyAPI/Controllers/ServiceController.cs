using System.Threading.Tasks;
using LBHTenancyAPI.Controllers.V1;
using LBHTenancyAPI.Extensions.Controller;
using LBHTenancyAPI.UseCases.Service;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers
{
    public class ServiceController : BaseController
    {
        private readonly IGetServiceDetailsUseCase _serviceDetailsUseCase;

        public ServiceController(IGetServiceDetailsUseCase serviceDetailsUseCase)
        {
            _serviceDetailsUseCase = serviceDetailsUseCase;
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Get()
        {
            var response = await _serviceDetailsUseCase.ExecuteAsync(Request.GetCancellationToken()).ConfigureAwait(false);
            return HandleResponse(response);
        }
    }
}
