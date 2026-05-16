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
    
    public string WinRate => GamesPlayed > 0
        ? $"{(double)GamesWon / GamesPlayed * 100:0}%"
        : "0%";
}