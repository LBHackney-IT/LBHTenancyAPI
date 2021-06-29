using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LBH.Data.Domain;
using LBHTenancyAPI.Extensions.String;
using LBHTenancyAPI.UseCases.V2.Search.Models;

namespace LBHTenancyAPI.Gateways.V2.Search
{
    /// <summary>
    /// Search Gateway V2 Searches for tenants attached to tenancies
    /// Currently connects to UH Database via SQL connection and SQL queries
    /// </summary>
    public class SearchGateway : ISearchGateway
    {
        private readonly string _connectionString;
        public SearchGateway(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Search for Tenants attached to tenancies
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedResults<TenancyListItem>> SearchTenanciesAsync(SearchTenancyRequest request, CancellationToken cancellationToken)
        {
            var results = new PagedResults<TenancyListItem>();

            var validate = request.Validate(request);
            if(!validate.IsValid)
                return results;

            string whiteSpace = "                    ";
            //Build actual query
            var queryStringBuilder = BuildQuery(request, whiteSpace);
            //Build query to find out total results count
            var countStringBuilder = BuildCountQuery(request, whiteSpace);

            using (var conn = new SqlConnection(_connectionString))
            {
                //open connection explicity
                conn.Open();
                //get paged results
                var all = await conn.QueryAsync<TenancyListItem>(queryStringBuilder.ToString(),
                    new { tenancyRef = request.TenancyRef, firstName = request.FirstName, lastName = request.LastName, address = request.Address, postcode = request.PostCode, page = request.Page > 0 ? request.Page - 1: 0, pageSize = request.PageSize }
                ).ConfigureAwait(false);

                //add to paged results
                results.Results = all?.ToList();

                //get results total count
                var totalCount = await conn.QueryAsync<int>(countStringBuilder.ToString(),
                    new { tenancyRef = request.TenancyRef, firstName = request.FirstName, lastName = request.LastName, address = request.Address, postcode = request.PostCode}
                ).ConfigureAwait(false);
                //add to pages results
                results.TotalResultsCount = totalCount.Sum();
                //close connection explicitly - do not pool connections
                //experienced sql connection issues with connection pooling due to UH database
                conn.Close();
            }

            return results;
        }

        /// <summary>
        /// Builds the query to return the TotalCount of the resultset
        /// A stored procedure would be much faster but we are unable to modify the UH Database
        /// in any way or risk breaking the support agreement
        /// </summary>
        /// <param name="request"></param>
        /// <param name="whiteSpace"></param>
        /// <returns></returns>
        private static StringBuilder BuildCountQuery(SearchTenancyRequest request, string whiteSpace)
        {
            var countStringBuilder = new StringBuilder();
            countStringBuilder.AppendLine($"{whiteSpace}");
            if (request.TenancyRef.IsNotNullOrEmptyOrWhiteSpace())
                countStringBuilder.AppendLine(
                    $"{whiteSpace}DECLARE @lowerTenancyRef nvarchar(16); SET @lowerTenancyRef = LOWER(@tenancyRef);");
            if (request.FirstName.IsNotNullOrEmptyOrWhiteSpace())
                countStringBuilder.AppendLine(
                    $"{whiteSpace}DECLARE @lowerFirstName nvarchar(64); SET @lowerFirstName = LOWER(@firstName);");
            if (request.LastName.IsNotNullOrEmptyOrWhiteSpace())
                countStringBuilder.AppendLine(
                    $"{whiteSpace}DECLARE @lowerLastName nvarchar(64); SET @lowerLastName = LOWER(@lastName);");
            if (request.Address.IsNotNullOrEmptyOrWhiteSpace())
                countStringBuilder.AppendLine(
                    $"{whiteSpace}DECLARE @lowerAddress nvarchar(128); SET @lowerAddress = LOWER(@address);");
            if (request.PostCode.IsNotNullOrEmptyOrWhiteSpace())
                countStringBuilder.AppendLine(
                    $"{whiteSpace}DECLARE @lowerPostCode nvarchar(16); SET @lowerPostCode = LOWER(@postCode);");

            countStringBuilder.Append(
                @"
                    SELECT count(tenagree.tag_ref)
                    FROM MATenancyAgreement tenagree
                    Left JOIN MAMember member WITH(NOLOCK)
                    ON member.house_ref = tenagree.house_ref AND member.responsible = 1
                    LEFT JOIN MAProperty property WITH(NOLOCK)
                    ON property.prop_ref = tenagree.prop_ref
                    WHERE tenagree.tag_ref IS NOT NULL
                    ");
            if (request.TenancyRef.IsNotNullOrEmptyOrWhiteSpace())
                countStringBuilder.AppendLine($"{whiteSpace}AND LOWER(tenagree.tag_ref) = @lowerTenancyRef");
            if (request.FirstName.IsNotNullOrEmptyOrWhiteSpace())
                countStringBuilder.AppendLine($"{whiteSpace}AND LOWER(member.forename) = @lowerFirstName");
            if (request.LastName.IsNotNullOrEmptyOrWhiteSpace())
                countStringBuilder.AppendLine($"{whiteSpace}AND LOWER(member.surname) = @lowerLastName");
            if (request.Address.IsNotNullOrEmptyOrWhiteSpace())
                countStringBuilder.AppendLine($@"{whiteSpace}AND LOWER(property.short_address) like '%' + @lowerAddress + '%'");
            if (request.PostCode.IsNotNullOrEmptyOrWhiteSpace())
                countStringBuilder.AppendLine($@"{whiteSpace}AND LOWER(property.post_code) like '%' + @lowerPostCode + '%'");
            return countStringBuilder;
        }

        /// <summary>
        /// Builds a query to return the paged subset of the resultset
        /// A stored procedure would be much faster but we are unable to modify the UH Database
        /// in any way or risk breaking the support agreement
        /// </summary>
        /// <param name="request"></param>
        /// <param name="whiteSpace"></param>
        /// <returns></returns>
        private static StringBuilder BuildQuery(SearchTenancyRequest request, string whiteSpace)
        {
            var queryStringBuilder = new StringBuilder();
            //Add conditional Parameter declarations to query
            queryStringBuilder.AppendLine($"{whiteSpace}");
            if (request.TenancyRef.IsNotNullOrEmptyOrWhiteSpace())
                queryStringBuilder.AppendLine(
                    $"{whiteSpace}DECLARE @lowerTenancyRef nvarchar(16); SET @lowerTenancyRef = LOWER(@tenancyRef);");
            if (request.FirstName.IsNotNullOrEmptyOrWhiteSpace())
                queryStringBuilder.AppendLine(
                    $"{whiteSpace}DECLARE @lowerFirstName nvarchar(64); SET @lowerFirstName = LOWER(@firstName);");
            if (request.LastName.IsNotNullOrEmptyOrWhiteSpace())
                queryStringBuilder.AppendLine(
                    $"{whiteSpace}DECLARE @lowerLastName nvarchar(64); SET @lowerLastName = LOWER(@lastName);");
            if (request.Address.IsNotNullOrEmptyOrWhiteSpace())
                queryStringBuilder.AppendLine(
                    $"{whiteSpace}DECLARE @lowerAddress nvarchar(128); SET @lowerAddress = LOWER(@address);");
            if (request.PostCode.IsNotNullOrEmptyOrWhiteSpace())
                queryStringBuilder.AppendLine(
                    $"{whiteSpace}DECLARE @lowerPostCode nvarchar(16); SET @lowerPostCode = LOWER(@postCode);");

            queryStringBuilder.Append(
                @"
                    DECLARE @Upper Integer;
                    DECLARE @Lower integer;

                    if(@page = 0) 
                    begin
                    Set @Lower = 1
                    Set @Upper = @Lower + @pageSize -1
                    end
                    if(@page > 0)
                    begin
                    Set @Lower = (@pageSize * @page) + 1
                    Set @Upper = @Lower + @pageSize -1
                    end

                    SELECT
                    Seq,
                    CurrentBalance,
                    TenancyRef,
                    PropertyRef,
                    Tenure,
                    PrimaryContactPostcode,
                    PrimaryContactShortAddress,
                    PrimaryContactName
                    FROM
                    (
                        SELECT
                        tenagree.cur_bal as CurrentBalance,
                        tenagree.tag_ref as TenancyRef,
                        tenagree.prop_ref as PropertyRef,
                        tenagree.tenure as Tenure,
                        property.post_code as PrimaryContactPostcode,
                        property.short_address as PrimaryContactShortAddress,
                        RTRIM(LTRIM(member.forename)) + ' ' + RTRIM(LTRIM(member.surname)) as PrimaryContactName,
                        ROW_NUMBER() OVER (ORDER BY member.surname, member.forename ASC) AS Seq
                        FROM MATenancyAgreement tenagree
                        Left JOIN MAMember member WITH(NOLOCK)
                        ON member.house_ref = tenagree.house_ref AND member.responsible = 1
                        LEFT JOIN MAProperty property WITH(NOLOCK)
                        ON property.prop_ref = tenagree.prop_ref
                        WHERE tenagree.tag_ref IS NOT NULL
");
            //Add conditional AND clauses to query
            if (request.TenancyRef.IsNotNullOrEmptyOrWhiteSpace())
                queryStringBuilder.AppendLine($"{whiteSpace}AND LOWER(tenagree.tag_ref) = @lowerTenancyRef");
            if (request.FirstName.IsNotNullOrEmptyOrWhiteSpace())
                queryStringBuilder.AppendLine($"{whiteSpace}AND LOWER(member.forename) = @lowerFirstName");
            if (request.LastName.IsNotNullOrEmptyOrWhiteSpace())
                queryStringBuilder.AppendLine($"{whiteSpace}AND LOWER(member.surname) = @lowerLastName");
            if (request.Address.IsNotNullOrEmptyOrWhiteSpace())
                queryStringBuilder.AppendLine($@"{whiteSpace}AND LOWER(property.short_address) like '%' + @lowerAddress + '%' ");
            if (request.PostCode.IsNotNullOrEmptyOrWhiteSpace())
                queryStringBuilder.AppendLine($"{whiteSpace}AND LOWER(property.post_code) like '%' + @lowerPostCode + '%' ");
            queryStringBuilder.Append(@"
                    )
                    orderByWithSequenceSubQuery
                    WHERE Seq BETWEEN @Lower AND @Upper");
            return queryStringBuilder;
        }
    }
}
