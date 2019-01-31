using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using AgreementService;
using LBH.Data.Domain;
using LBHTenancyAPI.Gateways.V2.Arrears;
using LBHTenancyAPI.Gateways.V2.Arrears.Impl;
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
        public async Task GivenTenancyAgreementRef_WhenCreateActionDiaryEntryWithCorrectParameters_ShouldNotBeNull()
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementServiceChannel>();
            var connectionString = "test";

            fakeArrearsAgreementService.Setup(s => s.CreateArrearsActionAsync(It.IsAny<ArrearsActionCreateRequest>()))
                .ReturnsAsync(new ArrearsActionResponse());


            IArrearsActionDiaryGateway classUnderTest = new ArrearsActionDiaryGateway(fakeArrearsAgreementService.Object, connectionString);

            var request = new ArrearsActionCreateRequest
            {
                ArrearsAction = new ArrearsActionInfo
                {
                    ActionBalance = 10,
                    ActionCategory = "8",
                    ActionCode = "GEN",
                    Comment = "Testing",
                    TenancyAgreementRef = "000017/01"
                },
                DirectUser = new UserCredential
                {
                    UserName = "TestUserName",
                    UserPassword = "TestUserPassword"
                },
                SourceSystem = "TestSystem"
            };

            //act
            var response = await classUnderTest.CreateActionDiaryEntryAsync(request);

            //assert
            Assert.NotNull(response);
        }

        [Theory]
        [InlineData("000017/01", 10, "8", "GEN", "Webservice action")]
        [InlineData("000017/02", 17, "9", "Test", "Testing")]
        public async Task GivenTenancyAgreementRef_WhenCreateActionDiaryEntryWithCorrectParameters_ShouldReturnAValidObject(
            string tenancyRef, decimal actionBalance, string actionCategory, string actionCode, string comment )
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementServiceChannel>();
            var connectionString = "test";
            IArrearsActionDiaryGateway classUnderTest = new ArrearsActionDiaryGateway(fakeArrearsAgreementService.Object, connectionString);

            var request = new ArrearsActionCreateRequest
            {
                ArrearsAction = new ArrearsActionInfo
                {
                    TenancyAgreementRef = tenancyRef,
                    ActionBalance = actionBalance,
                    ActionCategory = actionCategory,
                    ActionCode = actionCode,
                    Comment = comment

                },
                DirectUser = new UserCredential
                {
                    UserName = "TestUserName",
                    UserPassword = "TestUserPassword"
                },
                SourceSystem = "TestSystem"
            };

            fakeArrearsAgreementService.Setup(s => s.CreateArrearsActionAsync(It.IsAny<ArrearsActionCreateRequest>()))
                .ReturnsAsync(Fake.CreateArrearsActionAsync(request));

            //act
            var response = await classUnderTest.CreateActionDiaryEntryAsync(request);
            //assert
            response.ArrearsAction.TenancyAgreementRef.Should().Be(tenancyRef);
            response.ArrearsAction.ActionBalance.Should().Be(actionBalance);
            response.ArrearsAction.ActionCategory.Should().Be(actionCategory);
            response.ArrearsAction.ActionCode.Should().Be(actionCode);
        }

        [Fact]
        public async Task UpdateRecordingUserName_WhenUsernameSupplyed_ShouldChangeUsername()
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementServiceChannel>();

            ArrearsActionDiaryEntry diaryEntry = Fake.GenerateActionDiary();
            TestDataHelper.InsertArrearsActions(diaryEntry, _databaseFixture.Db);

            IArrearsActionDiaryGateway classUnderTest = new ArrearsActionDiaryGateway(fakeArrearsAgreementService.Object, _databaseFixture.ConnectionString);

            string username = "A Real username";

            //act
            await classUnderTest.UpdateRecordingDetails(username, diaryEntry.Id, DateTime.Now);
            //assert
            ArrearsActionDiaryEntry action = TestDataHelper.GetArrearsActionsByRef(diaryEntry.TenancyRef).First();

            action.TenancyRef.Should().Be(diaryEntry.TenancyRef);
            action.UniversalHousingUsername.Should().Be(username);
        }

        [Fact]
        public async Task UpdateRecordingDateShouldChangeDate()
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementServiceChannel>();

            ArrearsActionDiaryEntry diaryEntry = Fake.GenerateActionDiary();
            TestDataHelper.InsertArrearsActions(diaryEntry, _databaseFixture.Db);

            IArrearsActionDiaryGateway classUnderTest = new ArrearsActionDiaryGateway(fakeArrearsAgreementService.Object, _databaseFixture.ConnectionString);

            string username = "A Real username";
            DateTime date = DateTime.Now;

            //act
            await classUnderTest.UpdateRecordingDetails(username, diaryEntry.Id, date);
            //assert
            ArrearsActionDiaryEntry action = TestDataHelper.GetArrearsActionsByRef(diaryEntry.TenancyRef).First();

            action.TenancyRef.Should().Be(diaryEntry.TenancyRef);
            action.UniversalHousingUsername.Should().Be(username);
            action.Date.Should().Be(date);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task UpdateRecordingUserNameWhenNoUsernameSupplyed_ShouldNoOp(string username)
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementServiceChannel>();

            ArrearsActionDiaryEntry diaryEntry = Fake.GenerateActionDiary();
            TestDataHelper.InsertArrearsActions(diaryEntry, _databaseFixture.Db);

            IArrearsActionDiaryGateway classUnderTest = new ArrearsActionDiaryGateway(fakeArrearsAgreementService.Object, _databaseFixture.ConnectionString);

            //act
            await classUnderTest.UpdateRecordingDetails(username, diaryEntry.Id, DateTime.Now);
            //assert
            ArrearsActionDiaryEntry action = TestDataHelper.GetArrearsActionsByRef(diaryEntry.TenancyRef).First();

            action.TenancyRef.Should().Be(diaryEntry.TenancyRef);
            action.UniversalHousingUsername.Should().Be(null);
        }

        [Fact]
        public void GivenTenancyAgreementRef_WhenCreateActionDiaryEntryWithNull_ShouldThrowAnException()
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementServiceChannel>();
            var connectionString = "test";
            IArrearsActionDiaryGateway classUnderTest = new ArrearsActionDiaryGateway(fakeArrearsAgreementService.Object,connectionString);

            //act
            //assert
            Assert.Throws<AggregateException>(()=>classUnderTest.CreateActionDiaryEntryAsync(null).Result);
        }
    }
}
