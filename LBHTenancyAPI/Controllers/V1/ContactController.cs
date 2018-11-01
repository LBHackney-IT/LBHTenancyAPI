using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LBHTenancyAPI.Extensions.Controller;
using LBHTenancyAPI.Infrastructure.V1.API;
using LBHTenancyAPI.UseCases.V1.Contacts;
using LBHTenancyAPI.UseCases.V1.Contacts.Models;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers.V1
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(APIResponse<object>), 400)]
    [ProducesResponseType(typeof(APIResponse<object>), 500)]
    public class ContactController : BaseController
    {
        private readonly IGetContactsForTenancyUseCase _getContactsForTenancyUseCase;

        public ContactController(IGetContactsForTenancyUseCase getContactsForTenancyUseCase)
        {
            _getContactsForTenancyUseCase = getContactsForTenancyUseCase;
        }
        
        [HttpGet]
        [Route("api/v1/tenancies/{TenancyAgreementReference}/contacts/")]
        [ProducesResponseType(typeof(APIResponse<GetContactsForTenancyResponse>), 200)]
        public async Task<IActionResult> Get([FromRoute][Required]GetContactsForTenancyRequest request)
        {
            var result = await _getContactsForTenancyUseCase.ExecuteAsync(request, HttpContext.GetCancellationToken()).ConfigureAwait(false);

            return HandleResponse(result);
        }
    }
}
