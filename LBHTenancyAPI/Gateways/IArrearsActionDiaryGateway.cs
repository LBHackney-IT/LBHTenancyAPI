using System;
namespace LBHTenancyAPI.Gateways
{
    public interface IArrearsActionDiaryGateway
    {
        bool CreateActionDiaryEntry();
    }

    public class ArrearsActionDiaryGateway : IArrearsActionDiaryGateway
    {
        public bool CreateActionDiaryEntry()
        {
            throw new NotImplementedException();
        }
    }
}
