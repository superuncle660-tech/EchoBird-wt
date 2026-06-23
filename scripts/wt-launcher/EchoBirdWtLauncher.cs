using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

internal static class EchoBirdWtLauncher
{
    private static int Main(string[] args)
    {
        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string wtPath = Path.Combine(localAppData, "Microsoft", "WindowsApps", "wt.exe");
        if (!File.Exists(wtPath))
        {
            return 1;
        }

        string exeName = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]).ToLowerInvariant();
        bool isCodex = exeName.Contains("codex");
        string command = isCodex ? "codex" : "claude";
        string title = isCodex ? "Codex CLI" : "ClaudeCode";
        string commandLine = command + (args.Length == 0 ? string.Empty : " " + string.Join(" ", args.Select(QuoteForCmd)));

        var process = new ProcessStartInfo
        {
            FileName = wtPath,
            Arguments = "-w 0 new-tab --title " + QuoteForProcessArg(title) + " cmd /k " + QuoteForProcessArg(commandLine),
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process.Start(process);
        return 0;
    }

    private static string QuoteForCmd(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "\"\"";
        }

        if (value.IndexOfAny(new[] { ' ', '\t', '"', '&', '|', '<', '>', '^' }) < 0)
        {
            return value;
        }

        return "\"" + value.Replace("\"", "\\\"") + "\"";
    }

    private static string QuoteForProcessArg(string value)
    {
        return "\"" + value.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
    }
}
