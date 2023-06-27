namespace NewCryptoParser.Exceptions;

public class CompilerException : AggregateException
{
    public CompilerException(params Exception[] innerExceptions) : base(innerExceptions) { }
}