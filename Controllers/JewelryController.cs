using Microsoft.AspNetCore.Mvc;
using Project.interfaces;
using Project.Models;
using Project.Services;

namespace Project.Controllers;

[ApiController]
[Route("[controller]")]
public class JewelryController : ControllerBase
{
    private IJewelService iJewelService;

    public JewelryController(IJewelService iJewelService)
    {
        this.iJewelService = iJewelService;
    }
    //פונקציה לקבלת רשימת הנתונים
    [HttpGet]
    public ActionResult<List<Jewel>> Get()=>
       iJewelService.GetAllList();
   

    //id-פונקציה לקבלת אוביקט לפי 
    [HttpGet("{id}")]
    public ActionResult<Jewel> Get(int id)
    {
        Jewel? jewel = iJewelService.GetJewelById(id);
        if (jewel == null)
            return BadRequest("Invalid id");
        return jewel;
    }

    //מכניס אוביקט חדש לרשימה
    [HttpPost]
    public ActionResult Create(Jewel newJewel)
    {        
        iJewelService.Create(newJewel);
        return CreatedAtAction(nameof(Create), new { id = newJewel.Id }, newJewel);
    }  

    //מעדכן אוביקט מהרשימה
    [HttpPut("{id}")]
    public ActionResult Update(int id, Jewel jewel)
    { 
        Jewel? oldJewel = iJewelService.GetJewelById(id);
        if (oldJewel == null) 
            return BadRequest("Invalid id");    

        if (jewel.Id != oldJewel.Id)
            return BadRequest("id mismatch");

        iJewelService.Update(id, jewel);
     
        return NoContent();
        
    }

    //ID-פונקציה למחיקת אוביקט לפי 
    [HttpDelete]
    public ActionResult Delete(int id)
    {
        Jewel? jewelForDelete = iJewelService.GetJewelById(id);
        if (jewelForDelete == null) 
            return BadRequest("Invalid id");

        iJewelService.Delete(id);

        return NoContent();
         
    }
}
