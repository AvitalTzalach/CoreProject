using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace Project.interfaces
{
    public interface IUserService
    {
        List<User> GetAllList();

        User GetUserById(int id);

        void Create(User newUser);

        void Update(int id, User user);

        void Delete(string type, int id);

        User? GetExistUser(string name, string Password);

        string? Login(User user);

    }
}    