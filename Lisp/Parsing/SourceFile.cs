namespace Lisp.Parsing;

public class SourceFile
{
    public FileInfo? FileInfo { get; set; }
    
    public string Text { get; set; }
    private readonly List<int> _lineStarts = [];

    private int _currentLineZeroIndexed;
    public int CurrentLine => _currentLineZeroIndexed + 1;
    public int CurrentPosition { get; private set; }

    public bool EndOfFile => CurrentPosition >= Text.Length;
    
    public SourceFile(string text)
    {
        Text = text;
    }

    public SourceFile(FileInfo fileInfo)
    {
        try
        {
            FileInfo = fileInfo;
            Text = File.ReadAllText(fileInfo.FullName);

            _lineStarts.Add(0);
            
            for (var i = 0; i < Text.Length; i++)
            {
                if (Text[i] is '\r' && i + 1 < Text.Length && Text[i + 1] is '\n')
                {
                    i++;
                    _lineStarts.Add(i + 1);
                }
                else if (Text[i] is '\r' or '\n')
                {
                    _lineStarts.Add(i);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Text = string.Empty;
        }
    }

    public char ReadChar()
    {
        if (CurrentPosition >= Text.Length) return '\0';
        
        if (_currentLineZeroIndexed + 1 < _lineStarts.Count && CurrentPosition == _lineStarts[_currentLineZeroIndexed + 1])
        {
            _currentLineZeroIndexed++;
        }
        
        return Text[CurrentPosition++];
    }
    
    public char PeekChar()
    {
        if (CurrentPosition >= Text.Length) return '\0';
        return Text[CurrentPosition];
    }

    public (int start, int end) GetStartAndEndOfLine(int line)
    {
        return (_lineStarts[line - 1], line < _lineStarts.Count ? _lineStarts[line] : Text.Length);
    }
}