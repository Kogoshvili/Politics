using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using Microsoft.EntityFrameworkCore;
using Politics.Data;
using PoliticsNet.Models;

namespace PoliticsNet.Data
{
    public class PostRespository : IPostRespository
    {
        private readonly DataContext _context;
        public PostRespository(DataContext context)
        {
            _context = context;

        }

        public async Task<PagedList<Post>> GetPosts(PostParams postParams)
        {
            var posts = _context.Posts
                .OrderByDescending(u => u.CreatedAt).AsQueryable();

            if(postParams.Provider != null)
            {
                posts = posts.Where(p => p.Provider.Name == postParams.Provider);
            }

            if(postParams.Category != null)
            {
                posts = posts.Where(p => p.Category.Name == postParams.Category);
            }

            return await PagedList<Post>.CreateAsync(posts, postParams.PageNumber, postParams.PageSize);
        }

        public async Task<Post> GetPost(int id)
        {
            return await _context.Posts.FirstOrDefaultAsync(p => p.Id  == id);
        }

        public void CreatePost(int id, Post post)
        {
            post.User = _context.Users.Find(id);
            post.Provider = _context.Providers.FirstOrDefault(p => p.Members.Any(m => m.Id == id));
            _context.Posts.Add(post);
        }

        public void UpdatePost(Post post)
        {
            _context.Posts.Update(post);
        }
        public void DeleteImage(int id)
        {
            _context.Images.Remove(new Image{Id = id});
        }
        public void AddImage(int id, Image image)
        {
            _context.Posts.Find(id).Images.Add(image);
        }
        public void DeleteAllImages(int id)
        {
            var imgs = _context.Posts.FirstOrDefault(p => p.Id == id).Images.ToList();
            imgs.ForEach(
                (i) => {
                    _context.Images.Remove(i);
                }
            );
        }
        public List<Image> ListOfImages(int id)
        {
            return _context.Posts.FirstOrDefault(p => p.Id == id).Images.ToList();

        }
        public void DeletePost(Post post)
        {
            _context.Posts.Remove(post);
        }

        public async Task<Category> GetCategory(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<List<int>> GetPostLikes(int postId)
        {
            return await _context.PostLikes.Where(l => l.Post.Id == postId).Select(l => l.User.Id).ToListAsync();
        }
    }
}