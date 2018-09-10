using LBHTenancyAPI.Gateways;
using Xunit;
using FluentAssertions;

namespace LBHTenancyAPITest.Test.Gateways
{
    public class UhPaymentTransactionsGatewayTest
    {
        private IUhPaymentTransactionsGateway _classUnderTest;
        public UhPaymentTransactionsGatewayTest()
        {
            _classUnderTest = new UhPaymentTransactionsGateway();
        }

        [Fact]
        public void WhenGivenValidTransactionType_GetTransactionDescription_ShouldReturnCorrectDescription()
        {
            var response = _classUnderTest.GetTransactionDescription("RTrans");

            "Direct Debit".Should().BeEquivalentTo(response);
        }

        [Fact]
        public void WhenGivenValidTransactionType2_GetTransactionDescription_ShouldReturnCorrectDescription()
        {
            var response = _classUnderTest.GetTransactionDescription("DTrans");

            "Online Payment".Should().BeEquivalentTo(response);
        }

        [Fact]
        public void WhenGivenInValidTransactionType_GetTransactionDescription_ShouldReturnCorrectDescription()
        {
            var response = _classUnderTest.GetTransactionDescription(null);

            "Unknown transaction type".Should().BeEquivalentTo(response);
        }
    }
}
