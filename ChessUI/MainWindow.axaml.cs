using Avalonia.Controls;
using ChessLogic;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Image = Avalonia.Controls.Image;
using Point = Avalonia.Point;
using Rectangle = Avalonia.Controls.Shapes.Rectangle;
using Color = Avalonia.Media.Color;
using Brushes = Avalonia.Media.Brushes;

namespace ChessUI;

public partial class MainWindow : Window
{

    private readonly Image[,] pieceImages = new Image[8, 8];
    private readonly Rectangle[,] highlights = new Rectangle[8, 8];
    private readonly Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();

    private GameState gameState;
    private Position selectedPos = null;

    public MainWindow()
    {
        InitializeComponent();
        initializeBoard();

        gameState = new GameState(Player.White, Board.Initial());
        DrawBoard(gameState.Board);

        //add event listeners
        PieceGrid.AddHandler(PointerPressedEvent, BoardGrid_MouseDown, RoutingStrategies.Bubble, true);
    }

    private void initializeBoard()
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                Image image = new Image();
                pieceImages[r, c] = image;
                PieceGrid.Children.Add(image);

                Rectangle highlight = new Rectangle();
                highlights[r, c] = highlight;
                HightlightGrid.Children.Add(highlight);
            }
        }
    }

    private void DrawBoard(Board board)
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                Piece piece = board[r, c];
                pieceImages[r, c].Source = (IImage)Images.GetImage(piece);
            }
        }
    }

    void BoardGrid_MouseDown(object sender, PointerPressedEventArgs e)
    {
        Point point = e.GetPosition(BoardGrid);
        Position pos = ToSquarePosition(point);

        if (selectedPos == null)
        {
            OnFromPositionSelected(pos);
        }
        else
        {
            OnToPositionSelected(pos);
        }
    }

    private Position ToSquarePosition(Point point)
    {
        double squareSize = BoardGrid.Bounds.Width / 8;
        int row = (int)(point.Y / squareSize);
        int column = (int)(point.X / squareSize);
        return new Position(row, column);
    }

    private void OnFromPositionSelected(Position pos)
    {
        IEnumerable<Move> moves = gameState.LegalMoveForPiece(pos);
        if (moves.Any())
        {
            selectedPos = pos;
            CacheMoves(moves);
            ShowHighlights(moves.Select(move => move.ToPos), selectedPos);
        }
    }

    private void OnToPositionSelected(Position pos)
    {
        HideHighlights();
        selectedPos = null;

        if (moveCache.TryGetValue(pos, out Move move))
        {
            HandleMove(move);
        }
    }

    private static readonly Color HighlightColor = Color.FromArgb(125, 255, 255, 0); // Semi-transparent yellow
    private static readonly Color SelectedColor = Color.FromArgb(125, 255, 255, 0); // More transparent yellow

    private void ShowHighlights(IEnumerable<Position> legalPositions, Position selectedPosition = null)
    {
        // Highlight legal positions
        foreach (var pos in legalPositions)
        {
            highlights[pos.Row, pos.Column].Fill = new SolidColorBrush(HighlightColor);
        }

        // Highlight selected position, if any
        if (selectedPosition != null)
        {
            highlights[selectedPosition.Row, selectedPosition.Column].Fill = new SolidColorBrush(SelectedColor);
        }
    }


    private void HideHighlights()
    {
        foreach (Position to in moveCache.Keys)
        {
            highlights[to.Row, to.Column].Fill = Brushes.Transparent;
        }

        // Un-highlight the selected position.
        if (selectedPos != null)
        {
            highlights[selectedPos.Row, selectedPos.Column].Fill = Brushes.Transparent;
        }
    }

    private void HandleMove(Move move)
    {
        gameState.MakeMove(move);
        DrawBoard(gameState.Board);
    }

    private void CacheMoves(IEnumerable<Move> moves)
    {
        moveCache.Clear();

        foreach (Move move in moves)
        {
            moveCache[move.ToPos] = move;
        }
    }
}