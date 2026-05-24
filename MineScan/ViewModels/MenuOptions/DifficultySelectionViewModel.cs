using MineScan.Models;

namespace MineScan.ViewModels.MenuOptions;

public class DifficultySelectionViewModel : ViewModelBase
{
    public void GoBack() => NavigationService.Instance.NavigateTo<MainMenuViewModel>();
    public void OpenGameBoard() => NavigationService.Instance.NavigateTo<GameBoardViewModel>();
    
    public bool IsEasyChecked 
    { 
        get => SelectedDifficulty.Instance.ActualDifficulty == GameDifficulty.Easy; 
        set { if (value) SelectedDifficulty.Instance.ActualDifficulty = GameDifficulty.Easy; }
    }

    public bool IsMediumChecked 
    { 
        get => SelectedDifficulty.Instance.ActualDifficulty == GameDifficulty.Medium; 
        set { if (value) SelectedDifficulty.Instance.ActualDifficulty = GameDifficulty.Medium; }
    }

    public bool IsHardChecked 
    { 
        get => SelectedDifficulty.Instance.ActualDifficulty == GameDifficulty.Hard; 
        set { if (value) SelectedDifficulty.Instance.ActualDifficulty = GameDifficulty.Hard; } 
    }
    public DifficultySelectionViewModel()
    {
        
    }
}