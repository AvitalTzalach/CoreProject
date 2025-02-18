
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.interfaces;
using Project.Models;
using Project.Services;

namespace Project.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController(IUserService iUserService) : ControllerBase
{
    private IUserService iUserService = iUserService;

    //פונקציה לקבלת רשימת הנתונים
    [HttpGet]
    [Authorize(Policy = "Admin")]
    public ActionResult<List<User>> Get()
    {
        return iUserService.GetAllList();
    }


    //id-פונקציה לקבלת אוביקט לפי 
    [HttpGet("{id}")]
    [Authorize]
    public ActionResult<User> Get(int id)
    {
        string typeUser = User.FindFirst("Type")?.Value;
        if (typeUser.Equals("User"))
        {
            int userId = int.Parse(User.FindFirst("UserId")?.Value);
            if (userId != id)
            {
                return Unauthorized();
            }
        }
        User? user = iUserService.GetUserById(id);
        if (user == null)
            return BadRequest("Invalid id");
        return user;
    }

    //מכניס אוביקט חדש לרשימה
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public ActionResult Create([FromBody] User newUser)
    {
        int userId = int.Parse(User.FindFirst("UserId")?.Value);
        if (newUser.Id == userId)//מנהל לא יכול ליצור את עצמו
        {
            return Unauthorized();
        }
        iUserService.Create(newUser);
        return CreatedAtAction(nameof(Create), new { id = newUser.Id }, newUser);
    }

    //מעדכן אוביקט מהרשימה
    [HttpPut("{id}")]
    [Authorize]
    public ActionResult Update(int id, [FromBody] User newUser)
    {
        if (id != newUser.Id)
            return BadRequest("Id mismatch");
        string typeUser = User.FindFirst("Type")?.Value;
        if (typeUser.Equals("User"))
        {
            int userId = int.Parse(User.FindFirst("UserId")?.Value);
            if (userId != id || newUser.Type.Equals("Admin"))//משתמש יכול לעדכן רק את עצמו
            {
                return Unauthorized();
            }
        }
        User? oldUser = iUserService.GetUserById(id);
        if (oldUser == null)
            return BadRequest("Invalid id");
        iUserService.Update(id, newUser);
        return NoContent();
    }

    //ID-פונקציה למחיקת אוביקט לפי 
    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public ActionResult Delete(int id)
    {
        //האם מנהל יכול למחוק את עצמו
        //string jwtEncoded = Request.Headers.Authorization;
        User? userForDelete = iUserService.GetUserById(id);
        if (userForDelete == null)
            return BadRequest("Invalid id");
        iUserService.Delete(id);
        return NoContent();
    }


    [AllowAnonymous]
[HttpPost("login")]
public ActionResult Login([FromBody] User user)
{
    User? existUser = iUserService.GetExistUser(user.Name, user.Password);
    if (existUser == null)
        return Unauthorized();

    var claims = new List<Claim>();

    if ((user.Name == "Tami" || user.Name == "Avital") && user.Password == "T&a913114!")
    {
        claims.Add(new Claim("Type", "Admin"));
    }
    else
    {
        claims.Add(new Claim("Type", "User"));
    }

    claims.Add(new Claim("UserId", existUser.Id.ToString()));

    var token = TokenService.GetToken(claims);
    var generatedToken = TokenService.WriteToken(token);

    if (string.IsNullOrEmpty(generatedToken))
    {
        return StatusCode(500, "Error generating token");
    }

    return Ok(new { Name = existUser.Name, token = generatedToken });
}

}
