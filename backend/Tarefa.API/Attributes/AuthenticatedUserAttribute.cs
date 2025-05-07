using Microsoft.AspNetCore.Mvc;
using Tarefa.API.Filters;

namespace Tarefa.API.Attributes
{
    public class AuthenticatedUserAttribute : TypeFilterAttribute
    {
        public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter))
        {
        }
    }
}
