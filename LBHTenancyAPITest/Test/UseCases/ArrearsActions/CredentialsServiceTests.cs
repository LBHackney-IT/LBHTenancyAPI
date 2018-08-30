using LBHTenancyAPI.Services;
using Xunit;

namespace LBHTenancyAPITest.Test.UseCases.ArrearsActions
{
    public class CredentialsServiceTests
    {
        [Fact]
        public void CredentialsService_ReturnsValid_Hardcoded_UHSource_Credentials()
        {
            //arrange
            ICredentialsService classUnderTest = new CredentialsService();
            //act
            var response = classUnderTest.GetUhSourceSystem();
            //assert
            Assert.NotNull(response);
            Assert.Equal("Hackney1", response);
        }

        [Fact]
        public void CredentialsService_ReturnsValid_Hardcoded_UHSUser_Credentials()
        {
            //arrange
            ICredentialsService classUnderTest = new CredentialsService();
            //act
            var response = classUnderTest.GetUhUserCredentials();
            //assert
            Assert.NotNull(response);
            Assert.Equal("HackneyAPI", response.UserName);
            Assert.Equal("Hackney1", response.UserPassword);
        }
    }
}
