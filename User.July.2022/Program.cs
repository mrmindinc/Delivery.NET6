using MindMap;

var wifi = new Wifi(new[] { "MrMind", "mari 2G" });
await wifi.GetListAsync();

if (string.IsNullOrEmpty(wifi.Name) is false)
{
    await wifi.SetAsync("off");
}