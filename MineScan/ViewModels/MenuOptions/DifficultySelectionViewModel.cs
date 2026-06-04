using MineScan.Models;
using MineScan.Services;

namespace MineScan.ViewModels.MenuOptions;

public class DifficultySelectionViewModel : ViewModelBase
{
    public void GoBack() => NavigationService.Instance.NavigateTo<MainMenuViewModel>();
    public void OpenGameBoard() => NavigationService.Instance.NavigateTo<GameBoardViewModel>();
    public void OpenCustom() => NavigationService.Instance.NavigateTo<CustomDifficultyViewModel>();

    public bool IsEasyChecked
    {
        get => SelectedDifficulty.Instance.ActualDifficulty == GameDifficulty.Easy;
        set { if (value) { SelectedDifficulty.Instance.ActualDifficulty = GameDifficulty.Easy; NotifyAll(); } }
    }

    public bool IsMediumChecked
    {
        get => SelectedDifficulty.Instance.ActualDifficulty == GameDifficulty.Medium;
        set { if (value) { SelectedDifficulty.Instance.ActualDifficulty = GameDifficulty.Medium; NotifyAll(); } }
    }

    public bool IsHardChecked
    {
        get => SelectedDifficulty.Instance.ActualDifficulty == GameDifficulty.Hard;
        set { if (value) { SelectedDifficulty.Instance.ActualDifficulty = GameDifficulty.Hard; NotifyAll(); } }
    }

    public bool IsCustomChecked
    {
        get => SelectedDifficulty.Instance.ActualDifficulty == GameDifficulty.Custom;
        set { if (value) { SelectedDifficulty.Instance.ActualDifficulty = GameDifficulty.Custom; NotifyAll(); } }
    }

    public bool IsExtremeChecked
    {
        get => SelectedDifficulty.Instance.ActualDifficulty == GameDifficulty.Extreme;
        set { if (value) { SelectedDifficulty.Instance.ActualDifficulty = GameDifficulty.Extreme; NotifyAll(); } }
    }

    private void NotifyAll()
    {
        OnPropertyChanged(nameof(IsEasyChecked));
        OnPropertyChanged(nameof(IsMediumChecked));
        OnPropertyChanged(nameof(IsHardChecked));
        OnPropertyChanged(nameof(IsCustomChecked));
        OnPropertyChanged(nameof(IsExtremeChecked));
    }
}