using System.Linq;
using System.Collections.Generic;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using System;

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

    public void Create(User user)
    {
        user = _dataAccess.Create(user);
        Console.WriteLine(user.Id);
        var log = new Log
        {
            Type = LogType.Created,
            Description = $"User: {user.Id} created; Forname: {user.Forename}, Surname: {user.Surname}, Email: {user.Email}, IsActive: {user.IsActive}, DateOfBirth: {user.DateOfBirth:dd/MM/yyyy}",
            CreatedAt = DateTime.UtcNow,
            UserId = user.Id
        };
        _dataAccess.Create(log);
    }

    public User? GetById(long id) => _dataAccess.GetById<User>(id);

    public void Update(User user)
    {
        _dataAccess.Update(user);
        var log = new Log
        {
            Type = LogType.Updated,
            Description = $"User: {user.Id} updated; Forname: {user.Forename}, Surname: {user.Surname}, Email: {user.Email}, IsActive: {user.IsActive}, DateOfBirth: {user.DateOfBirth:dd/MM/yyyy}",
            CreatedAt = DateTime.UtcNow,
            UserId = user.Id
        };
        _dataAccess.Create(log);
    }

    public void Delete(User user)
    {
        _dataAccess.Delete<User>(user);
        var log = new Log
        {
            Type = LogType.Deleted,
            Description = $"User: {user.Id} deleted; Forname: {user.Forename}, Surname: {user.Surname}, Email: {user.Email}, IsActive: {user.IsActive}, DateOfBirth: {user.DateOfBirth:dd/MM/yyyy}",
            CreatedAt = DateTime.UtcNow,
            UserId = user.Id
        };
        _dataAccess.Create(log);
    }
}
