using System;
using System.Data.SqlClient;
using System.ServiceModel;
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
            var response = await _actionDiaryService.CreateArrearsActionAsync(request).ConfigureAwait(false);
            if (_actionDiaryService.State != CommunicationState.Closed)
                _actionDiaryService.Close();
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
    }
}
