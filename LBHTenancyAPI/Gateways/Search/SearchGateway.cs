using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LBH.Data.Domain;
using LBHTenancyAPI.UseCases.Contacts.Models;

namespace LBHTenancyAPI.Gateways.Search
{
    public class SearchGateway : ISearchGateway
    {
        private readonly SqlConnection _connection;
        public SearchGateway(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }
        public async Task<IList<TenancyListItem>> SearchTenanciesAsync(SearchTenancyRequest request, CancellationToken cancellationToken)
        {
            var all = await _connection.QueryAsync<TenancyListItem>(
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
                new { searchTerm = request.SearchTerm }
            ).ConfigureAwait(false);

            var results = all.ToList();

            return results;

        }
    }
}
