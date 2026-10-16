using Harjoitustyo.Models;
using Harjoitustyo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    //private readonly MessageServiceContext _context;
    private readonly IUserService _service;
    public UsersController(IUserService service)
    {
        _service = service;
    }

    // GET: api/User
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUser()
    {
        return Ok(await _service.GetUsersAsync());
    }

    // GET: api/User/5
    [HttpGet("{username}")]
    public async Task<ActionResult<UserDTO>> GetUser(string username)
    {
        var user = await _service.GetUserAsync(username);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // PUT: api/User/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{username}")]
    [Authorize]
    public async Task<IActionResult> PutUser(string username, User user)
    {
        if (username != user.Username)
        {
            return BadRequest();
        }

        string authname = this.User.FindFirst(ClaimTypes.Name).Value;

        bool result = await _service.UpdateUserAsync(user);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/User
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<UserDTO>> PostUser(User user)
    {
        UserDTO? newUser = await _service.CreateUserAsync(user); 
        if (newUser == null)
        {
            return Problem();
        }

        return CreatedAtAction("GetUser", new { username = newUser.UserName }, user);
    }

    // DELETE: api/User/5
    [HttpDelete("{username}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(string username)
    {
        string authname = this.User.FindFirst(ClaimTypes.Name).Value;

        bool result = await _service.DeleteUserAsync(username);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    /*private bool UserExists(long? id)
    {
        return _service.User.Any(e => e.Id == id);
    }*/
}
