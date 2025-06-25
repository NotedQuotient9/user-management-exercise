using System.Collections.Generic;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService
{

    IEnumerable<User> FilterByActive(bool isActive);
    IEnumerable<User> GetAll();
    void Create(User user);
    User? GetById(long id);
    void Update(User user);
}
