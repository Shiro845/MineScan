using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace MineScan.ViewModels;

public class GameBoardViewModel : ViewModelBase
{
    private readonly Action<string> _changePage;
    
    public ICommand  ChangePageCommand { get; }

    public GameBoardViewModel(Action<string> changePage)
    {
        _changePage = changePage;
        ChangePageCommand = new RelayCommand<string>(pageName =>
        {
            if (pageName != null) _changePage(pageName);
        });
    }
}