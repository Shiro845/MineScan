using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using Avalonia.Styling;
using MineScan.Services;
using MineScan.ViewModels;
using MineScan.Views;

namespace MineScan;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
            
            InitializeAppConfiguration(desktop.MainWindow);
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    /// <summary>
    /// Головна точка ініціалізації, налаштовує всю гру з файлу config.json при старті
    /// </summary>
    private void InitializeAppConfiguration(Window window)
    {
        var data = DataService.Instance.LocalData;

        var mergedDicts = Resources.MergedDictionaries;
        {
            for (int i = mergedDicts.Count - 1; i >= 0; i--)
            {
                if (mergedDicts[i] is ResourceInclude resInclude && 
                    resInclude.Source?.ToString().Contains("/Languages/") == true)
                {
                    mergedDicts.RemoveAt(i);
                }
            }
            string langUri = $"avares://MineScan/Resources/Languages/{data.CurrentLanguage}.axaml";
            try
            {
                mergedDicts.Add(new ResourceInclude(new Uri(langUri, UriKind.Absolute)) { Source = new Uri(langUri, UriKind.Absolute) });
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Помилка завантаження мови: {ex.Message}"); }
        }

        if (data.IsMinecraftFont && Resources.TryGetValue("MinecraftFont", out var font) && font is FontFamily mcFont)
        {
            Resources["GameFont"] = mcFont;
        }
        else
        {
            Resources["GameFont"] = FontFamily.Default;
        }

        window.WindowState = data.IsFullscreen ? WindowState.FullScreen : WindowState.Normal;
        
        RequestedThemeVariant = data.CurrentTheme switch
        {
            "Light" => ThemeVariant.Light,
            "Dark" => ThemeVariant.Dark,
            "Neon" => new ThemeVariant("Neon", ThemeVariant.Dark),
            "Pink" => new ThemeVariant("Pink", ThemeVariant.Light),
            _ => ThemeVariant.Default
        };
    }
}