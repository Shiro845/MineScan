using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MineScan.Models;

namespace MineScan.ViewModels;

public class GameBoardViewModel : ViewModelBase
{
    private readonly Action<string> _changePage;
    
    public ICommand ChangePageCommand { get; set; }

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

    public GameBoardViewModel(Action<string> changePage)
    {
        var difficulty = SelectedDifficulty.ActualDifficulty;
        sbyte width, height, mines;

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
            }
        });
        FlagCellCommand = new RelayCommand<Cell>(cell =>
        {
            if (cell != null)
            {
                MineField.ToggleFlag(cell.X, cell.Y);
            }
        });
        _changePage = changePage;
        ChangePageCommand = new RelayCommand<string>(pageName =>
        {
            if (pageName != null) _changePage(pageName);
        });
    }
}