using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using Microsoft.EntityFrameworkCore;
using Politics.Data;
using PoliticsNet.Models;

namespace PoliticsNet.Data
{
    public class RatingsRespository : IRatingsRespository
    {
        private readonly DataContext _context;
        public RatingsRespository(DataContext context)
        {
            _context = context;
        }

        public void CreateTopic(RatingTopic topic)
        {
            _context.RatingTopics.Add(topic);
        }
        public async Task<PagedList<RatingTopic>> GetTopics(TopicParams topicParams)
        {
            var topics = _context.RatingTopics.Where(t => t.Visible == true).OrderByDescending(t => t.RatingLikes.Sum(l => l.Value)); //.AsQueryable();

            return await PagedList<RatingTopic>.CreateAsync(topics, topicParams.PageNumber, topicParams.PageSize);
        }
        public async Task<RatingTopic> GetTopic(int id)
        {
            return await _context.RatingTopics.Where(t => t.Visible == true).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<RatingComment>> GetTopicComments(int id)
        {
            return await _context.RatingComments.Where(c => c.TopicId == id).OrderByDescending(c => c.Likes.Sum(l => l.UserId)).ToListAsync();
        }

        public async Task<List<RatingLike>> GetTopicLikes(int id)
        {
            return await _context.RatingLikes.Where(l => l.Topic.Id == id).AsNoTracking().ToListAsync();
        }

        public void CreatePost(int id, Post post)
        {
            post.User = _context.Users.Find(id);
            post.Provider = _context.Providers.FirstOrDefault(p => p.Members.Any(m => m.Id == id));
            _context.Posts.Add(post);
        }

        public void CreateComment(int userId, int topicId, RatingComment comment)
        {
            comment.User = _context.Users.Find(userId);
            comment.Topic = _context.RatingTopics.Find(topicId);
            _context.RatingComments.Add(comment);
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<RatingCommentLike>> GetCommentLikes(int commentId)
        {
            return await _context.RatingCommentLikes.Where(l => l.CommentId == commentId).AsNoTracking().ToListAsync();
        }
    }
}