
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
        public List<Jewel> GetAllList(string type, int userId)
        {
            if(type.Equals("User"))
            return jewelryList.Where((jewel) => jewel.UserId == userId).ToList();
            else
            return jewelryList;
        }


        //id-פונקציה לקבלת אוביקט לפי 
        public Jewel GetJewelById(int id)
        {
            return jewelryList.FirstOrDefault(p => p.Id == id);
        }  



        //מכניס אוביקט חדש לרשימה
        public void Create(Jewel newJewel)
        {
            int maxId = jewelryList.Any() ? jewelryList.Max(p => p.Id) : 0;
            newJewel.Id = maxId + 1;
            jewelryList.Add(newJewel);
            updateJson.UpdateListInJson(jewelryList);
        }

        //מעדכן אוביקט מהרשימה
        public void Update(Jewel oldJewel, Jewel newJewel)
        {  
            oldJewel.Name = newJewel.Name;
            oldJewel.Price = newJewel.Price;
            oldJewel.Category = newJewel.Category;
            updateJson.UpdateListInJson(jewelryList);
        }

        //ID-פונקציה למחיקת אוביקט לפי 
        public void Delete(Jewel jewel)
        {
            int index = jewelryList.IndexOf(jewel);
            jewelryList.RemoveAt(index);
            updateJson.UpdateListInJson(jewelryList);
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