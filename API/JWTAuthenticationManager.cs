using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API_TCC.Data; 
using Microsoft.EntityFrameworkCore;

namespace API_TCC
{
    public interface IJWTAuthenticationManager
    {
        Task<AuthenticationResponse> AuthenticateAsync(string nome, string senha);
    }

    public class JWTAuthenticationManager : IJWTAuthenticationManager
    {
        private readonly string tokenKey;
        private readonly Contexto _context;

        public JWTAuthenticationManager(string tokenKey, Contexto context)
        {
            this.tokenKey = tokenKey;
            _context = context;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(string nome, string senha)
        {
            // Recuperar o usuário do banco de dados usando apenas o nome
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nome == nome);

            // Se o usuário não for encontrado, retornar null
            if (user == null)
            {
                return null;
            }

            // Verificar se a senha fornecida corresponde ao hash armazenado
            if (!BCrypt.Net.BCrypt.Verify(senha, user.Senha))
            {
                return null; // Senha incorreta
            }

            // Gerar o token JWT se a autenticação for bem-sucedida
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user.Nome), // Use user.Nome em vez de nome
            new Claim("permissao", user.Permissao.ToString())
                }),
                Expires = DateTime.Now.AddHours(4),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new AuthenticationResponse
            {
                Token = tokenString,
                Permissao = user.Permissao // Adicione o campo Permissao na resposta se necessário
            };
        }
    }
}
