using System;
using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Data;
using LabApp.Server.Data.Repositories;
using LabApp.Server.Services.ImageService;
using LabApp.Server.Services.Interfaces;
using LabApp.Shared.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LabApp.Server.Controllers
{
    public class UserController : BaseCommonController
    {
        private readonly AppDbContext _db;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IImageService _imageService;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public UserController(AppDbContext dbContext, IImageService imageService,
            IServiceProvider serviceProvider, IMapper mapper, IUserService userService, IUserRepository userRepository)
        {
            _db = dbContext;
            _imageService = imageService;
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _scopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
        }

        [HttpGet]
        public IActionResult Check()
        {
            return Ok();
        }

        [HttpPost]
        public new async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        [HttpPost, ActionName("UploadProfilePhoto")]
        public async Task<IActionResult> UploadMainPhoto(IFormFile file)
        {
            var user = _userService.CurrentUser;
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError(nameof(file), "Error");
            }

            if (!file.ContentType.Contains("image", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(nameof(file), "Not an image");
            }

            try
            {
                await using (var stream = file.OpenReadStream())
                {
                    (string image, string thumb) = await _imageService.SaveImageAsync(stream, file.FileName, true);
                    _userRepository.AddProfilePhoto(user, image, thumb);
                }

                return Ok(_mapper.Map<ImageDto>(user.MainPhoto));
            }
            catch (ExtensionNotAllowedException e)
            {
                ModelState.AddModelError(nameof(file), e.Message);
            }

            return BadRequest(ModelState);
        }
    }
}