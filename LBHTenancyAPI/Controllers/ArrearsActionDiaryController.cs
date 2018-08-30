using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AgreementService;
using LBHTenancyAPI.Extensions.Validation;
using LBHTenancyAPI.UseCases.ArrearsActions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LBHTenancyAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/tenancies/arrearsactiondiary/")]
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

            var response = await _createArrearsActionDiaryUseCase.CreateActionDiaryRecordsAsync(request);

            if (!response.Success)
                return StatusCode(500, response);
            return Ok(response);
        }
    }
}
