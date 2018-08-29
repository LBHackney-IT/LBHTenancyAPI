using System;
using System.Data.SqlClient;
using Xunit;
using Dapper;
using LBHTenancyAPI.Domain;
using LBHTenancyAPI.Gateways;
using System.Collections.Generic;
using Bogus;
using System.Linq;
using System.Data;
using AgreementService;
using Moq;
using System.Threading.Tasks;
using LBHTenancyAPITest.Helpers;

namespace LBHTenancyAPITest.Test.Gateways
{
    public class UHArrearsDiaryCreationGatewayTest
    {
        [Fact]
        public async Task Given_TenancyAgreementRef_When_CreateActionDiaryEntry_WithCorrectParameters_ShouldNotBeNull()
        {
            //Arrange
            Mock<IArrearsAgreementService> fakeArrearsAgreementService = new Mock<IArrearsAgreementService>();

            fakeArrearsAgreementService.Setup(s => s.CreateArrearsActionAsync(It.IsAny<ArrearsActionCreateRequest>()))
                .ReturnsAsync(new ArrearsActionResponse());

            IArrearsActionDiaryGateway classUnderTest = new ArrearsActionDiaryGateway(fakeArrearsAgreementService.Object);

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
                    UserName = "HackneyAPI",
                    UserPassword = "Hackney1"
                },
                SourceSystem = "HackneyAPI"
            };

            //act
            var response = await classUnderTest.CreateActionDiaryEntryAsync(request);

            //assert
            Assert.NotNull(response);
        }

        [Fact]
        public async Task Given_TenancyAgreementRef_When_CreateActionDiaryEntry_WithCorrectParameters_ShouldReturnAValidObject()
        {
            //Arrange
            Mock<IArrearsAgreementService> fakeArrearsAgreementService = new Mock<IArrearsAgreementService>();

            IArrearsActionDiaryGateway classUnderTest = new ArrearsActionDiaryGateway(fakeArrearsAgreementService.Object);

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
                    UserName = "HackneyAPI",
                    UserPassword = "Hackney1"
                },
                SourceSystem = "HackneyAPI"
            };

            fakeArrearsAgreementService.Setup(s => s.CreateArrearsActionAsync(It.IsAny<ArrearsActionCreateRequest>()))
                .ReturnsAsync(Fake.CreateArrearsActionAsync(request));

            //act
            var response = await classUnderTest.CreateActionDiaryEntryAsync(request);
            //assert
            Assert.Equal(response.ArrearsAction.TenancyAgreementRef, request.ArrearsAction.TenancyAgreementRef);
        }

        //        [Fact]
        //        public void WhenGivenTenancyAgreementRef_CreatActionDiaryEntry_ShouldCorrectlyHandleResponse()
        //        {
        //            //Call mock webservice and test that we handle the response correctly.
        //        }

        //        private TenancyListItem CreateRandomTenancyListItem()
        //        {
        //            var random = new Faker();

        //            return new TenancyListItem
        //            {
        //                TenancyRef = random.Random.Hash(11),
        //                CurrentBalance = random.Finance.Amount(),
        //                LastActionDate = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
        //                LastActionCode = random.Random.Hash(3),
        //                ArrearsAgreementStatus = random.Random.Hash(10),
        //                ArrearsAgreementStartDate =
        //                    new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
        //                PrimaryContactName = random.Name.FullName(),
        //                PrimaryContactShortAddress = $"{random.Address.BuildingNumber()}\n{random.Address.StreetName()}\n{random.Address.Country()}",
        //                PrimaryContactPostcode = random.Random.Hash(10)
        //            };
        //        }

        //        #endregion

        //        #region Private Methods

        //        private TenancyListItem InsertRandomisedTenancyListItem()
        //        {
        //            TenancyListItem tenancy = CreateRandomTenancyListItem();
        //            InsertTenancyAttributes(tenancy);

        //            return tenancy;
        //        }

        //        private List<ArrearsActionDiaryEntry> GetArrearsActionsByRef(string tenancyRef)
        //        {
        //            var gateway = new UhTenanciesGateway(DotNetEnv.Env.GetString("UH_CONNECTION_STRING"));
        //            return gateway.GetActionDiaryEntriesbyTenancyRef(tenancyRef);
        //        }

        //        private List<ArrearsActionDiaryEntry> InsertRandomActionDiaryDetails(string tenancyRef, int num)
        //        {
        //            List<ArrearsActionDiaryEntry> items = null;
        //            SqlCommand command = null;
        //            try
        //            {
        //                var random = new Faker();
        //                items = new List<ArrearsActionDiaryEntry>();
        //                string commandText =

        //                    "INSERT INTO araction (tag_ref, action_code, action_type, action_date, action_comment, action_balance, " +
        //                    "username) " +
        //                    "VALUES (@tenancyRef, @actionCode, @actionType, @actionDate, @actionComment, @actionBalance, @uhUsername)";

        //                foreach (int i in Enumerable.Range(0, num))
        //                {
        //                    ArrearsActionDiaryEntry arrearsActionDiaryEntry = new ArrearsActionDiaryEntry
        //                    {
        //                        TenancyRef = tenancyRef,
        //                        Code = random.Random.Hash(3),
        //                        Date = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
        //                        Type = random.Random.Hash(3),
        //                        Comment = random.Random.Hash(50),
        //                        Balance = random.Finance.Amount(),
        //                        UniversalHousingUsername = random.Random.Hash(40)
        //                    };

        //                    command = new SqlCommand(commandText, db);
        //                    command.Parameters.Add("@tenancyRef", SqlDbType.Char);
        //                    command.Parameters["@tenancyRef"].Value = arrearsActionDiaryEntry.TenancyRef;

        //                    command.Parameters.Add("@actionCode", SqlDbType.Char);
        //                    command.Parameters["@actionCode"].Value = arrearsActionDiaryEntry.Code;

        //                    command.Parameters.Add("@actionDate", SqlDbType.SmallDateTime);
        //                    command.Parameters["@actionDate"].Value = arrearsActionDiaryEntry.Date;

        //                    command.Parameters.Add("@actionType", SqlDbType.Char);
        //                    command.Parameters["@actionType"].Value = arrearsActionDiaryEntry.Type;

        //                    command.Parameters.Add("@actionComment", SqlDbType.NVarChar);
        //                    command.Parameters["@actionComment"].Value = arrearsActionDiaryEntry.Comment;

        //                    command.Parameters.Add("@actionBalance", SqlDbType.Decimal);
        //                    command.Parameters["@actionBalance"].Value = arrearsActionDiaryEntry.Balance;

        //                    command.Parameters.Add("@uhUsername", SqlDbType.Char);
        //                    command.Parameters["@uhUsername"].Value = arrearsActionDiaryEntry.UniversalHousingUsername;

        //                    items.Add(arrearsActionDiaryEntry);
        //                    command.ExecuteNonQuery();
        //                }

        //                return items.OrderByDescending(i => i.Date).ToList();
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //            finally
        //            {
        //                command = null;
        //            }
        //        }

        //        private void InsertTenancyAttributes(TenancyListItem tenancyAttributes)
        //        {
        //            string commandText = InsertQueries();
        //            SqlCommand command = new SqlCommand(commandText, db);
        //            command.Parameters.Add("@tenancyRef", SqlDbType.Char);
        //            command.Parameters["@tenancyRef"].Value = tenancyAttributes.TenancyRef;
        //            command.Parameters.Add("@currentBalance", SqlDbType.Decimal);
        //            command.Parameters["@currentBalance"].Value = tenancyAttributes.CurrentBalance;
        //            command.Parameters.Add("@primaryContactName", SqlDbType.Char);
        //            command.Parameters["@primaryContactName"].Value = tenancyAttributes.PrimaryContactName;
        //            command.Parameters.Add("@primaryContactAddress", SqlDbType.Char);
        //            command.Parameters["@primaryContactAddress"].Value =
        //                tenancyAttributes.PrimaryContactShortAddress == null
        //                    ? DBNull.Value.ToString()
        //                    : tenancyAttributes.PrimaryContactShortAddress + "\n";
        //            command.Parameters.Add("@primaryContactPostcode", SqlDbType.Char);
        //            command.Parameters["@primaryContactPostcode"].Value = tenancyAttributes.PrimaryContactPostcode;
        //            command.Parameters.Add("@primaryContactPhone", SqlDbType.Char);
        //            command.Parameters["@primaryContactPhone"].Value = DBNull.Value.ToString();
        //            command.ExecuteNonQuery();

        //            InsertArrearsActions(tenancyAttributes.TenancyRef, tenancyAttributes.LastActionCode, tenancyAttributes.LastActionDate);
        //        }

        //        private string InsertQueries()
        //        {
        //            string commandText =
        //                "INSERT INTO tenagree (tag_ref, cur_bal) VALUES (@tenancyRef, @currentBalance);" +
        //                "INSERT INTO contacts (tag_ref, con_name, con_address, con_postcode, con_phone1) VALUES (@tenancyRef, @primaryContactName, @primaryContactAddress, @primaryContactPostcode, @primaryContactPhone);";

        //            return commandText;
        //        }

        //        private void InsertArrearsActions(string tenancyRef, string actionCode, DateTime actionDate)
        //        {
        //            string commandText =
        //                "INSERT INTO araction (tag_ref, action_code, action_date) VALUES (@tenancyRef, @actionCode, @actionDate)";

        //            SqlCommand command = new SqlCommand(commandText, db);
        //            command.Parameters.Add("@tenancyRef", SqlDbType.Char);
        //            command.Parameters["@tenancyRef"].Value = tenancyRef;
        //            command.Parameters.Add("@actionCode", SqlDbType.Char);
        //            command.Parameters["@actionCode"].Value = actionCode;
        //            command.Parameters.Add("@actionDate", SqlDbType.SmallDateTime);
        //            command.Parameters["@actionDate"].Value = actionDate;

        //            command.ExecuteNonQuery();
        //        }

        //        #endregion

        //        private List<ArrearsActionDiaryEntry> GetArrearsActionsDairyByRef(string tenancyRef)
        //        {
        //            var gateway = new UhTenanciesGateway(DotNetEnv.Env.GetString("UH_CONNECTION_STRING"));
        //            return gateway.GetActionDiaryEntriesbyTenancyRef(tenancyRef);
        //        }
    }
}
