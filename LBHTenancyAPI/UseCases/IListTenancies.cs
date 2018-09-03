using System.Collections.Generic;
using System.Threading.Tasks;

namespace LBHTenancyAPI.UseCases
{
    public interface IListTenancies
    {
        Task<ListTenancies.Response> ExecuteAsync(List<string> tenancyRefs);
    }
}
