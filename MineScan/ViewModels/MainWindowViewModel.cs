using Avalonia.Controls;
using MineScan.Views;

namespace MineScan.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public UserControl CurrentPage { get; private set; } = new MainMenu();
}