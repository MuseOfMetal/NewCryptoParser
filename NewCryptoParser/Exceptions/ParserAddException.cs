namespace NewCryptoParser.Exceptions;

public class ParserAddException : Exception
{
    public ParserAddException()
    {
        
    }
    public ParserAddException(string message) : base(message)
    {
        
    }
    public ParserAddException(string message, Exception ex) : base(message, ex)
    {

    }
}
