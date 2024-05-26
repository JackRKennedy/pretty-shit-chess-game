using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ChessLogic;

namespace ChessUI;

public partial class PromotionMenu : UserControl
{
    public event Action<PieceType> PieceSelected;
    public PromotionMenu(Player player)
    {
        InitializeComponent();
        
        QueenImage.Source = Images.GetImage(player, PieceType.Queen);
        KnightImage.Source = Images.GetImage(player, PieceType.Knight);
        BishopImage.Source = Images.GetImage(player, PieceType.Bishop);
        RookImage.Source = Images.GetImage(player, PieceType.Rook);
        
    }

    private void QueenImage_OnPointerPressed(object sender, PointerPressedEventArgs e)
    {
        PieceSelected?.Invoke(PieceType.Queen);
    }

    private void KnightImage_OnPointerPressed(object sender, PointerPressedEventArgs e)
    {
        PieceSelected?.Invoke(PieceType.Knight);
    }

    private void BishopImage_OnPointerPressed(object sender, PointerPressedEventArgs e)
    {
        PieceSelected?.Invoke(PieceType.Bishop);
    }

    private void RookImage_OnPointerPressed(object sender, PointerPressedEventArgs e)
    {
        PieceSelected?.Invoke(PieceType.Rook);
    }
}