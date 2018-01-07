using System;
using EFlogger.Network.Commands;

namespace EFlogger.Profiling
{
    public static class Nlogger
    {
        private static Action<string> _action;
        private static Action<SqlLogRecord> _sqlLogRecordEvent;


        public static void AddLogMessage(string message)
        {
            if (_action != null)
                _action(message);
        }

        public static void AddLogMessage(SqlLogRecord logRecord)
        { 
            if (_sqlLogRecordEvent != null)
                _sqlLogRecordEvent(logRecord);
        }

        public static void SetSqlLogRecordDelegate(Action<SqlLogRecord> action)
        {
            _sqlLogRecordEvent = action;
        }

        public static void SetLogDelegate(Action<string> action)
        {
            _action = action;
        }
    }
}
