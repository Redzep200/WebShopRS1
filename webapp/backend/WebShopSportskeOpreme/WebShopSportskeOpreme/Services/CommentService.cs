using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Services
{
    public class CommentService : ICommentService
    {
        private readonly WebShopDbContext _context;
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public CommentService(WebShopDbContext context, IUserService userService, IProductService productService)
        {
            _context = context;
            _userService = userService;
            _productService = productService;
        }

        public bool createComment(Comment comment)
        {
            if(comment == null || _productService.GetProductById(comment.ProductId)==null || _userService.GetUserById(comment.UserId)==null) return false;
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return true;
        }

        public bool deleteComment(int id)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == id);
            if(comment == null) return false;            
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return true;
        }

        public List<Comment> GetAllComments()
        {
            var comments = _context.Comments.ToList();
            foreach(var item in comments) 
            {
                item.User = _userService.GetUserById(item.UserId);
                item.Product = _productService.GetProductById(item.ProductId);
            }
            return comments;
        }

        public Comment GetCommentById(int id)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == id);
            if (comment != null)
            {
                comment.User = _userService.GetUserById(comment.UserId);
                comment.Product = _productService.GetProductById(comment.ProductId);
            }
            return comment;
        }

        public List<Comment> GetCommentByReviewRate(int rate)
        {
            List<Comment> comments = _context.Comments.Where(c => c.ReviewRating == rate).ToList();
            if (comments != null)
                return comments;
            return null;
        }

        public bool updateComment(int id, Comment comment)
        {
            var updatedComment = _context.Comments.FirstOrDefault(c=>c.Id==id);
            if(updatedComment == null || comment==null ) { return false; }
            updatedComment.Text = comment.Text;
            updatedComment.Date = comment.Date;
            updatedComment.ReviewRating = comment.ReviewRating;
            updatedComment.UpdateDate = DateTime.Now;
            _context.Comments.Update(updatedComment);
            _context.SaveChanges();
            return true;
        }

        public DateTime GetCommentDateById(int id) 
        {
            var Helper = _context.Comments.FirstOrDefault(c=>c.Id==id);
            DateTime Date = Helper.Date; return Date;
        }
    }
}
