using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using MineScan.Models;

namespace MineScan.ViewModels;

public class GameBoardViewModel : ViewModelBase
{
    public void GoBack() => NavigationService.Instance.NavigateTo<MainMenuViewModel>();
    public void Restart() => NavigationService.Instance.NavigateTo<GameBoardViewModel>();
    
    public ICommand OpenCellCommand { get; set; }
    public ICommand FlagCellCommand { get; set; }
    public ICommand RadarPingCommand { get; set; }
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
    public bool RadarUsed
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged(nameof(RadarUsed));
        }
    }
    
    public GameBoardViewModel()
    {
        var difficulty = SelectedDifficulty.Instance.ActualDifficulty;
        sbyte width, height, mines;

        Win = false;
        Lose = false;
        RadarUsed = false;
        
        switch (difficulty)
        {
            case GameDifficulty.Easy:
                width = 9;
                height = 9;
                mines = 10;
                break;
            case GameDifficulty.Medium:
                width = 15;
                height = 15;
                mines = 30;
                break;
            case GameDifficulty.Hard:
                width = 20;
                height = 20;
                mines = 60;
                break;
            default:
                width = 9;
                height = 9;
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
        RadarPingCommand = new RelayCommand(UseRadarPing);
    }

    private void UseRadarPing()
    {
        if (Lose || Win) return;
        
        RadarUsed = true;
        
        var hiddenMines = Cells.Where(cell => cell is { IsMine: true, IsFlagged: false, IsOpen: false }).ToList();
    
        if (hiddenMines.Any())
        {
            var randomMine = hiddenMines[Random.Shared.Next(hiddenMines.Count)];
        
            randomMine.IsRadarScanning = true;
        
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                Dispatcher.UIThread.Post(() =>
                {
                    randomMine.IsRadarScanning = false;
                });
            });
        }
    }
}