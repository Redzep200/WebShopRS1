using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface ICommentService
    {
        Comment GetCommentById(int id);
        List<Comment> GetAllComments();
        List<Comment> GetCommentByReviewRate(int rate);
        DateTime GetCommentDateById(int id);
        bool createComment (Comment comment);
        bool updateComment (int id, Comment comment);
        bool deleteComment (int id);
    }
}
