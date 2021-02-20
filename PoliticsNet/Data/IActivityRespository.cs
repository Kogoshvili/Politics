using System.Collections.Generic;
using System.Threading.Tasks;
using PoliticsNet.Models;

namespace PoliticsNet.Data
{
    public interface IActivityRespository
    {
        Task<Activity> GetActivity(int id);
        Task<List<ActivityComment>> GetActivityComments(int id);
        void CreateActivityComment(int userId, int activityId, ActivityComment comment);
        Task<List<ActivityLike>> GetActivityLikes(int activityId);
        Task<List<ActivityCommentLike>> GetActivityCommentLikes(int commentId);
    }
}