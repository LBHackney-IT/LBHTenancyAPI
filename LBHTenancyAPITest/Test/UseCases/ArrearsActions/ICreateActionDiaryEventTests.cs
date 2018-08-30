using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.Interfaces;
using LBHTenancyAPI.Services;
using LBHTenancyAPI.UseCases;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.UseCases.ArrearsActions
{
    public class CreateActionDiaryEventTests
    {
        private ICreateArrearsActionDiaryUseCase _classUnderTest;
        private Mock<IArrearsActionDiaryGateway> _fakeGateway;

        public CreateActionDiaryEventTests()
        {
            _fakeGateway = new Mock<IArrearsActionDiaryGateway>();
            ICredentialsService credentialsService = new CredentialsService();
            IArrearsServiceRequestBuilder requestBuilder = new ArrearsServiceRequestBuilder(credentialsService);
            _classUnderTest = new CreateArrearsActionDiaryUseCase(_fakeGateway.Object, requestBuilder);
        }

        [Fact]
        public async Task gateway_receives_correct_input()
        {
            //arrange
            var tenancyAgreementRef = "Test";
            _fakeGateway.Setup(s => s.CreateActionDiaryEntryAsync(It.Is<ArrearsActionCreateRequest>(i=> i.ArrearsAction.TenancyAgreementRef.Equals("Test"))))
                .ReturnsAsync(new ArrearsActionResponse
                {
                    Success = true,
                    ArrearsAction = new ArrearsActionLogDto
                    {
                        TenancyAgreementRef = tenancyAgreementRef
                    }
                    
                });
            var request = new ArrearsActionCreateRequest
            {
                ArrearsAction = new ArrearsActionInfo
                {
                    TenancyAgreementRef = tenancyAgreementRef
                }
            };
            //act
            var response = await _classUnderTest.CreateActionDiaryRecordsAsync(request);
            //assert
            _fakeGateway.Verify(v=> v.CreateActionDiaryEntryAsync(It.Is<ArrearsActionCreateRequest>(i => i.ArrearsAction.TenancyAgreementRef.Equals("Test"))));
        }

        [Fact]
        public async Task GivenValidedInput_GatewayResponseWith_Success()
        {
            //arrange
            var tenancyAgreementRef = "Test";
            _fakeGateway.Setup(s => s.CreateActionDiaryEntryAsync(It.Is<ArrearsActionCreateRequest>(i => i.ArrearsAction.TenancyAgreementRef.Equals("Test"))))
                .ReturnsAsync(new ArrearsActionResponse
                {
                    Success = true,
                    ArrearsAction = new ArrearsActionLogDto
                    {
                        TenancyAgreementRef = tenancyAgreementRef
                    }

                });
            var request = new ArrearsActionCreateRequest
            {
                ArrearsAction = new ArrearsActionInfo
                {
                    TenancyAgreementRef = tenancyAgreementRef
                }
            };
            //act
            var response = await _classUnderTest.CreateActionDiaryRecordsAsync(request);
            //assert
            Assert.Equal(true, response.Success);
            Assert.Equal("Test", response.ArrearsAction.TenancyAgreementRef);
        }

        [Fact]
        public async Task GivenInvalidInput_GatewayResponseWith_Failure()
        {
            //arrange
            var tenancyAgreementRef = "test";
            _fakeGateway.Setup(s => s.CreateActionDiaryEntryAsync(It.Is<ArrearsActionCreateRequest>(i => i.ArrearsAction.TenancyAgreementRef.Equals("Test"))))
                .ReturnsAsync(new ArrearsActionResponse
                {
                    Success = false,
                    ArrearsAction = new ArrearsActionLogDto
                    {
                        TenancyAgreementRef = tenancyAgreementRef
                    }

                });
            var request = new ArrearsActionCreateRequest
            {
                ArrearsAction = new ArrearsActionInfo
                {
                    TenancyAgreementRef = tenancyAgreementRef
                }
            };
            //act
            var response = await _classUnderTest.CreateActionDiaryRecordsAsync(request);
            //assert
            Assert.Equal(false, response.Success);
        }
    }
}
