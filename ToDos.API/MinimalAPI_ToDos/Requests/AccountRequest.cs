namespace MinimalAPI_ToDos.Requests;

public class AccountRequest
{
    public static IResult GetToken(IAccountService service, [FromQuery] string login, [FromQuery] string password)
    {
        var user = new LoginDto
        {
            Login = login,
            Password = password
        };
        var token = service.GetToken(user);

        if (token == null)
        {
            return Results.BadRequest();
        }
        else
        {
            return Results.Ok(token);
        }
    }
    public static IResult GetInfo(IAccountService service, ClaimsPrincipal user)
    {
        var userInfo = service.GetInfo(user);

        if (userInfo != null)
        {
            return Results.Ok(userInfo);
        }
        else
        {
            return Results.BadRequest();
        }

    }
}
