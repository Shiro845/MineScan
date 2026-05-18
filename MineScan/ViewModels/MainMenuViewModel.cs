using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MineScan.Models;
using MineScan.ViewModels.MenuOptions;
using MineScan.Views.MenuOptions;

namespace MineScan.ViewModels;

public class MainMenuViewModel : ViewModelBase
{
    public void OpenPlay() => NavigationService.Instance.NavigateTo<DifficultySelectionViewModel>();
    public void OpenStats() => NavigationService.Instance.NavigateTo<StatisticsViewModel>();
    public void OpenThemes() => NavigationService.Instance.NavigateTo<ThemesViewModel>();
    public void OpenSettings() => NavigationService.Instance.NavigateTo<SettingsViewModel>();
    public void OpenTutorial() => NavigationService.Instance.NavigateTo<PlayingTutorialViewModel>();
    public void OpenExitGame() => NavigationService.Instance.NavigateTo<ExitGameViewModel>();
    public MainMenuViewModel()
    {
        
    }
}