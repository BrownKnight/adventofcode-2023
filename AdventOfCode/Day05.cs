using System.Linq.Expressions;
using System.Reflection;
using Spectre.Console;

namespace AdventOfCode;

public class Day05 : BaseDay
{
    private readonly string[] _input;

    public Day05() => _input = File.ReadAllLines(InputFilePath);

    public override ValueTask<string> Solve_1()
    {
        // Populate seeds
        // Format: "seeds: 79 14 55 13"
        var seeds = _input[0]
            .Split(':')[1]
            .Trim()
            .Split(' ')
            .Select(long.Parse)
            .Select(x => new Seed(x))
            .ToArray();

        var line = 2;
        while (line < _input.Length)
        {
            line = _input[line] switch 
            {
                "seed-to-soil map:" => ParseSourceToDestinationMap(seeds, ++line, seed => seed.SeedId, seed => seed.SoilId),
                "soil-to-fertilizer map:" => ParseSourceToDestinationMap(seeds, ++line, seed => seed.SoilId, seed => seed.FertilizerId),
                "fertilizer-to-water map:" => ParseSourceToDestinationMap(seeds, ++line, seed => seed.FertilizerId, seed => seed.WaterId),
                "water-to-light map:" => ParseSourceToDestinationMap(seeds, ++line, seed => seed.WaterId, seed => seed.LightId),
                "light-to-temperature map:" => ParseSourceToDestinationMap(seeds, ++line, seed => seed.LightId, seed => seed.TempId),
                "temperature-to-humidity map:" => ParseSourceToDestinationMap(seeds, ++line, seed => seed.TempId, seed => seed.HumidityId),
                "humidity-to-location map:" => ParseSourceToDestinationMap(seeds, ++line, seed => seed.HumidityId, seed => seed.LocationId),
                _ or "" => ++line
            };
        }

        return new(seeds.MinBy(x => x.LocationId).LocationId.ToString());
    }

    private int ParseSourceToDestinationMap(Seed[] seeds, int line, Func<Seed, long> getSource, Expression<Func<Seed, long>> getDestination)
    {
        while (line < _input.Length && _input[line] != string.Empty)
        {
            var parts = _input[line].Split(' ').Select(long.Parse).ToArray();
            var (destRangeStart, sourceRangeStart, length) = (parts[0], parts[1], parts[2]);
            var sourceRangeEnd = sourceRangeStart + length;

            var seedsToMap = seeds.Where(x => getSource(x) >= sourceRangeStart && getSource(x) < sourceRangeEnd);
            foreach (var seed in seedsToMap)
            {
                // source:50, dest:70, then offset would be 20
                // source:50, dest:10, then offset would be -40
                var destinationOffset = destRangeStart - sourceRangeStart;
                var prop = (PropertyInfo)((MemberExpression)getDestination.Body).Member;
                prop.SetValue(seed, getSource(seed) + destinationOffset, null);
            }

            line++;
        }
        var compiledGetDestination = getDestination.Compile();
        // Populate all the unmapped entries to equal the source
        foreach (var seed in seeds.Where(x => compiledGetDestination(x) == default))
        {
            var prop = (PropertyInfo)((MemberExpression)getDestination.Body).Member;
            prop.SetValue(seed, getSource(seed), null);
        }

        return line;
    }

    private record Seed(
        long SeedId,
        long SoilId = default,
        long FertilizerId = default,
        long WaterId = default,
        long LightId = default,
        long TempId = default,
        long HumidityId = default,
        long LocationId = default) 
    {
        public long SeedId { get; set; } = SeedId;
        public long SoilId { get; set; } = SoilId;
        public long FertilizerId { get; set; } = FertilizerId;
        public long WaterId { get; set; } = WaterId;
        public long LightId { get; set; } = LightId;
        public long TempId { get; set; } = TempId;
        public long HumidityId { get; set; } = HumidityId;
        public long LocationId  { get; set; } = LocationId;
    };

    public override ValueTask<string> Solve_2() => new($"Solution to {ClassPrefix} {CalculateIndex()}, part 2");
}
