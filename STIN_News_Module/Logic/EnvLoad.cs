namespace STIN_News_Module.Logic
{
    public class EnvLoad
    {
        public static void Load()
        {
            var envFile = Path.Combine(Directory.GetCurrentDirectory(), ".env");
            if (!File.Exists(envFile))
            {
                return;
            }
            var lines = File.ReadAllLines(envFile);
            foreach (var line in lines)
            {
                var parts = line.Split("=");
                if (parts.Length != 2)
                {
                    continue;
                }
                var key = parts[0].Trim();
                var value = parts[1].Trim();
                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }
}
