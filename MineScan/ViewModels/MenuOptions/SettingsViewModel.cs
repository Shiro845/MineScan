using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using MineScan.Services;

namespace MineScan.ViewModels.MenuOptions;

public class SettingsViewModel : ViewModelBase
{
    public void GoBack() => NavigationService.Instance.NavigateTo<MainMenuViewModel>();
    public bool IsMinecraftFont
    {
        get => DataService.Instance.LocalData.IsMinecraftFont;
        set
        {
            DataService.Instance.LocalData.IsMinecraftFont = value;
            OnPropertyChanged(nameof(IsMinecraftFont));
            ApplyFont(value);
            DataService.Instance.Save();
        }
    }

    public bool IsFullscreen
    {
        get => DataService.Instance.LocalData.IsFullscreen;
        set
        {
            DataService.Instance.LocalData.IsFullscreen = value;
            OnPropertyChanged(nameof(IsFullscreen));
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: not null } desktop)
            {
                desktop.MainWindow.WindowState = value ? WindowState.FullScreen : WindowState.Normal;
            }
            DataService.Instance.Save();
        }
    }
    
    public bool IsRadarDisabled
    {
        get => DataService.Instance.LocalData.IsRadarDisabled;
        set
        {
            DataService.Instance.LocalData.IsRadarDisabled = value;
            OnPropertyChanged(nameof(IsRadarDisabled));
            DataService.Instance.Save();
        }
    }
    
    public byte SelectedLanguageIndex
    {
        get => (byte)(DataService.Instance.LocalData.CurrentLanguage == "Ukrainian" ? 1 : 0);
        set
        {
            string lang = (value == 1) ? "Ukrainian" : "English";
        
            if (DataService.Instance.LocalData.CurrentLanguage != lang)
            {
                DataService.Instance.LocalData.CurrentLanguage = lang;
                OnPropertyChanged(nameof(SelectedLanguageIndex));
                SetLanguage(lang);
                DataService.Instance.Save();
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
    
    public void SetLanguage(string langCode)
    {
        var mergedDicts = Application.Current?.Resources.MergedDictionaries;

        if (mergedDicts != null)
        {
            for (int i = mergedDicts.Count - 1; i >= 0; i--)
            {
                if (mergedDicts[i] is ResourceInclude resourceInclude)
                {
                    var dictUri = resourceInclude.Source?.ToString();
                    if (dictUri != null && dictUri.Contains("/Languages/"))
                    {
                        mergedDicts.RemoveAt(i);
                    }
                }
            }

            string sourceUri = $"avares://MineScan/Resources/Languages/{langCode}.axaml";

            try
            {
                var newDict = new ResourceInclude(new Uri(sourceUri, UriKind.Absolute))
                {
                    Source = new Uri(sourceUri, UriKind.Absolute)
                };
            
                mergedDicts.Add(newDict);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Помилка завантаження мови: {ex.Message}");
            }
        }
    }
}