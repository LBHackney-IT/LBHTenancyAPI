using System.Collections.Generic;
using LBHTenancyAPI.UseCases;
using Xunit;

namespace LBHTenancyAPITest.TestDoubles.UseCases
{
    class ListTenanciesSpy : IListTenancies
    {
        private readonly List<object> calledWith;

        public ListTenanciesSpy()
        {
            calledWith = new List<object>();
        }

        public ListTenancies.Response Execute(List<string> tenancyRefs)
        {
            calledWith.Add(new List<object> {tenancyRefs});
            return new ListTenancies.Response {Tenancies = new List<ListTenancies.ResponseTenancy>()};
        }

        public void AssertCalledOnce()
        {
            Assert.Single(calledWith);
        }

        public void AssertCalledWith(List<object> expectedArguments)
        {
            Assert.Equal(expectedArguments, calledWith[0]);
        }
    }
}
