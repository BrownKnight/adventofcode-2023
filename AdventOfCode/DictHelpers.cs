namespace AdventOfCode;

public static class DictHelpers
{
    public static TValue GetValueOrDefault<TKey, TValue>(
        this IDictionary<TKey, TValue> dict,
        TKey key,
        TValue defaultValue = default) 
    {
        if (dict.TryGetValue(key, out var value)) 
        {
            return value;
        }
        return defaultValue;
    }
}