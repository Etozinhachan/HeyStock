using heystock.models;

namespace heystock.Interfaces;

public interface IUserRepository
{
    public void AddUser(User user);
    public User? getUser(int id);
    public ICollection<User> getUsers();
    public User? getUser(string username);
    public User? getUserByEmail(string email);
    public bool UserExists(int id);
    public bool UserExists(string username);
    public bool UserExistsByEmail(string email);
    public bool isAdmin(int user_id);
    public bool isReallyAdmin(int user_id, bool isAdminJwtValue);
    public bool hasPerm(int jwt_id, int user_id, bool isAdmin);
    public User setAdmin(int user_id, bool admin);
    public void userModified(User user);
    public void SaveChanges();
}