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
            // Find the first holdTime where the recordDistance is beat
            var minHoldTime = 0;
            for (var holdTime = 0; holdTime < totalTime; holdTime++)
            {
                var timeToMove = totalTime - holdTime;
                var speed = holdTime;
                var distanceTravelled = timeToMove * speed;
                if (distanceTravelled > recordDistance)
                {
                    minHoldTime = holdTime;
                    break;
                }
            }

            // do the same but in reverse to find the highest amount of holdTime where the record is beat
            var maxHoldTime = 0;
            for (var holdTime = totalTime; holdTime > minHoldTime; holdTime--)
            {
                var timeToMove = totalTime - holdTime;
                var speed = holdTime;
                var distanceTravelled = timeToMove * speed;
                if (distanceTravelled > recordDistance)
                {
                    maxHoldTime = holdTime;
                    break;
                }
            }

            runningTotal *= maxHoldTime - minHoldTime + 1;
        }

        return new(runningTotal.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var totalTime = long.Parse(_input[0].Split(':')[1].Trim().Replace(" ", string.Empty));
        var recordDistance = long.Parse(_input[1].Split(':')[1].Trim().Replace(" ", string.Empty));

        // Find the first holdTime where the recordDistance is beat
        var minHoldTime = 0L;
        for (var holdTime = 0; holdTime < totalTime; holdTime++)
        {
            var timeToMove = totalTime - holdTime;
            var speed = holdTime;
            var distanceTravelled = timeToMove * speed;
            if (distanceTravelled > recordDistance)
            {
                minHoldTime = holdTime;
                break;
            }
        }

        // do the same but in reverse to find the highest amount of holdTime where the record is beat
        var maxHoldTime = 0L;
        for (var holdTime = totalTime; holdTime > minHoldTime; holdTime--)
        {
            var timeToMove = totalTime - holdTime;
            var speed = holdTime;
            var distanceTravelled = timeToMove * speed;
            if (distanceTravelled > recordDistance)
            {
                maxHoldTime = holdTime;
                break;
            }
        }

        return new((maxHoldTime - minHoldTime + 1).ToString());
    }
}
