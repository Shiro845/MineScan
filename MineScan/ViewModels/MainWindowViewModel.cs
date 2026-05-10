using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace MineScan.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ViewModelBase CurrentPage
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged(nameof(CurrentPage));
        }
    }
    public ICommand ChangePageCommand { get; }
    public MainWindowViewModel()
    {
        CurrentPage = new MainMenuViewModel(ChangePage);
        
        ChangePageCommand = new RelayCommand<string>(ChangePage);
    }
    private void ChangePage(string? pageName)
    {
        CurrentPage = pageName switch
        {
            "Play" => new DifficultySelectionViewModel(ChangePage),
            "Stats" => new StatisticsViewModel(),
            "Skins" => new SkinsViewModel(),
            "Settings" => new SettingsViewModel(),
            "Tutorial" => new PlayingTutorialViewModel(),
            "Exit" => new ExitGameViewModel(),
            "GameBoard" => new GameBoardViewModel(ChangePage),
            "MainMenu" => new MainMenuViewModel(ChangePage)
        };
    }
}