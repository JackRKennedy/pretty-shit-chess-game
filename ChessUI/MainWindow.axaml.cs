using Avalonia.Controls;
using ChessLogic;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using static Avalonia.Application;
using Image = Avalonia.Controls.Image;
using Point = Avalonia.Point;
using Rectangle = Avalonia.Controls.Shapes.Rectangle;
using Color = Avalonia.Media.Color;
using Brushes = Avalonia.Media.Brushes;

namespace ChessUI;

public partial class MainWindow : Window
{
    
    // set event listener for the p key being pressed
    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.P)
        {
            ShowPauseMenu();
        }
        
        else if (e.Key == Key.R)
        {
            ShowResignationMenu();
        }
    }

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
                pieceImages[r, c].Source = Images.GetImage(piece);
            }
        }
    }

    void BoardGrid_MouseDown(object sender, PointerPressedEventArgs e)
    {
        if (IsMenuOnScreen())
        {
            return;
        }
        
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
            if (move.Type == MoveType.PawnPromotion)
            {
                HandlePromotion(move.FromPos, move.ToPos);
            }
            else
            {
                HandleMove(move);
            }
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
    
    private void HandlePromotion(Position from, Position to)
    {
        
        pieceImages[to.Row, to.Column].Source = Images.GetImage(gameState.CurrentPlayer, PieceType.Pawn);
        pieceImages[from.Row, from.Column].Source = null;
        
        PromotionMenu promotionMenu = new PromotionMenu(gameState.CurrentPlayer);
        MenuContainer.Content = promotionMenu;

        promotionMenu.PieceSelected += pieceType =>
        {
            MenuContainer.Content = null;
            Move promotionMove = new PawnPromotion(from, to, pieceType); 
            HandleMove(promotionMove);
            
        };
    }

    private void HandleMove(Move move)
    {
        gameState.MakeMove(move);
        DrawBoard(gameState.Board);
        
        if (gameState.IsGameOver())
        {
            ShowGameOver();
        }
    }

    private void CacheMoves(IEnumerable<Move> moves)
    {
        moveCache.Clear();

        foreach (Move move in moves)
        {
            moveCache[move.ToPos] = move;
        }
    }

    private bool IsMenuOnScreen()
    {
        return MenuContainer.Content != null;
    }
    
    private void RestartGame()
    {
        HideHighlights();
        moveCache.Clear();
        gameState = new GameState(Player.White, Board.Initial());
        DrawBoard(gameState.Board);
    }
    
    // Method to show pause menu 'p' key is pressed
    private void ShowPauseMenu()
    {
        PauseMenu pauseMenu = new PauseMenu();
        MenuContainer.Content = pauseMenu;

        pauseMenu.OptionSelected += option =>
        {
            if (option == Option.Restart)
            {
                MenuContainer.Content = null;
                RestartGame();
            }
            if (option == Option.Continue)
            {
                MenuContainer.Content = null;
            }
        };
    }
    
    public void ShowResignationMenu()
    {
        ResignationMenu resignationMenu = new ResignationMenu();
        MenuContainer.Content = resignationMenu;

        resignationMenu.OptionSelected += option =>
        {
            if (option == Option.Resign)
            {
                gameState.Resign();
                ShowGameOver();
            }
            else if (option == Option.Continue)
            {
                MenuContainer.Content = null;
            }
        };
    }
    
    private void ShowGameOver()
    {
        GameOverMenu gameOverMenu = new GameOverMenu(gameState);
        MenuContainer.Content = gameOverMenu;

        gameOverMenu.OptionSelected += option =>
        {
            if (option == Option.Restart)
            {
                MenuContainer.Content = null;
                RestartGame();
            }
            else
            {
                // Shut down the application
                Close();
            }
        };
    }
}