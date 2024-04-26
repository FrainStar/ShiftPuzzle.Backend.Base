using Microsoft.AspNetCore.Mvc;


public class UserController : ControllerBase
{
    private readonly IUserManager _userManager;

    public UserController(IUserManager userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("/api/carpool/add")]
    public IActionResult Add([FromBody] User user)
    {
        _userManager.AddUser(user);
        return Ok("Попутчик добавлен");
    }

    [HttpDelete("/api/carpool/{id}")]
    public IActionResult Delete(int id)
    {
        if (_userManager.GetById(id) != null)
        {
            _userManager.DeleteUser(id);
            return Ok("Попутчик удален");
        }
        return NotFound("Попутчик не найден");
    }

    [HttpGet("/api/carpool/search")] 
    public IActionResult SearchCompanionByPath(string origin, string destination)
    {
        if (_userManager.SearchByPath(origin, destination) != null)
        {
            return Ok(_userManager.SearchByPath(origin, destination));
        }
        return NotFound($"Попутчиков из {origin} в {destination} не найдено");
    }
}