namespace Lisp.Parsing;

public class SourceFile
{
    public FileInfo? FileInfo { get; }
    
    private readonly char[] _text;
    private readonly List<(int start, int end)> _lines = [];

    public int CurrentLine { get; private set; } = 1;
    public int CurrentPosition { get; private set; } = -1;

    public bool EndOfFile => CurrentPosition >= _text.Length - 1;
    public int EndOfFilePosition => _text.Length - 1;

    public char Current => CurrentPosition is -1
        ? throw new InvalidOperationException("You must move to the first character to start reading the source file.")
        : CurrentPosition < _text.Length
            ? _text[CurrentPosition]
            : throw new InvalidOperationException("Oops, reached the end of the source file.");

    public char Peek => CurrentPosition + 1 >= 0 && CurrentPosition + 1 < _text.Length 
        ? _text[CurrentPosition + 1] 
        : '\0';
    
    public bool IsNewLine => 
        (Current is '\r' && Peek is '\n')
        || Current is '\r' or '\n';
     
    public SourceFile(string text)
    {
        _text = text.ToCharArray();
        
        DetermineLines();
    }

    public SourceFile(FileInfo fileInfo)
    {
        try
        {
            FileInfo = fileInfo;
            _text = File.ReadAllText(fileInfo.FullName).ToCharArray();
            
            DetermineLines();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            _text = [];
        }
    }

    public void MoveNext()
    {
        if (CurrentPosition >= _text.Length - 1) return;

        // we want to skip this if we haven't moved to the first character yet
        if (CurrentPosition >= 0)
        {
            if (_text[CurrentPosition] is '\r' && CurrentPosition + 1 < _text.Length &&
                _text[CurrentPosition + 1] is '\n')
            {
                CurrentLine++;
                CurrentPosition++;
            }
            else if (_text[CurrentPosition] is '\r' or '\n')
            {
                CurrentLine++;
            }
        }
        
        CurrentPosition++;
    }

    public void MoveToNextLine()
    {
        var lastLine = CurrentLine;
        while (!EndOfFile && CurrentLine == lastLine)
        {
            MoveNext();
        }
    }
    
    public void MoveToNonWhiteSpaceCharacter()
    {
        if (CurrentPosition is -1) MoveNext();
        
        while (!EndOfFile && char.IsWhiteSpace(Current))
        {
            MoveNext();
        }
    }

    public (int start, int end) GetStartAndEndOfLine(int line)
    {
        line--;
        if (line < 0 || line >= _text.Length) throw new ArgumentException("Line number is out of range.");
        return (_lines[line].start, _lines[line].end);
    }
    
    public ReadOnlySpan<char> GetLineSpan(int line)
    {
        line--;
        if (line < 0 || line >= _text.Length) throw new ArgumentException("Line number is out of range.");
        return new ReadOnlySpan<char>(_text, _lines[line].start, _lines[line].end - _lines[line].start + 1);
    }

    public ReadOnlySpan<char> GetSpan(int start, int end)
    {
        if (start < 0 || start >= _text.Length) throw new ArgumentException("Start is out of range.");
        if (end < 0 || end >= _text.Length) throw new ArgumentException("End is out of range.");
        return new ReadOnlySpan<char>(_text, start, end - start + 1);
    }

    public bool HasLine(int line)
    {
        return line - 1 >= 0 && line < _lines.Count;
    }

    private void DetermineLines()
    {
        var lineStart = 0;
        
        for (var lineEnd = 0; lineEnd < _text.Length; lineEnd++)
        {
            if (_text[lineEnd] == '\r' && lineEnd + 1 < _text.Length && _text[lineEnd + 1] == '\n')
            {
                _lines.Add((lineStart, lineEnd - 1 > lineStart ? lineEnd - 1 : lineStart));

                lineStart = lineEnd + 2;
                lineEnd++;
            }
            else if (_text[lineEnd] is '\r' or '\n')
            {
                _lines.Add((lineStart, lineEnd - 1 > lineStart ? lineEnd - 1 : lineStart));
                
                lineStart = lineEnd + 1;
            }
        }

        if (lineStart < _text.Length - 1)
        {
            _lines.Add((lineStart, _text.Length - 1));
        }
    }
}