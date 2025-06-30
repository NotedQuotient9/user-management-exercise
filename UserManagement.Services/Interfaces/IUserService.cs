using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService
{

    Task<IEnumerable<User>> FilterByActive(bool isActive);
    Task<IEnumerable<User>> GetAll();
    void Create(User user);
    Task<User?> GetById(long id);
    void Update(User user);
    void Delete(User user);
}
