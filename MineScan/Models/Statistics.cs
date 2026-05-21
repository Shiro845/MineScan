using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MineScan.Models;

public class Statistics : ObservableObject
{
    public int GamesPlayed
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged(nameof(GamesPlayed));
            OnPropertyChanged(nameof(WinRate));
        }
    }    
    public int GamesWon
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged(nameof(GamesWon));
            OnPropertyChanged(nameof(WinRate));
        }
    }
    
    public int BestTime
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged(nameof(BestTime));
            OnPropertyChanged(nameof(BestTimeText));
        }
    }
    
    public string BestTimeText => BestTime == 0 
        ? "---"
        : TimeSpan.FromSeconds(BestTime).ToString(@"mm\:ss");
    
    public string WinRate => GamesPlayed > 0
        ? $"{(double)GamesWon / GamesPlayed * 100:0}%"
        : "---";
}