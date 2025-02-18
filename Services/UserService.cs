
using Project.interfaces;
using Project.Models;

namespace Project.Services
{
    public class UserService : IUserService
    {
        private List<User> usersList { get; }
        private UpdateJson<User> updateJson;

        //בנאי שנקרא בפעם הרשונה שהמחלקה נטענת + אתחול למערך
        public UserService()
        {
            string basePath = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(basePath, "Data", "users.json");
            updateJson = new UpdateJson<User>(filePath);
            usersList = updateJson.GetList();
            Console.WriteLine(usersList);
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

    }

    public static class UsersServiceHelper
    {
        public static void AddUserService(this IServiceCollection BuilderServices)
        {
            BuilderServices.AddSingleton<IUserService, UserService>();
        }
    }
}
    
