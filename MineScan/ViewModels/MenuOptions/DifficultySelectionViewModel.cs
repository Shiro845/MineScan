using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MineScan.Models;

namespace MineScan.ViewModels;

public class DifficultySelectionViewModel : ViewModelBase
{
    public bool IsEasyChecked 
    { 
        get => SelectedDifficulty.ActualDifficulty == GameDifficulty.Easy; 
        set { if (value) SelectedDifficulty.ActualDifficulty = GameDifficulty.Easy; }
    }

    public bool IsMediumChecked 
    { 
        get => SelectedDifficulty.ActualDifficulty == GameDifficulty.Medium; 
        set { if (value) SelectedDifficulty.ActualDifficulty = GameDifficulty.Medium; }
    }

    public bool IsHardChecked 
    { 
        get => SelectedDifficulty.ActualDifficulty == GameDifficulty.Hard; 
        set { if (value) SelectedDifficulty.ActualDifficulty = GameDifficulty.Hard; } 
    }
    
    private Action<string> _changePage;
    public ICommand  ChangePageCommand { get; }

    public DifficultySelectionViewModel(Action<string> changePage)
    {
        _changePage = changePage;
        ChangePageCommand = new RelayCommand<string>(pageName =>
        {
            if (pageName != null) _changePage(pageName);
        });
    }
}