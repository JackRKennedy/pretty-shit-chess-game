using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using static ChessUI.GameOverMenu;

namespace ChessUI;

public partial class PauseMenu : UserControl
{

    public event Action<Option> OptionSelected;
    public PauseMenu()
    {
        InitializeComponent();
    }

    private void Continue_Click(object sender, RoutedEventArgs e)
    {
        Console.WriteLine("You pressed continue");
        OptionSelected?.Invoke(Option.Continue);
    }

    private void Restart_Click(object sender, RoutedEventArgs e)
    {
            Console.WriteLine("You pressed restart");
            OptionSelected?.Invoke(Option.Restart);
    }
}