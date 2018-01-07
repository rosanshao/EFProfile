namespace EFlogger.Profiling
{
    public class SqlLogRecord
    {
        public string CommandText { get; set; }

        public int ResultRowsCount { get; set; }

        public long QueryMiliseconds { get; set; }

        public string Created { get; set; }

        public string MethodName { get; set; }

        public string ClassName { get; set; }

        public string MethodBody { get; set; }

        public string StackTrace { get; set; }
    }
}
