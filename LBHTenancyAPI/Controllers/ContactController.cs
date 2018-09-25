using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LBHTenancyAPI.Extensions.Controller;
using LBHTenancyAPI.Infrastructure.API;
using LBHTenancyAPI.UseCases.Contacts;
using LBHTenancyAPI.UseCases.Contacts.Models;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers
{
    [Produces("application/json")]
    [ProducesResponseType(typeof(APIResponse<object>), 400)]
    [ProducesResponseType(typeof(APIResponse<object>), 500)]
    public class ContactController : BaseController
    {
        private readonly IGetContactsForTenancyUseCase _createArrearsAgreementUseCase;

        public ContactController(IGetContactsForTenancyUseCase createArrearsAgreementUseCase)
        {
            _createArrearsAgreementUseCase = createArrearsAgreementUseCase;
        }

        [HttpPost]
        [Route("api/v1/tenancies/{TenancyAgreementReference}/contacts/")]
        [ProducesResponseType(typeof(APIResponse<GetContactsForTenancyResponse>), 200)]
        public async Task<IActionResult> Get([FromRoute][Required]GetContactsForTenancyRequest request)
        {
            var result = await _createArrearsAgreementUseCase.ExecuteAsync(request, HttpContext.GetCancellationToken()).ConfigureAwait(false);

            return HandleResponse(result);
        }
    }
}
