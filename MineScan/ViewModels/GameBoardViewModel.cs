using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using MineScan.Models;
using MineScan.Services;

namespace MineScan.ViewModels;

public class GameBoardViewModel : ViewModelBase
{
    public void GoBack() => NavigationService.Instance.NavigateTo<MainMenuViewModel>();
    public void Restart() => NavigationService.Instance.NavigateTo<GameBoardViewModel>();

    public ICommand OpenCellCommand { get; set; }
    public ICommand FlagCellCommand { get; set; }
    public ICommand RadarPingCommand { get; set; }
    public ICommand CellPointerEnteredCommand { get; set; }
    public ICommand CellPointerExitedCommand { get; set; }
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

    public bool IsGhostMode
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged(nameof(IsGhostMode));
            foreach (var cell in Cells)
                cell.IsGhostVisible = value && cell.IsMine && !cell.IsOpen;
        }
    }

    public bool IsExtreme { get; private set; }

    public int ExtremeLosePhase
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged(nameof(ExtremeLosePhase));
            OnPropertyChanged(nameof(ShowExtremeJoke));
            OnPropertyChanged(nameof(ShowExtremeMercy));
        }
    }

    public bool ShowExtremeJoke => ExtremeLosePhase == 1;
    public bool ShowExtremeMercy => ExtremeLosePhase == 2;

    public double ExtremeProgress
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged(nameof(ExtremeProgress));
        }
    }

    public ICommand ExtremeMercyCommand { get; private set; }

    public bool IsRadarDisabled
    {
        get => DataService.Instance.LocalData.IsRadarDisabled;
        set
        {
            DataService.Instance.LocalData.IsRadarDisabled = value;
            OnPropertyChanged(nameof(IsRadarDisabled));
            DataService.Instance.Save();
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

    private void TriggerCascadeAnimation(int centerX, int centerY, HashSet<(int, int)> newlyOpened)
    {
        var queue = new Queue<(int x, int y, int level)>();
        var visited = new HashSet<(int, int)>();

        queue.Enqueue((centerX, centerY, 0));
        visited.Add((centerX, centerY));

        while (queue.Count > 0)
        {
            var (x, y, level) = queue.Dequeue();
            var cell = Cells.FirstOrDefault(c => c.X == x && c.Y == y);
            if (cell == null) continue;

            cell.RevealDelay = level * 40;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    var nx = x + dx;
                    var ny = y + dy;
                    if (visited.Contains((nx, ny))) continue;
                    if (!newlyOpened.Contains((nx, ny))) continue;
                    visited.Add((nx, ny));
                    queue.Enqueue((nx, ny, level + 1));
                }
            }
        }

        foreach (var (cx, cy) in newlyOpened)
        {
            var cell = Cells.FirstOrDefault(c => c.X == cx && c.Y == cy);
            if (cell == null) continue;
            var delay = cell.RevealDelay;
            Task.Run(async () =>
            {
                await Task.Delay(delay);
                Dispatcher.UIThread.Post(() => cell.IsRevealing = true);
                await Task.Delay(300);
                Dispatcher.UIThread.Post(() => cell.IsRevealing = false);
            });
        }
    }

    public void ToggleGhostMode()
    {
        IsGhostMode = !IsGhostMode;
    }

    public void AutoSolve()
    {
        if (Lose || Win) return;
        if (!MineField.IsMinesSpawned) return;

        var safeCells = Cells.Where(c => !c.IsMine && !c.IsOpen).ToList();
        if (safeCells.Count == 0) return;

        Task.Run(async () =>
        {
            foreach (var cell in safeCells)
            {
                await Task.Delay(30);
                Dispatcher.UIThread.Post(() =>
                {
                    if (Win || Lose) return;
                    var openBefore = new HashSet<(int, int)>(Cells.Where(c => c.IsOpen).Select(c => (c.X, c.Y)));
                    MineField.OpenCell(cell.X, cell.Y);
                    var newlyOpened = new HashSet<(int, int)>(
                        Cells.Where(c => c.IsOpen).Select(c => (c.X, c.Y)).Except(openBefore));
                    if (newlyOpened.Count > 0)
                        TriggerCascadeAnimation(cell.X, cell.Y, newlyOpened);
                });
            }

            await Task.Delay(safeCells.Count * 30 + 400);
            Dispatcher.UIThread.Post(() =>
            {
                if (!Win && MineField.IsWon)
                {
                    StopTimer();
                    var currentStats = SelectedDifficulty.Instance.GetCurrentStats();
                    currentStats.GamesPlayed++;
                    currentStats.GamesWon++;
                    DataService.Instance.Save();
                    Win = true;
                }
            });
        });
    }

    private void TriggerExtremeJoke()
    {
        ExtremeLosePhase = 1;
        ExtremeProgress = 0;
        Task.Run(async () =>
        {
            for (int i = 0; i <= 100; i++)
            {
                await Task.Delay(40);
                var i1 = i;
                Dispatcher.UIThread.Post(() => ExtremeProgress = i1);
            }
            await Task.Delay(600);
            Dispatcher.UIThread.Post(() =>
            {
                ExtremeLosePhase = 2;
                Lose = true;
            });
        });
    }

    public sbyte Width { get; set; }
    public sbyte Height { get; set; }
    private int Mines { get; }
    
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
            case GameDifficulty.Custom:
                Width = (sbyte)DataService.Instance.LocalData.CustomWidth;
                Height = (sbyte)DataService.Instance.LocalData.CustomHeight;
                Mines = DataService.Instance.LocalData.CustomMines;
                break;
            case GameDifficulty.Extreme:
                Width = 30;
                Height = 20;
                Mines = 150;
                IsExtreme = true;
                break;
            default:
                Width = 9;
                Height = 9;
                Mines = 10;
                break;
        }

        MineField = new MineField(Width, Height);
        Cells = MineField.ToCellList();

        ExtremeMercyCommand = new RelayCommand(() =>
        {
            ExtremeLosePhase = 0;
            Lose = false;
            NavigationService.Instance.NavigateTo<GameBoardViewModel>();
        });
        
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
                var openBefore = new HashSet<(int, int)>(Cells.Where(c => c.IsOpen).Select(c => (c.X, c.Y)));
                MineField.OpenCell(cell.X, cell.Y);
                var newlyOpened = new HashSet<(int, int)>(
                    Cells.Where(c => c.IsOpen).Select(c => (c.X, c.Y))
                         .Except(openBefore));
                if (newlyOpened.Count > 0)
                    TriggerCascadeAnimation(cell.X, cell.Y, newlyOpened);

                if (MineField.IsExploded)
                {
                    StopTimer();
                    currentStats.GamesPlayed++;
                    DataService.Instance.Save();
                    if (IsExtreme)
                        TriggerExtremeJoke();
                    else
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
                    DataService.Instance.Save();
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
        
        CellPointerEnteredCommand = new RelayCommand<Cell>((cell) =>
        {
            if (cell == null || !IsRadarTargeting || RadarUsed || Lose || Win) return;

            var areaCells = Cells.Where(c => 
                Math.Abs(c.X - cell.X) <= 1 && 
                Math.Abs(c.Y - cell.Y) <= 1).ToList();

            foreach (var c in areaCells)
            {
                c.IsRadarHighlighted = true;

                if (c is { IsMine: true, IsOpen: false, IsFlagged: false })
                {
                    c.IsRadarScanning = true;
                }
            }
        });

        CellPointerExitedCommand = new RelayCommand<Cell>(_ =>
        {
            foreach (var c in Cells)
            {
                c.IsRadarHighlighted = false;
                c.IsRadarScanning = false;
            }
        });
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

        foreach (var c in Cells)
        {
            c.IsRadarHighlighted = false;
        }
        
        var targetX = centerCell.X;
        var targetY = centerCell.Y;

        var minesInRadius = Cells.Where(cell => 
            Math.Abs(cell.X - targetX) <= 1 && 
            Math.Abs(cell.Y - targetY) <= 1 && 
            cell is { IsMine: true, IsFlagged: false, IsOpen: false }).ToList();

        if (minesInRadius.Any())
        {
            foreach (var mine in minesInRadius)
            {
                mine.IsRadarScanning = true;
            }

            Task.Run(async () =>
            {
                for (int i = 0; i < 3; i++)
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        foreach (var mine in minesInRadius)
                        {
                            mine.IsRadarScanning = true;
                        }
                    });

                    await Task.Delay(500);

                    Dispatcher.UIThread.Post(() =>
                    {
                        foreach (var mine in minesInRadius)
                        {
                            mine.IsRadarScanning = false;
                        }
                    });

                    await Task.Delay(250);
                }
            });
        }
    }
}