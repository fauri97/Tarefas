using Tarefa.Application.Services.Cryptography;

namespace UseCasesTests
{
    public class PasswordServiceTests
    {
        private readonly PasswordService _passwordService;

        public PasswordServiceTests()
        {
            _passwordService = new PasswordService();
        }

        [Fact]
        public void Deve_Criptografar_E_Verificar_Senha_Corretamente()
        {
            var plainPassword = "MinhaSenhaSegura123!";

            var hashedPassword = _passwordService.Hash(plainPassword);
            var isMatch = _passwordService.Verify(plainPassword, hashedPassword);

            Assert.NotEqual(plainPassword, hashedPassword);
            Assert.True(isMatch);
        }

        [Fact]
        public void Deve_Falhar_Verificacao_Para_Senha_Errada()
        {
            var plainPassword = "SenhaCorreta123";
            var wrongPassword = "SenhaErrada456";
            var hash = _passwordService.Hash(plainPassword);

            var result = _passwordService.Verify(wrongPassword, hash);

            Assert.False(result);
        }
    }
}
