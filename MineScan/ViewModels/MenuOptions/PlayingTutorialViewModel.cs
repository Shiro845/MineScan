using MineScan.Services;

namespace MineScan.ViewModels.MenuOptions;

public class PlayingTutorialViewModel : ViewModelBase
{
    public void GoBack() => NavigationService.Instance.NavigateTo<MainMenuViewModel>();

    private bool _isUa => DataService.Instance.LocalData.CurrentLanguage == "Ukrainian";

    public int SelectedTab
    {
      get;
      set
      {
        field = value;
        OnPropertyChanged(nameof(SelectedTab));
        OnPropertyChanged(nameof(ShowRules));
        OnPropertyChanged(nameof(ShowHotkeys));
        OnPropertyChanged(nameof(ShowDifficulties));
      }
    }

    public bool ShowRules => SelectedTab == 0;
    public bool ShowHotkeys => SelectedTab == 1;
    public bool ShowDifficulties => SelectedTab == 2;

    public void SelectRules() => SelectedTab = 0;
    public void SelectHotkeys() => SelectedTab = 1;
    public void SelectDifficulties() => SelectedTab = 2;

    public string RulesText => _isUa ? RulesUa : RulesEn;
    public string HotkeysText => _isUa ? HotkeysUa : HotkeysEn;
    public string DifficultiesText => _isUa ? DifficultiesUa : DifficultiesEn;

    public string GameRulesText => RulesText;

    private string RulesEn => """
        GAME RULES

        1. Open all safe cells without hitting a mine.
        2. The number in a cell = how many mines surround it.
        3. Right-click cycles: empty → 🚩 Flag → ❓ Question → empty.
        4. Click an open numbered cell to chord — opens neighbors if flag count matches the number.
        5. First click is always safe (mines spawn after it).

        RADAR:
        Click "Radar", then click a cell.
        Mines in the 3×3 area pulse red briefly.
        One use per game only!
        """;

    private string RulesUa => """
        ПРАВИЛА ГРИ

        1. Відкрий усі безпечні клітинки, не підірвавшись.
        2. Цифра = кількість мін навколо клітинки.
        3. Правий клік циклює: порожня → 🚩 Прапор → ❓ Питання → порожня.
        4. Клік на відкриту цифру — хордінг: відкриває сусідів якщо к-сть прапорів збігається.
        5. Перший клік завжди безпечний (міни з'являються після нього).

        РАДАР:
        Натисни "Радар", потім на клітинку.
        Міни в зоні 3×3 підсвітяться червоним.
        Використовується лише один раз за гру!
        """;

    private string HotkeysEn => """
        SECRET HOTKEYS
        (work during gameplay)

        Ctrl + G
          👻 Ghost Mode
          Toggle: see all mines through
          closed cells (semi-transparent red).

        Ctrl + S
          🤖 Auto-Solve
          Opens all safe cells one by one
          with cascade animation.
          Counts as a win.
        """;

    private string HotkeysUa => """
        СЕКРЕТНІ ХОТКЕЇ
        (працюють під час гри)

        Ctrl + G
          👻 Режим Привида
          Toggle: бачиш усі міни крізь
          закриті клітинки (напівпрозоро).

        Ctrl + S
          🤖 Авто-розв'язання
          Відкриває всі безпечні клітинки
          по черзі з cascade анімацією.
          Зараховується як перемога.
        """;

    private string DifficultiesEn => """
        DIFFICULTY LEVELS

        🟢 EASY
          9×9 grid, 10 mines
          Good for beginners.

        🟠 MEDIUM
          16×16 grid, 40 mines
          Classic minesweeper.

        🔴 HARD
          30×20 grid, 120 mines
          For experienced players.

        🟣 CUSTOM
          You choose: width, height, mines.
          5–30 wide, 5–20 tall.

        💀 EXTREME
          30×20 grid, 150 mines.
          Lose → watch System32 "delete"...
          then get spared. >:)
        """;

    private string DifficultiesUa => """
        РІВНІ СКЛАДНОСТІ

        🟢 ЛЕГКА
          9×9, 10 мін
          Для початківців.

        🟠 СЕРЕДНЯ
          16×16, 40 мін
          Класичний сапер.

        🔴 ВАЖКА
          30×20, 120 мін
          Для досвідчених.

        🟣 СВОЯ
          Вибираєш сам: ширина, висота, міни.
          5–30 ширина, 5–20 висота.

        💀 ЕКСТРЕМ
          30×20, 150 мін.
          Програш → System32 "видаляється"...
          потім жаліють. >:)
        """;
}
