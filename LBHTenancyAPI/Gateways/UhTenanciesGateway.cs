using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LBHTenancyAPI.Domain;

namespace LBHTenancyAPI.Gateways
{
    public class UhTenanciesGateway : ITenanciesGateway
    {
        private readonly SqlConnection conn;

        public UhTenanciesGateway(string connectionString)
        {
            conn = new SqlConnection(connectionString);
            conn.Open();
        }

        public List<TenancyListItem> GetTenanciesByRefs(List<string> tenancyRefs)
        {
            var all = conn.Query<TenancyListItem>(
                "SELECT " +
                "tenagree.tag_ref as TenancyRef, " +
                "tenagree.cur_bal as CurrentBalance, " +
                "tenagree.prop_ref as PropertyRef, " +
                "tenagree.tenure as Tenure, " +
                "tenagree.rent as Rent, " +
                "tenagree.service as Service, " +
                "tenagree.other_charge as OtherCharge, " +
                "contacts.con_name as PrimaryContactName, " +
                "property.short_address as PrimaryContactShortAddress, " +
                "property.post_code as PrimaryContactPostcode, " +
                "araction.tag_ref AS TenancyRef, " +
                "araction.action_code AS LastActionCode, " +
                "araction.action_date AS LastActionDate, " +
                "arag.arag_status as ArrearsAgreementStatus, " +
                "arag.arag_startdate as ArrearsAgreementStartDate " +
                "FROM tenagree " +
                "LEFT JOIN contacts " +
                "ON contacts.tag_ref = tenagree.tag_ref " +
                "LEFT JOIN property " +
                "ON property.prop_ref = tenagree.prop_ref " +
                "LEFT JOIN ( " +
                "SELECT " +
                "araction.tag_ref, " +
                "araction.action_code, " +
                "araction.action_date " +
                "FROM araction " +
                "WHERE araction.tag_ref IN @allRefs " +
                ") AS araction ON araction.tag_ref = tenagree.tag_ref " +
                "LEFT JOIN ( " +
                "SELECT " +
                "arag.tag_ref," +
                "arag.arag_status, " +
                "arag.arag_startdate " +
                "FROM arag " +
                "WHERE arag.tag_ref IN @allRefs " +
                ") AS arag ON arag.tag_ref = tenagree.tag_ref " +
                "WHERE tenagree.tag_ref IN @allRefs " +
                "ORDER BY arag.arag_startdate DESC, araction.action_date DESC",
                new {allRefs = tenancyRefs}
            ).ToList();

            var results = new List<TenancyListItem>();

            foreach (var reference in tenancyRefs)
            {
                try
                {
                    results.Add(all.First(e => e.TenancyRef == reference));
                }
                catch (InvalidOperationException)
                {
                    Console.Write($"No valid tenancy for ref: {reference}");
                }
            }

            return results;
        }

        public List<ArrearsActionDiaryEntry> GetActionDiaryEntriesbyTenancyRef(string tenancyRef)
        {
            return conn.Query<ArrearsActionDiaryEntry>(
                "SELECT " +
                "tag_ref as TenancyRef, " +
                "action_code as ActionCode, " +
                "action_type as Type, " +
                "action_date as ActionDate, " +
                "action_comment as ActionComment, " +
                "username as UHUsername, " +
                "action_balance as ActionBalance " +
                "FROM araction " +
                "WHERE tag_ref = @tRef " +
                "ORDER BY araction.action_date DESC",
                new {tRef = tenancyRef.Replace("%2F", "/")}
            ).ToList();
        }

        public async Task<List<PaymentTransaction>> GetPaymentTransactionsByTenancyRefAsync(string tenancyRef)
        {
            var query = await conn.QueryAsync<PaymentTransaction>(
                "SELECT " +
                "tag_ref AS TenancyRef," +
                "prop_ref AS PropertyRef, " +
                "trans_type AS Type, " +
                "real_value AS Amount, " +
                "post_date AS Date, " +
                "trans_ref AS TransactionRef " +
                "d.Description AS Description" +
                "FROM rtrans r with(nolock),( " +
                "select d.deb_code as Code, d.deb_desc as Description from dbo.debtype d with(nolock)" +
                "where d.deb_code <> ''" +
                "UNION" +
                "select rec_code as Code, rec_desc as Description from dbo.rectype with(nolock)" +
                "where rectype.rec_code <> '') as d" +
                "WHERE tag_ref = @tRef " +
                "ORDER BY post_date DESC",
                new {tRef = tenancyRef.Replace("%2F", "/")}
            ).ConfigureAwait(false);

            var paymentTransactions = query.ToList();

            return paymentTransactions;
        }

        public Tenancy GetTenancyForRef(string tenancyRef)
        {
            var result = conn.Query<Tenancy>(
                "SELECT " +
                "tenagree.tag_ref as TenancyRef, " +
                "tenagree.cur_bal as CurrentBalance, " +
                "tenagree.tenure as Tenure, " +
                "contacts.con_name as PrimaryContactName, " +
                "property.address1 as PrimaryContactLongAddress, " +
                "property.post_code as PrimaryContactPostcode, " +
                "contacts.con_phone1 as PrimaryContactPhone " +
                "FROM tenagree " +
                "LEFT JOIN arag " +
                "ON arag.tag_ref = tenagree.tag_ref " +
                "LEFT JOIN contacts " +
                "ON contacts.tag_ref = tenagree.tag_ref " +
                "LEFT JOIN property " +
                "ON property.prop_ref = tenagree.prop_ref " +
                "WHERE tenagree.tag_ref = @tRef " +
                "ORDER BY arag.arag_startdate DESC",
                new {tRef = tenancyRef.Replace("%2F", "/")}
            ).FirstOrDefault();

            result.ArrearsAgreements = GetLastFiveAgreementsForTenancy(tenancyRef);
            result.ArrearsActionDiary = GetLatestFiveArrearsActionForRef(tenancyRef);

            return result;
        }

        private List<ArrearsAgreement> GetLastFiveAgreementsForTenancy(string tenancyRef)
        {
            return conn.Query<ArrearsAgreement>(
                "SELECT TOP 5" +
                "tag_ref AS TenancyRef," +
                "arag_status AS Status, " +
                "arag_startdate Startdate, " +
                "arag_amount Amount, " +
                "arag_frequency AS Frequency, " +
                "arag_breached AS Breached, " +
                "arag_startbal AS StartBalance, " +
                "arag_clearby AS ClearBy " +
                "FROM arag " +
                "WHERE tag_ref = @tRef " +
                "ORDER BY arag_startdate DESC ",
                new {tRef = tenancyRef.Replace("%2F", "/")}
            ).ToList();
        }

        public List<ArrearsActionDiaryEntry> GetLatestFiveArrearsActionForRef(string tenancyRef)
        {
            return conn.Query<ArrearsActionDiaryEntry>(
                "SELECT top 5" +
                "tag_ref as TenancyRef, " +
                "action_code as Code, " +
                "action_type as Type, " +
                "action_date as Date, " +
                "action_comment as Comment, " +
                "username as UniversalHousingUsername, " +
                "action_balance as Balance " +
                "FROM araction " +
                "WHERE tag_ref = @tRef " +
                "ORDER BY araction.action_date DESC",
                new {tRef = tenancyRef.Replace("%2F", "/")}
            ).ToList();
        }
    }
}
