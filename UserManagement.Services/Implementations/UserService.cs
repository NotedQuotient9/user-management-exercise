using System.Linq;
using System.Collections.Generic;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    public IEnumerable<User> FilterByActive(bool isActive)
    {
        return _dataAccess.GetAll<User>().Where(u => u.IsActive == isActive);
    }

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();

    public void Create(User user) => _dataAccess.Create(user);

    public User? GetById(long id) => _dataAccess.GetById<User>(id);
}
