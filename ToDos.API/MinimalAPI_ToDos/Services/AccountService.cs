namespace MinimalAPI_ToDos.Services;
public interface IAccountService
{
    string GetToken(LoginDto dto);
    User GetInfo(ClaimsPrincipal user);

}

public class AccountService : IAccountService
{
    private readonly AuthenticationSettings _authenticationSettings;
    public AccountService(AuthenticationSettings authenticationSettings)
    {
        _authenticationSettings = authenticationSettings;
    }
    public string GetToken(LoginDto dto)
    {
       var userList = new List<User>{
            new User { UserId= "1", UserLogin = "J.Kowalski", UserName = "Jan Kowalski", UserPassword = "janek", UserRole = "Administrator"},
            new User { UserId= "2", UserLogin = "D.Nowicki", UserName = "Dariusz Nowicki", UserPassword = "darek", UserRole = "Maintenance"},
            new User { UserId= "3", UserLogin = "B.Szymczak", UserName = "Bartosz Szymczak", UserPassword = "bartek", UserRole = "Operator"},
        };
        var user = userList.FirstOrDefault(u => u.UserLogin == dto.Login);
        if (user != null)
        {
            if(user.UserLogin == dto.Login)
            {
                var token = new JwtSecurityToken
                (
                    issuer: _authenticationSettings.JwtIssuer,
                    audience: _authenticationSettings.JwtIssuer,
                    claims: new[]
                        {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.UserRole),
                        },
                    expires: DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey)),
                        SecurityAlgorithms.HmacSha256)
                );

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return jwtToken;
            }
        }
        return null; 
        
    }

    public User GetInfo(ClaimsPrincipal user)
    {
        var userInfo = new User();
        if (user.Claims.Count() == 0)
        {
            return null;
        }
        else
        {
            userInfo.UserId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            userInfo.UserName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            userInfo.UserRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            return userInfo;
        }
    }
}