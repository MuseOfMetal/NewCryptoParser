﻿namespace NewCryptoParser.Exceptions;

public class TooManyAttemptsException : Exception
{
    public TooManyAttemptsException(string? message) : base(message) { }
}