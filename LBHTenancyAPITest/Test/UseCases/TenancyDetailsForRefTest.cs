using System;
using System.Collections.Generic;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.UseCases;
using LBHTenancyAPITest.Helpers;
using Xunit;


namespace LBHTenancyAPITest.Test.UseCases
{
    public class TenancyDetailsForRefTest
    {
        [Fact]
        public void WhenGivenATenancyRefThatDoesntExist_ShouldReturnAnEmptyTenancyResponse()
        {
            var gateway = new StubTenanciesGateway();
            var tenancyDetailsForRef = new TenancyDetailsForRef(gateway);

            var response = tenancyDetailsForRef.Execute(" ");

            Assert.IsType(typeof(TenancyDetailsForRef.TenancyResponse), response);
            Assert.Equal(null,response.TenancyDetails.TenancyRef);
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
                TenancyDetails = new TenancyDetailsForRef.Tenancy()
                {
                    TenancyRef = tenancy.TenancyRef,
                    PropertyRef = tenancy.PropertyRef,
                    Tenure = tenancy.Tenure,
                    Rent = tenancy.Rent.ToString("C"),
                    Service = tenancy.Service.ToString("C"),
                    OtherCharge = tenancy.OtherCharge.ToString("C"),
                    CurrentBalance = tenancy.CurrentBalance.ToString("C"),
                    PrimaryContactName = tenancy.PrimaryContactName,
                    PrimaryContactLongAddress = tenancy.PrimaryContactLongAddress,
                    PrimaryContactPostcode = tenancy.PrimaryContactPostcode,
                    ArrearsAgreementStatus=tenancy.AgreementStatus,

                    ArrearsActionDiary = tenancy.ArrearsActionDiary.ConvertAll(tenancyActionDiary => new TenancyDetailsForRef.ArrearsActionDiaryEntry
                    {
                        Code = tenancyActionDiary.Code,
                        Type = tenancyActionDiary.Type,
                        Balance = tenancyActionDiary.Balance.ToString("C"),
                        Comment = tenancyActionDiary.Comment,
                        Date = string.Format("{0:u}", tenancyActionDiary.Date),
                        UniversalHousingUsername = tenancyActionDiary.UniversalHousingUsername
                    }),

                    ArrearsAgreements = tenancy.ArrearsAgreements.ConvertAll(tenancyAgreement => new TenancyDetailsForRef.ArrearsAgreement
                    {
                        Amount = tenancyAgreement.Amount.ToString("C"),
                        Breached = tenancyAgreement.Breached.ToString(),
                        ClearBy = string.Format("{0:u}", tenancyAgreement.ClearBy),
                        Frequency = tenancyAgreement.Frequency,
                        StartBalance = tenancyAgreement.StartBalance.ToString("C"),
                        Startdate = string.Format("{0:u}", tenancyAgreement.Startdate),
                        Status = tenancyAgreement.Status
                    })
                }
            };

            Assert.Equal(expectedResponse.TenancyDetails.ArrearsAgreementStatus, response.TenancyDetails.ArrearsAgreementStatus);
            Assert.Equal(expectedResponse.TenancyDetails.CurrentBalance, response.TenancyDetails.CurrentBalance);
            Assert.Equal(expectedResponse.TenancyDetails.PropertyRef, response.TenancyDetails.PropertyRef);
            Assert.Equal(expectedResponse.TenancyDetails.Tenure, response.TenancyDetails.Tenure);
            Assert.Equal(expectedResponse.TenancyDetails.Rent, response.TenancyDetails.Rent);
            Assert.Equal(expectedResponse.TenancyDetails.Service, response.TenancyDetails.Service);
            Assert.Equal(expectedResponse.TenancyDetails.OtherCharge, response.TenancyDetails.OtherCharge);
            Assert.Equal(expectedResponse.TenancyDetails.PrimaryContactLongAddress, response.TenancyDetails.PrimaryContactLongAddress);
            Assert.Equal(expectedResponse.TenancyDetails.PrimaryContactName, response.TenancyDetails.PrimaryContactName);
            Assert.Equal(expectedResponse.TenancyDetails.PrimaryContactPostcode, response.TenancyDetails.PrimaryContactPostcode);
            Assert.Equal(expectedResponse.TenancyDetails.ArrearsActionDiary, response.TenancyDetails.ArrearsActionDiary);
            Assert.Equal(expectedResponse.TenancyDetails.ArrearsAgreements, response.TenancyDetails.ArrearsAgreements);
        }
    }


}
