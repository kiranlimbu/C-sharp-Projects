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
    public class InviteController : Controller
    {
        private readonly IUserRepository _repository;
        public InviteController(IUserRepository userRepo)
        {
            _repository = userRepo;
        }

        [HttpGet]
        public async Task<InviteModel> GetInviteList(int inviteId, int companyId)
        {
            return await _repository.GetInviteAsync(inviteId, companyId);
        }

        
        [HttpPost("Create")]
        public async Task<IActionResult> Create(InviteDto invite)
        {
            // Required fields
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            // if everything is correct
            var newInvite = new InviteModel {
                joinDate = invite.joinDate,
                companyId = invite.companyId,
                projectId = invite.projectId,
                inviteeId = invite.inviteeId,
                inviteeFName = invite.inviteeFName,
                inviteeLName = invite.inviteeLName,
                inviteeEmail = invite.inviteeEmail,
                IsValid = invite.IsValid
            };
            // Add the new information
            await _repository.AddInviteAsync(newInvite);

            return Ok("Invite Successfully Created.");
        }
    }
}