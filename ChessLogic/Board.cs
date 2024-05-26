namespace ChessLogic;

public class Board
{
    // stores the pieces on the board
    private readonly Piece[,] pieces = new Piece[8, 8];

    private readonly Dictionary<Player, Position> pawnSkipPositions = new Dictionary<Player, Position>
    {
        { Player.White, null },
        { Player.Black, null }
    };

    public Piece this[int row, int col]
    {
        get { return pieces[row, col]; }
        set { pieces[row, col] = value; }
    }

    public Position GetPawnSkipPosition(Player player)
    {
        return pawnSkipPositions[player];
    }

    public void SetPawnSkipPosition(Player player, Position pos)
    {
        pawnSkipPositions[player] = pos;
    }

    public Piece this[Position pos]
    {
        get { return this[pos.Row, pos.Column]; }
        set { this[pos.Row, pos.Column] = value; }
    }

    public static Board Initial()
    {
        Board board = new Board();
        board.AddStartPieces();
        return board;
    }

    private void AddStartPieces()
    {
        // add pawns
        for (int col = 0; col < 8; col++)
        {
            this[1, col] = new Pawn(Player.Black);
            this[6, col] = new Pawn(Player.White);
        }

// add rooks
        this[0, 0] = new Rook(Player.Black);
        this[0, 7] = new Rook(Player.Black);
        this[7, 0] = new Rook(Player.White);
        this[7, 7] = new Rook(Player.White);
// add knights
        this[0, 1] = new Knight(Player.Black);
        this[0, 6] = new Knight(Player.Black);
        this[7, 1] = new Knight(Player.White);
        this[7, 6] = new Knight(Player.White);
// add bishops
        this[0, 2] = new Bishop(Player.Black);
        this[0, 5] = new Bishop(Player.Black);
        this[7, 2] = new Bishop(Player.White);
        this[7, 5] = new Bishop(Player.White);
// add queens
        this[0, 3] = new Queen(Player.Black);
        this[7, 3] = new Queen(Player.White);
// add kings
        this[0, 4] = new King(Player.Black);
        this[7, 4] = new King(Player.White);
    }

    public static bool IsInside(Position pos)
    {
        return pos.Row >= 0 && pos.Column >= 0 && pos.Row < 8 && pos.Column < 8;
    }

    public bool IsEmpty(Position pos)
    {
        return this[pos] == null;
    }

    public IEnumerable<Position> PiecePositions()
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                Position pos = new Position(r, c);

                if (!IsEmpty(pos))
                {
                    yield return pos;
                }
            }
        }
    }

    public IEnumerable<Position> PiecePositionsFor(Player player)
    {
        return PiecePositions().Where(pos => this[pos].Color == player);
    }

    public bool IsInCheck(Player player)
    {
        return PiecePositionsFor(player.Opponent()).Any(pos =>
        {
            Piece piece = this[pos];
            return piece.CanCaptureOpponentKing(pos, this);
        });
    }

    public Board Copy()
    {
        Board copy = new Board();

        foreach (Position pos in PiecePositions())
        {
            copy[pos] = this[pos].Copy();
        }

        return copy;
    }

    public Counting CountPieces()
    {
        Counting counting = new Counting();

        foreach (Position pos in PiecePositions())
        {
            Piece piece = this[pos];
            counting.Increment(piece.Color, piece.Type);
        }

        return counting;
    }

    public bool InsufficientMaterial()
    {
        Counting counting = CountPieces();

        return IsKingvKing(counting) || IsKingvKingBishop(counting) || IsKingvKingKnight(counting) ||
               IsKingBishopvKingBishop(counting);
    }

    private static bool IsKingvKing(Counting counting)
    {
        return counting.TotalCount == 2;
    }

    private static bool IsKingvKingBishop(Counting counting)
    {
        return counting.TotalCount == 3 &&
               (counting.white(PieceType.Bishop) == 1 || counting.black(PieceType.Bishop) == 1);
    }

    private static bool IsKingvKingKnight(Counting counting)
    {
        return counting.TotalCount == 3 &&
               (counting.white(PieceType.Knight) == 1 || counting.black(PieceType.Knight) == 1);
    }

    private bool IsKingBishopvKingBishop(Counting counting)
    {
        if (counting.TotalCount != 4)
        {
            return false;
        }

        if (counting.white(PieceType.Bishop) != 1 || counting.black(PieceType.Bishop) != 1)
        {
            return false;
        }

        Position wBishopPos = FindPiece(Player.White, PieceType.Bishop);
        Position bBishopPos = FindPiece(Player.Black, PieceType.Bishop);

        return wBishopPos.SquareColor() == bBishopPos.SquareColor();
    }

    private Position FindPiece(Player color, PieceType type)
    {
        return PiecePositionsFor(color).First(pos => this[pos].Type == type);
    }

    private bool IsUnmovedKingAndRook(Position kingPos, Position rookPos)
    {
        if (IsEmpty(kingPos) || IsEmpty(rookPos))
        {
            return false;
        }

        Piece king = this[kingPos];
        Piece rook = this[rookPos];

        return king.Type == PieceType.King && rook.Type == PieceType.Rook && king.HasMoved == false &&
               rook.HasMoved == false;
    }

    public bool CastleRightKS(Player player)
    {
        return player switch
        {
            Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 7)),
            Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 7)),
            _ => false
        };
    }

    public bool CastleRightQS(Player player)
    {
        return player switch
        {
            Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 0)),
            Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 0)),
            _ => false
        };
    }

    private bool HasPawnInPosition(Player player, Position[] pawnPositons, Position skipPos)
    {
        foreach (Position pos in pawnPositons.Where(IsInside))
        {
            Piece piece = this[pos];
            if (piece == null || piece.Color != player || piece.Type != PieceType.Pawn)
            {
                continue;
            }

            EnPassant move = new EnPassant(pos, skipPos);
            if (move.IsLegal(this))
            {
                return true;
            }
        }

        return false;
    }
    
    public bool CanCaptureEnPassant(Player player)
    {
        Position skipPos = GetPawnSkipPosition(player.Opponent());
        
        if (skipPos == null)
        {
            return false;
        }

        Position[] pawnPositions = player switch
        {
            Player.White => new Position[] { skipPos + Direction.SouthWest, skipPos + Direction.SouthEast },
            Player.Black => new Position[] { skipPos + Direction.NorthWest, skipPos + Direction.NorthEast },
            _ => Array.Empty<Position>()
        };

        return HasPawnInPosition(player, pawnPositions, skipPos);
    }
}
    