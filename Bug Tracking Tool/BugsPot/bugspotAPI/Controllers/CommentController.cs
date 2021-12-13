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
    public class CommentController : Controller
    {
        private readonly IProjectRepository _repository;
        public CommentController(IProjectRepository projRepo)
        {
            _repository = projRepo;
        }

        [HttpGet]
        public IEnumerable<CommentModel> GetCommentList()
        {
            return _repository.GetComment();
        }

        [HttpGet("{id}")]
        public IActionResult GetComment(int id)
        {
            var existingComment = _repository.CommentById(id);
            
            if(existingComment == null) 
            {
                return BadRequest(new { message = "No match found in the database." });
            }
            
            return Ok(existingComment);
        }

        
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CommentDto comment)
        {
            // Required fields
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            // if everything is correct
            var newComment = new CommentModel {
                userId = comment.userId,
                content = comment.content
            };
            // Add the new information
            return Created("Comment successfully added.", await _repository.AddComment(newComment));
        }

        [HttpPut("Edit/{id}")]
        public IActionResult Edit(int id, CommentDto comment)
        {
            // get company info by that id
            var existingComment = _repository.CommentById(id);
            
            // check if the value is null
            if (existingComment == null)
            {
                return BadRequest(new
                {
                    message = "No match found in the database."
                });
            }

            if (comment.userId != null) existingComment.userId = comment.userId;
            if (comment.content != null) existingComment.content = comment.content;

            // save the changes
            _repository.SaveChanges();

            return Ok(existingComment);
        }

        [HttpDelete("Remove/{id}")]
        public IActionResult Delete(int id)
        {
            // get company by id
            var existingComment = _repository.CommentById(id);

            // check if value is null
            if (existingComment == null)
            {
                return BadRequest(new
                {
                    message = "Operation could not complete. No match found in the database."
                });     
            }

            _repository.DeleteComment(existingComment); 

            return Ok(new { message = "Comment was successfully removed from your database." });
        }

    }
}