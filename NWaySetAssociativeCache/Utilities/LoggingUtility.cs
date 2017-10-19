using log4net.Core;
using log4net.Appender;

namespace NWaySetAssociativeCache
{
    internal static class LoggingUtility
    {
        internal static void LogData(AppenderSkeleton appender, string message, Level trace)
        {
            LoggingEventData eventData = new LoggingEventData()
            {
                Level = trace,
                Message = message
            };

            appender.DoAppend(new LoggingEvent(eventData));
        }
    }
}
