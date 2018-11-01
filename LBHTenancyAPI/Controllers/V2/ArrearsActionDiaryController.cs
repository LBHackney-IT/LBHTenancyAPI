using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Controllers.V1;
using LBHTenancyAPI.Extensions.Validation;
using LBHTenancyAPI.Infrastructure.V1.API;
using LBHTenancyAPI.UseCases.V1.ArrearsActions;
using LBHTenancyAPI.UseCases.V2.Search.Models;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers.V2
{
    //Do not copy this pattern please refer to SearchV2 Controller
    [ApiVersion("2")]
    [Produces("application/json")]
    [Route("api/v{apiVersion:ApiVersion}/tenancies/arrears-action-diary/")]
    [ProducesResponseType(typeof(APIResponse<object>), 400)]
    [ProducesResponseType(typeof(APIResponse<object>), 500)]
    public class ArrearsActionDiaryController : BaseController
    {
        private readonly ICreateArrearsActionDiaryUseCase _createArrearsActionDiaryUseCase;

        public ArrearsActionDiaryController (ICreateArrearsActionDiaryUseCase createArrearsActionDiaryUseCase)
        {
            _createArrearsActionDiaryUseCase = createArrearsActionDiaryUseCase;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<ArrearsActionResponse>), 200)]
        public async Task<IActionResult> Post([FromBody][Required] ArrearsActionCreateRequest request)
        {
            var response = await _createArrearsActionDiaryUseCase.ExecuteAsync(request);

            return HandleResponse(response);
        }
    }
}
