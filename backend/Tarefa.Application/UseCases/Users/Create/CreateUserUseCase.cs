using AutoMapper;
using Tarefa.Application.Services.Cryptography;
using Tarefa.Application.UseCases.Users.Create.Dto;
using Tarefa.Domain.Entities;
using Tarefa.Domain.Repositories;
using Tarefa.Domain.Repositories.Users;
using Tarefa.Domain.Security.Tokens;
using Tarefa.Exceptions.ExceptionBase;

namespace Tarefa.Application.UseCases.Users.Create
{
    public class CreateUserUseCase (
        IUserReadOnlyRepository readOnlyRepository,
        IUserWriteOnlyRepository writeOnlyRepository,
        IAccessTokenGenerator accessTokenGenerator,
        IMapper mapper,
        IPasswordService passwordService,
        IUnityOfWork unityOfWork) : ICreateUserUseCase
    {
        private readonly IUserReadOnlyRepository _readOnlyRepository = readOnlyRepository;
        private readonly IUserWriteOnlyRepository _writeOnlyRepository = writeOnlyRepository;
        private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
        private readonly IMapper _mapper = mapper;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly IUnityOfWork _unityOfWork = unityOfWork;
        public async Task<CreatedUserDto> ExecuteAsync(CreateUserDto createUserDto)
        {
            await Validate(createUserDto);
            var user = _mapper.Map<User>(createUserDto);
            user.Password = _passwordService.Hash(createUserDto.Password);
            user.UserIdentifier = Guid.NewGuid();

            _writeOnlyRepository.CreateUser(user);
            await _unityOfWork.SaveChangesAsync();

            var response = _mapper.Map<CreatedUserDto>(user);
            response.AccessToken = _accessTokenGenerator.Generate(user);
            return response;
        }

        private async Task Validate(CreateUserDto createUserDto)
        {
            var validator = new CreateUserValidator();
            var result = validator.Validate(createUserDto);

            bool exist = await _readOnlyRepository.ExistUserWithEmail(createUserDto.Email);

            if ( exist )
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("Email", "Já existe um usuário com esse email."));
            }

            if (!result.IsValid)
            {
                throw new BusinessValidationException(result.Errors.Select(e => e.ErrorMessage));
            }

        }
    }
}
