using System;
using MineScan.Models;

namespace MineScan.ViewModels.MenuOptions;

public class ExitGameViewModel : ViewModelBase
{
    public void GoBack() => NavigationService.Instance.NavigateTo<MainMenuViewModel>();
    public void Exit() => Environment.Exit(0);
}