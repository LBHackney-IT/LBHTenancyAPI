using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LBH.Data.Domain;
using LBHTenancyAPI.Extensions.String;
using LBHTenancyAPI.UseCases.Search.Models;

namespace LBHTenancyAPI.Gateways.Search
{
    public class SearchGateway : ISearchGateway
    {
        private readonly string _connectionString;
        public SearchGateway(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<PagedResults<TenancyListItem>> SearchTenanciesAsync(SearchTenancyRequest request, CancellationToken cancellationToken)
        {
            var results = new PagedResults<TenancyListItem>();

            var validate = request.Validate(request);
            if(!validate.IsValid)
                return results;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var queryStringBuilder = new StringBuilder();
                var whiteSpace = "                    ";

                if (request.TenancyRef.IsNotNullOrEmptyOrWhiteSpace())
                    queryStringBuilder.AppendLine($"{whiteSpace}DECLARE @lowerTenancyRef nvarchar(16); SET @lowerTenancyRef = LOWER(@tenancyRef);");
                if (request.FirstName.IsNotNullOrEmptyOrWhiteSpace())
                    queryStringBuilder.AppendLine($"{whiteSpace}DECLARE @lowerFirstName nvarchar(64); SET @lowerFirstName = LOWER(@firstName);");
                if (request.LastName.IsNotNullOrEmptyOrWhiteSpace())
                    queryStringBuilder.AppendLine($"{whiteSpace}DECLARE @lowerLastName nvarchar(64); SET @lowerLastName = LOWER(@lastName);");
                if (request.Address.IsNotNullOrEmptyOrWhiteSpace())
                    queryStringBuilder.AppendLine($"{whiteSpace}DECLARE @lowerAddress nvarchar(128); SET @lowerAddress = LOWER(@address);");
                if (request.PostCode.IsNotNullOrEmptyOrWhiteSpace())
                    queryStringBuilder.AppendLine($"{whiteSpace}DECLARE @lowerPostCode nvarchar(16); SET @lowerPostCode = LOWER(@postCode);");

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
                        FROM tenagree
                        Left JOIN dbo.member member WITH(NOLOCK)
                        ON member.house_ref = tenagree.house_ref
                        LEFT JOIN property WITH(NOLOCK)
                        ON property.prop_ref = tenagree.prop_ref
                        WHERE tenagree.tag_ref IS NOT NULL
");

                if (request.TenancyRef.IsNotNullOrEmptyOrWhiteSpace())
                    queryStringBuilder.AppendLine($"{whiteSpace}AND LOWER(tenagree.tag_ref) = @lowerTenancyRef");
                if (request.FirstName.IsNotNullOrEmptyOrWhiteSpace())
                    queryStringBuilder.AppendLine($"{whiteSpace}AND LOWER(member.forename) = @lowerFirstName");
                if (request.LastName.IsNotNullOrEmptyOrWhiteSpace())
                    queryStringBuilder.AppendLine($"{whiteSpace}AND LOWER(member.surname) = @lowerLastName");
                if (request.Address.IsNotNullOrEmptyOrWhiteSpace())
                    queryStringBuilder.AppendLine($"{whiteSpace}AND LOWER(property.short_address) like '%'+ @lowerAddress +'%'");
                if (request.PostCode.IsNotNullOrEmptyOrWhiteSpace())
                    queryStringBuilder.AppendLine($"{whiteSpace}AND LOWER(property.post_code) like '%'+ @lowerPostCode +'%'");
                queryStringBuilder.Append(@"
                    )
                    orderByWithSequenceSubQuery
                    WHERE Seq BETWEEN @Lower AND @Upper");
                var all = await conn.QueryAsync<TenancyListItem>(queryStringBuilder.ToString(),
                    new { tenancyRef = request.TenancyRef, firstName = request.FirstName, lastName = request.LastName, address = request.Address, postcode = request.PostCode, page = request.Page > 0 ? request.Page - 1: 0, pageSize = request.PageSize }
                ).ConfigureAwait(false);

                results.Results = all?.ToList();

                var countStringBuilder = new StringBuilder();
                countStringBuilder.AppendLine($"{whiteSpace}");
                if (request.TenancyRef.IsNotNullOrEmptyOrWhiteSpace())
                    countStringBuilder.AppendLine($"{whiteSpace}DECLARE @lowerTenancyRef nvarchar(16); SET @lowerTenancyRef = LOWER(@tenancyRef);");
                if (request.FirstName.IsNotNullOrEmptyOrWhiteSpace())
                    countStringBuilder.AppendLine($"{whiteSpace}DECLARE @lowerFirstName nvarchar(64); SET @lowerFirstName = LOWER(@firstName);");
                if (request.LastName.IsNotNullOrEmptyOrWhiteSpace())
                    countStringBuilder.AppendLine($"{whiteSpace}DECLARE @lowerLastName nvarchar(64); SET @lowerLastName = LOWER(@lastName);");
                if (request.Address.IsNotNullOrEmptyOrWhiteSpace())
                    countStringBuilder.AppendLine($"{whiteSpace}DECLARE @lowerAddress nvarchar(128); SET @lowerAddress = LOWER(@address);");
                if (request.PostCode.IsNotNullOrEmptyOrWhiteSpace())
                    countStringBuilder.AppendLine($"{whiteSpace}DECLARE @lowerPostCode nvarchar(16); SET @lowerPostCode = LOWER(@postCode);");

                countStringBuilder.Append(
                    @"
                    SELECT count(tenagree.tag_ref)
                    FROM tenagree
                    Left JOIN dbo.member member WITH(NOLOCK)
                    ON member.house_ref = tenagree.house_ref
                    LEFT JOIN property WITH(NOLOCK)
                    ON property.prop_ref = tenagree.prop_ref
                    WHERE tenagree.tag_ref IS NOT NULL");
                if (request.TenancyRef.IsNotNullOrEmptyOrWhiteSpace())
                    queryStringBuilder.AppendLine("AND LOWER(tenagree.tag_ref) = @lowerTenancyRef");
                if (request.FirstName.IsNotNullOrEmptyOrWhiteSpace())
                    queryStringBuilder.AppendLine("AND LOWER(member.forename) = @lowerFirstName");
                if (request.LastName.IsNotNullOrEmptyOrWhiteSpace())
                    queryStringBuilder.AppendLine("AND LOWER(member.surname) = @lowerLastName");
                if (request.Address.IsNotNullOrEmptyOrWhiteSpace())
                    queryStringBuilder.AppendLine("AND LOWER(property.short_address) like '%'+ @lowerAddress +'%'");
                if (request.PostCode.IsNotNullOrEmptyOrWhiteSpace())
                    queryStringBuilder.AppendLine("AND LOWER(property.post_code) like  '%'+ @lowerPostCode +'%'");

                var totalCount = await conn.QueryAsync<int>(countStringBuilder.ToString(),
                    new { tenancyRef = request.TenancyRef, firstName = request.FirstName, lastName = request.LastName, address = request.Address, postcode = request.PostCode}
                ).ConfigureAwait(false);
                results.TotalResultsCount = totalCount.Sum();
                conn.Close();
            }

            return results;
        }


    }
}
