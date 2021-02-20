using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using PoliticsNet.Models;

namespace PoliticsNet.Data
{
    public interface IRatingsRespository
    {
        Task<PagedList<RatingTopic>> GetTopics(TopicParams topicParams);
        Task<RatingTopic> GetTopic(int id);
        Task<List<RatingComment>> GetTopicComments(int id);
        void CreateTopic(RatingTopic topic);
        void CreateComment(int userId, int topicId, RatingComment comment);
        Task<List<RatingLike>> GetTopicLikes(int postId);
        Task<List<RatingCommentLike>> GetCommentLikes(int commentId);
        Task<bool> SaveAll();
    }
}