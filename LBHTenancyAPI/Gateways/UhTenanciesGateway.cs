using System;
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
            List<TenancyListItem> results = new List<TenancyListItem>();
            List<TenancyListItem> withoutActions = conn.Query<TenancyListItem>($"" +
                                               $"SELECT TOP 1 " +
                                               $"tenagree.tag_ref as TenancyRef, " +
                                               $"tenagree.cur_bal as CurrentBalance, " +
                                               $"arag.arag_status as ArrearsAgreementStatus, " +
                                               $"arag.start_date as ArrearsAgreementStartDate, " +
                                               $"contacts.con_name as PrimaryContactName, " +
                                               $"contacts.con_address as PrimaryContactShortAddress, " +
                                               $"contacts.con_postcode as PrimaryContactPostcode " +
                                               $"FROM tenagree " +
                                               $"LEFT JOIN arag " +
                                               $"ON arag.tag_ref = tenagree.tag_ref " +
                                               $"LEFT JOIN contacts " +
                                               $"ON contacts.tag_ref = tenagree.tag_ref " +
                                               $"WHERE tenagree.tag_ref IN ('{tenancyRefs.Join("', '")}') " +
                                               $"ORDER BY arag.start_date DESC")
                .ToList();


            foreach (TenancyListItem tenancyListItem in withoutActions)
            {
                TenancyListItem result = new TenancyListItem();

                 var row = conn.Query<TenancyListItem>(
                    $"SELECT TOP 1 " +
                    $"araction.tag_ref as TenancyRef, " +
                    $"araction.action_code as LastActionCode, " +
                    $"araction.action_date as LastActionDate " +
                    $"FROM araction " +
                    $"WHERE araction.tag_ref = ('{tenancyListItem.TenancyRef}')" +
                    $"ORDER BY araction.action_date DESC"
                ).ToList();

                result.TenancyRef = row[0].TenancyRef;
                result.LastActionCode = row[0].LastActionCode;
                result.LastActionDate = row[0].LastActionDate;
                result = tenancyListItem.mergeWith(result);

                results.Add(result);
            }

            return results;
        }
    }
}
