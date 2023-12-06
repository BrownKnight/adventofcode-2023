namespace AdventOfCode;

public class Day06 : BaseDay
{
    private readonly string[] _input;

    public Day06() => _input = File.ReadAllLines(InputFilePath);

    public override ValueTask<string> Solve_1()
    {
        var times = _input[0].Split(':')[1].Trim().Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray();
        var distances = _input[1].Split(":")[1].Trim().Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray();

        var timeDistances = times.Zip(distances);

        var runningTotal = 1;
        foreach (var (totalTime, recordDistance) in timeDistances)
        {
            var numTimesRecordBeat = 0;
            for (var holdTime = 0; holdTime < totalTime; holdTime++)
            {
                var timeToMove = totalTime - holdTime;
                var speed = holdTime;
                var distanceTravelled = timeToMove * speed;
                if (distanceTravelled > recordDistance)
                {
                    numTimesRecordBeat++;
                }
            }
            runningTotal *= numTimesRecordBeat;
        }

        return new(runningTotal.ToString());
    }

    public override ValueTask<string> Solve_2() => new($"Solution to {ClassPrefix} {CalculateIndex()}, part 2");
}
