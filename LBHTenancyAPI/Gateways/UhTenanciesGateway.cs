using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using LBHTenancyAPI.Domain;
using Dapper;
using Microsoft.EntityFrameworkCore.Internal;

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
            return conn.Query<TenancyListItem>($"" +
                                               $"SELECT DISTINCT" +
                                               $"(tenagree.tag_ref) as TenancyRef, " +
                                               $"tenagree.cur_bal as CurrentBalance, " +
                                               $"arag.arag_status as ArrearsAgreementStatus, " +
                                               $"arag.start_date as ArrearsAgreementStartDate, " +
                                               $"contacts.con_name as PrimaryContactName, " +
                                               $"contacts.con_address as PrimaryContactShortAddress, " +
                                               $"contacts.con_postcode as PrimaryContactPostcode, " +
                                               $"araction.action_code as LastActionCode, " +
                                               $"araction.action_date as LastActionDate " +
                                               $"FROM tenagree " +
                                               $"LEFT JOIN arag " +
                                               $"ON arag.tag_ref = tenagree.tag_ref " +
                                               $"LEFT JOIN contacts " +
                                               $"ON contacts.tag_ref = tenagree.tag_ref " +
                                               $"LEFT JOIN araction " +
                                               $"ON araction.tag_ref = tenagree.tag_ref " +
                                               $"WHERE tenagree.tag_ref IN ('{tenancyRefs.Join("', '")}') " +
                                               $"ORDER BY arag.start_date DESC, araction.action_date DESC")
                .ToList();

        }
    }
}
