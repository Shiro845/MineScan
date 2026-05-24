using MineScan.Models;

namespace MineScan.ViewModels.MenuOptions;

public class StatisticsViewModel : ViewModelBase
{
    public void GoBack() => NavigationService.Instance.NavigateTo<MainMenuViewModel>();
    public SelectedDifficulty Stats => SelectedDifficulty.Instance;
}