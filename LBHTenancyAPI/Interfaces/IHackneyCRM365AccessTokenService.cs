using System;
using System.Threading.Tasks;
using LBH.Data.Domain;

namespace LBHTenancyAPI.Interfaces
{
    public interface IHackneyCRM365AccessTokenService
    {
        Task<string> GetAccessToken(CRM365AccessTokenRequest accessTokenRequest);
    }
}
