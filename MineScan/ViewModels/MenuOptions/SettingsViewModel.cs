using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;

namespace MineScan.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    public void GoBack() => NavigationService.Instance.NavigateTo<MainMenuViewModel>();
    public bool IsMinecraftFont
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged(nameof(IsMinecraftFont));
            ApplyFont(value);
        }
    }

    public bool IsFullscreen
    {
        get => (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) 
               && desktop.MainWindow?.WindowState == WindowState.FullScreen;
        set
        {
            OnPropertyChanged(nameof(IsFullscreen));
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: not null } desktop)
            {
                desktop.MainWindow.WindowState = value ? WindowState.FullScreen : WindowState.Normal;
            }
        }
    }
    
    private void ApplyFont(bool useMinecraft)
    {
        if (Application.Current == null) return;

        if (useMinecraft)
        {
            if (Application.Current.TryFindResource("MinecraftFont", out var font) && font is FontFamily mcFont)
            {
                Application.Current.Resources["GameFont"] = mcFont;
            }
        }
        else
        {
            Application.Current.Resources["GameFont"] = FontFamily.Default;
        }
    }
    
    public SettingsViewModel()
    {
        if (Application.Current != null)
        {
            if (Application.Current.Resources.TryGetValue("GameFont", out var currentFont) &&
                Application.Current.Resources.TryGetValue("MinecraftFont", out var mcFont))
            {   
                IsMinecraftFont = (currentFont == mcFont);
            }
        }
    }
}