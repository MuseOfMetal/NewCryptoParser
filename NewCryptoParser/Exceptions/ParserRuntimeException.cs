namespace NewCryptoParser.Exceptions;

public class ParserRuntimeException :Exception
{
    public ParserRuntimeException(Exception ex) : base(ex.Message, ex) { }
}