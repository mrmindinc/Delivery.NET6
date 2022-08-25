using System.Diagnostics;

namespace MindMap
{
    class Wifi
    {
        internal async Task GetListAsync()
        {
            using (var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    FileName = "nmcli",
                    Arguments = "dev wifi list",
                    RedirectStandardOutput = true
                }
            })
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    var use = false;

                    if (string.IsNullOrEmpty(e.Data) is false)
                        foreach (var str in e.Data.Split(' '))
                            if (str.Trim().Length > 0)
                            {
                                if (Array.Exists(names, o => o.Equals(str)) && use)
                                {
                                    Name = str;

                                    return;
                                }
                                use = str.Length == 1 && str[0] == '*';
                            }
                };
                if (process.Start())
                {
                    process.BeginOutputReadLine();
                    await process.WaitForExitAsync();
                }
            }
        }
        internal async Task SetAsync(string arg)
        {
            psi.ArgumentList.Add($"echo rock | sudo -S nmcli radio wifi {arg}");

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
        internal Wifi(IEnumerable<string> names)
        {
            psi = new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = "/bin/bash",
                RedirectStandardOutput = true
            };
            psi.ArgumentList.Add("-c");
            this.names = names.ToArray();
        }
        internal string? Name
        {
            get; private set;
        }
        readonly ProcessStartInfo psi;
        readonly string[] names;
    }
}