using Avalonia;
using Avalonia.Styling;
using MineScan.Models;

namespace MineScan.ViewModels.MenuOptions;

public class ThemesViewModel : ViewModelBase
{
    public void GoBack() => NavigationService.Instance.NavigateTo<MainMenuViewModel>();

    public static readonly ThemeVariant LightTheme = new ThemeVariant("Light", ThemeVariant.Light);
    public static readonly ThemeVariant DarkTheme = new ThemeVariant("Dark", ThemeVariant.Dark);
    public static readonly ThemeVariant NeonTheme = new ThemeVariant("Neon", ThemeVariant.Dark);
    public static readonly ThemeVariant PinkTheme = new ThemeVariant("Pink", ThemeVariant.Light);
    
    public ThemeVariant CurrentTheme
    {
        get => Application.Current?.RequestedThemeVariant ?? ThemeVariant.Default;
        set
        {
            if (Application.Current != null && Application.Current.RequestedThemeVariant != value)
            {
                Application.Current.RequestedThemeVariant = value;
                OnPropertyChanged(nameof(CurrentTheme));
            }
        }
    }
    
    public void SetDarkTheme() => CurrentTheme = DarkTheme;
    public void SetLightTheme() => CurrentTheme = LightTheme;
    public void SetNeonTheme() => CurrentTheme = NeonTheme;
    public void SetPinkTheme() => CurrentTheme = PinkTheme;
}