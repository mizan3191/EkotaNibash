namespace EkotaNibash.DataAccess
{
    public interface IAccount
    {
        int CreateUser(User user);
        bool UpdateUser(User user);
        IList<AppUsersDTO> GetUsers();
        User GetUser(int id);
        User GetLoginUser(LoginModel loginModel);
        bool DeleteUser(int id);
        User CheckForExistingAppUserId(string loginId);
    }
}