#pragma warning disable CS8622

namespace MineScan
{
    public partial class MainWindow : Window
    {

        public static readonly FontFamily MinecraftFont = new FontFamily("avares://MineScan/Assets/Fonts/#Minecraftia");

        public Button[,] Cells;
        private Grid MenuButtons;
        private Gridinfo gameField;
        bool minesPlaced = false;
        public int gridx, gridy;


        public MainWindow()
        {
            InitializeComponent();
            Width = Config.WindowWidth;
            Height = Config.WindowHeight;
            StartMenu();
        }

        private void StartMenu()
        {
            Button PlayButton = new Button
            {
                Content = "Start Game",
                FontSize = 24,
                FontFamily = MinecraftFont,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = new SolidColorBrush(Colors.Green)
            };
            Button SettingsButton = new Button
            {
                Content = "Settings",
                FontSize = 24,
                FontFamily = MinecraftFont,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 100, 0, 0),
                Foreground = new SolidColorBrush(Colors.DarkGray)
            };
            Button ExitButton = new Button
            {
                Content = "Exit",
                FontSize = 24,
                FontFamily = MinecraftFont,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 200, 0, 0),
                Foreground = new SolidColorBrush(Colors.Red)
            };
            PlayButton.Click += StartGameBtn;
            ExitButton.Click += ExitBtn;
            SettingsButton.Click += SettingsBtn;
            MenuButtons = new Grid { Children = { PlayButton, SettingsButton, ExitButton } };
            this.Content = MenuButtons;

        }
        private void StartGameBtn(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            this.Content = null;
            CreateField();
        }
        private void SettingsBtn(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Settings();
        }
        private void ExitBtn(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            this.Close();
        }
        public void Settings()
        {

        }

        private void CreateField()
        {
            int gridSize = Config.GridSize;
            int btnSize = Config.buttonSize;
            gameField = new Gridinfo(gridSize, btnSize);

            double gridx = (Width / 2) - (btnSize * gridSize / 2);
            double gridy = (Height / 2) - (btnSize * gridSize / 2);

            var mineField = new UniformGrid
            {
                Rows = gridSize,
                Columns = gridSize,
                Margin = new Thickness(gridx, gridy),
                RowSpacing = 0,
                ColumnSpacing = 0,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            Cells = new Button[gridSize, gridSize];

            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    var btn = new Button { Width = btnSize, Height = btnSize, CornerRadius = new CornerRadius(0)};
                    btn.Tag = new Point(x, y);
                    btn.Click += OnButtonClick;
                    btn.PointerPressed += Flag;
                    Cells[x, y] = btn;
                    mineField.Children.Add(btn);
                }
            }

            this.Content = mineField;
        }

        private void OpenCell(int x, int y)
        {
            if (x < 0 || y < 0 || y >= Config.GridSize || x >= Config.GridSize) return;
            if (gameField.GridData[x, y].IsFlagged || gameField.GridData[x, y].IsRevealed) return;
            if (gameField.GridData[x, y].IsMine)
            {
                Cells[x, y].Content = "💣";
                Cells[x, y].IsHitTestVisible = false;
                Cells[x, y].Background = new SolidColorBrush(Colors.Red);
                //this.Content = null;
                Label label = new Label
                {
                    Content = "Game Over",
                    FontSize = 48,
                    FontFamily = MinecraftFont,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                this.Content = label;
                return;
            }

            Cells[x, y].FontFamily = MinecraftFont;
            int MinesCount = gameField.GridData[x, y].MinesAround;
            if (MinesCount > 0) Cells[x, y].Content = $"{MinesCount}";
            gameField.GridData[x, y].IsRevealed = true;
            Cells[x, y].Background = new SolidColorBrush(Colors.White);
            switch (MinesCount)
            {
                case 1: Cells[x, y].Foreground = new SolidColorBrush(Colors.Blue); break;
                case 2: Cells[x, y].Foreground = new SolidColorBrush(Colors.Lime); break;
                case 3: Cells[x, y].Foreground = new SolidColorBrush(Colors.Red); break;
                case 4: Cells[x, y].Foreground = new SolidColorBrush(Colors.DarkBlue); break;
                case 5: Cells[x, y].Foreground = new SolidColorBrush(Colors.DarkRed); break;
                case 6: Cells[x, y].Foreground = new SolidColorBrush(Colors.Cyan); break;
                case 7: Cells[x, y].Foreground = new SolidColorBrush(Colors.Pink); break;
                case 8: Cells[x, y].Foreground = new SolidColorBrush(Colors.Orange); break;
            }

            if (MinesCount == 0)
            {
                for (int cx = x - 1; cx <= x + 1; cx++)
                {
                    for (int cy = y - 1; cy <= y + 1; cy++)
                    {

                        if (cx == x && cy == y) { Cells[x, y].IsHitTestVisible = false; continue; }
                        OpenCell(cx, cy);
                    }
                }
            }
        }
        private void OnButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Point pos)
            {
                int x = (int)pos.X;
                int y = (int)pos.Y;
                if (!minesPlaced)
                {
                    gameField.PlaceMines(x, y);
                    minesPlaced = true;
                }
                OpenCell(x, y);
                if (minesPlaced && gameField.GridData[x, y].IsRevealed)
                {
                    Chord(x, y);
                }
                
            }
        }

        private void Flag(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Point pos)
            {
                int x = (int)pos.X;
                int y = (int)pos.Y;
                if (gameField.GridData[x, y].IsRevealed) return;
                if (gameField.GridData[x, y].IsFlagged)
                {
                    btn.Content = "";
                    gameField.GridData[x, y].IsFlagged = false;
                }
                else
                {
                    btn.Content = "🚩";
                    gameField.GridData[x, y].IsFlagged = true;
                }
            }
        }

        private void Chord(int x, int y)
        {
            int MinesAround = gameField.GridData[x, y].MinesAround;
            int FlaggedCount = 0;
            if (MinesAround == 0) return;

            for (int cx = x - 1; cx <= x + 1; cx++)
            {
                for (int cy = y - 1; cy <= y + 1; cy++)
                {
                    if (cx >= 0 && cy >= 0 && cx < Config.GridSize && cy < Config.GridSize)
                    {
                        if (gameField.GridData[cx, cy].IsFlagged) FlaggedCount++;
                    }
                }
            }

            if (FlaggedCount == MinesAround)
            {
                for (int cx = x - 1; cx <= x + 1; cx++)
                {
                    for (int cy = y - 1; cy <= y + 1; cy++)
                    {
                        OpenCell(cx, cy);
                    }
                }
            }
        }
    }
}