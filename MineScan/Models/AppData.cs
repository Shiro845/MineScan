namespace MineScan.Models;
public class AppData
{
    public static AppData Instance { get; } = new();
    
    public bool IsRadarDisabled { get; set; }
    public bool IsMinecraftFont { get; set; }
    public bool IsFullscreen { get; set; }
    public string CurrentLanguage { get; set; } = "English";
    public string CurrentTheme { get; set; } = "Light";

    public Statistics EasyStats { get; set; } = new();
    public Statistics MediumStats { get; set; } = new();
    public Statistics HardStats { get; set; } = new();
}