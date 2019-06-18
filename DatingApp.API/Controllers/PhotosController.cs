using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly IMapper _mapper;
        private IUserRepository _userRepo;
        private Cloudinary _cloudinary;
        public PhotosController(IUserRepository userRepo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _userRepo = userRepo;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            Photo photoFromRepo = await _userRepo.GetPhotoAsync(id);
            var photo = _mapper.Map<PhotoForReturnDTO>(photoFromRepo);
            return Ok(photo);
        }
        [HttpGet(Name = "GetPhotos")]
        [Route("getphotos")]
        public async Task<IActionResult> GetPhotos(int userId)
        {
            var photosFromRepo = await _userRepo.GetPhotosAsync(userId);
            var photo = _mapper.Map<List<PhotoForReturnDTO>>(photosFromRepo);
            return Ok(photo);
        }
        [Authorize(Roles = "admin")]
        [HttpGet(Name = "GetAllPhotos")]
        [Route("getallphotos")]
        public async Task<IActionResult> GetPhotos()
        {
            var photosFromRepo = await _userRepo.GetPhotosAsync();
            var photo = _mapper.Map<List<PhotoForReturnDTO>>(photosFromRepo);
            return Ok(photo);
        }
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUserAsync(int userId, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.Claims.FirstOrDefault(a => a.Type == "sub").Value))
            {
                return Unauthorized();
            }
            var userFromRepo = await _userRepo.GetUserAsync(userId);
            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParam = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(800).Height(800).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParam);
                }
            }
            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;
            var photo = _mapper.Map<Photo>(photoForCreationDto);
            if (!userFromRepo.Photos.Any(u => u.IsMain))
                photo.IsMain = true;

            userFromRepo.Photos.Add(photo);

            if (await _userRepo.SaveAllAsync())
            {
                var photoReturned = _mapper.Map<PhotoForReturnDTO>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoReturned);
            }

            return BadRequest("Could not add the photo");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.Claims.FirstOrDefault(a => a.Type == "sub").Value))
            {
                return Unauthorized();
            }
            var user = await _userRepo.GetUserAsync(userId);
            if (!user.Photos.Any(p => p.Id == id))
                return Unauthorized();

            var photoFromRepo = user.Photos.FirstOrDefault(p => p.Id == id);//await _userRepo.GetPhotoAsync(id);
            if (photoFromRepo.IsMain)
                return BadRequest("This photo is already main.");

            var currentPhoto = user.Photos.FirstOrDefault(p => p.IsMain);
            if (currentPhoto != null)
                currentPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if (await _userRepo.SaveAllAsync())
                return NoContent();

            return BadRequest("Could not set photo to main.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.Claims.FirstOrDefault(a => a.Type == "sub").Value))
            {
                return Unauthorized();
            }
            var user = await _userRepo.GetUserAsync(userId);
            if (!user.Photos.Any(p => p.Id == id))
                return Unauthorized();

            var photoFromRepo = user.Photos.FirstOrDefault(p => p.Id == id);//await _userRepo.GetPhotoAsync(id);
            if (photoFromRepo.IsMain)
                return BadRequest("You cannot delete your main photo");

            if (photoFromRepo.PublicID != null)
            {
                DeletionParams param = new DeletionParams(photoFromRepo.PublicID);
                var result = _cloudinary.Destroy(param);

                if (result.Result == "ok")
                {
                    _userRepo.Delete(photoFromRepo);
                }
            }
            else
                _userRepo.Delete(photoFromRepo);

            if (await _userRepo.SaveAllAsync())
                return Ok();

            return BadRequest("Failed to delete photo.");


        }
    }

}