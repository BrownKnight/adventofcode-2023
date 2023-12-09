namespace AdventOfCode;

public class Day09 : BaseDay
{
    private readonly string[] _input;

    public Day09() => _input = File.ReadAllLines(InputFilePath);

    public override ValueTask<string> Solve_1()
    {
        var runningSum = 0;
        foreach (var line in _input)
        {
            var values = line.Split(' ').Select(int.Parse).ToArray();
            var sequences = new List<int[]>() { values };

            var lastSequence = sequences.Last();
            while (lastSequence.Any(x => x != 0))
            {
                var nextSequence = new int[lastSequence.Length - 1];
                for (var i = 0; i < nextSequence.Length; i++)
                {
                    nextSequence[i] = lastSequence[i + 1] - lastSequence[i];
                }

                sequences.Add(nextSequence);
                lastSequence = nextSequence;
            }

            // Starting from second last sequence, to the first sequence, calculate the new value
            for (var i = sequences.Count - 2; i >= 0; i--)
            {
                var newValue = sequences[i].Last() + sequences[i + 1].Last();
                Console.WriteLine($"new value {newValue}");
                sequences[i] = sequences[i].Append(newValue).ToArray();
            }

            runningSum += sequences.First().Last();
        }

        return new(runningSum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var runningSum = 0;
        foreach (var line in _input)
        {
            var values = line.Split(' ').Select(int.Parse).ToArray();
            var sequences = new List<int[]>() { values };

            var lastSequence = sequences.Last();
            while (lastSequence.Any(x => x != 0))
            {
                var nextSequence = new int[lastSequence.Length - 1];
                for (var i = 0; i < nextSequence.Length; i++)
                {
                    nextSequence[i] = lastSequence[i + 1] - lastSequence[i];
                }

                sequences.Add(nextSequence);
                lastSequence = nextSequence;
            }

            // Starting from second last sequence, to the first sequence, calculate the new value
            for (var i = sequences.Count - 2; i >= 0; i--)
            {
                var newValue = sequences[i].First() - sequences[i + 1].First();
                Console.WriteLine($"new first value {newValue}");
                sequences[i] = [newValue, ..sequences[i]];
            }
            Console.WriteLine($"New extrapolated value {sequences.First().First()}");
            runningSum += sequences.First().First();
        }

        return new(runningSum.ToString());
    }
}
