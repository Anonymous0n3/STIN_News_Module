namespace STIN_News_Module.Logic.Logginf
{
    public static class LogStore
    {
        private static List<string> logs = new();
        private static readonly object _lock = new();

        public static void AddLog(string message)
        {
            lock (_lock)
            {
                logs.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
                if (logs.Count > 1000) logs.RemoveAt(0); // limit na 1000 záznamů
            }
        }

        public static List<string> GetLogs()
        {
            lock (_lock)
            {
                return new List<string>(logs);
            }
        }
    }
}
