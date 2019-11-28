using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using AgreementService;
using LBH.Data.Domain;
using LBHTenancyAPI.Gateways.V2.Arrears;
using LBHTenancyAPI.Gateways.V2.Arrears.Impl;
using LBHTenancyAPI.UseCases.V2.ArrearsActions.Models;
using LBHTenancyAPITest.Helpers;
using LBHTenancyAPITest.Helpers.Data;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.Gateways.V2.ArrearsActions
{
    public class UHArrearsDiaryCreationGatewayTest: IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _databaseFixture;

        public UHArrearsDiaryCreationGatewayTest(DatabaseFixture fixture)
        {
            _databaseFixture = fixture;
        }

        [Fact]
        public async Task GivenTenancyAgreementRef_WhenCreateActionDiaryEntryWithIncorrectParameters_ShouldReturnAnError()
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementServiceChannel>();


            IArrearsActionDiaryGateway classUnderTest = new ArrearsActionDiaryGateway(fakeArrearsAgreementService.Object, _databaseFixture.ConnectionString);

//[Required] public string ActionCode { get; set; }
//[Required] public string Comment { get; set; }
//[Required] public string TenancyAgreementRef { get; set; }
//public string Username { get; set; }

            var request = new ActionDiaryRequest
            {
                ActionCode = "GEN",
                Comment = "Testing",
                TenancyAgreementRef = "Not a real tenancy ref",
                Username = "TestUserName",
            };

            //act
            var response = await classUnderTest.CreateActionDiaryEntryAsync(request);

            //assert
            response.Success.Should().BeFalse();
            response.ErrorCode.Should().Be(1);
            response.ErrorMessage.Should().Be("Failed to add entry into action diary");
        }

        [Theory]
        [InlineData("000017/01", 10, "8", "GEN", "An action diary entry comment")]
        [InlineData("000017/02", 17, "9", "TST", "Testing")]
        public async Task GivenTenancyAgreementRef_WhenCreateActionDiaryEntryWithCorrectParameters_ShouldReturnAValidObject(
            string tenancyRef, decimal actionBalance, string actionCategory, string actionCode, string comment )
        {
            //Arrange

            var fakeArrearsAgreementService = new Mock<IArrearsAgreementServiceChannel>();
            IArrearsActionDiaryGateway classUnderTest = new ArrearsActionDiaryGateway(fakeArrearsAgreementService.Object, _databaseFixture.ConnectionString);

            var tenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            tenancy.tag_ref = tenancyRef;
            TestDataHelper.InsertTenancy(tenancy, _databaseFixture.Db);

            var request = new ActionDiaryRequest
            {
                ActionCode = actionCode,
                Comment = comment,
                TenancyAgreementRef = tenancyRef,
                Username = "TestUserName"
            };

            //act
            var response = await classUnderTest.CreateActionDiaryEntryAsync(request);
            //assert
            response.ArrearsAction.TenancyAgreementRef.Should().Be(tenancyRef);
            response.ArrearsAction.ActionCode.Should().Be(actionCode);
            response.Success.Should().BeTrue();
        }


        [Fact]
        public void GivenTenancyAgreementRef_WhenCreateActionDiaryEntryWithNull_ShouldThrowAnException()
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementServiceChannel>();
            IArrearsActionDiaryGateway classUnderTest = new ArrearsActionDiaryGateway(fakeArrearsAgreementService.Object, _databaseFixture.ConnectionString);

            //act
            //assert
            Assert.Throws<AggregateException>(()=>classUnderTest.CreateActionDiaryEntryAsync(null).Result);
        }
    }
}
