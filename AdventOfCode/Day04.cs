namespace AdventOfCode;

public class Day04 : BaseDay
{
    private readonly string[] _input;

    public Day04()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var runningTotal = 0d;
        foreach (var card in _input)
        {
            var data = card.Split(':')[1].Trim().Split('|');
            var winningNumbers = data[0].Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray();
            var playedNumbers = data[1].Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray();

            var matchedNumberCount = playedNumbers.Intersect(winningNumbers).Count();
            if (matchedNumberCount > 0) 
            {
                runningTotal += Math.Pow(2, matchedNumberCount - 1);
            }
        }

        return new(runningTotal.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var gameData = _input.Select(card => 
        {
            var gameId = int.Parse(card.Split(':')[0].Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ElementAt(1));
            var data = card.Split(':')[1];
            var numberData = data.Trim().Split('|');
            var winningNumbers = numberData[0].Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray();
            var playedNumbers = numberData[1].Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray();
            return new Game(gameId, winningNumbers, playedNumbers);
        }).ToArray();

        var rounds = new List<Game[]>() { gameData };

        var nextRound = new List<Game>();
        do 
        {
            nextRound = new List<Game>();
            foreach(var card in rounds.Last())
            {
                if (card.MatchedNumbersCount > 0) 
                {
                    var gamesToAdd = gameData.Skip(card.GameId).Take(card.MatchedNumbersCount);
                    nextRound.AddRange(gamesToAdd);
                }
            }

            if (nextRound.Any())
            {
                rounds.Add(nextRound.ToArray());
            }

        } while (nextRound.Any());

        var totalCards = rounds.SelectMany(x => x).Count();
        return new(totalCards.ToString());
    }

    private record Game(int GameId, int[] WinningNumbers, int[] PlayedNumbers)
    {
        public int MatchedNumbersCount { get; } = PlayedNumbers.Intersect(WinningNumbers).Count();
    }
}
