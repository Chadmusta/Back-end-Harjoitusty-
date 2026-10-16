using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Harjoitustyo.Models;
using Harjoitustyo.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Harjoitustyo.Middleware;
using Azure.Identity;

[Route("api/[controller]")]
[ApiController]
public class MessagesController : ControllerBase
{
    //private readonly MessageServiceContext _context;
    private readonly IMessageService _messageService;
    private readonly IUserAuthenticationService _userAuthenticationService;
    public MessagesController(IMessageService service, IUserAuthenticationService userAuthService)
    {
        _messageService = service;
        _userAuthenticationService = userAuthService;
    }

    // GET: api/Message
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessage()
    {
        return Ok(await _messageService.GetMessagesAsync());
    }

    // GET: api/Message/5
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<MessageDTO>> GetMessage(long id)
    {
        string username = this.User.FindFirst(ClaimTypes.Name).Value;
        if (!await _userAuthenticationService.CanReadMessage(username, id))

        {
            return Unauthorized();
        }
        var message = await _messageService.GetMessageAsync(id);

        if (message == null)
        {
            return NotFound();
        }

        return message;
    }

        //Hae minulle lähetetyt viestit
    [HttpGet("received/{username}")]
    [Authorize]

    public async  Task<ActionResult<IEnumerable<MessageDTO>>>GetMyMessages(string username)
    {
        string authuser = this.User.FindFirst(ClaimTypes.Name).Value;
        if (username != authuser)
        {
            return Unauthorized();
        }
        return Ok(await _messageService.GetMyReceivedMessages(username));
    }
    // PUT: api/Message/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMessage(long id, MessageDTO message)
    {

        if (id != message.Id)
        {
            return BadRequest();
        }

        //Taarkistetaan onko oma viesti
        string username = this.User.FindFirst(ClaimTypes.Name).Value;
        if (!await _userAuthenticationService.IsMyMessage(username, id))

            {
                return Unauthorized();
            }


        bool result = await _messageService.UpdateMessageAsync(message);

        if(!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/Message
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<MessageDTO>> PostMessage(MessageDTO message)
    {
        string username = this.User.FindFirst(ClaimTypes.Name).Value;
        if(username != message.SenderID)
        {
            return Unauthorized();
        }

        MessageDTO? newMessage = await _messageService.CreateMessageAsync(message);
        if(newMessage == null)
        {
            return Problem();
        }

        return CreatedAtAction("GetMessage", new { id = newMessage.Id }, newMessage);
    }

    // DELETE: api/Message/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(long id)
    {
        //tarkistetaan onko oma viesti
        string username = this.User.FindFirst(ClaimTypes.Name).Value;
        if(!await _userAuthenticationService.IsMyMessage(username, id))
        {
            return Unauthorized();
        }

        bool result = await _messageService.DeleteMessageAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
