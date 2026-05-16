using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MineScan.Models;

namespace MineScan.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public NavigationService Nav => NavigationService.Instance;
    public MainWindowViewModel()
    {
        Nav.NavigateTo<MainMenuViewModel>();
    }
}