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
    private MineField MineField { get; }

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

    public bool IsRadarTargeting
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged(nameof(IsRadarTargeting));
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

    public bool IsRadarDisabled
    {
        get => AppSettings.Instance.IsRadarDisabled;
        set
        {
            AppSettings.Instance.IsRadarDisabled = value;
            OnPropertyChanged(nameof(IsRadarDisabled));
        }
    }

private DispatcherTimer? _timer;

    public int SecondsPassed
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged(nameof(SecondsPassed));
            OnPropertyChanged(nameof(TimerText));
        }
    }
    public string TimerText => TimeSpan.FromSeconds(SecondsPassed).ToString(@"mm\:ss");
    
    private void StartTimer()
    {
        if (_timer != null) return;
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
    
        _timer.Tick += (_, _) => SecondsPassed++;
        _timer.Start();
    }

    private void StopTimer()
    {
        _timer?.Stop();
        _timer = null;
    }

    public sbyte Width { get; set; }
    public sbyte Height { get; set; }
    private sbyte Mines { get; }
    
    public GameBoardViewModel()
    {
        var difficulty = SelectedDifficulty.Instance.ActualDifficulty;
        var currentStats = SelectedDifficulty.Instance.GetCurrentStats();
        
        switch (difficulty)
        {
            case GameDifficulty.Easy:
                Width = 9;
                Height = 9;
                Mines = 10;
                break;
            case GameDifficulty.Medium:
                Width = 16;
                Height = 16;
                Mines = 40;
                break;
            case GameDifficulty.Hard:
                Width = 30;
                Height = 20;
                Mines = 120;
                break;
            default:
                Width = 9;
                Height = 9;
                Mines = 10;
                break;
        }
        
        MineField = new MineField(Width, Height);
        Cells = MineField.ToCellList();
        
        OpenCellCommand = new RelayCommand<Cell>(cell =>
        {
            if (cell != null)
            {
                if (IsRadarTargeting)
                {
                    ExecuteRadarAt(cell);
                    return;
                }
                
                if (cell.IsOpen) { MineField.Chording(cell.X, cell.Y); }

                if (!MineField.IsMinesSpawned)
                {
                    MineField.SpawnMines(Mines, cell.X, cell.Y);
                    StartTimer();
                }
                MineField.OpenCell(cell.X, cell.Y);
                
                if (MineField.IsExploded)
                {
                    StopTimer();
                    currentStats.GamesPlayed++;
                    Lose = true;
                }

                else if (MineField.IsWon)
                {
                    StopTimer();
                    currentStats.GamesPlayed++;
                    currentStats.GamesWon++;
                    if (currentStats.BestTime == 0 || SecondsPassed < SelectedDifficulty.Instance.GetCurrentStats().BestTime)
                    {
                        currentStats.BestTime = SecondsPassed;
                    }
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
        if (Lose || Win || RadarUsed) return;

        IsRadarTargeting = !IsRadarTargeting;
    }
    
    private void ExecuteRadarAt(Cell centerCell)
    {
        if (!IsRadarTargeting || RadarUsed) return;

        IsRadarTargeting = false;
        RadarUsed = true;

        var targetX = centerCell.X;
        var targetY = centerCell.Y;

        var minesInRadius = Cells.Where(cell => 
            Math.Abs(cell.X - targetX) <= 1 && 
            Math.Abs(cell.Y - targetY) <= 1 && 
            cell.IsMine &&
            !cell.IsFlagged &&
            !cell.IsOpen).ToList();

        if (minesInRadius.Any())
        {
            foreach (var mine in minesInRadius)
            {
                mine.IsRadarScanning = true;
            }

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                Dispatcher.UIThread.Post(() =>
                {
                    foreach (var mine in minesInRadius)
                    {
                        mine.IsRadarScanning = false;
                    }
                });
            });
        }
    }
}