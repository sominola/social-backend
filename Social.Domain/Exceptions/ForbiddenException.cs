﻿namespace Social.Domain.Exceptions;

public class ForbiddenException:AppException
{
    public ForbiddenException(string message) : base(message)
    {
    }
}