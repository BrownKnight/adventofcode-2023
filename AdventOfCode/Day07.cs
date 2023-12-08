using System.Data;
using System.Security.Cryptography;

namespace AdventOfCode;

public class Day07 : BaseDay
{
    private readonly string[] _input;

    public Day07() => _input = File.ReadAllLines(InputFilePath);

    public override ValueTask<string> Solve_1()
    {
        var hands = _input.Select(line =>
        {
            var hand = line.Split(' ')[0];
            var bid = int.Parse(line.Split(' ')[1]);
            return Hand.FromHandString(hand, bid);
        });

        var ordered = hands
            .OrderBy(x => (int)x.GetHandValue())
            .ThenBy(x => x.ToStringRepresentation())
            .Select((hand, rank) => (hand, rank + 1, hand.Bid * (rank + 1)))
            .ToArray();

        return new(ordered.Sum(x => x.Item3).ToString());
    }

    private record Hand(int[] CardValues, int Bid)
    {
        public static Hand FromHandString(string hand, int bid) =>
            new(
                hand
                    .Select(card => card switch
                    {
                        'A' => 14,
                        'K' => 13,
                        'Q' => 12,
                        'J' => 11,
                        'T' => 10,
                        _ => int.Parse(card.ToString())
                    })
                    .ToArray(),
                bid);

        public string ToStringRepresentation() =>
            new(CardValues.Select(val => (char)('A' + val)).ToArray());

        public HandValue GetHandValue()
        {
            var groups = CardValues.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            if (groups.Count == 1) return HandValue.FiveOfAKind;
            if (groups.Count == 2)
            {
                if (groups.Any(x => x.Value == 4)) return HandValue.FourOfAKind;
                if (groups.Any(x => x.Value == 3)) return HandValue.FullHouse;
            }
            if (groups.Count == 3)
            {
                if (groups.Any(x => x.Value == 3)) return HandValue.ThreeOfAKind;
                if (groups.Count(x => x.Value == 2) == 2) return HandValue.TwoPair;
            }
            if (groups.Count == 4) return HandValue.OnePair;
            
            return HandValue.HighCard;
        }
    }

    private enum HandValue {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }

    public override ValueTask<string> Solve_2() => new($"Solution to {ClassPrefix} {CalculateIndex()}, part 2");
}
