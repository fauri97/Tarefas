using FluentValidation;
using Tarefa.Application.UseCases.Tasks.Create.Dto;

namespace Tarefa.Application.UseCases.Tasks.Create
{
    public class CreateTaskValidator : AbstractValidator<CreateTaskDto>
    {
        public CreateTaskValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Descrição é obrigatória.")
                .MinimumLength(3).WithMessage("Descrição deve ter pelo menos 3 caracteres.");

            RuleFor(x => x.ExpectedDate)
                .GreaterThan(DateTime.Now).WithMessage("Data esperada deve ser no futuro.");
        }
    }
}
