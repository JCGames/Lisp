using Lisp.Parsing.Nodes;

namespace Lisp.Parsing;

public class Parser
{
    private readonly SourceFile _sourceFile;
    private int _currentLine = 1;
    
    public Parser(SourceFile sourceFile)
    {
        _sourceFile = sourceFile;
    }
    
    public List<LispList> Parse()
    {
        var list = new List<LispList>();
        
        while (!_sourceFile.EndOfFile)
        {
            var c = _sourceFile.ReadChar();

            if (c is ';')
            {
                SkipComment();
                continue;
            }

            if (c is '(')
            {
                list.Add(ReadTokens());
            }
        }

        return list;
    }

    private LispList ReadTokens()
    {
        var list = new LispList();

        var c = '\0';
            
        while (!_sourceFile.EndOfFile)
        {
            c = _sourceFile.ReadChar();
            
            if (c is ')') break;

            if (c is '\r' && _sourceFile.PeekChar() is '\n')
            {
                _sourceFile.ReadChar();
                _currentLine++;
            }
            else if (c is '\r' or '\n')
            {
                _currentLine++;
            }
            else if (c is ';')
            {
                SkipComment();
            }
            else if (c is '\'' && _sourceFile.PeekChar() is '(')
            {
                _sourceFile.ReadChar();
                
                var newList = ReadTokens();
                newList.IsQuoted = true;
                list.Nodes.Add(newList);
            }
            else if (c is '"')
            {
                list.Nodes.Add(ReadStringLiteralToken(c));
            }
            else if (char.IsDigit(c) || (c is '.' && char.IsDigit(_sourceFile.PeekChar())))
            {
                list.Nodes.Add(ReadNumberToken(c));
            }
            else if (char.IsWhiteSpace(c))
            {
                SkipWhitespace();
            }
            else if (c is '(')
            {
                list.Nodes.Add(ReadTokens());
            }
            else
            {
                list.Nodes.Add(ReadIdentifierToken(c));
            }
        }

        if (_sourceFile.EndOfFile && c is not ')')
        {
            // throw error here
        } 
        
        return list;
    }
    
    private Token ReadStringLiteralToken(char startQuote)
    {
        var token = string.Empty;
        var c = '\0';
        
        while (!_sourceFile.EndOfFile)
        {
            c = _sourceFile.ReadChar();

            if (c == startQuote) break;
            
            token += c;
        }

        if (_sourceFile.EndOfFile && c != startQuote)
        {
            // throw error
        }

        return new Token
        {
            FileInfo = _sourceFile.FileInfo,
            Text = token,
            Line = _currentLine,
            Type = TokenType.StringLiteral
        };
    }

    private Token ReadIdentifierToken(char firstCharacter)
    {
        var token = firstCharacter.ToString();
        
        while (!_sourceFile.EndOfFile && !char.IsWhiteSpace(_sourceFile.PeekChar()) && _sourceFile.PeekChar() is not '(' and not ')')
        {
            token += _sourceFile.ReadChar();
        }

        // if (token is "true" or "false")
        // {
        //     return new()
        //     {
        //         FileInfo = _sourceFile.FileInfo,
        //         Text = token,
        //         Line = _currentLine,
        //         Type = TokenType.Boolean
        //     };
        // }
        
        if (token.StartsWith('&'))
        {
            return new()
            {
                FileInfo = _sourceFile.FileInfo,
                Text = token,
                Line = _currentLine,
                Type = TokenType.RestIdentifier
            };
        }
        
        return new()
        {
            FileInfo = _sourceFile.FileInfo,
            Text = token,
            Line = _currentLine,
            Type = TokenType.Identifier
        };
    }

    private Token ReadNumberToken(char firstCharacter)
    {
        var token = firstCharacter.ToString();
        var c = '\0';
        var foundPoint = false;
        
        while (!_sourceFile.EndOfFile && char.IsNumber(_sourceFile.PeekChar()) || _sourceFile.PeekChar() is '.' or ',')
        {
            c = _sourceFile.ReadChar();
            
            if (c is '.' && !foundPoint)
            {
                foundPoint = true;
            }
            else if (c is '.' && foundPoint)
            {
                // throw error
            }
            
            token += c;
        }

        if (foundPoint)
        {
            return new Token
            {
                FileInfo = _sourceFile.FileInfo,
                Text = token,
                Line = _currentLine,
                Type = TokenType.Decimal
            };
        }
        
        return new Token
        {
            FileInfo = _sourceFile.FileInfo,
            Text = token,
            Line = _currentLine,
            Type = TokenType.Integer
        };
    }

    private void SkipWhitespace()
    {
        while (!_sourceFile.EndOfFile && char.IsWhiteSpace(_sourceFile.PeekChar()))
        {
            _sourceFile.ReadChar();
        }
    }

    private void SkipComment()
    {
        var c = '\0';
        
        while (!_sourceFile.EndOfFile)
        {
            c = _sourceFile.ReadChar();

            if (c is '\r' && _sourceFile.PeekChar() is '\n')
            {
                _sourceFile.ReadChar();
                _currentLine++;
                break;
            }
            
            if (c is '\n' or '\r')
            {
                _currentLine++;
                break;
            }
        }
    }
}