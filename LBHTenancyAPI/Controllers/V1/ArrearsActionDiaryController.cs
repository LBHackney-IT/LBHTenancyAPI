using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Extensions.Validation;
using LBHTenancyAPI.UseCases.V1.ArrearsActions;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers.V1
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v1/tenancies/arrears-action-diary/")]
    public class ArrearsActionDiaryController : Controller
    {
        private readonly ICreateArrearsActionDiaryUseCase _createArrearsActionDiaryUseCase;

        public ArrearsActionDiaryController (ICreateArrearsActionDiaryUseCase createArrearsActionDiaryUseCase)
        {
            _createArrearsActionDiaryUseCase = createArrearsActionDiaryUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody][Required] ArrearsActionCreateRequest request)
        {
            if (!request.IsValid())
                return BadRequest();

            var response = await _createArrearsActionDiaryUseCase.ExecuteAsync(request);

            if (!response.Success)
                return StatusCode(500, response);
            return Ok(response);
        }
    }
}
