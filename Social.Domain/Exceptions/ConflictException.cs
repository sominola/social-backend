﻿namespace Social.Domain.Exceptions;

public class ConflictException:AppException
{
    public ConflictException(string message) : base(message)
    {
    }
}