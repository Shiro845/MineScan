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

    public int CustomWidth { get; set; } = 16;
    public int CustomHeight { get; set; } = 16;
    public int CustomMines { get; set; } = 40;

    public Statistics CustomStats { get; set; } = new();
    public Statistics ExtremeStats { get; set; } = new();
}