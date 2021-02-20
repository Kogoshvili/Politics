using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Politics.Data;
using PoliticsNet.Models;

namespace PoliticsNet.Data
{
    public class ActivityRespository : IActivityRespository
    {
        private readonly DataContext _context;
        public ActivityRespository(DataContext context)
        {
            _context = context;
        }
        public void CreateActivityComment(int userId, int activityId, ActivityComment comment)
        {
            comment.User = _context.Users.Find(userId);
            comment.Activity = _context.Activities.Find(activityId);
            _context.ActivityComments.Add(comment);
        }

        public async Task<Activity> GetActivity(int id)
        {
            return await _context.Activities.Where(t => t.Visible == true).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<ActivityCommentLike>> GetActivityCommentLikes(int commentId)
        {
            return await _context.ActivityCommentLikes.Where(l => l.CommentId == commentId).AsNoTracking().ToListAsync();
        }

        public async Task<List<ActivityComment>> GetActivityComments(int id)
        {
            return await _context.ActivityComments.Where(c => c.ActivityId == id).OrderByDescending(c => c.Likes.Sum(l => l.UserId)).ToListAsync();
        }

        public async Task<List<ActivityLike>> GetActivityLikes(int activityId)
        {
            return await _context.ActivityLikes.Where(l => l.ActivityId == activityId).AsNoTracking().ToListAsync();
        }
    }
}