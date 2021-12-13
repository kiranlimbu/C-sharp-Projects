using System.Collections.Generic;
using System.Threading.Tasks;
using bugspotAPI.Dtos;
using bugspotAPI.Models;
using bugspotAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace bugspotAPI.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class NotifiController : Controller
    {
        private readonly IUserRepository _repository;
        public NotifiController(IUserRepository userRepo)
        {
            _repository = userRepo;
        }

        [HttpGet]
        public async Task<List<NotifiModel>> GetReceiverNotifiAsync(string userId)
        {
            return await _repository.GetReceivedNotifiAsync(userId);
        }

        [HttpGet("{id}")]
        public async Task<List<NotifiModel>> GetSenderNotifiAsync(string userId)
        {
            return await _repository.GetSentNotifiAsync(userId);
        }

        
        [HttpPost("Create")]
        public async Task<IActionResult> Create(NotifiDto notifi)
        {
            // Required fields
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            // if everything is correct
            var newNotifi = new NotifiModel {
                title = notifi.title,
                message = notifi.message,
                receiverId = notifi.receiverId,
                viewed = notifi.viewed
            };
            // Add the new information
            await _repository.AddNotifiAsync(newNotifi);

            return Ok(new { message = "Notification successfully created." });
        }

    }
}