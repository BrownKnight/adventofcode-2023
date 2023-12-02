using System.Collections.Concurrent;
using System.ComponentModel;

namespace AdventOfCode;

public class Day02 : BaseDay
{
    private readonly string[] _input;

    public Day02()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var runningSum = 0;

        foreach (var game in _input) 
        {
            var gameIdAndCubeLists = game.Split(':');
            var gameId = int.Parse(gameIdAndCubeLists[0].Replace("Game ", string.Empty));
            var subsetList = gameIdAndCubeLists[1].Split(";");
            var allSubsetResults = subsetList
                .Select(subset => 
                {
                    var countsByColor = GetCountsByColor(subset);
                    return GameIsWithinLimits(countsByColor);
                });
            if (allSubsetResults.All(x => x)) 
            {
                runningSum += gameId;
            }
        }

        return new(runningSum.ToString());
    }

    private ConcurrentDictionary<string, int> GetCountsByColor(string cubeList) 
    {
        var dict = new ConcurrentDictionary<string, int>();
        var cubes = cubeList.Split(',');
        foreach (var cube in cubes)
        {
            var count = int.Parse(cube.Trim().Split(" ")[0].Trim());   
            var color = cube.Trim().Split(" ")[1].Trim();
            _ = dict.AddOrUpdate(color, count, (_, old) => old + count);
        }

        return dict;
    }

    private bool GameIsWithinLimits(IDictionary<string, int> countsByColor) => 
        countsByColor.GetValueOrDefault("red") <= 12 &&
        countsByColor.GetValueOrDefault("green") <= 13 &&
        countsByColor.GetValueOrDefault("blue") <= 14;

    public override ValueTask<string> Solve_2()
    {
        var runningSum = 0;

        foreach (var game in _input) 
        {
            var gameIdAndCubeLists = game.Split(':');
            var gameId = int.Parse(gameIdAndCubeLists[0].Replace("Game ", string.Empty));
            var subsetList = gameIdAndCubeLists[1].Split(";");
            var allSubsetResults = subsetList.Select(GetCountsByColor);
            
            // Default as 1 because we're going to multiply them together so dont want zeros
            var maxReds = allSubsetResults.Select(x => x.GetValueOrDefault("red", 1)).Max();
            var maxGreens = allSubsetResults.Select(x => x.GetValueOrDefault("green", 1)).Max();
            var maxBlues = allSubsetResults.Select(x => x.GetValueOrDefault("blue", 1)).Max();

            runningSum += maxReds * maxGreens * maxBlues;
        }

        return new(runningSum.ToString());
    }
}
