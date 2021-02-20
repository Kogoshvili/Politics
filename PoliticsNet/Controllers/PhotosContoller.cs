using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PoliticsNet.Controllers
{
    [Authorize]
    [Route("api/posts/{userId}/photos/{postId}")]
    public class PhotosContoller : ControllerBase
    {

    }
}