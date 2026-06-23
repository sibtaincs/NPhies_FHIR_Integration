using System;

namespace Nphies.Core.Brokers.Loggings
{
    public interface ILoggingBroker
    {
        void LogError(string message);
        void LogWarning(string message);
        void LogInfo(string message);
        void LogWarning(Exception exception);

        void LogCritical(Exception exception);
    }
}
