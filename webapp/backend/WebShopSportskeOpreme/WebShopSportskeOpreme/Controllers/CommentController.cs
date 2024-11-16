using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Viewmodels;

namespace WebShopSportskeOpreme.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public IActionResult GetCommentByID(int id) 
        {
            var comment = _commentService.GetCommentById(id);
            if(comment == null) { return NotFound("nije pronađen komentar"); }
            return Ok(comment);
        }

        [HttpGet]
        public IActionResult GetAllComments()
        {
            var comments = _commentService.GetAllComments();
            return Ok(comments);
        }

        [HttpGet]
        public IActionResult GetCommentByRate(int rate) 
        {
            var comment = _commentService.GetCommentByReviewRate(rate);
            if(comment == null) { return NotFound("Nema ni jedan komentar sa tom ocjenom"); }
            return Ok(comment);
        }

        [HttpGet]
        public IActionResult GetDateById(int id) 
        {
            var Date = _commentService.GetCommentDateById(id);
            return Ok(Date);
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin, Customer, Support")]
        [HttpPost]
        public IActionResult CreateNewComment(AddCommentVM model)
        {
            var newComment = _commentService.createComment(new Models.Comment
            {
                Text = model.Text,
                Date = DateTime.Now,
                ReviewRating = model.Rating,
                UserId = model.UserId,
                ProductId = model.ProductId,
                UpdateDate = (DateTime)model.UpdateDate
            });

            if (!newComment) return BadRequest("Greška pri kreiranju komentara");
            return Ok(new { success = true, message = "Uspjesno objavljen komentar" });
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpDelete]
        public IActionResult DeleteComment(int id) 
        {
            var deletedComment = _commentService.deleteComment(id);
            if (deletedComment == false) return NotFound("Pokušavate obrisati nepostojeći komentar");
            return Ok(new { success = true, message = "Uspjesno obrisan komentar" });
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin,Customer,Support")]
        [HttpPut]
        public IActionResult UpdateComment(int id,string text, int rating) 
        {
            var Helper = _commentService.GetCommentDateById(id);
            var updatedComment = _commentService.updateComment(id,new Models.Comment { Text = text, ReviewRating = rating,UpdateDate =DateTime.Now, Date=Helper});
            if (updatedComment == false) return NotFound("Pokušavate ažurirati nepostojeći komentar");
            return Ok("Komentar uspješno ažuriran");
        }
    }
}
