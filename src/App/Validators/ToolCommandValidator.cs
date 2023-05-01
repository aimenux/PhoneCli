﻿using App.Commands;
using App.Exceptions;
using FluentValidation;

namespace App.Validators;

public static class ToolCommandValidator
{
    public static ValidationErrors Validate<TCommand>(TCommand command) where TCommand : AbstractCommand
    {
        return command switch
        {
            ToolCommand _ => ValidationErrors.New<ToolCommand>(),
            GenerateCommand generateCommand => Validate(new GenerateCommandValidator(), generateCommand),
            ValidateCommand validateCommand => Validate(new ValidateCommandValidator(), validateCommand),
            _ => throw new PhoneCliException($"Unexpected command type {typeof(TCommand)}")
        };
    }

    private static ValidationErrors Validate<TCommand>(IValidator<TCommand> validator, TCommand command) where TCommand : AbstractCommand
    {
        var errors = validator
            .Validate(command)
            .Errors;
        return ValidationErrors.New<TCommand>(errors);
    }
}