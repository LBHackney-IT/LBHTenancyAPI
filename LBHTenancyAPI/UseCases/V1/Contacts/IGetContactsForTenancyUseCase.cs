using LBHTenancyAPI.Infrastructure.UseCase;
using LBHTenancyAPI.UseCases.V1.Contacts.Models;

namespace LBHTenancyAPI.UseCases.V1.Contacts
{
    public interface IGetContactsForTenancyUseCase: IRawUseCaseAsync<GetContactsForTenancyRequest,GetContactsForTenancyResponse>
    {

    }
}
