using LBH.Data.Domain;
using LBHTenancyAPI.Gateways.V1;
using LBHTenancyAPI.UseCases.V1;
using LBHTenancyAPITest.Helpers;
using Xunit;

namespace LBHTenancyAPITest.Test.UseCases.V1
{
    public class TenancyDetailsForRefTest
    {
        [Fact]
        public void WhenGivenATenancyRefThatDoesntExist_ShouldReturnNull()
        {
            var gateway = new StubTenanciesGateway();
            var tenancyDetailsForRef = new TenancyDetailsForRef(gateway);

            var response = tenancyDetailsForRef.Execute(" some random string to look for ");

            Assert.Equal(null, response.TenancyDetails);
        }

        [Fact]
        public void WhenGivenATenancyRef_ShouldReturnAnTenancyResponse()
        {
            var gateway = new StubTenanciesGateway();
            var tenancyDetailsForRef = new TenancyDetailsForRef(gateway);

            var tenancy = Fake.GenerateTenancyDetails();
            gateway.SetTenancyDetails(tenancy.TenancyRef, tenancy);

            var response = tenancyDetailsForRef.Execute(tenancy.TenancyRef);

            Assert.IsType(typeof(TenancyDetailsForRef.TenancyResponse), response);
        }

        [Fact]
        public void WhenATenancyRefIsGiven_ResponseShouldIncludeTenancyDetails()
        {
            var gateway = new StubTenanciesGateway();
            var tenancy = Fake.GenerateTenancyDetails();

            gateway.SetTenancyDetails(tenancy.TenancyRef, tenancy);

            var tenancyDetails = new TenancyDetailsForRef(gateway);
            var response = tenancyDetails.Execute(tenancy.TenancyRef);

            var expectedResponse = new TenancyDetailsForRef.TenancyResponse()
            {
                TenancyDetails = new Tenancy()
                {
                    TenancyRef = tenancy.TenancyRef,
                    PropertyRef = tenancy.PropertyRef,
                    Tenure = tenancy.Tenure,
                    Rent = tenancy.Rent,
                    Service = tenancy.Service,
                    OtherCharge = tenancy.OtherCharge,
                    CurrentBalance = tenancy.CurrentBalance,
                    PrimaryContactName = tenancy.PrimaryContactName,
                    PrimaryContactLongAddress = tenancy.PrimaryContactLongAddress,
                    PrimaryContactPostcode = tenancy.PrimaryContactPostcode,
                    AgreementStatus = tenancy.AgreementStatus,

                    ArrearsActionDiary = tenancy.ArrearsActionDiary.ConvertAll(tenancyActionDiary => new ArrearsActionDiaryEntry
                    {
                        Code = tenancyActionDiary.Code,
                        Type = tenancyActionDiary.Type,
                        Balance = tenancyActionDiary.Balance,
                        Comment = tenancyActionDiary.Comment,
                        Date = tenancyActionDiary.Date,
                        UniversalHousingUsername = tenancyActionDiary.UniversalHousingUsername
                    }),

                    ArrearsAgreements = tenancy.ArrearsAgreements.ConvertAll(tenancyAgreement => new ArrearsAgreement
                    {
                        Amount = tenancyAgreement.Amount,
                        Breached = tenancyAgreement.Breached,
                        ClearBy = tenancyAgreement.ClearBy,
                        Frequency = tenancyAgreement.Frequency,
                        StartBalance = tenancyAgreement.StartBalance,
                        Startdate = tenancyAgreement.Startdate,
                        Status = tenancyAgreement.Status
                    })
                }
            };

            Assert.True(response.TenancyDetails.HasValue);
            Assert.True(response.TenancyDetails.HasValue);


            Assert.Equal(expectedResponse.TenancyDetails.Value.AgreementStatus, response.TenancyDetails.Value.AgreementStatus);
            Assert.Equal(expectedResponse.TenancyDetails.Value.CurrentBalance.Value, response.TenancyDetails.Value.CurrentBalance.Value);
            Assert.Equal(expectedResponse.TenancyDetails.Value.CurrentBalance.CurrencyCode, response.TenancyDetails.Value.CurrentBalance.CurrencyCode);
            Assert.Equal(expectedResponse.TenancyDetails.Value.PropertyRef, response.TenancyDetails.Value.PropertyRef);
            Assert.Equal(expectedResponse.TenancyDetails.Value.Tenure, response.TenancyDetails.Value.Tenure);
            Assert.Equal(expectedResponse.TenancyDetails.Value.Rent, response.TenancyDetails.Value.Rent);
            Assert.Equal(expectedResponse.TenancyDetails.Value.Service, response.TenancyDetails.Value.Service);
            Assert.Equal(expectedResponse.TenancyDetails.Value.OtherCharge, response.TenancyDetails.Value.OtherCharge);
            Assert.Equal(expectedResponse.TenancyDetails.Value.PrimaryContactLongAddress, response.TenancyDetails.Value.PrimaryContactLongAddress);
            Assert.Equal(expectedResponse.TenancyDetails.Value.PrimaryContactName, response.TenancyDetails.Value.PrimaryContactName);
            Assert.Equal(expectedResponse.TenancyDetails.Value.PrimaryContactPostcode, response.TenancyDetails.Value.PrimaryContactPostcode);
            Assert.Equal(expectedResponse.TenancyDetails.Value.ArrearsActionDiary, response.TenancyDetails.Value.ArrearsActionDiary);
            Assert.Equal(expectedResponse.TenancyDetails.Value.ArrearsAgreements, response.TenancyDetails.Value.ArrearsAgreements);
        }
    }


}
