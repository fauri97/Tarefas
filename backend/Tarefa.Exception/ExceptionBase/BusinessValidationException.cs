namespace Tarefa.Exceptions.ExceptionBase
{
    public class BusinessValidationException : TarefasException
    {
        public IEnumerable<string> Errors { get; }

        public BusinessValidationException(string message) : base(message)
        {
            Errors = new List<string> { message };
        }

        public BusinessValidationException(IEnumerable<string> errors)
            : base("Erro(s) de validação de regra de negócio.")
        {
            Errors = errors;
        }
    }
}