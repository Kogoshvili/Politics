using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using PoliticsNet.Models;

namespace PoliticsNet.Data
{
    public interface IPostRespository
    {
        Task<Post> GetPost(int id);
        Task<PagedList<Post>> GetPosts(PostParams postParams);
        void CreatePost(int id, Post post);
        void UpdatePost(Post post);
        void DeletePost(Post post);
        void DeleteImage(int id);
        void DeleteAllImages(int id);
        void AddImage(int id, Image image);
        Task<List<int>> GetPostLikes(int postId);
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategory(string name);
        Task<bool> SaveAll();
    }
}