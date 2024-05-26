using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ChessLogic;

namespace ChessUI;

public partial class GameOverMenu : UserControl
{

    public event Action<Option> OptionSelected;
    public GameOverMenu(GameState gameState)
    {
        InitializeComponent();
        
        Result result = gameState.Result;
        string winnerText = GetWinnerText(result.Winner);
        string reasonText = GetReasonText(result.Reason, gameState.CurrentPlayer);
        
        WinnerText.Text = $"{winnerText}";
        ReasonText.Text = $"{reasonText}";
    }

    private static string GetWinnerText(Player winner)
    {
        return winner switch
        {
            Player.White => "White wins!",
            Player.Black => "Black wins!",
            _ => "It's a draw!",
        };
    }

    private static string PlayerString(Player player)
    {
        return player switch
        {
            Player.White => "White",
            Player.Black => "Black",
            _ => "",
        };

    }

    private static string GetReasonText(EndReason reason, Player currentPlayer)
    {
        return reason switch
        {
            EndReason.Stalemate => $"STALEMATE! {PlayerString(currentPlayer)} has no legal moves.",
            EndReason.Checkmate => $"CHECKMATE! {PlayerString(currentPlayer.Opponent())} Wins!",
            EndReason.Resignation => $"{PlayerString(currentPlayer)} has resigned.",
            EndReason.FiftyMoveRule => "DRAW! 50 moves without a capture or pawn move.",
            EndReason.InsufficientMaterial => "DRAW! Insufficient material to checkmate.",
            EndReason.ThreefoldRepetition => "DRAW! Threefold repetition.",
            _ => "",
        };
    }

    private void Restart_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        Console.WriteLine("You pressed restart");
        OptionSelected?.Invoke(Option.Restart);
    }

    private void Exit_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        Console.WriteLine("You pressed exit");
        OptionSelected?.Invoke(Option.Exit);
    }
}