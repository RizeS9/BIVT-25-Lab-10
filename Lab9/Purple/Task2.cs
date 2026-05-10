using System.Text;

namespace Lab9.Purple;

public class Task2 : Purple
{
    private string[] _output;
    
    public string[] Output => _output;

    public Task2(string input) : base(input) { }
    
    public override void Review()
    {
        string[] words = Input.Split();
        List<string> lines = new List<string>();
        
        int i = 0;
        
        while (i < words.Length)
        {
            List<string> currentLineWords = new List<string>();
            int currentLength = 0;
            
            while (i < words.Length)
            {
                string word = words[i];
                int newLength = currentLength + (currentLineWords.Count > 0 ? 1 : 0) + word.Length;
                
                if (newLength <= 50)
                {
                    currentLineWords.Add(word);
                    currentLength = newLength;
                    i++;
                }
                else
                {
                    break;
                }
            }
            
            string line;
            if (currentLineWords.Count == 1)
            {
                line = currentLineWords[0];
            }
            else
            {
                line = DistributeSpaces(currentLineWords);
            }
            
            lines.Add(line);
        }
        
        _output = lines.ToArray();
    }
    
    private string DistributeSpaces(List<string> words)
    {
        int wordsTotalLength = 0;
        foreach (string word in words)
        {
            wordsTotalLength += word.Length;
        }
        
        int spacesToAdd = 50 - wordsTotalLength;
        int gaps = words.Count - 1;
        int minSpacesPerGap = spacesToAdd / gaps;
        int extraSpaces = spacesToAdd % gaps;
        
        string result = words[0];
        
        for (int i = 1; i < words.Count; i++)
        {
            int spacesCount = minSpacesPerGap;
            if (i - 1 < extraSpaces)
            {
                spacesCount++;
            }
            
            result += new string(' ', spacesCount) + words[i];
        }
        
        return result;
    }
    
    public override string ToString()
    {
        return string.Join("\n", _output);
    }
}