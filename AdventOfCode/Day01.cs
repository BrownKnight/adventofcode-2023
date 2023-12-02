using System.ComponentModel;

namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string[] _input;

    public Day01()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var total = _input
            .Select(x => (GetFirstNumber(x), GetLastNumber(x)))
            .Select(x => int.Parse($"{x.Item1}{x.Item2}"))
            .Sum();

        return new(total.ToString());
    }

    private string GetFirstNumber(string input) 
    {
        foreach (var letter in input)
        {
            if (char.IsDigit(letter)) 
            {
                return letter.ToString();
            }
        }
        return "";
    }
    private string GetLastNumber(string input) 
    {
        return GetFirstNumber(new string(input.Reverse().ToArray()));
    }

    public override ValueTask<string> Solve_2()
    {
        var total = _input
            .Select(x => (GetFirstNumberOrWord(x), GetLastNumberOrWord(x)))
            .Select(x => int.Parse($"{x.Item1}{x.Item2}"))
            .Sum();

        return new(total.ToString());
    }

    private static readonly (string, int)[] s_validNumberStrings = [
        ("one", 1),
        ("two", 2),
        ("three", 3),
        ("four", 4),
        ("five", 5),
        ("six", 6),
        ("seven", 7),
        ("eight", 8),
        ("nine", 9)
    ];

    private string GetFirstNumberOrWord(string input)
    {
        if (char.IsDigit(input[0])) {
            return input[0].ToString();
        }

        var startsWithNumberWord = s_validNumberStrings
            .FirstOrDefault(x => input.StartsWith(x.Item1), ("", 0));

        if (!string.IsNullOrEmpty(startsWithNumberWord.Item1)) {
            return startsWithNumberWord.Item2.ToString();
        }

        return GetFirstNumberOrWord(input[1..]);
    }

    private string GetLastNumberOrWord(string input)
    {
        if (char.IsDigit(input[^1])) {
            return input[^1].ToString();
        }

        var startsWithNumberWord = s_validNumberStrings
            .FirstOrDefault(x => input.EndsWith(x.Item1), ("", 0));

        if (!string.IsNullOrEmpty(startsWithNumberWord.Item1)) {
            return startsWithNumberWord.Item2.ToString();
        }

        return GetLastNumberOrWord(input[..^1]);
    }
}
