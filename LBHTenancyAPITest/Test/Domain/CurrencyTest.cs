using LBH.Data.Domain;
using Xunit;

namespace LBHTenancyAPITest.Test.Domain
{
    public class ArrearsServiceRequestBuilderTests
    {

        [Fact]
        public void WhenGivenEqualCurrencies_Equals_ReturnsTrue()
        {
            Currency c1 = new Currency(20.2m);
            Currency c2 = new Currency(20.2m);

            //assert
            Assert.Equal(c1, c2);
            Assert.Equal(c1.GetHashCode(), c2.GetHashCode());
        }

        [Fact]
        public void WhenGivenDiffrentCurrencies_Equals_ReturnsFalse()
        {
            Currency c1 = new Currency(21.4m);
            Currency c2 = new Currency(20.2m);

            //assert
            Assert.NotEqual(c1, c2);
            Assert.NotEqual(c1.GetHashCode(), c2.GetHashCode());

        }
    }
}
