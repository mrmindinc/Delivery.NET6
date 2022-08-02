﻿using MindMap;
using MindMap.Device;

using System.Diagnostics;

var index = 0;

if (Console.ReadLine() is string volume)
{
    if (volume[^1] is '%' && char.IsDigit(volume[0]) && volume[0] > '0')
        using (var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = "pactl",
                Arguments = $"set-source-volume 0 {volume}",
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
    using (var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            UseShellExecute = false,
            FileName = "amixer",
            Arguments = "sget Capture",
            RedirectStandardOutput = true
        }
    })
    {
        process.OutputDataReceived += (sender, e) =>
        {
            Console.WriteLine(index++);
            Console.WriteLine(e.Data);
        };
        if (process.Start())
        {
            process.BeginOutputReadLine();
            await process.WaitForExitAsync();
        }
    }
}
using (SoundDevice wave = new UnixSoundDevice(new SoundConnectionSettings
{
    RecordingSampleRate = 16000,
    RecordingChannels = 1
}))
{
    var source = new CancellationTokenSource();
    wave.RecordingStopped += (sender, e) => Console.WriteLine(e.Exception?.Message);
    wave.DataAvailable += (sender, e) =>
    {
        var count = e.Buffer?.Count(o => o > 0);

        if (count > 0)
            Console.WriteLine(count);
    };
    wave.StartRecordingAsync(source);

    while (source.Token.IsCancellationRequested is false)
    {
        using (var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = "clear",
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
        await Task.Delay(0x400 * 5);
    }
}