﻿namespace WebCRM.Domain.Exceptions;

public class EntityNotFoundException(string message) : Exception(message)
{
}