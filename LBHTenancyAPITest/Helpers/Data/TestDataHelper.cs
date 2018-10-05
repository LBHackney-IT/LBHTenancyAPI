using System.Data;
using System.Data.SqlClient;
using LBHTenancyAPITest.EF.Entities;

namespace LBHTenancyAPITest.Helpers.Data
{
    public static class TestDataHelper
    {
        public static void InsertMember(Member member, SqlConnection db)
        {
            var commandText = "INSERT INTO member (house_ref, person_no, title,forename,surname,age,responsible) VALUES (@house_ref, @person_no, @title,@forename,@surname,@age,@responsible);";
            var command = new SqlCommand(commandText, db);

            command.Parameters.Add("@house_ref", SqlDbType.Char);
            command.Parameters["@house_ref"].Value = member.house_ref;

            command.Parameters.Add("@title", SqlDbType.Char);
            command.Parameters["@title"].Value = member.title;

            command.Parameters.Add("@person_no", SqlDbType.Char);
            command.Parameters["@person_no"].Value = member.person_no;

            command.Parameters.Add("@forename", SqlDbType.Char);
            command.Parameters["@forename"].Value = member.forename;

            command.Parameters.Add("@surname", SqlDbType.Char);
            command.Parameters["@surname"].Value = member.surname;

            command.Parameters.Add("@age", SqlDbType.Int);
            command.Parameters["@age"].Value = member.age;

            command.Parameters.Add("@responsible", SqlDbType.Bit);
            command.Parameters["@responsible"].Value = member.responsible;

            command.ExecuteNonQuery();
        }

        public static void InsertProperty(Property property, SqlConnection db)
        {
            var commandText = "INSERT INTO property(short_address, address1, prop_ref, post_code) VALUES(@short_address, @address1, @prop_ref, @post_code);";
            var command = new SqlCommand(commandText, db);

            command.Parameters.Add("@short_address", SqlDbType.Char);
            command.Parameters["@short_address"].Value = property.short_address;

            command.Parameters.Add("@address1", SqlDbType.Char);
            command.Parameters["@address1"].Value = property.address1;

            command.Parameters.Add("@prop_ref", SqlDbType.Char);
            command.Parameters["@prop_ref"].Value = property.prop_ref;

            command.Parameters.Add("@post_code", SqlDbType.Char);
            command.Parameters["@post_code"].Value = property.post_code;

            command.ExecuteNonQuery();
        }
        public static void InsertTenancy(TenancyAgreement tenancyAgreement, SqlConnection db)
        {
            var commandText = @"INSERT INTO [dbo].[tenagree]
                                ([tag_ref],[prop_ref],[house_ref],[cur_bal],[tenure],[rent],[service],[other_charge]) 
                                VALUES
                                (@tag_ref,@prop_ref,@house_ref,@cur_bal,@tenure, @rent, @service, @other_charge)";

            var command = new SqlCommand(commandText, db);

            command.Parameters.Add("@tag_ref", SqlDbType.Char);
            command.Parameters["@tag_ref"].Value = tenancyAgreement.tag_ref;

            command.Parameters.Add("@prop_ref", SqlDbType.Char);
            command.Parameters["@prop_ref"].Value = tenancyAgreement.prop_ref;

            command.Parameters.Add("@house_ref", SqlDbType.Char);
            command.Parameters["@house_ref"].Value = tenancyAgreement.house_ref;

            command.Parameters.Add("@cur_bal", SqlDbType.Char);
            command.Parameters["@cur_bal"].Value = tenancyAgreement.cur_bal;

            command.Parameters.Add("@tenure", SqlDbType.Char);
            command.Parameters["@tenure"].Value = tenancyAgreement.tenure;

            command.Parameters.Add("@rent", SqlDbType.Decimal);
            command.Parameters["@rent"].Value = tenancyAgreement.rent;

            command.Parameters.Add("@service", SqlDbType.Decimal);
            command.Parameters["@service"].Value = tenancyAgreement.service;

            command.Parameters.Add("@other_charge", SqlDbType.Decimal);
            command.Parameters["@other_charge"].Value = tenancyAgreement.other_charge;

            command.ExecuteNonQuery();
        }
    }
}
