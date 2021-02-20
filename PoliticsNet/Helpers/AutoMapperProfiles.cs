using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PoliticsNet.DTO;
using PoliticsNet.Models;

namespace PoliticsNet.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Post, PostToReturn>()
                .ForMember(dest => dest.Likes, opt =>
                    opt.MapFrom(src => src.PostLikes.Select(l => l.UserId)));

            CreateMap<Post, PostsToReturn>()
                .ForMember(dest => dest.Likes, opt =>
                    opt.MapFrom(src => src.PostLikes.Select(l => l.UserId)))
                .ForMember(dest => dest.Images, opt =>
                    opt.MapFrom(src => src.Images.Select(i => i.ImageUrl).ToList()));

            CreateMap<PostIncoming, Post>()
                .ForMember(dest => dest.Category, opt =>
                    opt.Ignore())
                .ForMember(dest => dest.Images, opt =>
                    opt.MapFrom(src => new List<Image>()));

            CreateMap<User, UserToReturn>()
                .ForMember(dest => dest.Role, opt =>
                    opt.MapFrom(src => src.Provider.Type))
                .ForMember(dest => dest.Posts, opt =>
                    opt.MapFrom(src => src.Posts.Select(p => p.Id).ToList()));
            CreateMap<User, UserLite>();

            CreateMap<Provider, ProviderToReturn>();
            CreateMap<Provider, ProviderForPost>();
            CreateMap<Provider, ProviderToReturnProvider>()
                .ForMember(dest => dest.Members, opt =>
                    opt.MapFrom(src => src.Members.Select(p => p.Id).ToList()))
                .ForMember(dest => dest.Posts, opt =>
                    opt.MapFrom(src => src.Posts.Select(p => p.Id).ToList()));

            CreateMap<Image, ImageToReturn>();

            CreateMap<PhotoForUpload, Image>()
                .ForMember(d => d.ImageUrl, opt =>
                    opt.MapFrom(s => s.Url));

            CreateMap<Category, CategoryToReturn>();

            CreateMap<RatingTopic, TopicToReturnList>()
                .ForPath(dest => dest.Likes.UserId, opt =>
                    opt.MapFrom(src => src.RatingLikes.Select(l => l.UserId)))
                .ForPath(dest => dest.Likes.Sum, opt =>
                    opt.MapFrom(src => src.RatingLikes.Sum(s => s.Value)))
                .ForMember(dest => dest.Comments, opt =>
                    opt.MapFrom(src => src.RatingComment.Count));

            CreateMap<RatingTopic, TopicToReturn>()
                .ForPath(dest => dest.RatingLikes.UserId, opt =>
                    opt.MapFrom(src => src.RatingLikes.Select(l => l.UserId)))
                .ForPath(dest => dest.RatingLikes.Sum, opt =>
                    opt.MapFrom(src => src.RatingLikes.Sum(s => s.Value)))
                .ForMember(dest => dest.Comments, opt =>
                    opt.MapFrom(src => src.RatingComment));

            CreateMap<RatingComment, CommentToReturn>()
                .ForMember(dest => dest.Likes, opt =>
                    opt.MapFrom(src => src.Likes.Select(l => l.UserId)));

            CreateMap<List<RatingLike>, RatingLikesToReturn>()
                .ForMember(dest => dest.UserId, opt =>
                    opt.MapFrom(src => src.Select(l => l.UserId)))
                .ForMember(dest => dest.Sum, opt =>
                    opt.MapFrom(src => src.Sum(s => s.Value)));

            CreateMap<Activity, ActivityToReturn>()
                .ForMember(dest => dest.Comments, opt =>
                    opt.MapFrom(src => src.ActivityComments))
                .ForMember(dest => dest.Likes, opt =>
                    opt.MapFrom(src => src.ActivityLikes));

            CreateMap<ActivityComment, CommentToReturn>()
                .ForMember(dest => dest.Likes, opt =>
                    opt.MapFrom(src => src.Likes.Select(l => l.UserId)));

            CreateMap<List<ActivityLike>, ActivityLikesToReturn>()
                .ForMember(dest => dest.Likes, opt =>
                    opt.MapFrom(src => src.Where(l => l.Value == 1).Select(l => l.UserId)))
                .ForMember(dest => dest.Dislikes, opt =>
                    opt.MapFrom(src => src.Where(l => l.Value == -1).Select(l => l.UserId)));
        }
    }
}