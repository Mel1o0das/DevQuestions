using DevQuestions.Contracts.Questions;
using FluentValidation;

namespace DevQuestions.Application.Questions;

public class CreateQuestionValidator : AbstractValidator<CreateQuestionDto>
{
    public CreateQuestionValidator()
    {
        RuleFor(q => q.Title).NotEmpty().MaximumLength(500)
            .WithMessage("Заголовок не может быть пустым или слишком длинный.");

        RuleFor(x => x.Text).NotEmpty().MaximumLength(5000).WithMessage("Текст невалидный");

        RuleFor(x => x.UserId).NotEmpty();
    }
}