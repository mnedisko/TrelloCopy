public interface IUserAuthorityService
{
    List<string> GetUserAuthorities();
}

public class UserAuthorityService : IUserAuthorityService
{
    public List<string> GetUserAuthorities() => ["TaskManager", "Moderator", "Member"];
}