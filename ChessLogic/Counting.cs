using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ChessLogic;

public class Counting
{
    private readonly Dictionary<PieceType, int> whiteCount = new();
    private readonly Dictionary<PieceType, int> blackCount = new();

    public int TotalCount { get; private set; }

    public Counting()
    {
        foreach (PieceType pieceType in Enum.GetValues(typeof(PieceType)))
        {
            whiteCount[pieceType] = 0;
            blackCount[pieceType] = 0;
        }
    }

    public void Increment(Player color, PieceType type)
    {
        if (color == Player.White)
        {
            whiteCount[type]++;
        }
        else
        {
            blackCount[type]++;
        }
        
        TotalCount++;
    }
    
    public int white(PieceType type) => whiteCount[type];
    public int black(PieceType type) => blackCount[type];

}