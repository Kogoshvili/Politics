using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoliticsNet.Data;
using PoliticsNet.DTO;
using PoliticsNet.Models;

namespace PoliticsNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMainRespository _mainRepo;
        private readonly IActivityRespository _activityRepo;
        public ActivitiesController(IMapper mapper, IMainRespository mainRepo, IActivityRespository activityRepo)
        {
            _activityRepo = activityRepo;
            _mainRepo = mainRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAction()
        {
            var activity = await _activityRepo.GetActivity(1);

            var activityToReturn = _mapper.Map<ActivityToReturn>(activity);

            if (activity.ActivityComments.Count > 0)
            {
                var list1 = activity.ActivityComments.Where(c => c.Side == "for").ToList();

                var list2 = activity.ActivityComments.Where(c => c.Side == "against").ToList();

                var c = new List<ActivityComment>();
                int length = Math.Min(list1.Count, list2.Count);

                var a = list1.Take(length)
                .Zip(list2.Take(length), (a, b) => new ActivityComment[] { a, b })
                .SelectMany(array => array)
                .Concat(list1.Skip(length))
                .Concat(list2.Skip(length));

                activityToReturn.Comments = _mapper.Map<List<CommentToReturn>>(a);
            }

            return Ok(activityToReturn);
        }

        [Authorize]
        [HttpGet("{userId}/like/{activityId}")]
        public async Task<IActionResult> LikeAction(int userId, int activityId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            //Primary Key userid+postid
            var activityLikes = await _activityRepo.GetActivityLikes(activityId);

            var like = new ActivityLike {
                ActivityId = activityId,
                UserId = userId,
                Value = 1
            };

            var dislike = new ActivityLike {
                ActivityId = activityId,
                UserId = userId,
                Value = -1
            };

            var likeDeleted = false;

            if(activityLikes.Any(a => a.UserId == userId))
            {
                if(activityLikes.Any(a => a.UserId == userId && a.Value == 1))
                {
                    likeDeleted = true;
                    _mainRepo.Delete<ActivityLike>(like);
                }else{
                    _mainRepo.Delete<ActivityLike>(dislike);
                }
            }

            if(!likeDeleted){
                _mainRepo.Add<ActivityLike>(like);
            }

            try {
                if (await _mainRepo.SaveAll())
                    return NoContent();
            }catch {
                return BadRequest("Failed");
            }

            return BadRequest("Failed");
        }

        [Authorize]
        [HttpGet("{userId}/dislike/{activityId}")]
        public async Task<IActionResult> DislikeAction(int userId, int activityId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            //Primary Key userid+postid
            var activityLikes = await _activityRepo.GetActivityLikes(activityId);

            var like = new ActivityLike {
                ActivityId = activityId,
                UserId = userId,
                Value = 1
            };

            var dislike = new ActivityLike {
                ActivityId = activityId,
                UserId = userId,
                Value = -1
            };

            var likeDeleted = false;

            if(activityLikes.Any(a => a.UserId == userId))
            {
                if(activityLikes.Any(a => a.UserId == userId && a.Value == 1))
                {
                    _mainRepo.Delete<ActivityLike>(like);
                }else{
                    likeDeleted = true;
                    _mainRepo.Delete<ActivityLike>(dislike);
                }
            }

            if(!likeDeleted){
                _mainRepo.Add<ActivityLike>(dislike);
            }

            try {
                if (await _mainRepo.SaveAll())
                    return NoContent();
            }catch {
                return BadRequest("Failed");
            }

            return BadRequest("Failed");
        }

        [Authorize]
        [HttpPost("{userId}/comment/{activityId}")]
        public async Task<IActionResult> CommentTopic(int userId, int activityId, IncomingComment comment)
        {
            var commentToSave = new ActivityComment {
                Content = comment.Content,
                Side = comment.Side
            };

            _activityRepo.CreateActivityComment(userId, activityId, commentToSave);

            if (await _mainRepo.SaveAll())
                    return NoContent();

            return BadRequest("Failed");
        }

        [Authorize]
        [HttpGet("{userId}/likeComment/{commentId}")]
        public async Task<IActionResult> LikeComment(int userId, int commentId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var likesFormRepo = await _activityRepo.GetActivityCommentLikes(commentId);

            var like = new ActivityCommentLike {
                UserId = userId,
                CommentId = commentId
            };

            if(likesFormRepo.Any(l => l.UserId == userId)){
                _mainRepo.Delete<ActivityCommentLike>(like);
            }else{
                _mainRepo.Add<ActivityCommentLike>(like);
            }

            try {
                if (await _mainRepo.SaveAll())
                    return NoContent();
            }catch {
                return BadRequest("Failed");
            }

            return BadRequest("Failed");
        }
    }
}