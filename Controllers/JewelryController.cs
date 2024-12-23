using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace Project.Controllers;

[ApiController]
[Route("[controller]")]
public class JewelryController : ControllerBase
{
    private static List<Jewel> jewelryList;

    //בנאי סטאטי שנקרא בפעם הרשונה שהמחלקה נטענת
    static JewelryController()
    {
        jewelryList = new List<Jewel> 
        {
            new Jewel { Id = 1, Name = "עגילי כסף דרופ פפיון נוצץ", Price = 335, Category = CategoryJewel.Earrings },
            new Jewel { Id = 2, Name = "טבעת כסף משאלת הנסיכה", Price = 379, Category = CategoryJewel.Ring }
        };
    }

    //ID-פונקציה שמחזירה אוביקט קיים לפי 
    private Jewel? SearchJewelById(int id)
    {
        Jewel? oldJewel = jewelryList.FirstOrDefault(p => p.Id == id);
        return oldJewel;
    }

    //פונקציה לקבלת רשימת הנתונים
    [HttpGet]
    public IEnumerable<Jewel> Get()
    {
        return jewelryList;
    }

    //id-פונקציה לקבלת אוביקט לפי 
    [HttpGet("{id}")]
    public ActionResult<Jewel> Get(int id)
    {
        Jewel? jewel = SearchJewelById(id);
        if (jewel == null)
            return BadRequest("Invalid id");
        return jewel;
    }

    //מכניס אוביקט לרשימה
    [HttpPost]
    public ActionResult Insert(Jewel newJewel)
    {        
        int maxId = jewelryList.Max(p => p.Id);
        newJewel.Id = maxId + 1;
        jewelryList.Add(newJewel);

        return CreatedAtAction(nameof(Insert), new { id = newJewel.Id }, newJewel);
    }  

    //מעדכן אוביקט מהרשימה
    [HttpPut("{id}")]
    public ActionResult Update(int id, Jewel jewel)
    { 
        Jewel? oldJewel = SearchJewelById(id);
        if (oldJewel == null) 
            return BadRequest("Invalid id");
        if (jewel.Id != oldJewel.Id)
            return BadRequest("id mismatch");

        oldJewel.Name = jewel.Name;
        oldJewel.Price = jewel.Price;
        oldJewel.Category = jewel.Category;
     
        return NoContent();
        
    }

    //ID-פונקציה למחיקת אוביקט לפי 
    [HttpDelete]
    public ActionResult Delete(int id)
    {
        Jewel? jewelForDelete = SearchJewelById(id);
        if (jewelForDelete == null) 
            return BadRequest("Invalid id");
        int index = jewelryList.IndexOf(jewelForDelete); 
        jewelryList.RemoveAt(index); 

        return NoContent();
    }
}
