using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Extensions.Validation;
using LBHTenancyAPI.UseCases.V2.ArrearsActions;
using LBHTenancyAPI.UseCases.V2.ArrearsActions.Models;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers.V2
{
    /// <summary>
    /// Arrears Action Diary Controller V2 to create an action diary record with the specified user recorded
    /// </summary>
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

        /// <summary>
        /// Arrears Action Diary Controller V2 to create an action diary record with the specified user recorded
        /// </summary>
        /// <param name="actionBalance"></param>
        /// <param name="actionCategory"></param>
        /// <param name="actionCode"></param>
        /// <param name="comment"></param>
        /// <param name="tenancyAgreementRef"></param>
        /// <param name="companyCode"></param>
        /// <param name="appUser"></param>
        /// <param name="sessionToken"></param>
        /// <returns></returns>
        [HttpPost, MapToApiVersion("2")]
        public async Task<IActionResult> Post([FromBody][Required] ActionDiaryRequest request)
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
