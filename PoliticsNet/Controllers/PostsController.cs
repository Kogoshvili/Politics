using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PoliticsNet.Data;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using PoliticsNet.DTO;
using System.Collections.Generic;
using System.Security.Claims;
using System;
using PoliticsNet.Models;
using DatingApp.API.Helpers;
using PoliticsNet.Helpers;
using Microsoft.Extensions.Options;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace PoliticsNet.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IMainRespository _mainRepo;
        private readonly IPostRespository _repo;
        private readonly IMapper _mapper;
        private readonly IProviderRespository _provider;
        private readonly IOptions<CloudinarySettings> _cloudinaryCfg;
        private Cloudinary _cloudinary;
        public PostsController(
            IPostRespository repo, IMapper mapper,
            IProviderRespository provider, IOptions<CloudinarySettings> cloudinaryCfg,
            IMainRespository mainRepo)
        {
            _cloudinaryCfg = cloudinaryCfg;
            _provider = provider;
            _mapper = mapper;
            _repo = repo;
            _mainRepo = mainRepo;

            Account acc = new Account(
                _cloudinaryCfg.Value.CloudName,
                _cloudinaryCfg.Value.ApiKey,
                _cloudinaryCfg.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] PostParams postParams)
        {
            var posts = await _repo.GetPosts(postParams);
            var postsToReturn = _mapper.Map<IEnumerable<PostsToReturn>>(posts);
            Response.AddPagination(posts.CurrentPage, posts.PageSize,
                 posts.TotalCount, posts.TotalPages);
            return Ok(postsToReturn);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _repo.GetPost(id);

            if (post == null)
                throw new Exception("Post not found");

            var postToReturn = _mapper.Map<PostToReturn>(post);
            return Ok(postToReturn);
        }
        [NonAction]
        public async Task<Image> UploadPhoto(IFormFile file)
        {
            var photoForUpload = new PhotoForUpload();

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream),
                        //Transformation = new Transformation().Width(500).Height(500).Crop("fill");
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }

            }

            photoForUpload.Url = uploadResult.Uri.ToString();
            photoForUpload.PublicId = uploadResult.PublicId;

            var Image = _mapper.Map<Image>(photoForUpload);
            return Image;
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> Create(int id, [FromForm] PostIncoming postIncoming)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();


            var postToCreate = _mapper.Map<Post>(postIncoming);
            if(postIncoming.Images != null){
                var images = new List<Image>();

                foreach(var i in postIncoming.Images){
                    images.Add(await UploadPhoto(i));
                }
                postToCreate.Images = images;
            }

            _repo.CreatePost(id, postToCreate);

            if (!await _repo.SaveAll())
                throw new Exception("Failed");

            postToCreate.Category = JsonConvert.DeserializeObject<Category>(postIncoming.Category);

            _repo.UpdatePost(postToCreate);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception("Failed");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromForm] PostIncoming postIncoming)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var postFromRepo = await _repo.GetPost(postIncoming.Id);

            if (postFromRepo == null)
                throw new Exception("Post not found");

            if (postFromRepo.User.Id != id)
                return Unauthorized();


            var images = new List<Image>();

            if(postIncoming.ImagesToSave != null || (postFromRepo.Images.Count != 0 && postIncoming.ImagesToSave == null)){
                if(postIncoming.ImagesToSave != null){
                    postIncoming.ImagesToSave.ForEach(
                        (jimg) => {
                            images.Add(JsonConvert.DeserializeObject<Image>(jimg));
                        }
                    );
                }
                _repo.DeleteAllImages(postIncoming.Id);
            }

            if(postIncoming.Images != null){
                foreach(var i in postIncoming.Images){
                    images.Add(await UploadPhoto(i));
                }
            }

            postFromRepo.Category = JsonConvert.DeserializeObject<Category>(postIncoming.Category);

            postFromRepo.Images = images;
            postFromRepo.Content = postIncoming.Content;
            _repo.UpdatePost(postFromRepo);
            //_mapper.Map(postIncoming, postFromRepo);


            // if(images.Count > 0){
            //     images.ForEach(
            //         (img) => {
            //             _repo.AddImage(postIncoming.Id, img);
            //         }
            //     );
            // }

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception("Updating. failed on save");
        }
        [NonAction]
        public async Task<bool> DeletePhoto(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Result == "ok")
                return true;

            return false;
        }
        [HttpDelete("{userId}/delete/{postId}")]
        public async Task<IActionResult> DeletePost(int userId, int postId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var postFromRepo = await _repo.GetPost(postId);

            if (postFromRepo == null)
                throw new Exception("Post not found");

            if (postFromRepo.User.Id != userId)
                return Unauthorized();

            if (postFromRepo.Images.Count > 0)
            {
                foreach(var img in postFromRepo.Images){
                    // ????? ignore?
                    var res = await DeletePhoto(img.PublicId);

                }
            }

            _repo.DeletePost(postFromRepo);
            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception("Failed");
        }

        [AllowAnonymous]
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _repo.GetCategories();
            return Ok(_mapper.Map<IEnumerable<CategoryToReturn>>(categories));
        }

        [AllowAnonymous]
        [HttpGet("providers")]
        public async Task<IActionResult> GetProviders()
        {
            var provider = await _provider.GetProviderList();
            return Ok(_mapper.Map<IEnumerable<ProviderToReturn>>(provider));
        }

        [HttpGet("{userId}/like/{postId}")]
        public async Task<IActionResult> LikePost(int userId, int postId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            //Primary Key userid+postid
            var postLikees = await _repo.GetPostLikes(postId);

            var like = new PostLike {
                UserId = userId,
                PostId = postId
            };

            if(postLikees.Contains(userId)){
                _mainRepo.Delete<PostLike>(like);
            }else{
                _mainRepo.Add<PostLike>(like);
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