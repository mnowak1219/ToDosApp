﻿namespace MinimalAPI_ToDos.Validators;

public class ToDoValidator : AbstractValidator<ToDo>
{
    public ToDoValidator()
    {
        RuleFor(t => t.Value)
            .NotEmpty()
            .MinimumLength(5)
            .WithMessage("Value of a todo must be at least 5 characters.");
    }
}
