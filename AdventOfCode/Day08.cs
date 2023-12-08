using System.Diagnostics;

namespace AdventOfCode;

public class Day08: BaseDay
{
    private readonly string[] _input;

    public Day08() => _input = File.ReadAllLines(InputFilePath);

    public override ValueTask<string> Solve_1()
    {
        var instructions = _input[0];
        var nodes = new Dictionary<string, Node>();
        foreach (var line in _input.Skip(2))
        {
            var parts = line.Split(" = ");
            var name = parts[0];
            var leftRight = parts[1].Split(", ");
            var left = leftRight[0][1..];
            var right = leftRight[1][..^1];
            nodes.Add(name, new(left, right));
        }

        var currentNodeKey = "AAA";
        var level = 0;
        while (currentNodeKey != "ZZZ")
        {
            var instruction = instructions[level % instructions.Length];
            currentNodeKey = instruction switch 
            {
                'L' => nodes[currentNodeKey].Left,
                'R' => nodes[currentNodeKey].Right,
                _ => throw new InvalidOperationException("unknown instruction")
            };
            level++;
        }

        return new(level.ToString());
    }

    public record Node(string Left, string Right);

    public override ValueTask<string> Solve_2()
    {
        var instructions = _input[0];
        var nodes = new Dictionary<string, Node>();
        foreach (var line in _input.Skip(2))
        {
            var parts = line.Split(" = ");
            var name = parts[0];
            var leftRight = parts[1].Split(", ");
            var left = leftRight[0][1..];
            var right = leftRight[1][..^1];
            nodes.Add(name, new(left, right));
        }

        var currentNodeKeys = nodes.Where(x => x.Key.EndsWith('A')).Select(x => x.Key).ToArray();
        var level = 0;
        while (!currentNodeKeys.All(x => x.EndsWith('Z')))
        {
            var instruction = instructions[level % instructions.Length];
            for (var i = 0; i < currentNodeKeys.Length; i++)
            {
                currentNodeKeys[i] = instruction switch 
                {
                    'L' => nodes[currentNodeKeys[i]].Left,
                    'R' => nodes[currentNodeKeys[i]].Right,
                    _ => throw new InvalidOperationException("unknown instruction")
                };
            }

            level++;
            if (level % 10_000_000 == 0) Console.WriteLine($"At level {level}");
        }

        return new(level.ToString());
    }
}
