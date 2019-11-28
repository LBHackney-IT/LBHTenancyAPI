using System;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.UseCases.V2.ArrearsActions.Models;

namespace LBHTenancyAPI.Gateways.V2.Arrears.Impl
{
    /// <summary>
    /// ArrearsActionDiaryGateway marshalls calls to the Database for reads and Web Service for writes
    /// </summary>
    public class ArrearsActionDiaryGateway : IArrearsActionDiaryGateway
    {
        /// <summary>
        /// WCF Service Interface which allows us to create action diary entries
        /// </summary>
        private readonly IArrearsAgreementServiceChannel _actionDiaryService;

        private readonly string _connectionString;
        public ArrearsActionDiaryGateway(IArrearsAgreementServiceChannel actionDiaryService, string connectionString)
        {
            _actionDiaryService = actionDiaryService;
            _connectionString = connectionString;
        }

        public async Task<ArrearsActionResponse> CreateActionDiaryEntryAsync(ActionDiaryRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request is null");

            var diaryEntry = AddActionDiaryEntry(
                request.ActionCode,
                request.Comment,
                request.TenancyAgreementRef,
                request.Username
            );

            var response = new ArrearsActionResponse();

            if (diaryEntry == 1)
            {
                response.Success = true;
                response.ArrearsAction = new ArrearsActionLogDto();
                response.ArrearsAction.TenancyAgreementRef = request.TenancyAgreementRef;
                response.ArrearsAction.ActionCode = request.ActionCode;
                response.ArrearsAction.UserName = request.Username;
            }
            else
            {
                response.ErrorCode = 1;
                response.ErrorMessage = "Failed to add entry into action diary";
            }
            return response;
        }

        private int AddActionDiaryEntry(string actionCode, string comment, string tenancyAgreementRef, string username)
        {
                int rows;
                string insertQuery = @"
                    WITH context_cte (
                        tag_ref,
                        max_action_set,
                        max_action_no,
                        cur_bal
                    ) AS (
                        SELECT
    	                    TOP 1 tenagree.tag_ref,
    	                    ISNULL(action_set, 0) AS max_action_set,
    	                    ISNULL(action_no, 0) AS max_action_no,
    	                    tenagree.cur_bal
                        FROM
    	                    tenagree
                        LEFT JOIN araction ON tenagree.tag_ref = araction.tag_ref
                        WHERE
    	                    tenagree.tag_ref = @tag_ref
                        ORDER BY
    	                    action_set DESC,
    	                    action_no DESC
                    )
                    INSERT INTO araction (
                        tag_ref, action_set, action_no, action_code, action_date, action_balance, action_comment, 
                        username, comm_only, araction_sid, action_deferred, deferred_until, deferral_reason, 
                        severity_level, action_nr_balance, action_type, act_status, action_cat, action_subno, 
                        action_subcode, action_process_no, notice_sid, courtord_sid, warrant_sid, action_doc_no, 
                        comp_avail, comp_display
                    )
                    SELECT DISTINCT
                        context_cte.tag_ref AS tag_ref,
                        context_cte.max_action_set as action_set,
                        context_cte.max_action_no + 1 as action_no,
                        @action_code as action_code,
                        CURRENT_TIMESTAMP as action_date,
                        context_cte.cur_bal as action_balance,
                        @action_comment as action_comment,
                        @username as username,
                        1 as comm_only,
                        (SELECT MAX(araction_sid) FROM araction) +1,
                        0 AS action_deferred,
                        0 AS deferred_until,
                        '' AS deferral_reason,
                        1 AS severity_level,
                        0 AS action_nr_balance,
                        9 AS action_type,
                        '001' AS act_status,
                        8 AS action_cat,
                        1 AS action_subno,
                        '' AS action_subcode,
                        0 AS action_process_no,
                        0 AS notice_sid,
                        0 AS courtord_sid,
                        0 AS warrant_sid,
                        0 AS action_doc_no,
                        '' AS comp_avail,
                        '' AS comp_display
                    FROM
                        context_cte;
                ";

            using (var conn = new SqlConnection(_connectionString))
            {
                SqlDataAdapter cmd = null;
                cmd = new SqlDataAdapter();
                cmd.InsertCommand = new SqlCommand(insertQuery);
                cmd.InsertCommand.Connection = conn;

                conn.Open();

                using (cmd)
                {
                    cmd.InsertCommand.Parameters.AddWithValue("@action_comment", comment);
                    cmd.InsertCommand.Parameters.AddWithValue("@username", username);
                    cmd.InsertCommand.Parameters.AddWithValue("@action_code", actionCode);
                    cmd.InsertCommand.Parameters.AddWithValue("@tag_ref", tenancyAgreementRef);
                    rows = cmd.InsertCommand.ExecuteNonQuery();
                }
            }
            return rows;
        }
    }
}
