using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface ILogService
{

    Task<IEnumerable<Log>> GetByUserId(long userId);
    Task<IEnumerable<Log>> GetAll();
    Task<Log?> GetById(long id);

}
