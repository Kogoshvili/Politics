using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoliticsNet.Data;
using PoliticsNet.DTO;
using PoliticsNet.Helpers;
using PoliticsNet.Models;

namespace PoliticsNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IMainRespository _mainRepo;
        private readonly IRatingsRespository _ratingRepo;
        private readonly IMapper _mapper;
        public RatingsController(IMainRespository mainRepo, IRatingsRespository ratingRepo, IMapper mapper)
        {
            _mapper = mapper;
            _ratingRepo = ratingRepo;
            _mainRepo = mainRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetTopics([FromQuery] TopicParams topicParams)
        {
            var topics = await _ratingRepo.GetTopics(topicParams);

            var topicsToReturn = _mapper.Map<IEnumerable<TopicToReturnList>>(topics);
            Response.AddPagination(topics.CurrentPage, topics.PageSize,
                 topics.TotalCount, topics.TotalPages);

            return Ok(topicsToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTopic(int id)
        {
            var topic = await _ratingRepo.GetTopic(id);

            if (topic == null)
                throw new Exception("Topic not found");

            var topicToReturn = _mapper.Map<TopicToReturn>(topic);
            return Ok(topicToReturn);
        }

        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetTopicComments(int id)
        {
            var comments = await _ratingRepo.GetTopicComments(id);

            if (comments == null)
                throw new Exception("Comments not found");

            var list1 = comments.Where(c => c.Side == "for").ToList();

            var list2 = comments.Where(c => c.Side == "against").ToList();

            var c = new List<RatingComment>();
            int length = Math.Min(list1.Count, list2.Count);

            // Combine the first 'length' elements from both lists into pairs
            var a = list1.Take(length)
            .Zip(list2.Take(length), (a, b) => new RatingComment[] { a, b })
            // Flatten out the pairs
            .SelectMany(array => array)
            // Concatenate the remaining elements in the lists)
            .Concat(list1.Skip(length))
            .Concat(list2.Skip(length));

            var commentToReturn = _mapper.Map<List<CommentToReturn>>(a);
            return Ok(commentToReturn);
        }

        [HttpGet("{id}/likes")]
        public async Task<IActionResult> GetTopicLikes(int id)
        {
            var likes = await _ratingRepo.GetTopicLikes(id);

            if (likes == null)
                throw new Exception("Comments not found");

            var likesToReturn = _mapper.Map<RatingLikesToReturn>(likes);
            return Ok(likesToReturn);
        }

        [Authorize]
        [HttpGet("{userId}/like/{topicId}")]
        public async Task<IActionResult> LikeTopic(int userId, int topicId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            //Primary Key userid+postid
            var topicLikees = await _ratingRepo.GetTopicLikes(topicId);

            var like = new RatingLike {
                TopicId = topicId,
                UserId = userId,
                Value = 1
            };

            var dislike = new RatingLike {
                TopicId = topicId,
                UserId = userId,
                Value = -1
            };
            var likeDeleted = false;

            topicLikees.ForEach(
                (i) => {
                    if(i.UserId == userId)
                    {
                        if(i.Value == 1){
                            _mainRepo.Delete<RatingLike>(like);
                            likeDeleted = true;
                        }else{
                            _mainRepo.Delete<RatingLike>(dislike);
                        }
                    }
                }
            );
            if(!likeDeleted){
                _mainRepo.Add<RatingLike>(like);
            }

            try {
                if (await _mainRepo.SaveAll())
                    return NoContent();
            }catch {
                return BadRequest("Failed");
            }

            return BadRequest("Failed");
        }

        // [Authorize]
        // [HttpGet("{userId}/dislike/{topicId}")]
        // public async Task<IActionResult> DislikeTopic(int userId, int topicId)
        // {
        //     if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        //         return Unauthorized();

        //     //Primary Key userid+postid
        //     var topicLikees = await _ratingRepo.GetTopicLikes(topicId);


        //     var like = new RatingLike {
        //         TopicId = topicId,
        //         UserId = userId,
        //         Value = 1
        //     };

        //     var dislike = new RatingLike {
        //         TopicId = topicId,
        //         UserId = userId,
        //         Value = -1
        //     };


        //     var likeDeleted = false;

        //     topicLikees.ForEach(
        //         (i) => {
        //             if(i.UserId == userId)
        //             {
        //                 if(i.Value == 1){
        //                     _mainRepo.Delete<RatingLike>(like);
        //                 }else{
        //                     _mainRepo.Delete<RatingLike>(dislike);
        //                     likeDeleted = true;
        //                 }
        //             }
        //         }
        //     );
        //     if(!likeDeleted){
        //         _mainRepo.Add<RatingLike>(dislike);
        //     }

        //     try {
        //         if (await _mainRepo.SaveAll())
        //             return NoContent();
        //     }catch {
        //         return BadRequest("Failed");
        //     }

        //     return BadRequest("Failed");
        // }

        [Authorize]
        [HttpPost("{userId}/comment/{topicId}")]
        public async Task<IActionResult> CommentTopic(int userId, int topicId, IncomingComment comment)
        {
            var commentToSave = new RatingComment {
                Content = comment.Content,
                Side = comment.Side
            };

            _ratingRepo.CreateComment(userId, topicId, commentToSave);

            if (await _mainRepo.SaveAll())
                    return NoContent();
            return BadRequest("Failed");
        }

        [AllowAnonymous]
        [HttpPost("offertopic")]
        public async Task<IActionResult> OfferTopic(IncomingTopic topic)
        {
            var topicToSave = new RatingTopic {
                Title = topic.Title,
                Description = topic.Description,
                Visible = false
            };

            _ratingRepo.CreateTopic(topicToSave);

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

            var likesFormRepo = await _ratingRepo.GetCommentLikes(commentId);

            var like = new RatingCommentLike {
                UserId = userId,
                CommentId = commentId
            };

            if(likesFormRepo.Any(l => l.UserId == userId)){
                _mainRepo.Delete<RatingCommentLike>(like);
            }else{
                _mainRepo.Add<RatingCommentLike>(like);
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