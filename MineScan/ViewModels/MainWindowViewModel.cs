namespace MineScan.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public NavigationService Nav => NavigationService.Instance;
    public MainWindowViewModel()
    {
        Nav.NavigateTo<MainMenuViewModel>();
    }
}