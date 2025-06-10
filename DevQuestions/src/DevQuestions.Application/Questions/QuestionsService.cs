using DevQuestions.Contracts.Questions;
using DevQuestions.Domain.Questions;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DevQuestions.Application.Questions;

public class QuestionsService : IQuestionsService
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly ILogger<QuestionsService> _logger;
    private readonly IValidator<CreateQuestionDto> _validator;

    public QuestionsService(
        IQuestionsRepository questionsRepository,
        IValidator<CreateQuestionDto> validator,
        ILogger<QuestionsService> logger)
    {
        _questionsRepository = questionsRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Guid> Create(CreateQuestionDto questionDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(questionDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        int countOfOpenUserQuestions = await _questionsRepository
            .GetOpenUserQuestionsAsync(questionDto.UserId, cancellationToken);
        if (countOfOpenUserQuestions > 3)
        {
            throw new Exception("Пользователь не может открыть больше 3 вопросов.");
        }
        
        var questionId = Guid.NewGuid();

        var question = new Question(
            questionId,
            questionDto.Title,
            questionDto.Text,
            questionDto.UserId,
            null,
            questionDto.TagIds);
        
        await _questionsRepository.AddAsync(question, cancellationToken);
        
        _logger.LogInformation("Created question with id: {QuestionId}", questionId);
        
        return questionId;
    }
    
    // public async Task<IActionResult> Update(
    //     Guid questionId, 
    //     UpdateQuestionDto request, 
    //     CancellationToken cancellationToken)
    // {
    //     
    // }
    //
    // public async Task<IActionResult> Delete(Guid questionId, CancellationToken cancellationToken)
    // {
    //     
    // }
    //
    // public async Task<IActionResult> SelectSolution(
    //     Guid questionId, 
    //     Guid answerId, 
    //     CancellationToken cancellationToken)
    // {
    //     
    // }
    //
    // public async Task<IActionResult> AddAnswer(
    //     Guid questionId, 
    //     AddAnswerDto request, 
    //     CancellationToken cancellationToken)
    // {
    //     
    // }
}