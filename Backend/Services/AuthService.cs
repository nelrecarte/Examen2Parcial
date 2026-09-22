using BCrypt.Net;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto dto);
}

public class AuthService : IAuthService
{
    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

      
        var userData = new Dictionary<string, object>
        {
            { "Name", dto.Name },
            { "Email", dto.Email },
            { "PasswordHash", passwordHash },
            { "CreatedAt", DateTime.UtcNow }
        };

 

        return true;
    }
}