using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using MineScan.Views;
using MineScan.Views.MenuOptions;

namespace MineScan.ViewModels;

public class MainMenuViewModel : ViewModelBase
{
    public UserControl CurrentPage
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged(nameof(CurrentPage));
        }
    }
    public ICommand ChangePageCommand { get; }
    public MainMenuViewModel()
    {
        CurrentPage = new MainMenuUserControl();
        
        ChangePageCommand = new RelayCommand<string>(OpenPage);
    }
    private void OpenPage(string? pageName)
    {
        CurrentPage = pageName switch
        {
            "Play" => new DificultySelectionUserControl(),
            "Stats" => new StatisticsUserControl(),
            "Skins" => new SkinsUserControl(),
            "Settings" => new SettingsUserControl(),
            "Tutorial" => new PlayingTutorialUserControl(),
            "Exit" => new ExitGameUserControl(),
            _ => new MainMenuUserControl()
        };
    }
}