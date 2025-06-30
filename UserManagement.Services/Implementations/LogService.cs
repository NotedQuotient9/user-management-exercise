using System.Linq;
using System.Collections.Generic;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using System.Threading.Tasks;

namespace UserManagement.Services.Domain.Implementations;

public class LogService : ILogService
{
    private readonly IDataContext _dataAccess;
    public LogService(IDataContext dataAccess) => _dataAccess = dataAccess;
    public async Task<IEnumerable<Log>> GetByUserId(long userId)
    {
        var logs = await _dataAccess.GetAll<Log>();
        return logs.Where(l => l.UserId == userId);
    }
    public async Task<IEnumerable<Log>> GetAll() => await _dataAccess.GetAll<Log>();
    public async Task<Log?> GetById(long id) => await _dataAccess.GetById<Log>(id);
}
