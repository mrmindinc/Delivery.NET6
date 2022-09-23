using System.Diagnostics;

namespace MindMap.Branch
{
    class Usim
    {
        internal Usim()
        {
            psi = new ProcessStartInfo
            {
                WorkingDirectory = "/home/rock",
                FileName = "lte_monitor",
                Arguments = "-i 300 -c 1 -f 1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            exists = new FileInfo(monitor).Exists;
        }
        internal async Task RunAsync()
        {
            if (exists is false)
            {
                await File.WriteAllBytesAsync(monitor, Properties.Resources.lte_monitor);
                await GrantExecutePermission();
            }
            using (var process = new Process
            {
                StartInfo = psi
            })
            {
                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);

                if (process.Start())
                {
                    process.BeginOutputReadLine();
                    await process.WaitForExitAsync();
                }
            }
        }
        async Task GrantExecutePermission()
        {
            using (var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = "/home/rock",
                    FileName = "chmod",
                    Arguments = $"755 {monitor}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            })
            {
                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);

                if (process.Start())
                {
                    process.BeginOutputReadLine();
                    await process.WaitForExitAsync();
                }
            }
        }
        readonly ProcessStartInfo psi;
        readonly bool exists;
        const string monitor = "/home/rock/lte_monitor";
    }
}