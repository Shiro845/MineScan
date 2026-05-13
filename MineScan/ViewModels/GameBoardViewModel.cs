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
        MineField = new MineField(8, 8, 10);
        Cells = MineField.ToCellList();

        OpenCellCommand = new RelayCommand<Cell>(cell =>
        {
            if (cell != null) MineField.OpenCell(cell.X, cell.Y);
            Cells = new List<Cell>(MineField.ToCellList());
        });
        _changePage = changePage;
        ChangePageCommand = new RelayCommand<string>(pageName =>
        {
            if (pageName != null) _changePage(pageName);
        });
    }
}