using System.Collections.Generic;
using LBHTenancyAPI.UseCases;

namespace LBHTenancyAPITest.TestDoubles.UseCases
{
    class ListTenanciesStub : IListTenancies
    {
        private readonly Dictionary<string, ListTenancies.ResponseTenancy> stubTenancies;

        public ListTenanciesStub()
        {
            stubTenancies = new Dictionary<string, ListTenancies.ResponseTenancy>();
        }

        public void AddTenancyResponse(string tenancyRef, ListTenancies.ResponseTenancy tenancyResponse)
        {
            stubTenancies[tenancyRef] = tenancyResponse;
        }

        public ListTenancies.Response Execute(List<string> tenancyRefs)
        {
            return new ListTenancies.Response
            {
                Tenancies = tenancyRefs.ConvertAll(tenancyRef => stubTenancies[tenancyRef])
            };
        }
    }
}
