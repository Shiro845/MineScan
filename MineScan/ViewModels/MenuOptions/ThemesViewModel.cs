using Avalonia;
using Avalonia.Styling;
using MineScan.Services;

namespace MineScan.ViewModels.MenuOptions;

public class ThemesViewModel : ViewModelBase
{
    public void GoBack() => NavigationService.Instance.NavigateTo<MainMenuViewModel>();

    public static readonly ThemeVariant LightTheme = new("Light", ThemeVariant.Light);
    public static readonly ThemeVariant DarkTheme = new("Dark", ThemeVariant.Dark);
    public static readonly ThemeVariant NeonTheme = new("Neon", ThemeVariant.Dark);
    public static readonly ThemeVariant PinkTheme = new("Pink", ThemeVariant.Light);
    
    public ThemeVariant CurrentTheme
    {
        get
        {
            return DataService.Instance.LocalData.CurrentTheme switch
            {
                "Light" => ThemeVariant.Light,
                "Dark" => ThemeVariant.Dark,
                "Neon" => ThemeVariant.Dark,
                "Pink" => ThemeVariant.Light,
                _ => ThemeVariant.Default
            };
        }
        set
        {
            if (Application.Current != null)
            {
                string themeStr = "Light";
                if (value == ThemeVariant.Dark) themeStr = "Dark";
                else if (value == ThemeVariant.Light) themeStr = "Light";
                else if (value.Key.ToString() == "Neon") themeStr = "Neon";
                else if (value.Key.ToString() == "Pink") themeStr = "Pink";

                if (DataService.Instance.LocalData.CurrentTheme != themeStr)
                {
                    DataService.Instance.LocalData.CurrentTheme = themeStr;
                    Application.Current.RequestedThemeVariant = value;
                
                    OnPropertyChanged(nameof(CurrentTheme));
                    DataService.Instance.Save();
                }
            }
        }
    }
    
    public void SetDarkTheme() => CurrentTheme = DarkTheme;
    public void SetLightTheme() => CurrentTheme = LightTheme;
    public void SetNeonTheme() => CurrentTheme = NeonTheme;
    public void SetPinkTheme() => CurrentTheme = PinkTheme;
}