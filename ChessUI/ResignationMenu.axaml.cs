using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using static ChessUI.GameOverMenu;
using static ChessUI.Option;

namespace ChessUI;

public partial class ResignationMenu : UserControl
{
    
    public event Action<Option> OptionSelected;
    public ResignationMenu()
    {
        InitializeComponent();
    }

    private void Resign_Click(object sender, RoutedEventArgs e)
    {
        Console.WriteLine("You have resigned");
        OptionSelected?.Invoke(Option.Resign);
    }

    private void Continue_Click(object sender, RoutedEventArgs e)
    {
        Console.WriteLine("You pressed continue");
        OptionSelected?.Invoke(Option.Continue);
    }
}