using System.Linq;
using System.Collections.Generic;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class LogService : ILogService
{
    private readonly IDataContext _dataAccess;
    public LogService(IDataContext dataAccess) => _dataAccess = dataAccess;
    public IEnumerable<Log> GetByUserId(long userId) => _dataAccess.GetAll<Log>().Where(l => l.UserId == userId);
    public IEnumerable<Log> GetAll() => _dataAccess.GetAll<Log>();
    public Log? GetById(long id) => _dataAccess.GetById<Log>(id);
}
