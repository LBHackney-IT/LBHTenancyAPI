using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Extensions.Validation;
using LBHTenancyAPI.UseCases.V2.ArrearsActions;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers.V2
{
    [ApiVersion("2")]
    [Produces("application/json")]
    [Route("api/v{apiVersion:apiVersion}/tenancies/arrears-action-diary/", Name = "ArrearsActionDiaryV2")]
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
