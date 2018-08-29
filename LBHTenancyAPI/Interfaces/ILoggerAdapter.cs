using System;
namespace LBHTenancyAPI.Interfaces
{
    public interface ILoggerAdapter<T>
    {
        // add just the logger methods your app uses
        void LogInformation(string message);

        void LogError(string message, params object[] args);
    }
}
