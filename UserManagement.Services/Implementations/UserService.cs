using System.Linq;
using System.Collections.Generic;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    public async Task<IEnumerable<User>> FilterByActive(bool isActive)
    {
        var users = await _dataAccess.GetAll<User>();
        return users.Where(u => u.IsActive == isActive);
    }

    public async Task<IEnumerable<User>> GetAll() => await _dataAccess.GetAll<User>();

    public async void Create(User user)
    {
        user = await _dataAccess.Create(user);
        Console.WriteLine(user.Id);
        var log = new Log
        {
            Type = LogType.Created,
            Description = $"User: {user.Id} created; Forname: {user.Forename}, Surname: {user.Surname}, Email: {user.Email}, IsActive: {user.IsActive}, DateOfBirth: {user.DateOfBirth:dd/MM/yyyy}",
            CreatedAt = DateTime.UtcNow,
            UserId = user.Id
        };
        await _dataAccess.Create(log);
    }

    public async Task<User?> GetById(long id) => await _dataAccess.GetById<User>(id);

    public async void Update(User user)
    {
        _dataAccess.Update(user);
        var log = new Log
        {
            Type = LogType.Updated,
            Description = $"User: {user.Id} updated; Forname: {user.Forename}, Surname: {user.Surname}, Email: {user.Email}, IsActive: {user.IsActive}, DateOfBirth: {user.DateOfBirth:dd/MM/yyyy}",
            CreatedAt = DateTime.UtcNow,
            UserId = user.Id
        };
        await _dataAccess.Create(log);
    }

    public async void Delete(User user)
    {
        _dataAccess.Delete<User>(user);
        var log = new Log
        {
            Type = LogType.Deleted,
            Description = $"User: {user.Id} deleted; Forname: {user.Forename}, Surname: {user.Surname}, Email: {user.Email}, IsActive: {user.IsActive}, DateOfBirth: {user.DateOfBirth:dd/MM/yyyy}",
            CreatedAt = DateTime.UtcNow,
            UserId = user.Id
        };
        await _dataAccess.Create(log);
    }
}
