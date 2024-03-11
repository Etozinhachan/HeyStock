using Microsoft.EntityFrameworkCore;
using heystock.data;
using heystock.Interfaces;
using heystock.models;

namespace heystock.Repositories;

public class UserRepository : IUserRepository
{

    private readonly DbDataContext _context;

    public UserRepository(DbDataContext context)
    {
        _context = context;
    }

    public void AddUser(User user){
        _context.Users.Add(user);
        SaveChanges();
    }

    public User? getUser(int id)
    {
        return _context.Users.Where(u => u.id == id).FirstOrDefault();
    }

    public User? getUser(string username)
    {
        return _context.Users.Where(u => u.UserName == username).FirstOrDefault();
    }

    public bool hasPerm(int jwt_id, int user_id, bool isAdmin)
    {
        return  (user_id == jwt_id && !isReallyAdmin(jwt_id, isAdmin)) || (user_id != jwt_id && isReallyAdmin(jwt_id, isAdmin)) || (user_id == jwt_id && isReallyAdmin(jwt_id, isAdmin));
    }

    public bool isAdmin(int user_id)
    {
        return getUser(user_id).isAdmin;
    }

    public bool isReallyAdmin(int user_id, bool isAdminJwtValue)
    {
        var userHasAdmin = isAdmin(user_id);
        return !((!userHasAdmin && isAdminJwtValue) || (userHasAdmin && !isAdminJwtValue) || (!userHasAdmin && !isAdminJwtValue));
    }

    public User setAdmin(int user_id, bool admin)
    {
        var user = _context.Users.Where(u => u.id == user_id).First();

        user.isAdmin = admin;

        userModified(user);

        return user;
    }

    public bool UserExists(int id)
    {
        return _context.Users.Any(u => u.id == id);
    }

    public bool UserExists(string username)
    {
        return _context.Users.Any(u => u.UserName == username);
    }

    public void userModified(User user)
    {
        _context.Entry(user).State = EntityState.Modified;
        SaveChanges();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public ICollection<User> getUsers()
    {
        return _context.Users.ToList();
    }
}