namespace ChessLogic;

public enum Player
{
    //Game states for whos turn it is, colour chess pieces and whether the game is over or not
    
    None, // Set winning player to none in case of a draw 
    White,
    Black
}

public static class PlayerExtensions
{
    public static Player Opponent(this Player player)
    {
        //Switches between players
        return player == Player.White ? Player.Black : Player.White;
    }
}