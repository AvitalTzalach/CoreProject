
using Project.interfaces;
using System.Security.Claims;
using Project.Models;

namespace Project.Services
{
    public class UserService : IUserService
    {
        private List<User> usersList { get; }
        private UpdateJson<User> updateJson;
        private readonly ITokenService iTokenService;

        //בנאי שנקרא בפעם הרשונה שהמחלקה נטענת + אתחול למערך
        public UserService(ITokenService iTokenService)
        {
            string basePath = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(basePath, "Data", "users.json");
            updateJson = new UpdateJson<User>(filePath);
            usersList = updateJson.GetList();
            Console.WriteLine(usersList);
            this.iTokenService = iTokenService;
        }


        ///CRUD///

        //פונקציה לקבלת רשימת הנתונים-GET
        public List<User> GetAllList()
        {
            return usersList;
        }


        //id-פונקציה לקבלת אוביקט לפי 
        public User GetUserById(int id)
        {
            return usersList.FirstOrDefault(user => user.Id == id);
        }


        //מכניס אוביקט חדש לרשימה
        public void Create(User newUser)
        {
            int maxId = usersList.Any() ? usersList.Max(p => p.Id) : 0;
            newUser.Id = maxId + 1;
            if (!(newUser.Type.Equals("User") || newUser.Type.Equals("Admin")))
            {
                newUser.Type = "User";
            }
            usersList.Add(newUser);
            updateJson.UpdateListInJson(usersList);
        }

        //מעדכן אוביקט מהרשימה
        public void Update(int id, User user)
        {
            User oldUser = GetUserById(id);
            oldUser.Name = user.Name;
            oldUser.Password = user.Password;
            oldUser.Type = user.Type;
            updateJson.UpdateListInJson(usersList);
        }

        //ID-פונקציה למחיקת אוביקט לפי 
        public void Delete(int id)
        {
            int index = usersList.IndexOf(GetUserById(id));
            usersList.RemoveAt(index);
            updateJson.UpdateListInJson(usersList);
        }


        public User? GetExistUser(string name, string Password)
        {
            return usersList.FirstOrDefault(user => user.Name == name && user.Password == Password);
        }

        public string? Login(User existUser)
        {
            var claims = new List<Claim>();

            if (existUser.Type.Equals("Admin"))
            {
                claims.Add(new Claim("Type", "Admin"));
            }
            else
            {
                claims.Add(new Claim("Type", "User"));
            }

            claims.Add(new Claim("UserId", existUser.Id.ToString()));

            var token = iTokenService.GetToken(claims);
            String generatedToken = iTokenService.WriteToken(token);
            return generatedToken;

        }


    }

    public static class UsersServiceHelper
    {
        public static void AddUserService(this IServiceCollection BuilderServices)
        {
            BuilderServices.AddSingleton<IUserService, UserService>();
        }
    }
}

