namespace Lab9.Purple;

public class Task4 : Purple
{
    private string _output;
    private (string bigram, char code)[] _codes;
    public string Output => _output;
    public (string bigram, char code)[] Codes => _codes;
    public Task4(string input, (string, char)[] codes) : base(input)
    {
        _output = input;
        _codes = codes;
    }

    public override void Review()
    {
        string result = _output;
        foreach (var (bigram, code) in _codes)
        {
            result = result.Replace(code.ToString(), bigram);
        }
        _output = result;
    }

    public override string ToString()
    {
        return _output;
    }
}