using MineScan.Models;

namespace MineScan.ViewModels.MenuOptions;

public class PlayingTutorialViewModel : ViewModelBase
{
    public void GoBack() => NavigationService.Instance.NavigateTo<MainMenuViewModel>();

    public string? GameRulesText => AppSettings.Instance.CurrentLanguage == "English"
        ? GameRulesTextEng
        : GameRulesTextUa;

    private string GameRulesTextEng => """
                                           GAME RULES "MINESCAN"
       
                                           1. The main goal of the game is to open all safe cells on the field without detonating any mines.
                                           2. The number in a cell shows exactly how many mines are located around it (within a 1-cell radius).
                                           3. Use "Right-click" to place a flag on a cell where you think a mine is hidden.
                                           
                                           USING THE RADAR:
                                           Click the "Radar" button, then click on the desired cell. For 1 second, the locations of all mines within a 3x3 area will be highlighted (flags are ignored).
                                           The radar can be used only once per game!
                                           """;
    private string GameRulesTextUa => """
                                          ПРАВИЛА ГРИ "MINESCAN"
                                          
                                          1. Основна мета гри — відкрити всі безпечні клітинки на полі, не підірвавшись на мінах.
                                          2. Цифра в клітинці показує, скільки саме мін розташовано навколо неї (у радіусі 1 клітинки).
                                          3. Використовуйте "Правий клік", щоб поставити прапорець на клітинку, де на вашу думку схована міна.
                                          
                                          ВИКОРИСТАННЯ РАДАРА:
                                          Натисніть кнопку "Радар", натисність на бажану клітинку, на 1 секунду в області 3x3 буде підсвічено місцезнаходження мін(флаги ігноруються).
                                          Радар можна використати лише один раз за гру!
                                          """;
}