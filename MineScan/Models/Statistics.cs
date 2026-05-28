using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MineScan.Models;

public class Statistics : ObservableObject
{
    public int GamesPlayed
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(WinRate));
        }
    }

    public int GamesWon
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(WinRate));
        }
    }

    public int BestTime
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
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