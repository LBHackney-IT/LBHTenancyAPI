using System.Threading.Tasks;
using LBHTenancyAPI.Extensions.Controller;
using LBHTenancyAPI.Infrastructure.V1.API;
using LBHTenancyAPI.UseCases.Service;
using LBHTenancyAPI.UseCases.V1.Service;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers.V1
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(APIResponse<object>), 400)]
    [ProducesResponseType(typeof(APIResponse<object>), 500)]
    public class ServiceController : BaseController
    {
        private readonly IGetServiceDetailsUseCase _serviceDetailsUseCase;

        public ServiceController(IGetServiceDetailsUseCase serviceDetailsUseCase)
        {
            _serviceDetailsUseCase = serviceDetailsUseCase;
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<GetServiceDetailsResponse>), 200)]
        [Route("/")]
        public async Task<IActionResult> Get()
        {
            var response = await _serviceDetailsUseCase.ExecuteAsync(Request.GetCancellationToken()).ConfigureAwait(false);
            return HandleResponse(response);
        }
    }
}
