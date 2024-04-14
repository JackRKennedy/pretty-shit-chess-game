namespace ChessLogic
{
    // This class represents a direction on a chessboard.
    public class Direction
    {
        // Directions are represented as static readonly fields for certain common directions.
        public static readonly Direction North = new Direction(-1, 0); // North direction
        public static readonly Direction South = new Direction(1, 0); // South direction
        public static readonly Direction East = new Direction(0, 1); // East direction
        public static readonly Direction West = new Direction(0, -1); // West direction

        // Diagonal directions combine horizontal and vertical directions.
        public static readonly Direction NorthEast = North + East; // NorthEast direction
        public static readonly Direction NorthWest = North + West; // NorthWest direction
        public static readonly Direction SouthEast = South + East; // SouthEast direction
        public static readonly Direction SouthWest = South + West; // SouthWest direction

        // A direction is defined by a delta (change) in row and column.
        public int RowDelta { get; } // Change in row
        public int ColumnDelta { get; } // Change in column

        // Constructor to initialize the direction with provided row and column deltas
        public Direction(int rowDelta, int columnDelta)
        {
            RowDelta = rowDelta;
            ColumnDelta = columnDelta;
        }

        // Overload of '+' operator to add two direction objects.
        // The new direction is defined by the sum of the respective row and column deltas.
        public static Direction operator +(Direction dir1, Direction dir2)
        {
            return new Direction(dir1.RowDelta + dir2.RowDelta, dir1.ColumnDelta + dir2.ColumnDelta);
        }

        // Overload of '*' operator for multiplication of a direction by a scalar value.
        // The new direction is defined by the multiplication of the respective row and column deltas by the scalar.
        public static Direction operator *(int scalar, Direction dir)
        {
            return new Direction(scalar * dir.RowDelta, scalar * dir.ColumnDelta);
        }
    }
}