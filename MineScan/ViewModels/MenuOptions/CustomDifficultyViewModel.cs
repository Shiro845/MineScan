using MineScan.Models;
using MineScan.Services;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace MineScan.ViewModels.MenuOptions;

public class CustomDifficultyViewModel : ViewModelBase
{
    public void GoBack() => NavigationService.Instance.NavigateTo<DifficultySelectionViewModel>();

    public string WidthText
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged(nameof(WidthText));
            OnPropertyChanged(nameof(IsWidthValid));
            OnPropertyChanged(nameof(IsMinesValid));
            OnPropertyChanged(nameof(CanPlay));
        }
    } = DataService.Instance.LocalData.CustomWidth.ToString();

    public string HeightText
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged(nameof(HeightText));
            OnPropertyChanged(nameof(IsHeightValid));
            OnPropertyChanged(nameof(IsMinesValid));
            OnPropertyChanged(nameof(CanPlay));
        }
    } = DataService.Instance.LocalData.CustomHeight.ToString();

    public string MinesText
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged(nameof(MinesText));
            OnPropertyChanged(nameof(IsMinesValid));
            OnPropertyChanged(nameof(CanPlay));
        }
    } = DataService.Instance.LocalData.CustomMines.ToString();

    public bool IsWidthValid =>
        int.TryParse(WidthText, out int w) && w >= 5 && w <= 30;

    public bool IsHeightValid =>
        int.TryParse(HeightText, out int h) && h >= 5 && h <= 20;

    public bool IsMinesValid
    {
        get
        {
            if (!int.TryParse(WidthText, out int w) || !int.TryParse(HeightText, out int h))
                return false;
            if (!int.TryParse(MinesText, out int m))
                return false;
            int maxMines = w * h - 9;
            return m >= 1 && m <= maxMines;
        }
    }

    public bool CanPlay => IsWidthValid && IsHeightValid && IsMinesValid;

    public ICommand PlayCommand { get; }

    public CustomDifficultyViewModel()
    {
        PlayCommand = new RelayCommand(() =>
        {
            DataService.Instance.LocalData.CustomWidth = int.Parse(WidthText);
            DataService.Instance.LocalData.CustomHeight = int.Parse(HeightText);
            DataService.Instance.LocalData.CustomMines = int.Parse(MinesText);
            DataService.Instance.Save();
            SelectedDifficulty.Instance.ActualDifficulty = GameDifficulty.Custom;
            NavigationService.Instance.NavigateTo<GameBoardViewModel>();
        }, () => CanPlay);
    }
}
