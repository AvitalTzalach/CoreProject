
using System.Reflection.Metadata.Ecma335;
using Project.interfaces;
using Project.Models;


namespace Project.Services
{
    public class JewelService : IJewelService
    {
        private List<Jewel> jewelryList { get; }
        private UpdateJson<Jewel> updateJson;

        //בנאי שנקרא בפעם הרשונה שהמחלקה נטענת + אתחול למערך
        public JewelService()
        {
            string basePath = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(basePath, "Data", "jewelry.json");
            updateJson = new UpdateJson<Jewel>(filePath);
            jewelryList = updateJson.GetList();
        }


        ///CRUD///

        //פונקציה לקבלת רשימת הנתונים-GET
        public List<Jewel> GetAllList(string token)
        {
            int userId = TokenService.GetUserIdFromToken(token);
            return jewelryList.Where((jewel) => jewel.UserId == userId).ToList();
            
        }


        //id-פונקציה לקבלת אוביקט לפי 
        public Jewel GetJewelById(int id, string token)
        {
            List<Jewel> userJewelryList = GetAllList(token);
            return userJewelryList.FirstOrDefault(p => p.Id == id);
        }  



        //מכניס אוביקט חדש לרשימה
        public int Create(Jewel newJewel, string token)
        {
            int userId = TokenService.GetUserIdFromToken(token);
            if (newJewel.UserId != userId)
                return -1;
            int maxId = jewelryList.Any() ? jewelryList.Max(p => p.Id) : 0;
            newJewel.Id = maxId + 1;
            newJewel.UserId = userId;
            jewelryList.Add(newJewel);
            updateJson.UpdateListInJson(jewelryList);
            return 1;
        }

        //מעדכן אוביקט מהרשימה
        public int Update(int id, Jewel jewel, string token)
        {
            if(jewel.UserId != TokenService.GetUserIdFromToken(token))
            {
                return -1;
            }
            Jewel oldJewel = GetJewelById(id, token);
            if (oldJewel != null)
            {
                oldJewel.Name = jewel.Name;
                oldJewel.Price = jewel.Price;
                oldJewel.Category = jewel.Category;
                updateJson.UpdateListInJson(jewelryList);
            }
            return 1;
        }

        //ID-פונקציה למחיקת אוביקט לפי 
        public void Delete(int id, string token)
        {
            Jewel jewelForDelete = GetJewelById(id, token);
            if (jewelForDelete != null)
            {
                int index = jewelryList.IndexOf(jewelForDelete);
                jewelryList.RemoveAt(index);
                updateJson.UpdateListInJson(jewelryList);
            }
        }

    }

    public static class JewelryServiceHelper
    {
        public static void AddJewelService(this IServiceCollection BuilderServices)
        {
            BuilderServices.AddSingleton<IJewelService, JewelService>();
        }
    }
}

// jewelryList = new List<Jewel>
// {
//   new Jewel { Id = 1, Name = "עגילי כסף דרופ פפיון נוצץ", Price = 335, Category = CategoryJewel.Earrings },
//   new Jewel { Id = 2, Name = "טבעת כסף משאלת הנסיכה", Price = 379, Category = CategoryJewel.Ring }
// };