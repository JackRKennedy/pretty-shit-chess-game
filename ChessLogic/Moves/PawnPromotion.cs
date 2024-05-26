namespace ChessLogic;

public class PawnPromotion : Move
{
    public override MoveType Type => MoveType.PawnPromotion;
    public override Position FromPos { get; }
    public override Position ToPos { get; }
    
    private readonly PieceType _newType;
    
    public PawnPromotion(Position from, Position to, PieceType newType)
    {
        FromPos = from;
        ToPos = to;
        this._newType = newType;
    }

    private Piece CreatePromotionPiece(Player color) =>
        _newType switch
        {
            PieceType.Rook => new Rook(color),
            PieceType.Bishop => new Bishop(color),
            PieceType.Knight => new Knight(color),
            _ => new Queen(color)
        };

    public override bool Execute(Board board)
    {
        Piece pawn = board[FromPos];
        board[FromPos] = null;
        
        Piece promotionPiece = CreatePromotionPiece(pawn.Color);
        promotionPiece.HasMoved = true;
        board[ToPos] = promotionPiece;

        return true;
    }
}