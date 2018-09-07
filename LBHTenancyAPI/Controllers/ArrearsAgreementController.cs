using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LBHTenancyAPI.Extensions.Validation;
using LBHTenancyAPI.Infrastructure.API;
using LBHTenancyAPI.UseCases.ArrearsActions;
using LBHTenancyAPI.UseCases.ArrearsAgreements;

namespace LBHTenancyAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/tenancies/arrears-agreement/")]
    public class ArrearsAgreementController : Controller
    {
        private readonly ICreateArrearsAgreementUseCase _createArrearsAgreementUseCase;

        public ArrearsAgreementController(ICreateArrearsAgreementUseCase createArrearsAgreementUseCase)
        {
            _createArrearsAgreementUseCase = createArrearsAgreementUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody][Required]CreateArrearsAgreementRequest request)
        {
            var result = await _createArrearsAgreementUseCase.ExecuteAsync(request, HttpContext.RequestAborted).ConfigureAwait(false);

            var response = new APIResponse(result);

            return StatusCode(response.StatusCode, response);
        }
    }
}
