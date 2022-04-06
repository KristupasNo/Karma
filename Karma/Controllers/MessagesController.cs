using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Karma.Models;
using Karma.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Karma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        public MessagesController(IMessageRepository messageService)
        {
            MessageService = messageService;
        }

        public IMessageRepository MessageService { get; }

        [HttpGet("to/{email}")]
        public async Task<ActionResult<Message>> GetMessagesTo(string email)
        {
            try
            {
                var messages = await MessageService.GetMessagesToEmail(email);
                return Ok(messages);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("from/{email}")]
        public async Task<ActionResult<Message>> GetMessagesFrom(string email)
        {
            try
            {
                var messages = await MessageService.GetMessagesFromEmail(email);
                return Ok(messages);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            try
            {
                var postedMessage = await MessageService.AddMessage(message);
                return Ok(postedMessage);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        public async Task<ActionResult<Message>> Delete(int id)
        {
            try
            {
                var result = await MessageService.GetMessages();

                if (result == null)
                {
                    return NotFound();
                }

                return await MessageService.DeleteMsg(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
    }
}
