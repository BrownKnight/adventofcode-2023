namespace AdventOfCode;

public class Day10 : BaseDay
{
    private readonly string[] _input;

    public Day10() => _input = File.ReadAllLines(InputFilePath);

    public override ValueTask<string> Solve_1()
    {
        // Construct grid from input
        var grid = new SlotType[_input.Length][];
        var startingRowCol = (0, 0);
        for (var row = 0; row < _input.Length; row++)
        {
            grid[row] = new SlotType[_input[row].Length];
            for (var col = 0; col < _input[row].Length; col++)
            {
                var gridSlot = _input[row][col] switch 
                {
                    '.' => SlotType.Ground,
                    '|' => SlotType.UpDown,
                    '-' => SlotType.LeftRight,
                    '7' => SlotType.LeftDown,
                    'J' => SlotType.LeftUp,
                    'F' => SlotType.RightDown,
                    'L' => SlotType.RightUp,
                    'S' => SlotType.Start,
                    _ => throw new InvalidOperationException($"Unknown char {_input[row][col]}")
                };

                grid[row][col] = gridSlot;
                if (gridSlot == SlotType.Start) startingRowCol = (row, col);
            }
        }

        // Starting from S, construct a path back to S
        var path = new List<(int Row, int Col)>() { startingRowCol };
        var pointAfterStart = GetNextSlotFromStart(grid, startingRowCol.Item1, startingRowCol.Item2);
        path.Add((pointAfterStart.Row, pointAfterStart.Col));

        var previousDirection = pointAfterStart.Direction;
        while (grid[path.Last().Row][path.Last().Col] != SlotType.Start)
        {
            var (row, col) = path.Last();
            var nextDirection = grid[row][col] switch
            {
                SlotType.UpDown => previousDirection,
                SlotType.LeftRight => previousDirection,

                SlotType.LeftDown when previousDirection is Direction.Right => Direction.Down,
                SlotType.LeftDown when previousDirection is Direction.Up => Direction.Left,

                SlotType.LeftUp when previousDirection is Direction.Right => Direction.Up,
                SlotType.LeftUp when previousDirection is Direction.Down => Direction.Left,

                SlotType.RightDown when previousDirection is Direction.Left => Direction.Down,
                SlotType.RightDown when previousDirection is Direction.Up => Direction.Right,

                SlotType.RightUp when previousDirection is Direction.Left => Direction.Up,
                SlotType.RightUp when previousDirection is Direction.Down => Direction.Right,

                _ => throw new InvalidOperationException("Unknown slot type")
            };

            path.Add(ApplyDir((row, col), nextDirection));
            previousDirection = nextDirection;
        }

        return new((path.Count / 2).ToString());
    }

    private (int Row, int Col, Direction Direction) GetNextSlotFromStart(SlotType[][] grid, int row, int col)
    {       
        if (row != 0 && CheckForNonGround(grid, row - 1, col)) return (row - 1, col, Direction.Up);
        if (row != _input.Length - 1 && CheckForNonGround(grid, row + 1, col)) return (row + 1, col, Direction.Down);
        if (col != 0 && CheckForNonGround(grid, row, col - 1)) return (row, col - 1, Direction.Left);
        if (col != _input[row].Length - 1 && CheckForNonGround(grid, row, col + 1)) return (row - 1, col, Direction.Right);

        throw new InvalidOperationException("Cannot find next point from start");
    }

    private (int Row, int Col) ApplyDir((int Row, int Col) point, Direction direction) =>
        direction switch
        {
            Direction.Up => (point.Row - 1, point.Col),
            Direction.Down => (point.Row + 1, point.Col),
            Direction.Left => (point.Row, point.Col - 1),
            Direction.Right => (point.Row, point.Col + 1),
            _ => throw new InvalidOperationException("Unknwon direction")
        };

    private bool CheckForNonGround(SlotType[][] grid, int row, int col) => grid[row][col] != SlotType.Ground;

    private enum SlotType { Ground, UpDown, LeftRight, LeftDown, LeftUp, RightDown, RightUp, Start }
    private enum Direction { Up, Down, Left, Right }

    public override ValueTask<string> Solve_2() => new($"Solution to {ClassPrefix} {CalculateIndex()}, part 2");
}
