using CommunityToolkit.Mvvm.ComponentModel;

namespace MineScan.Models;

public class Cell : ObservableObject
{
    public bool IsMine
    {
        get => field;
        set => SetProperty(ref field, value);
    }

    public bool IsOpen
    {
        get => field;
        set => SetProperty(ref field, value);
    }
    

    public bool IsFlagged
    {
        get => field;
        set => SetProperty(ref field, value);
    }

    public sbyte MinesAround
    {
        get => field;
        set { SetProperty(ref field, value);
            OnPropertyChanged(nameof(MinesAroundText));
            OnPropertyChanged(nameof(NumberColor));}
    }
    public string MinesAroundText => MinesAround switch
    {
        0 => "",
        -1 => "",
        _ => MinesAround.ToString()
    };
    public string NumberColor
    {
        get
        {
            return MinesAround switch
            {
                1 => "Blue",
                2 => "Green",
                3 => "Red",
                4 => "DarkBlue",
                5 => "Maroon",
                6 => "Turquoise",
                7 => "Black",
                8 => "DimGray",
                _ => "Black"
            };
        }
    }
    public int X { get; set; }
    public int Y { get; set; }

    public bool IsRadarScanning
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }
}