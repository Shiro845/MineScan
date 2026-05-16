using CommunityToolkit.Mvvm.ComponentModel;

namespace MineScan.Models;

public partial class NavigationService : ObservableObject
{
    public static NavigationService Instance { get; } = new();
    
    public object CurrentPage
    {
        get => field;
        set  => SetProperty(ref field, value);
    }
    
    public void NavigateTo<T>() where T : new()
    {
        CurrentPage = new T();
    }
    
    public void NavigateTo(object viewModel)
    {
        CurrentPage = viewModel;
    }
}