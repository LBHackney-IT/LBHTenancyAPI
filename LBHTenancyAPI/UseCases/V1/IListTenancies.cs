using System.Collections.Generic;

namespace LBHTenancyAPI.UseCases.V1
{
    public interface IListTenancies
    {
        ListTenancies.Response Execute(List<string> tenancyRefs);
    }
}
