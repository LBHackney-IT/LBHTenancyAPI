using System;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AgreementService;

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

        public async Task<ArrearsActionResponse> CreateActionDiaryEntryAsync(ArrearsActionCreateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request is null");

            var diaryEntry = AddActionDiaryEntry(
                request.ArrearsAction.ActionCode,
                request.ArrearsAction.Comment,
                request.ArrearsAction.TenancyAgreementRef,
                request.DirectUser.UserName
            );

            var response = new ArrearsActionResponse();

            if (diaryEntry >= 1)
            {
                response.Success = true;
            }
            else
            {
                response.ErrorCode = 1;
                response.ErrorMessage = "Failed to add entry into action diary";
            }
            return response;
        }

        public async Task UpdateRecordingDetails(string requestAppUser, int actionDiaryId, DateTime updateDate)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = null;
                if (string.IsNullOrWhiteSpace(requestAppUser))
                {
                    cmd = new SqlCommand("UPDATE araction SET action_date=@action_date" +
                                                    " WHERE araction_sid=@Id", conn);
                }
                else
                {
                    cmd = new SqlCommand("UPDATE araction SET username=@username, action_date=@action_date" +
                                                    " WHERE araction_sid=@Id", conn);
                }
                conn.Open();
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@Id", actionDiaryId);
                    if(!string.IsNullOrWhiteSpace(requestAppUser))
                    {
                        cmd.Parameters.AddWithValue("@username", requestAppUser);
                    }
                    cmd.Parameters.AddWithValue("@action_date", updateDate);
                    int rows = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
        }

        private int AddActionDiaryEntry(string actionCode, string comment, string tenancyAgreementRef, string username)
        {
                int rows;
                string insertQuery = @"
                DECLARE @sid int
                SET @sid = (SELECT MAX(araction_sid) FROM araction)

                DECLARE @action_no int
                SET @action_no = (SELECT MAX(action_no) FROM araction WHERE tag_ref = @tag_ref)

                DECLARE @current_balance numeric(9,2)
                SET @current_balance = (SELECT cur_bal FROM tenagree WHERE tag_ref = @tag_ref)

                INSERT INTO araction (tag_ref, action_set, action_no, action_code,
                    action_date, action_balance, action_comment, username, comm_only, araction_sid,
                    action_deferred, deferred_until, deferral_reason, severity_level, action_nr_balance,
                    action_type, act_status, action_cat, action_subno, action_subcode, action_process_no,
                    notice_sid, courtord_sid, warrant_sid, action_doc_no, comp_avail, comp_display)
                VALUES (@tag_ref, 1, (@action_no+1), @action_code,
                    GETDATE(), @current_balance, @action_comment, @username, 1, (@sid+1),
                    0, 0, '', 1, 0, 9, '001', 8, 1, '', 0,0, 0, 0, 0, '', '');
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

//    cmd.InsertCommand.Parameters.Add("@SourceName", SqlDbType.VarChar).Value = entry.Source;
}
