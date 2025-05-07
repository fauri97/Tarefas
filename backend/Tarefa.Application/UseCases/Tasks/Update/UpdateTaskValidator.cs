using FluentValidation;
using Tarefa.Application.UseCases.Tasks.Update.Dto;

namespace Tarefa.Application.UseCases.Tasks.Update
{
    public class UpdateTaskValidator : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskValidator()
        {
            RuleFor(x => x.Description)    
                .NotEmpty().WithMessage("Descrição é obrigatória.")    
                .MinimumLength(3).WithMessage("Descrição deve ter pelo menos 3 caracteres.")    
                .When(x => x.Description != null);
        }
    }
}
