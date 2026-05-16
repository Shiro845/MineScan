using MineScan.Models;

namespace MineScan.ViewModels;

public class StatisticsViewModel : ViewModelBase
{
    public void GoBack() => NavigationService.Instance.NavigateTo<MainMenuViewModel>();
    public SelectedDifficulty Stats => SelectedDifficulty.Instance;
}