using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MineScan.Models;

namespace MineScan.ViewModels;

public class GameBoardViewModel : ViewModelBase
{
    public void GoBack() => NavigationService.Instance.NavigateTo<MainMenuViewModel>();
    public void Restart() => NavigationService.Instance.NavigateTo<GameBoardViewModel>();
    
    public ICommand OpenCellCommand { get; set; }
    public ICommand FlagCellCommand { get; set; }
    
    public MineField MineField { get; }

    public List<Cell> Cells
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged(nameof(Cells));
        }
    }

    public bool Win
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged(nameof(Win));
        }
    }
    public bool Lose
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged(nameof(Lose));
        }
    }

    public GameBoardViewModel()
    {
        var difficulty = SelectedDifficulty.Instance.ActualDifficulty;
        sbyte width, height, mines;

        Win = false;
        Lose = false;
        
        switch (difficulty)
        {
            case GameDifficulty.Easy:
                width = 8 ;
                height = 8 ;
                mines = 10 ;
                break;
            case GameDifficulty.Medium:
                width = 12 ;
                height = 12 ;
                mines = 20 ;
                break;
            case GameDifficulty.Hard:
                width = 16 ;
                height = 16 ;
                mines = 30 ;
                break;
            default:
                width = 8;
                height = 8;
                mines = 10;
                break;
        }
        
        MineField = new MineField(width, height);
        Cells = MineField.ToCellList();
        
        OpenCellCommand = new RelayCommand<Cell>(cell =>
        {
            if (cell != null)
            {
                if (cell.IsOpen) { MineField.Chording(cell.X, cell.Y); }
                if (!MineField.IsMinesSpawned) { MineField.SpawnMines(mines, cell.X, cell.Y); }
                MineField.OpenCell(cell.X, cell.Y);

                if (MineField.IsExploded)
                {
                    SelectedDifficulty.Instance.GetCurrentStats().GamesPlayed++;
                    Lose = true;
                }

                else if (MineField.IsWon)
                {
                    SelectedDifficulty.Instance.GetCurrentStats().GamesPlayed++;
                    SelectedDifficulty.Instance.GetCurrentStats().GamesWon++;
                    Win = true;
                }
            }
        });
        FlagCellCommand = new RelayCommand<Cell>(cell =>
        {
            if (cell != null)
            {
                MineField.ToggleFlag(cell.X, cell.Y);
            }
        });
    }
}