namespace Tarefa.Exceptions.ExceptionBase
{
    public class NotFoundException(string message) : TarefasException(message)
    {
    }
}