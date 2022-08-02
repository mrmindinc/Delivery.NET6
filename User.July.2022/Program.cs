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
    wave.RecordingStopped += (sender, e) =>
    {
        if (e.Exception is not null)
            Console.WriteLine(e.Exception.Message);
    };
    wave.DataAvailable += (sender, e) =>
    {
        
    };
    wave.StartRecordingAsync(new CancellationTokenSource());
    Console.WriteLine(Console.ReadLine());
}