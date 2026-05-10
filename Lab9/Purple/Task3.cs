namespace Lab9.Purple;

public class Task3 : Purple
{
    private string _output;
    private (string, char)[] _codes;
    public string Output => _output;
    public (string, char)[] Codes => _codes;

    public Task3(string input) : base(input){}

    public override void Review()
    {
        string text = Input;
        
        var bigramStats = new Dictionary<string, (int count, int firstPos)>();
    
        for (int i = 0; i < text.Length - 1; i++)
        {
            if (!char.IsLetter(text[i]) || !char.IsLetter(text[i + 1]))
                continue;
        
            string bigram = text.Substring(i, 2);
        
            if (!bigramStats.ContainsKey(bigram))
            {
                bigramStats[bigram] = (count: 0, firstPos: i);
            }
            
            bigramStats[bigram] = (bigramStats[bigram].count + 1, bigramStats[bigram].firstPos);
        }
        
        List<(string bigram, int count, int firstPos)> bigramList = new List<(string, int, int)>();
        foreach (var kvp in bigramStats)
        {
            bigramList.Add((kvp.Key, kvp.Value.count, kvp.Value.firstPos));
        }
        
        bigramList = bigramList.OrderByDescending(b => b.count).ThenBy(b => b.firstPos).ToList();
        
        int topCount = Math.Min(5, bigramList.Count);
        var topBigrams = bigramList.GetRange(0, topCount);
        
        List<char> availableChars = new List<char>();
        for (char c = ' '; c <= '~'; c++)  // символы от пробела до тильды (ASCII 32-126)
        {
            if (!text.Contains(c))
            {
                availableChars.Add(c);
                if (availableChars.Count == topCount)
                    break;
            }
        }
        
        _codes = new (string, char)[topCount];
        for (int i = 0; i < topCount; i++)
        {
            _codes[i] = (topBigrams[i].bigram, availableChars[i]);
        }
        
        string result = text;
        foreach (var (bigram, code) in _codes)
        {
            result = result.Replace(bigram, code.ToString());
        }
        _output = result;
    }

    public override string ToString()
    {
        return _output;
    }
}