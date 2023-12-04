namespace AdventOfCode;

public class Day03 : BaseDay
{
    private readonly string[] _input;

    public Day03()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var runningSum = 0;
        for (var line = 0; line < _input.Length; line++)
        {
            var num = string.Empty;
            for (var ch = 0; ch < _input[line].Length; ch++)
            {
                if (char.IsDigit(_input[line][ch]))
                {
                    // Found a number, so add it to the tracker
                    num += _input[line][ch];

                    // Reached the end of the line and have a number, so search
                    if (ch == _input[line].Length - 1)
                    {
                        var numberStartIndex = ch - num.Length;
                        var numberEndIndex = ch;
                        if (IsAdjacentToSymbol(numberStartIndex, numberEndIndex, line))
                        {
                            runningSum += int.Parse(num);
                        }
                        
                        // Reset num and continue parsing
                        num = string.Empty;
                    }
                }
                else if (num != string.Empty)
                {
                    // Recorded a number and now it is complete. Find out if it is adjacent to a symbol
                    var numberStartIndex = ch - num.Length;
                    var numberEndIndex = ch - 1;
                    if (IsAdjacentToSymbol(numberStartIndex, numberEndIndex, line))
                    {
                        runningSum += int.Parse(num);
                    }
                    
                    // Reset num and continue parsing
                    num = string.Empty;
                }
            }
        }

        return new(runningSum.ToString());
    }

    private bool IsAdjacentToSymbol(int numberStartIndex, int numberEndIndex, int lineIndex)
    {
        // Look around the number for a symbol
        var searchStartX = Math.Max(numberStartIndex - 1, 0);
        var searchEndX = Math.Min(numberEndIndex + 1, _input[lineIndex].Length - 1);
        var searchStartY = Math.Max(lineIndex - 1, 0);
        var searchEndY = Math.Min(lineIndex + 1, _input.Length - 1);

        for (var x = searchStartX; x <= searchEndX; x++)
        {
            for (var y = searchStartY; y <= searchEndY; y++)
            {
                if (!char.IsDigit(_input[y][x]) && _input[y][x] != '.') return true;
            }
        }

        return false;
    }

    public override ValueTask<string> Solve_2() => new($"Solution to {ClassPrefix} {CalculateIndex()}, part 2");
}
