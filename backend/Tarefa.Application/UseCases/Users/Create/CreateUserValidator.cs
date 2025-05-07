using FluentValidation;
using Tarefa.Application.UseCases.Users.Create.Dto;

namespace Tarefa.Application.UseCases.Users.Create
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Name)           
                .NotEmpty()           
                .WithMessage("O nome não pode estar vazio.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O email não pode estar vazio.")
                .EmailAddress()
                .WithMessage("O email informado não é válido.");

            RuleFor(x => x.Password)
                .MinimumLength(6)
                .WithMessage("A senha deve conter no mínimo 6 caracteres.");
        }
    }
}
