namespace Lisp.Parsing;

public class SourceFile
{
    public FileInfo? FileInfo { get; set; }
    
    private readonly string _text;
    private readonly List<int> _lineStarts = [];
    
    public int CurrentLine { get; private set; }
    public int CurrentPosition { get; private set; }

    public bool EndOfFile => CurrentPosition >= _text.Length;
    
    public SourceFile(string text)
    {
        _text = text;
    }

    public SourceFile(FileInfo fileInfo)
    {
        try
        {
            FileInfo = fileInfo;
            _text = File.ReadAllText(fileInfo.FullName);

            _lineStarts.Add(0);
            
            for (var i = 0; i < _text.Length; i++)
            {
                if (_text[i] is '\r' && i + 1 < _text.Length && _text[i + 1] is '\n')
                {
                    i++;
                    _lineStarts.Add(i + 1);
                }
                else if (_text[i] is '\r' or '\n')
                {
                    _lineStarts.Add(i);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            _text = string.Empty;
        }
    }

    public char ReadChar()
    {
        if (CurrentPosition >= _text.Length) return '\0';
        
        if (CurrentLine + 1 < _lineStarts.Count && CurrentPosition == _lineStarts[CurrentLine + 1] + 1)
        {
            CurrentLine++;
        }
        
        return _text[CurrentPosition++];
    }
    
    public char PeekChar()
    {
        if (CurrentPosition >= _text.Length) return '\0';
        return _text[CurrentPosition];
    }
}