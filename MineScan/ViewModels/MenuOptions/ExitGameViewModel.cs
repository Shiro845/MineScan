using System;

namespace MineScan.ViewModels.MenuOptions;

public class ExitGameViewModel : ViewModelBase
{
    public void GoBack() => NavigationService.Instance.NavigateTo<MainMenuViewModel>();
    public void Exit() => Environment.Exit(0);
}