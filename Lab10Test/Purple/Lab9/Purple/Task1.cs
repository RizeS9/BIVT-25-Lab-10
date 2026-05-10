using System.Text;

namespace Lab9.Purple;

public class Task1 : Purple
{
    private string _output;
    
    public string Output =>  _output;

    public Task1(string input) : base(input)
    {
        _output = input;
    }

    public override void Review()
    {
        char[] punctuationAndSpace = new[] { '.', '!', '?', ',', ':', '\"', ';', '–', '(', ')', '[', ']', '{', '}', '/', ' ' };
        string[] words = _output.Split(punctuationAndSpace, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < words.Length; i++)
        {
            char[] chars = words[i].ToCharArray();
            if (!char.IsDigit(chars[0]))
                Array.Reverse(chars);
            words[i] = new string(chars);
        }
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < words.Length; i++)
        {
            sb.Append(words[i]);
        }

        for (int i = 0; i < _output.Length; i++)
        {
            if (punctuationAndSpace.Contains(_output[i]))
            {
                sb.Insert(i, _output[i]);
            }
        }
        _output = sb.ToString();
    }

    public override string ToString()
    {
        return _output;
    }
}