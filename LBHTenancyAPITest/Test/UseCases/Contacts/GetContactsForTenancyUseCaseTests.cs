using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LBHTenancyAPI.Gateways.Arrears;
using LBHTenancyAPI.UseCases.Contacts;
using LBHTenancyAPI.UseCases.Contacts.Models;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.UseCases.Contacts
{

    public class GetContactsForTenancyUseCaseTests
    {
        private IGetContactsForTenancyUseCase _classUnderTest;
        private Mock<IContactsGateway> _fakeGateway;

        public GetContactsForTenancyUseCaseTests()
        {
            _fakeGateway = new Mock<IContactsGateway>();
            
            _classUnderTest = new GetContactsForTenancyUseCase(_fakeGateway.Object);
        }

        [Fact]
        public async Task GivenValidedInput_GatewayReceivesCorrectInput()
        {
            //arrange
            var tenancyAgreementRef = "Test";
            _fakeGateway.Setup(s => s.GetContactsByTenancyReferenceAsync(It.Is<GetContactsForTenancyRequest>(i => i.TenancyAgreementReference.Equals("Test")), CancellationToken.None))
                .ReturnsAsync(new List<LBH.Data.Domain.Contact>
                {

                });

            var request = new GetContactsForTenancyRequest
            {
                TenancyAgreementReference = tenancyAgreementRef
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            _fakeGateway.Verify(v => v.GetContactsByTenancyReferenceAsync(It.Is<GetContactsForTenancyRequest>(i => i.TenancyAgreementReference.Equals("Test"))));
        }

        //[Fact]
        //public async Task GivenValidedInput_GatewayResponseWith_Success()
        //{
        //    //arrange
        //    var tenancyAgreementRef = "Test";
        //    _fakeGateway.Setup(s => s.CreateActionDiaryEntryAsync(It.Is<ArrearsActionCreateRequest>(i => i.ArrearsAction.TenancyAgreementRef.Equals("Test"))))
        //        .ReturnsAsync(new ArrearsActionResponse
        //        {
        //            Success = true,
        //            ArrearsAction = new ArrearsActionLogDto
        //            {
        //                TenancyAgreementRef = tenancyAgreementRef
        //            }

        //        });
        //    var request = new ArrearsActionCreateRequest
        //    {
        //        ArrearsAction = new ArrearsActionInfo
        //        {
        //            TenancyAgreementRef = tenancyAgreementRef
        //        }
        //    };
        //    //act
        //    var response = await _classUnderTest.ExecuteAsync(request);
        //    //assert
        //    Assert.Equal(true, response.Success);
        //    Assert.Equal("Test", response.ArrearsAction.TenancyAgreementRef);
        //}

        //[Fact]
        //public async Task GivenInvalidInput_GatewayResponseWith_Failure()
        //{
        //    //arrange
        //    var tenancyAgreementRef = "InvalidTest";
        //    _fakeGateway.Setup(s => s.CreateActionDiaryEntryAsync(It.Is<ArrearsActionCreateRequest>(i => i.ArrearsAction.TenancyAgreementRef.Equals(string.Empty))))
        //        .ReturnsAsync(new ArrearsActionResponse
        //        {
        //            Success = false,
        //            ArrearsAction = new ArrearsActionLogDto
        //            {
        //                TenancyAgreementRef = string.Empty
        //            }

        //        });
        //    var request = new ArrearsActionCreateRequest
        //    {
        //        ArrearsAction = new ArrearsActionInfo
        //        {
        //            TenancyAgreementRef = string.Empty
        //        }
        //    };
        //    //act
        //    var response = await _classUnderTest.ExecuteAsync(request);
        //    //assert
        //    Assert.Equal(false, response.Success);
        //}

        //[Fact]
        //public void GivenNull_GatewayResponseWith_Failure()
        //{
        //    //arrange            
        //    _fakeGateway.Setup(s => s.CreateActionDiaryEntryAsync(It.Is<ArrearsActionCreateRequest>(i => i == null)))
        //        .Throws<AggregateException>();
        //    //act
        //    //assert
        //    Assert.Throws<AggregateException>(() => _classUnderTest.ExecuteAsync(null).Result);
        //}

        //[Fact]
        //public async Task GivenValidInput_ThenRequestBuilder_AddsCredentials_ToRequest()
        //{
        //    //arrange
        //    var tenancyAgreementRef = "Test";
        //    _fakeGateway.Setup(s => s.CreateActionDiaryEntryAsync(It.Is<ArrearsActionCreateRequest>(i => i.ArrearsAction.TenancyAgreementRef.Equals("Test"))))
        //        .ReturnsAsync(new ArrearsActionResponse
        //        {
        //            Success = true,
        //            ArrearsAction = new ArrearsActionLogDto
        //            {
        //                TenancyAgreementRef = tenancyAgreementRef
        //            }

        //        });
        //    var request = new ArrearsActionCreateRequest
        //    {
        //        ArrearsAction = new ArrearsActionInfo
        //        {
        //            TenancyAgreementRef = tenancyAgreementRef
        //        }
        //    };
        //    //act
        //    var response = await _classUnderTest.ExecuteAsync(request);
        //    //assert
        //    _fakeGateway.Verify(v => v.CreateActionDiaryEntryAsync(It.Is<ArrearsActionCreateRequest>(i => i.DirectUser != null && !string.IsNullOrEmpty(i.SourceSystem))));
        //}
    }
}
