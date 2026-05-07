using Avalonia.Controls;

namespace MineScan.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MinWidth = 600;
        MinHeight = 400;
    }
}