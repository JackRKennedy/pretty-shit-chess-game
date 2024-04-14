namespace ChessLogic
{
    // This class represents a position on a chessboard with a row and a column.
    public class Position
    {
        // Row of the position on the chessboard
        public int Row { get; }

        // Column of the position on the chessboard
        public int Column { get; }

        // Constructor to initialize the position with provided row and column
        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        // Method to determine the color of the square at this position.
        public Player SquareColor()
        {
            // If the sum of row and column is even, it's a white square, else it's a black square.
            return (Row + Column) % 2 == 0 ? Player.White : Player.Black;
        }

        // Method to compare this position with another object.
        public override bool Equals(object obj)
        {
            // If the object is null or of different type, return false.
            if (obj == null || GetType() != obj.GetType())
                return false;

            Position p = (Position)obj;
            // If both row and column are equal with the other object, return true.
            return (Row == p.Row) && (Column == p.Column);
        }

        // Method to get a hash code of this position
        public override int GetHashCode()
        {
            // Combines the hash code of row and column into a tuple
            return Tuple.Create(Row, Column).GetHashCode();
        }

        // Operator overloading for '==', which return true if two positions are equal
        public static bool operator ==(Position left, Position right)
        {
            return EqualityComparer<Position>.Default.Equals(left, right);
        }

        // Operator overloading for '!=', which return true if two positions are not equal
        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

        // Operator overloading for '+', which allows adding a direction to a position
        // This is useful for moving pieces on the chessboard.
        public static Position operator +(Position pos, Direction dir)
        {
            // Return a new position calculated by adding the row delta and column delta values from the direction to the position.
            return new Position(pos.Row + dir.RowDelta, pos.Column + dir.ColumnDelta);
        }
    }
}