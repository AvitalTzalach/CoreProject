using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.interfaces;
using Project.Models;
using Project.Services;

namespace Project.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class JewelryController : ControllerBase
{
    private IJewelService iJewelService;

    public JewelryController(IJewelService iJewelService)
    {
        this.iJewelService = iJewelService;
    }
    //פונקציה לקבלת רשימת הנתונים
    [HttpGet]
    public ActionResult<List<Jewel>> Get() 
    {
        string jwtEncoded = Request.Headers.Authorization;
        return iJewelService.GetAllList(jwtEncoded);
    }
       

    //id-פונקציה לקבלת אוביקט לפי 
    [HttpGet("{id}")]
    public ActionResult<Jewel> Get(int id)
    {
        string jwtEncoded = Request.Headers.Authorization;
        Jewel? jewel = iJewelService.GetJewelById(id, jwtEncoded);
        if (jewel == null)
            return BadRequest("Invalid id");    
        return jewel;
    }

    //מכניס אוביקט חדש לרשימה
    [HttpPost]
    public ActionResult Create(Jewel newJewel)
    {    
        string jwtEncoded = Request.Headers.Authorization; 
        int result = iJewelService.Create(newJewel, jwtEncoded);
        if(result == -1){
            return Unauthorized();
        }
        return CreatedAtAction(nameof(Create), new { id = newJewel.Id }, newJewel);
    }  

    //מעדכן אוביקט מהרשימה
    [HttpPut("{id}")]
    public ActionResult Update(int id, Jewel jewel)
    { 
        string jwtEncoded = Request.Headers.Authorization; 
        Jewel? oldJewel = iJewelService.GetJewelById(id, jwtEncoded);
        if (oldJewel == null) 
            return BadRequest("Invalid id");    
        if (jewel.Id != oldJewel.Id)
            return BadRequest("Id mismatch");
        int result = iJewelService.Update(id, jewel, jwtEncoded);
        if(result == -1)
        {
            return Unauthorized();
        }

        return NoContent();
        
    }

    //ID-פונקציה למחיקת אוביקט לפי 
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        string jwtEncoded = Request.Headers.Authorization;
        Jewel? jewelForDelete = iJewelService.GetJewelById(id, jwtEncoded);
        if (jewelForDelete == null) 
            return BadRequest("Invalid id");
        iJewelService.Delete(id, jwtEncoded);
        
        return NoContent();  
    }

}
