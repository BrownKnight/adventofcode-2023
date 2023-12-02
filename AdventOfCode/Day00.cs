namespace AdventOfCode;

public class Day00 : BaseDay
{
    private readonly string _input;

    public Day00()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new(_input.Length.ToString());

    public override ValueTask<string> Solve_2() => new($"Solution to {ClassPrefix} {CalculateIndex()}, part 2");
}
