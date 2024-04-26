
public class UserManager : IUserManager 
{   
    private readonly UserContext _context;

    public UserManager(UserContext context)
    {
        _context = context;
    }

    public void AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public User GetById(int id)
    {
        return _context.Users.FirstOrDefault(u => u.ID == id);
    }

    public void DeleteUser(int id)
    {   
        if (_context.Users.FirstOrDefault(u => u.ID == id) != null) 
        {
            _context.Users.Where(u => u.ID == id).ToList().ForEach(t => _context.Users.Remove(t));
            _context.SaveChanges();
        }
    }

    public List<User> SearchByPath(string origin, string destination)
    {
        return _context.Users.Where(o => o.Origin == origin && o.Destination == destination).ToList();
    } 
}