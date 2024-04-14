using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging; // Adjusted to use Avalonia's imaging
using ChessLogic;
using Avalonia.Controls; // For using Image control

namespace ChessUI;

public static class Images
{
    // Adjusted the dictionary to store IBitmap, which is Avalonia's equivalent for image sources
    public static readonly Dictionary<PieceType, Bitmap> whiteSources = new()
    {
        { PieceType.Pawn, LoadImage("/Users/jackkennedy/Desktop/csharp/ChessGame/ChessGame/ChessUI/Assets/whitePawn.png") },
        { PieceType.Rook, LoadImage("/Users/jackkennedy/Desktop/csharp/ChessGame/ChessGame/ChessUI/Assets/whiteRook.png") },
        { PieceType.Knight, LoadImage("/Users/jackkennedy/Desktop/csharp/ChessGame/ChessGame/ChessUI/Assets/whiteKnight.png") },
        { PieceType.Bishop, LoadImage("/Users/jackkennedy/Desktop/csharp/ChessGame/ChessGame/ChessUI/Assets/whiteBishop.png") },
        { PieceType.Queen, LoadImage("/Users/jackkennedy/Desktop/csharp/ChessGame/ChessGame/ChessUI/Assets/whiteQueen.png") },
        { PieceType.King, LoadImage("/Users/jackkennedy/Desktop/csharp/ChessGame/ChessGame/ChessUI/Assets/whiteKing.png") }
    };
    
    public static readonly Dictionary<PieceType, Bitmap> blackSources = new()
    {
        { PieceType.Pawn, LoadImage("/Users/jackkennedy/Desktop/csharp/ChessGame/ChessGame/ChessUI/Assets/blackPawn.png") },
        { PieceType.Rook, LoadImage("/Users/jackkennedy/Desktop/csharp/ChessGame/ChessGame/ChessUI/Assets/blackRook.png") },
        { PieceType.Knight, LoadImage("/Users/jackkennedy/Desktop/csharp/ChessGame/ChessGame/ChessUI/Assets/blackKnight.png") },
        { PieceType.Bishop, LoadImage("/Users/jackkennedy/Desktop/csharp/ChessGame/ChessGame/ChessUI/Assets/blackBishop.png") },
        { PieceType.Queen, LoadImage("/Users/jackkennedy/Desktop/csharp/ChessGame/ChessGame/ChessUI/Assets/blackQueen.png") },
        { PieceType.King, LoadImage("/Users/jackkennedy/Desktop/csharp/ChessGame/ChessGame/ChessUI/Assets/blackKing.png") }
    };

    // Adjusted LoadImage to directly return an Avalonia IBitmap
    private static Bitmap LoadImage(string filePath)
    {
        // Assuming the path is relative to the bin directory; adjust if necessary.
        // Avalonia supports loading from absolute paths, pack URIs, or use assets directly included in the project
        
        string basePath = AppContext.BaseDirectory;
        string fullPath = System.IO.Path.Combine(basePath, filePath);
        return new Bitmap(fullPath);
    }
    
    // Changed return type to IBitmap, the interface Avalonia uses for images
    public static Bitmap GetImage(Player color, PieceType type)
    {
        return color switch
        {
            Player.White => whiteSources[type],
            Player.Black => blackSources[type],
            _ => null,
        };
    }

    public static Bitmap GetImage(Piece piece)
    {
        return piece == null ? null : GetImage(piece.Color, piece.Type);
    }
}
