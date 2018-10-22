using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LBHTenancyAPI.Extensions.Controller;
using LBHTenancyAPI.Infrastructure.API;
using LBHTenancyAPI.UseCases.V1.ArrearsAgreements;
using LBHTenancyAPI.UseCases.V1.ArrearsAgreements.Models;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers.V1
{
    [Produces("application/json")]
    [Route("api/v1/tenancies/arrears-agreement/")]
    [ProducesResponseType(typeof(APIResponse<object>), 400)]
    [ProducesResponseType(typeof(APIResponse<object>), 500)]
    public class ArrearsAgreementController : BaseController
    {
        private readonly ICreateArrearsAgreementUseCase _createArrearsAgreementUseCase;

        public ArrearsAgreementController(ICreateArrearsAgreementUseCase createArrearsAgreementUseCase)
        {
            _createArrearsAgreementUseCase = createArrearsAgreementUseCase;
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<CreateArrearsAgreementResponse>), 200)]
        public async Task<IActionResult> Post([FromBody][Required]CreateArrearsAgreementRequest request)
        {
            var result = await _createArrearsAgreementUseCase.ExecuteAsync(request, HttpContext.GetCancellationToken()).ConfigureAwait(false);

            return HandleResponse(result);
        }
    }
}
