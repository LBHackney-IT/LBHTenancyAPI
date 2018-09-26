using LBHTenancyAPI.Infrastructure.UseCase;
using LBHTenancyAPI.UseCases.Contacts.Models;

namespace LBHTenancyAPI.UseCases.Contacts
{
    public interface IGetContactsForTenancyUseCase: IRawUseCaseAsync<GetContactsForTenancyRequest,GetContactsForTenancyResponse>
    {

    }
}