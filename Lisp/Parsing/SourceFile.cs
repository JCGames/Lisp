namespace Lisp;

public class SourceFile
{
    public FileInfo? FileInfo { get; set; }
    
    private readonly string _text;
    private int _currentIndex;
    
    public bool EndOfFile => _currentIndex >= _text.Length;
    
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
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            _text = string.Empty;
        }
    }

    public char ReadChar()
    {
        if (_currentIndex >= _text.Length) return '\0';
        return _text[_currentIndex++];
    }
    
    public char PeekChar()
    {
        if (_currentIndex >= _text.Length) return '\0';
        return _text[_currentIndex];
    }
}