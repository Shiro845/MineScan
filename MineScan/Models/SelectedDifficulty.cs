using CommunityToolkit.Mvvm.ComponentModel;
using MineScan.Services;

namespace MineScan.Models;

public class SelectedDifficulty : ObservableObject
{
    public static SelectedDifficulty Instance { get; } = new();
    public GameDifficulty ActualDifficulty { get; set; } = GameDifficulty.Easy;

    public Statistics EasyStats => DataService.Instance.LocalData.EasyStats;
    public Statistics MediumStats => DataService.Instance.LocalData.MediumStats;
    public Statistics HardStats => DataService.Instance.LocalData.HardStats;
    
    public Statistics GetCurrentStats() => ActualDifficulty switch
    {
        GameDifficulty.Easy => EasyStats,
        GameDifficulty.Medium => MediumStats,
        GameDifficulty.Hard => HardStats,
        _ => EasyStats
    };
}