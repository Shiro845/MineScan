using CommunityToolkit.Mvvm.ComponentModel;

namespace MineScan.Models;

public class SelectedDifficulty : ObservableObject
{
    public static SelectedDifficulty Instance { get; } = new();
    public GameDifficulty ActualDifficulty { get; set; } = GameDifficulty.Easy;

    public Statistics EasyStats { get; } = new();
    public Statistics MediumStats { get; } = new();
    public Statistics HardStats { get; } = new();
    
    public Statistics GetCurrentStats() => ActualDifficulty switch
    {
        GameDifficulty.Easy => EasyStats,
        GameDifficulty.Medium => MediumStats,
        GameDifficulty.Hard => HardStats,
        _ => EasyStats
    };
}