
using System.Reflection.Metadata.Ecma335;
using Project.interfaces;
using Project.Models;

namespace Project.Services
{
    public class JewelService : IJewelService
    {
        private static List<Jewel> jewelryList { get; }

        //בנאי סטאטי שנקרא בפעם הרשונה שהמחלקה נטענת + אתחול למערך
        static JewelService()
        {
            jewelryList = new List<Jewel>
            {
              new Jewel { Id = 1, Name = "עגילי כסף דרופ פפיון נוצץ", Price = 335, Category = CategoryJewel.Earrings },
              new Jewel { Id = 2, Name = "טבעת כסף משאלת הנסיכה", Price = 379, Category = CategoryJewel.Ring }
            };
        }


        ///CRUD///

        //פונקציה לקבלת רשימת הנתונים-GET
        public  List<Jewel> GetAllList() => jewelryList;


        //id-פונקציה לקבלת אוביקט לפי 
        public  Jewel GetJewelById(int id) => jewelryList.FirstOrDefault(p => p.Id == id);


        //מכניס אוביקט חדש לרשימה
        public  void Create(Jewel newJewel)
        {
            int maxId = jewelryList.Max(p => p.Id);
            newJewel.Id = maxId + 1;
            jewelryList.Add(newJewel);
        }

        //מעדכן אוביקט מהרשימה
        public  void Update(int id, Jewel jewel)
        {
            Jewel? oldJewel = GetJewelById(id);
            if (oldJewel != null)
            {
                oldJewel.Name = jewel.Name;
                oldJewel.Price = jewel.Price;
                oldJewel.Category = jewel.Category;
            }

        }

        //ID-פונקציה למחיקת אוביקט לפי 
        public  void Delete(int id)
        {
            Jewel? jewelForDelete = GetJewelById(id);
            if (jewelForDelete != null)
            {
                int index = jewelryList.IndexOf(jewelForDelete);
                jewelryList.RemoveAt(index);
            }   
        }

    }
}

