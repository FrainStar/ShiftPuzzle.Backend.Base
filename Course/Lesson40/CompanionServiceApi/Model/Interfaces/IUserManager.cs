
public interface IUserManager
{
    void AddUser(User user);
    User GetById(int id);
    void DeleteUser(int id);
    List<User> SearchByPath(string origin, string destination);
}