namespace MineScan.Models;

public class AppSettings
{
    public static AppSettings Instance { get; } = new();
    
    public bool IsRadarDisabled { get; set; }
    public string CurrentLanguage { get; set; } = "English";
}